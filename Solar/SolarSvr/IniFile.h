#ifndef _INIFILE_H
#define _INIFILE_H

#include <list>
#include <vector>
#include <string>

using namespace std;

namespace inifile
{
    const int RET_OK = 0;
    const int RET_ERR = -1;
    const string delim = "\n";
    struct IniItem
    {
        string key;
        string value;
        string comment;
    };
    struct IniSection {
        typedef vector<IniItem>::iterator iterator;
        iterator begin() {
            return items.begin();
        }
        iterator end() {
            return items.end();
        }

        string name;
        string comment;
        vector<IniItem> items;
    };

    struct IniGroup {
        typedef list<IniSection*>::iterator iterator;
        iterator begin() {
            return sectionsItems.begin();
        }
        iterator end() {
            return sectionsItems.end();
        }
        string group;
        string groupcomment;
        list<IniSection*> sectionsItems;//makepair<sectionname, IniSection>
    };

    enum IniToken
    {
        Default = 0,
        InGroup, 
        InSection,
        GroupSkip,
        SectionSkip
    };


    class IniFile
    {
    public:
        IniFile();
        ~IniFile()
        {
            release();
        }

    public:
        typedef list<IniGroup *>::iterator iterator;

        iterator begin() {
            return groups_.begin();
        }
        iterator end() {
            return groups_.end();
        }
    public:
        /* 打开并解析一个名为fname的INI文件 */
        int load(const string &fname);
        /*将内容保存到当前文件*/
        int save();
        /*将内容另存到一个名为fname的文件*/
        int saveas(const string &fname);

        /*获取section段第一个键为key的值,并返回其string型的值*/
        string getStringValue(const string &group, const string &section, const string &key, int &ret);
        /*获取section段第一个键为key的值,并返回其int型的值*/
        int getIntValue(const string &group, const string &section, const string &key, int &ret);
        /*获取section段第一个键为key的值,并返回其double型的值*/
        double getDoubleValue(const string &group, const string &section, const string &key, int &ret);

        /*获取section段第一个键为key的值,并将值赋到value中*/
        int getValue(const string &group, const string &section, const string &key, string &value);
        /*获取section段第一个键为key的值,并将值赋到value中,将注释赋到comment中*/
        int getValue(const string &group, const string &section, const string &key, string &value, string &comment);

        /*获取section段所有键为key的值,并将值赋到values的vector中*/
        int getValues(const string &group, const string &section, const string &key, vector<string> &values);
        /*获取section段所有键为key的值,并将值赋到values的vector中,,将注释赋到comments的vector中*/
        int getValues(const string &group, const string &section, const string &key, vector<string> &value, vector<string> &comments);

        bool hasSection(const string &group,const string &section);
        bool hasKey(const string &group, const string &section, const string &key);

        /* 获取group段的注释 */
        int getGroupComment(const string &group, string &comment);
        /* 设置group段的注释 */
        int setGroupComment(const string &group, const string &comment);

        /* 获取section段的注释 */
        int getSectionComment(const string &group, const string &section, string &comment);
        /* 设置section段的注释 */
        int setSectionComment(const string &group, const string &section, const string &comment);
        /*获取注释标记符列表*/
        void getCommentFlags(vector<string> &flags);
        /*设置注释标记符列表*/
        void setCommentFlags(const vector<string> &flags);

        /*同时设置值和注释*/
        int setValue(const string &group, const string &section, const string &key, const string &value, const string &comment = "");
        /*删除段*/
        void deleteGroup(const string &group);
        /*删除段*/
        void deleteSection(const string &group, const string &section);
        /*删除特定段的特定参数*/
        void deleteKey(const string &group, const string &section, const string &key);
    public:
        /*去掉str前面的c字符*/
        static void trimleft(string &str, char c = ' ');
        /*去掉str后面的c字符*/
        static void trimright(string &str, char c = ' ');
        /*去掉str前面和后面的空格符,Tab符等空白符*/
        static void trim(string &str);
        /*将字符串str按分割符delim分割成多个子串*/
    private:
        int ParseTokenName(const string& sName, const string& sGroup, IniToken& current_,string& comment);
        void ResetParse(const string& line, int nConstBrace, IniToken ConstToken, int& nBrace, IniToken& current_);
        IniSection *getSection(const string &group, const string &section);
        IniGroup * getGroup(const string &group);
        void release();
        int getline(string &str, FILE *fp);
        bool isComment(const string &str);
        bool suffixComment(const string &str);
        bool parse(const string &content, string &key, string &value, char c = '=');
      
        //for debug
    private:
        void print();

    private:
        IniToken current_;
        list<IniGroup *> groups_;
        string fname_;  //文件名称
        vector<string> flags_;//注释标志
    };
}

#endif
