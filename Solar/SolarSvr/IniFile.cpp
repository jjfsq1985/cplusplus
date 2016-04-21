#include "stdafx.h"
#include "inifile.h"

namespace inifile
{
    int INI_BUF_SIZE = 1024*16;

    IniFile::IniFile()
    {
        current_ = IniToken::Default;
        flags_.push_back(";");
        flags_.push_back("#");
    }

    //按'='分割Key,Value
    bool IniFile::parse(const string &content, string &key, string &value, char c/*= '='*/)
    {
        int i = 0;
        int len = content.length();

        while (i < len && content[i] != c) {
            ++i;
        }

        if (i >= 0 && i < len) {
            key = string(content.c_str(), i);
            value = string(content.c_str() + i + 1, len - i - 1);
            return true;
        }

        return false;
    }

    int IniFile::getline(string &str, FILE *fp)
    {
        int plen = 0;
        int buf_size = INI_BUF_SIZE * sizeof(char);

        char *buf = (char *)malloc(buf_size);
        char *pbuf = NULL;
        char *p = buf;

        if (buf == NULL) {
            Aprintf("no enough memory!exit!\n");
            exit(-1);
        }

        memset(buf, 0, buf_size);
        int total_size = buf_size;

        while (fgets(p, buf_size, fp) != NULL) {
            plen = strlen(p);

            if (plen > 0 && p[plen - 1] != '\n' && !feof(fp)) {

                total_size = strlen(buf) + buf_size;
                pbuf = (char *)realloc(buf, total_size);

                if (pbuf == NULL) {
                    free(buf);
                    Aprintf("no enough memory!exit!\n");
                    exit(-1);
                }

                buf = pbuf;

                p = buf + strlen(buf);

                continue;
            }
            else {
                break;
            }
        }

        str = buf;

        free(buf);
        buf = NULL;
        return str.length();
    }

    void IniFile::ResetParse(const string& line, int nConstBrace, IniToken ConstToken, int& nBrace, IniToken& current_)
    {
        if (line.find("=(") != string::npos)
        {
            nBrace++;
        }
        else if (line == ")")
        {
            nBrace--;
            if (nBrace == nConstBrace)
            {
                current_ = ConstToken;
            }
        }
    }

    int IniFile::load(const string &filename)
    {
        release();
        fname_ = filename;
        FILE *fp = NULL;
            
        fopen_s(&fp,filename.c_str(), "r");

        if (fp == NULL) {
            return -1;
        }

        string line = "";
        string comment = "";

        string strgroup = ""; //组名
        string strsection = "";//段名
        int nBraceCount = 0;

        while (getline(line, fp) > 0)
        {
            trimright(line, '\n');
            trimright(line, '\r');
            trim(line);

            if (suffixComment(line))
            {
                /* 针对 “value=1 #测试” 这种后面有注释的语句
                * 重新分割line，并添加注释到commnet
                * 注意：这种情况保存后会变成
                * #测试
                * value=1
                * */
                string subline;
                string tmp = line;

                for (size_t i = 0; i < flags_.size(); ++i) {
                    subline = line.substr(0, line.find(flags_[i]));
                    line = subline;
                }
                comment += tmp.substr(line.length());

                trimleft(line, ' ');
                trimright(line, ' ');
            }

            if (line.length() <= 0)
            {
                continue;
            }
            if (current_ == IniToken::GroupSkip)
            {
                comment = ""; 
                strgroup = "";
                ResetParse(line, 0, IniToken::Default, nBraceCount, current_);
            }
            else if (current_ == IniToken::SectionSkip)
            {
                 comment = "";
                 strsection = "";
                 ResetParse(line, 1, IniToken::InGroup, nBraceCount, current_);
            }
            else if (isComment(line))
            {
                if (comment != "")
                {
                    comment += delim + line;
                }
                else
                {
                    comment = line;
                }
            }
            else if (line == ")")
            {
                //group/section结束
                if (current_ == IniToken::InGroup)
                {
                    current_ = IniToken::Default;
                    strgroup = "";
                }
                else if (current_ == IniToken::InSection)
                {
                    current_ = IniToken::InGroup;
                    strsection = "";
                }
                nBraceCount--;
                comment = "";
            }
            else if (line.find("=(") != string::npos)
            {
                int len = line.find("=(");
                if (len <= 0)
                {
                    Aprintf("group/section为空\n");
                    current_ = IniToken::GroupSkip;
                    nBraceCount++;
                    continue;
                }
                string s(line, 0, len);                
                trimleft(s, ' ');
                trimright(s, ' ');

                int nResult = ParseTokenName(s, strgroup, current_,comment);
                if (nResult == 2 || nResult == 4)
                {
                    Aprintf("此group/section已存在:%s\n", s.c_str());
                    nBraceCount++;
                    continue;
                }
                else if (nResult == 1)
                {
                    nBraceCount++;
                    strsection = s;
                }
                else if (nResult == 3)
                {
                    nBraceCount++;
                    strgroup = s;
                }
            }            
            else 
            {
                string key, value;
                assert(!strgroup.empty() && !strsection.empty());

                if (parse(line, key, value, '=')) 
                {
                    IniItem item;
                    item.key = key;
                    item.value = value;
                    item.comment = comment;
                    
                    IniSection *sectionFind = getSection(strgroup, strsection);
                    if (sectionFind != NULL)
                        sectionFind->items.push_back(item);                   
                }
                else
                {
                    Aprintf("解析参数失败:Group %s Section %s: [%s]\n",strgroup, strsection, line.c_str());
                }

                comment = "";
            }
        }

        fclose(fp);

        return 0;
    }

    int IniFile::ParseTokenName(const string& sName, const string& sGroup, IniToken& current_, string& comment)
    {
        if (current_ == IniToken::InGroup && !sGroup.empty())
        {
            if (getSection(sGroup, sName.c_str()) != NULL)
            {
                current_ = IniToken::SectionSkip;
                return 2;
            }
            IniGroup *groupFind = getGroup(sGroup);
            if (groupFind != NULL)
            {
                IniSection *section = new IniSection();
                groupFind->sectionsItems.push_back(section);
                section->name = sName;
                section->comment = comment;
            }
            current_ = IniToken::InSection;
            comment = "";
            return 1;
        }
        else if (current_ == IniToken::Default)
        {
            if (getGroup(sName.c_str()) != NULL)
            {
                current_ = IniToken::GroupSkip;
                return 4;
            }
            IniGroup *group = new IniGroup();
            groups_.push_back(group);
            group->group = sName;
            group->groupcomment = comment;

            current_ = IniToken::InGroup;
            comment = "";
            return 3;
        }
        return 0;
    }

    int IniFile::save()
    {
        return saveas(fname_);
    }

    int IniFile::saveas(const string &filename)
    {
        string data = "";

        for (iterator grou = groups_.begin(); grou != groups_.end(); ++grou)
        {
            if( (*grou)->groupcomment != "")
            {
                data += (*grou)->groupcomment;
                data += delim;
            }
            assert((*grou)->group != "");
            data += (*grou)->group + string("=(");
            data += delim;

            list<IniSection*>& Data = (*grou)->sectionsItems;
            for (IniGroup::iterator sect = Data.begin(); sect != Data.end(); ++sect)
            {
                if ((*sect)->comment != "")
                {
                    data += (*sect)->comment;
                    data += delim;
                }
                assert((*sect)->name != "");

                data += (*sect)->name + string("=(");
                data += delim;
                vector<IniItem>& vecValue = (*sect)->items;
                for (IniSection::iterator item = vecValue.begin(); item != vecValue.end(); ++item)
                {
                    if (item->comment != "")
                    {
                        data += item->comment;
                        data += delim;
                    }

                    data += item->key + "=" + item->value;
                    data += delim;
                }
                data += string(")");
                data += delim;
            }
            data += string(")");
            data += delim;
        }

        FILE *fp = NULL;
        fopen_s(&fp, filename.c_str(), "w");
        if (fp == NULL) {
            return -1;
        }

        fwrite(data.c_str(), 1, data.length(), fp);

        fclose(fp);

        return 0;
    }
    IniSection *IniFile::getSection(const string &group, const string &section)
    {
        IniSection *sectionFind = NULL;
        for (iterator it = groups_.begin(); it != groups_.end(); ++it)
        {
            if ((*it)->group == group)
            {
                list<IniSection*>& sections = (*it)->sectionsItems;
                for (IniGroup::iterator itsect = sections.begin(); itsect != sections.end(); ++itsect)
                {
                    if ((*itsect)->name == section)
                    {
                        sectionFind = (*itsect);
                        break;
                    }
                }
            }
        }
        return sectionFind;
    }

    IniGroup *IniFile::getGroup(const string &group)
    {
        IniGroup *groupFind = NULL;
        for (iterator it = groups_.begin(); it != groups_.end(); ++it)
        {
            if ((*it)->group == group)
            {
                groupFind = (*it);
                break;
            }
        }
        return groupFind;
    }

    string IniFile::getStringValue(const string &group, const string &section, const string &key, int &ret)
    {
        string value, comment;

        ret = getValue(group,section, key, value, comment);

        return value;
    }

    int IniFile::getIntValue(const string &group, const string &section, const string &key, int &ret)
    {
        string value, comment;

        ret = getValue(group,section, key, value, comment);

        return atoi(value.c_str());
    }

    double IniFile::getDoubleValue(const string &group, const string &section, const string &key, int &ret)
    {
        string value, comment;

        ret = getValue(group,section, key, value, comment);

        return atof(value.c_str());

    }

    int IniFile::getValue(const string &group, const string &section, const string &key, string &value)
    {
        string comment;
        return getValue(group,section, key, value, comment);
    }

    int IniFile::getValue(const string &group, const string &section, const string &key, string &value, string &comment)
    {
        IniSection *sect = getSection(group,section);

        if (sect != NULL) 
        {
            for (IniSection::iterator it = sect->begin(); it != sect->end(); ++it) {
                if (it->key == key)
                {
                    value = it->value;
                    comment = it->comment;
                    return RET_OK;
                }
            }
        }

        return RET_ERR;
    }
    int IniFile::getValues(const string &group, const string &section, const string &key, vector<string> &values)
    {
        vector<string> comments;
        return getValues(group,section, key, values, comments);
    }
    int IniFile::getValues(const string &group, const string &section, const string &key,
        vector<string> &values, vector<string> &comments)
    {
        string value, comment;

        values.clear();
        comments.clear();

        IniSection *sect = getSection(group,section);

        if (sect != NULL) {
            for (IniSection::iterator it = sect->begin(); it != sect->end(); ++it) {
                if (it->key == key) {
                    value = it->value;
                    comment = it->comment;

                    values.push_back(value);
                    comments.push_back(comment);
                }
            }
        }

        return (values.size() ? RET_OK : RET_ERR);

    }
    bool IniFile::hasSection(const string &group, const string &section)
    {
        return (getSection(group,section) != NULL);

    }

    bool IniFile::hasKey(const string &group, const string &section, const string &key)
    {
        IniSection *sect = getSection(group,section);

        if (sect != NULL)
        {
            for (IniSection::iterator it = sect->begin(); it != sect->end(); ++it) 
            {
                if (it->key == key) 
                {
                    return true;
                }
            }
        }

        return false;
    }

    int IniFile::getGroupComment(const string &group, string &comment)
    {
        comment = "";
        IniGroup *groupFind = getGroup(group);

        if (groupFind != NULL) {
            comment = groupFind->groupcomment;
            return RET_OK;
        }

        return RET_ERR;
    }

    int IniFile::setGroupComment(const string &group, const string &comment)
    {
        IniGroup *groupFind = getGroup(group);

        if (groupFind != NULL) {
            groupFind->groupcomment = comment;
            return RET_OK;
        }

        return RET_ERR;
    }


    int IniFile::getSectionComment(const string &group, const string &section, string &comment)
    {
        comment = "";
        IniSection *sect = getSection(group,section);

        if (sect != NULL) {
            comment = sect->comment;
            return RET_OK;
        }

        return RET_ERR;
    }
    int IniFile::setSectionComment(const string &group, const string &section, const string &comment)
    {
        IniSection *sect = getSection(group,section);

        if (sect != NULL) {
            sect->comment = comment;
            return RET_OK;
        }

        return RET_ERR;
    }

    int IniFile::setValue(const string &group, const string &section, const string &key,
        const string &value, const string &comment /*=""*/)
    {
        IniSection *sect = NULL;
        IniGroup *groupFind = getGroup(group);
        if (groupFind == NULL)
        {
            groupFind = new IniGroup();
            if (groupFind == NULL)
            {
                Aprintf("no enough memory!\n");
                exit(-1);
            }
            groupFind->group = group;
            groups_.push_back(groupFind);
            sect = new IniSection();
            if (sect == NULL) {
                Aprintf("no enough memory!\n");
                exit(-1);
            }
            sect->name = section;            
            groupFind->sectionsItems.push_back(sect);
        }
        else
        {
            sect = getSection(group, section);
            if (sect == NULL)
            {
                sect = new IniSection();
                if (sect == NULL) {
                    Aprintf("no enough memory!\n");
                    exit(-1);
                }
                sect->name = section;
                groupFind->sectionsItems.push_back(sect);
            }
        }
        string comt = comment;
        if (comt != "")
        {
            comt = flags_[0] + comt;
        }
        for (IniSection::iterator it = sect->begin(); it != sect->end(); ++it)
        {
            if (it->key == key) 
            {
                it->value = value;
                it->comment = comt;
                return RET_OK;
            }
        }

        //not found key
        IniItem item;
        item.key = key;
        item.value = value;
        item.comment = comt;

        sect->items.push_back(item);

        return RET_OK;

    }
    void IniFile::getCommentFlags(vector<string> &flags)
    {
        flags = flags_;
    }
    void IniFile::setCommentFlags(const vector<string> &flags)
    {
        flags_ = flags;
    }

    void IniFile::deleteSection(const string &group, const string &section)
    {
        IniGroup *groupFind = getGroup(group);       
        if (groupFind != NULL)
        {
            list<IniSection*>& sections = groupFind->sectionsItems;
            for (IniGroup::iterator itsect = sections.begin(); itsect != sections.end(); ++itsect)
            {
                if ((*itsect)->name == section)
                {
                    groupFind->sectionsItems.erase(itsect);
                    delete (*itsect);
                    break;
                }
            }
        }
    }

    void IniFile::deleteGroup(const string &group)
    {
        IniGroup *groupFind = getGroup(group);

        if (groupFind != NULL)
        {
            list<IniSection*>& sections = groupFind->sectionsItems;
            for (IniGroup::iterator itsect = sections.begin(); itsect != sections.end(); ++itsect)
            {
                delete (*itsect);
            }
            groups_.remove(groupFind);
            delete groupFind;
        }
    }

    void IniFile::deleteKey(const string &group, const string &section, const string &key)
    {
        IniSection *sect = getSection(group,section);

        if (sect != NULL)
        {
            for (IniSection::iterator it = sect->begin(); it != sect->end(); ++it)
            {
                if (it->key == key)
                {
                    sect->items.erase(it);
                    break;
                }
            }
        }

    }

    void IniFile::release()
    {
        fname_ = "";

        for (iterator it = groups_.begin(); it != groups_.end(); ++it)
        {
            list<IniSection*>& lstData = (*it)->sectionsItems;
            for (IniGroup::iterator sect = lstData.begin(); sect != lstData.end(); ++sect)
            {
                delete (*sect);
            }
            delete (*it);
        }

        groups_.clear();
        current_ = IniToken::Default;
    }

    bool IniFile::suffixComment(const string &str)
    {
        bool ret = false;
        for (size_t i = 0; i < flags_.size(); ++i) 
        {
            size_t pos = str.find(flags_[i]);//注释在字符串后面
            if (pos != string::npos && pos > 0)
            {
                ret = true;
                break;
            }
        }
        return ret;
    }

    bool IniFile::isComment(const string &str)
    {
        bool ret = false;

        for (size_t i = 0; i < flags_.size(); ++i)
        {
            size_t k = 0;
            if (str.length() < flags_[i].length())
            {
                continue;
            }

            for (k = 0; k < flags_[i].length(); ++k)
            {
                if (str[k] != flags_[i][k]) {
                    break;
                }
            }

            if (k == flags_[i].length())
            {
                ret = true;
                break;
            }
        }

        return ret;
    }
    //for debug
    void IniFile::print()
    {
        Aprintf("filename:[%s]\n", fname_.c_str());

        string strflag = "";

        for (size_t i = 0; i < flags_.size(); ++i) 
        {
            strflag += " " + flags_[i] + " ";
        }

        Aprintf("flags_:[ %s ]\n", strflag.c_str());

        for (iterator it = groups_.begin(); it != groups_.end(); ++it)
        {
            Aprintf("group:[%s]\n", (*it)->group.c_str());
            Aprintf("groupcomment:[%s]\n", (*it)->groupcomment.c_str());

            list<IniSection*>& lstData = (*it)->sectionsItems;
            for (IniGroup::iterator itsect = lstData.begin(); itsect != lstData.end(); ++itsect)
            {
                Aprintf("section:[%s]\n", (*itsect)->name.c_str());
                Aprintf("comment:[%s]\n", (*itsect)->comment.c_str());

                for (IniSection::iterator iKV = (*itsect)->items.begin(); iKV != (*itsect)->items.end(); ++iKV)
                {
                    Aprintf("    comment:%s\n", iKV->comment.c_str());
                    Aprintf("    parm   :%s=%s\n", iKV->key.c_str(), iKV->value.c_str());
                }
            }
        }
    }

    void IniFile::trimleft(string &str, char c/*=' '*/)
    {
        //trim head

        int len = str.length();

        int i = 0;

        while (str[i] == c && str[i] != '\0') {
            i++;
        }

        if (i != 0) {
            str = string(str, i, len - i);
        }
    }

    void IniFile::trimright(string &str, char c/*=' '*/)
    {
        //trim tail
        int i = 0;
        int len = str.length();


        for (i = len - 1; i >= 0; --i) {
            if (str[i] != c) {
                break;
            }
        }

        str = string(str, 0, i + 1);
    }

    void IniFile::trim(string &str)
    {
        //trim head

        int len = str.length();

        int i = 0;

        while (isspace(str[i]) && str[i] != '\0') {
            i++;
        }

        if (i != 0) {
            str = string(str, i, len - i);
        }

        //trim tail
        len = str.length();

        for (i = len - 1; i >= 0; --i) {
            if (!isspace(str[i])) {
                break;
            }
        }
        str = string(str, 0, i + 1);
        size_t posEqual = str.find_first_of('=');
        size_t posBarce = str.find_last_of('(');
        if (posEqual != string::npos && posBarce != string::npos)
        {
            if (posBarce > posEqual + 1)
            {
                string strSpace(posBarce - posEqual - 1, ' ');                
                if (string(str, posEqual + 1, posBarce - posEqual - 1) == strSpace)
                {
                    str.erase(posEqual + 1, posBarce - posEqual - 1);
                }
            }
        }
    }

}
