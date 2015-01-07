
// Module:  Log4CPLUS
// File:    env.h

#ifndef LOG4CPLUS_INTERNAL_ENV_H
#define LOG4CPLUS_INTERNAL_ENV_H

#include <vector>

#include "log4cplus/platform.h"

//!Get environment variable value.
bool getEnvString(std::string& envString, std::string const& name);

//!Parse a std::string as a boolean value.
bool parse2bool(bool& val, std::string const& str);

//!Parse a path into path components.
bool splitFilePath(std::vector<std::string>& components, std::size_t& special, std::string const& path);

//!Makes directories leading to file.
void make_dirs(std::string const& file_path);



#endif // LOG4CPLUS_INTERNAL_ENV_H
