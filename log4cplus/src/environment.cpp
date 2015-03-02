
#include "log4cplus/environment.h"
#include "log4cplus/stringhelper.h"
#include "log4cplus/loglog.h"

#include <sys/stat.h>
#include <cassert>
#include <cerrno>
#include <sstream>
#include <stdexcept>
#include <string.h>

#ifdef _MSC_VER 
#include <direct.h>
#include <tchar.h>
#endif

using namespace std;
using namespace log4cplus;

#ifdef _MSC_VER 
string const dir_sep("\\");
#else
string const dir_sep("/");
#endif


bool getEnvString(string& envString, string const& name)
{
    char const* val = std::getenv(name.c_str());
    if(val)
        envString = val;

    return !!val;
}


bool parse2bool(bool& val, string const& str)
{
    istringstream iss(str);
    string word;

    // Read a single "word".

    if(!(iss >> word))
        return false;

    // Following extraction of a character should fail
    // because there should only be a single "word".

    char ch;
    if(iss >> ch)
        return false;

    // Compare with "true" and "false".

    word = toLower(word);
    bool result = true;
    if(word == "true")
        val = true;
    else if(word == "false")
        val = false;

    // Try to interpret the "word" as a number.

    else
    {
        // Seek back to the start of stream.

        iss.clear();
        iss.seekg(0);
        assert(iss);

        // Extract value.

        long lval;
        iss >> lval;

        // The extraction should be successful and
        // following extraction of a characters should fail.

        result = !!iss && !(iss >> ch);
        if(result)
            val = !!lval;
    }

    return result;
}


static bool is_sep(char ch)
{
#ifdef _MSC_VER 
	return ch == '\\' || ch == '/';
#else
	return ch == '/';
#endif
}


static bool isStringEmpty(string const& str)
{
	return str.empty();
}


static void remove_empty(vector<string>& cont, std::size_t special)
{
    cont.erase(std::remove_if(cont.begin() + special, cont.end(), isStringEmpty), cont.end());
}


static bool is_drive_letter(char ch)
{
    char dl = toUpper(ch);
    return 'A' <= dl && dl <= 'Z';
}


#ifdef _MSC_VER 

static string get_drive_cwd(char drive)
{
    string path;

    drive = toUpper(drive);
    
    char * cstr = _getdcwd(drive - 'A' + 1, 0, 0x7FFF);
    if(!cstr)
    {
        int const eno = errno;
        LogLog::getLogLog()->error("_getdcwd: " + convertIntegerToString(eno), true);
    }

    try
    {
        path.assign(cstr);
    }
    catch(...)
    {
        std::free(cstr);
        throw;
    }

    std::free(cstr);
    return path;
}

#endif


static string get_current_dir()
{
#ifdef _MSC_VER 
    string result(0x8000, '\0');

    DWORD len = GetCurrentDirectory(static_cast<DWORD>(result.size()), &result[0]);
    if(len == 0 || len >= result.size())
        throw std::runtime_error("GetCurrentDirectory");

    result.resize(len);
    return result;

#else
    string buf;
    string::size_type buf_size = 1024;
    char * ret;
    do
    {
        buf.resize(buf_size);
        ret = getcwd(&buf[0], buf.size());
        if(!ret)
        {
            int const eno = errno;
            if(eno == ERANGE)
                buf_size *= 2;
            else
                LogLog::getLogLog()->error("getcwd: " + convertIntegerToString(eno), true);
        }
    }
    while(!ret);

    buf.resize(strlen(buf.c_str()));
    return buf;

#endif
}


#ifdef _MSC_VER 
static char get_current_drive()
{
    string const cwd = get_current_dir();
    if(cwd.size() >= 2 && cwd[1] == ':')
        return cwd[0];
    else
        return 0;
}
#endif


static void split_into_components(vector<string>& components, string const& path)
{
    string::const_iterator const end = path.end();
    string::const_iterator it = path.begin();
    while(it != end)
    {
        string::const_iterator sep = std::find_if(it, end, is_sep); components.push_back(string(it, sep));
        it = sep;
        if(it != end)
            ++it;
    }
}


static void expandDriveRelativePath(vector<string>& components, std::size_t rel_path_index)
{
    // Save relative path attached to drive,
    // e.g., relpath in "C:relpath\foo\bar".

    string relative_path_first_component(components[rel_path_index], 2);

    // Get current working directory of a drive.
#ifdef _MSC_VER
	 string const drive_path = get_drive_cwd(components[rel_path_index][0]);
#else		// __linux__
	 string const drive_path = "";
#endif

    // Split returned path.
    vector<string> drive_cwd_components;
    split_into_components(drive_cwd_components, drive_path);

    // Move the saved relative path into place.
    components[rel_path_index].swap(relative_path_first_component);

    // Insert the current working directory for a drive.
    components.insert(components.begin() + rel_path_index, drive_cwd_components.begin(), drive_cwd_components.end());
}


static void expand_relative_path(vector<string>& components)
{
    // Get the current working director.

    string const cwd = get_current_dir();

    // Split the CWD.

    vector<string> cwd_components;

    // Use qualified call to appease IBM xlC.
    split_into_components(cwd_components, cwd);

    // Insert the CWD components at the beginning of components.

    components.insert(components.begin(), cwd_components.begin(), cwd_components.end());
}


bool splitFilePath(vector<string>& components, std::size_t& special, string const& path)
{
    typedef string::const_iterator const_iterator;

    components.reserve(10);
    special = 0;

    // First split the path into individual components separated by
    // system specific separator.

    split_into_components(components, path);
	
    // Try to recognize the path to find out how many initial components
    // of the path are special and should not be attempted to be created
    // using mkdir().

retry_recognition:;
    std::size_t const comp_count = components.size();
    std::size_t comp_0_size;

#ifdef _MSC_VER 
    // Special Windows paths recognition:
    // \\?\UNC\hostname\share\ - long UNC path
    // \\?\ - UNC path
    // \\.\ - for special devices like COM1
    // \\hostname\share\ - shared folders

    // "" "" "?" "UNC" "hostname" "share" "file or dir"
    // "" "" "?" "UNC" "server" "volume" "file or dir"
    if(comp_count >= 7 
		&& components[0].empty() && components[1].empty() 
		&& components[2] == "?" &&(components[3].size() == 3
		&& toUpper(components[3][0]) == 'U'
		&& toUpper(components[3][1]) == 'N'
		&& toUpper(components[3][2]) == 'C'))
    {
        remove_empty(components, 2);
        special = 6;
        return components.size() >= special + 1;
    }
    // comp_count >= 6: "" "" "?" "hostname" "share" "file or dir"
    // comp_count >= 5: "" "" "?" "C:" "file or dir"
    else if(comp_count >= 5 && components[0].empty() && components[1].empty() && components[2] == "?")
    {
        remove_empty(components, 2);

        std::size_t const comp_3_size = components[3].size();
        // "C:"
        if(comp_3_size >= 2 && is_drive_letter(components[3][0]) && components[3][1] == ':')
        {
            if(comp_3_size > 2)
                expandDriveRelativePath(components, 3);

            special = 4;
        }
        // "hostname" "share"
        else
            special = 5;

        return components.size() >= special + 1;
    }
    // "" "" "hostname" "share" "file or dir"
    else if(comp_count >= 5 && components[0].empty() && components[1].empty())
    {
        remove_empty(components, 2);
        special = 4;
        return components.size() >= special + 1;
    }
    // "" "" "." "device"
    else if(comp_count >= 4
        && components[0].empty()
        && components[1].empty()
        && components[2] == ".")
    {
        remove_empty(components, 3);
        special = 3;
        return components.size() >= special + 1;
    }
    // "" "file", i.e. "\path\to\file"
    else if(comp_count >= 2
        && components[0].empty() && !components[1].empty())
    {
        remove_empty(components, 1);

        char const cur_drive = get_current_drive();
        if(!cur_drive)
            // Current path is not on a drive. It is likely on a share.
            return false;

        // Replace the first empty component with the current drive.
        components[0] = cur_drive;
        components[0] += ':';

        special = 1;

        return true;
    }
    // comp_count >= 2: "C:" "file"
    // comp_count >= 2: "C:relpath" "file"
    // comp_count >= 1: "C:relpath"
    else if(comp_count >= 1
        &&(comp_0_size = components[0].size()) >= 2 && is_drive_letter(components[0][0]) && components[0][1] == ':')
    {
        remove_empty(components, 1);

        // "C:relpath"
        if(comp_0_size > 2)
            expandDriveRelativePath(components, 0);

        special = 1;
        return components.size() >= special + 1;
    }
#else
    // "" "file", e.g., "/var/log/foo.0"
    if(comp_count >= 2 && components[0].empty())
    {
        remove_empty(components, 1);
        special = 1;
        return components.size() >= special + 1;
    }
#endif

    // "relative\path\to\some\file.log
    else
    {
        remove_empty(components, 0);
        expand_relative_path(components);
        goto retry_recognition;
    }
	
}


static long makeDirectory(string const& dir)
{
#ifdef _MSC_VER 
    if(_mkdir(dir.c_str()) == 0)

#else
    if(mkdir(dir.c_str(), 0777) == 0)

#endif
        return 0;
    else
        return errno;
}


static void loglog_makeDirectoryResult(LogLog* loglog, string const& path, long ret)
{
    if(ret != 0)
    {
        ostringstream oss;
        oss << "Failed to create directory " << path << "; error" << ret;
        loglog->error(oss.str());
    }
}

int checkFileOK(string const& name)
{
#ifdef _MSC_VER 
	struct _stat fileStatus;
	if(_tstat(name.c_str(), &fileStatus) == -1)
		return -1;

#else
	struct stat fileStatus;
	if(stat(name.c_str(), &fileStatus) == -1)
		return -1;

#endif

	return 0;
}

//!Creates missing directories in file path.
void make_dirs(string const& file_path)
{
    vector<string> components;
    std::size_t special = 0;
    LogLog* loglog = LogLog::getLogLog();

    // Split file path into components.

    if(!splitFilePath(components, special, file_path))
        return;

    // Remove file name from path components list.

    components.pop_back();

    // Loop over path components, starting first non-special path component.

    string path;
    join(path, components.begin(), components.begin() + special, dir_sep);

    for(std::size_t i = special, components_size = components.size(); i != components_size; ++i)
    {
        path += dir_sep;
        path += components[i];

        // Check whether path exists.

        if(checkFileOK(path) == 0)
            // This directory exists. Move forward onto another path component.
            continue;

        // Make new directory.

        long const eno = makeDirectory(path);
        loglog_makeDirectoryResult(loglog, path, eno);
    }
}

