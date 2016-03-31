// stdafx.cpp : 只包括标准包含文件的源文件
// SolarSvr.pch 将作为预编译头
// stdafx.obj 将包含预编译类型信息

#include "stdafx.h"

// TODO:  在 STDAFX.H 中
// 引用任何所需的附加头文件，而不是在此文件中引用
void __cdecl Tprintf(const char *format, ...)
{
    char buf[4096], *p = buf;
    va_list args;
    va_start(args, format);
    p += _vsnprintf_s(p, sizeof(buf) - 2, 4094, format, args);
    va_end(args);
    while (p > buf  &&  isspace(p[-1]))//超过4096则清除前一字节
        *--p = '\0';
    *p++ = '\r';
    *p++ = '\n';
    *p = '\0';
    OutputDebugString(buf);
}
