using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Lexical : MonoBehaviour
{
    public Text programtext;
    public Text resulttext;

    public static bool correct = false;

    // Use this for initialization
    void Start()
    {
        correct = false;
        programindex = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }


    //string inputtext = new StreamReader("loop2.txt").ReadToEnd();
    int programindex = 0;

    //token结构体
    struct Token
    {
        public string type;
        public int pos;

        public Token(string type, int pos) : this()
        {
            this.type = type;
            this.pos = pos;
        }
    };

    private Token pToken;
    //FileStream fs;

    List<string> IdTable;             //全局变量，词法分析得到的标示符表
    List<int> NbTable;                //全局变量，词法分析得到的INTC值表

    /*#define ERROR_INVALID_SYMBOL  1		//宏定义错误：非法符号
     #define NO_ERROR			  0	*/    //宏定义	：没有错误

    int error = 0;              //全局变量，记录词法分析中的错误类型
    int column = 0;

    /*
     *判断字符是不是其他字符
     */
    bool IsOther(char ch)
    {
        if (ch >= 'A' && ch <= 'Z')
            return false;
        if (ch >= 'a' && ch <= 'z')
            return false;
        if (ch >= '0' && ch <= '9')
            return false;
        return true;
    }
    /*
     *判断字符串是不是保留字
     */
    bool IsKeyWord(string str)
    {
        if (str == "integer")
            return true;
        if (str == "char")
            return true;
        if (str == "program")
            return true;
        if (str == "array")
            return true;
        if (str == "of")
            return true;
        if (str == "record")
            return true;
        if (str == "end")
            return true;
        if (str == "var")
            return true;
        if (str == "procedure")
            return true;
        if (str == "begin")
            return true;
        if (str == "if")
            return true;
        if (str == "then")
            return true;
        if (str == "else")
            return true;
        if (str == "fi")
            return true;
        if (str == "while")
            return true;
        if (str == "do")
            return true;
        if (str == "endwh")
            return true;
        if (str == "read")
            return true;
        if (str == "write")
            return true;
        if (str == "return")
            return true;
        if (str == "type")
            return true;
        return false;
    }

    /*
     *返回字符串在表中的位置，没有就加到最后，并返回下标
     */
    int AddIdTable(string str)
    {
        for (int i = 0; i < IdTable.Count; i++)
        {
            if (str == IdTable[i])
                return i;
        }
        IdTable.Add(str);
        return IdTable.Count - 1;
    }

    /*
     *返回数字在表中的位置，没有就加到最后，并返回下标
     */
    int AddNbTable(string str)
    {
        int num = int.Parse(str);
        for (int i = 0; i < NbTable.Count; i++)
        {
            if (num == NbTable[i])
                return i;
        }
        NbTable.Add(num);
        return (int)NbTable.Count - 1;
    }

    /*
     *词法分析扫描程序，每次调用返回一个Token指针
     */
    int Scanner()
    {
        //StreamReader sr = new StreamReader(filepath, Encoding.Default);
        char ch = ' ';
        string tmpStr = "";
        /*LL:     bool save=true;
         ch=fgetc(pf);
         if(ch=='{')
         save=false;
         if(ch=='}')
         save=true;
         if(save)
         {
         ungetc(ch, pf);
         goto LS0;
         }
         if(save==false)
         goto LL;
         */

        bool save = true;
        int gotoflag = 0;

        if (programtext.text.Equals(""))    //输入为空，直接停止分析
        {
            error = 1;
            return -1;
        }

        LS0://根据第一个字符确定程序走向
            //int next = fs.ReadByte();
        if (programindex == programtext.text.Length)
        {
            Debug.Log("1111111");
            return -1;
        }

        ch = programtext.text[programindex++];
       
        if (ch == '\n')
            column++;

        if ((int)ch >= 0)
        {
            if (ch == '{')
                save = false;//忽略掉注释
            if (ch == '}')
            {
                save = true;
                goto LS0;
            }
            if (save == false)
                goto LS0;
            if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z'))
                goto LS1;
            if (ch >= '0' && ch <= '9')
                goto LS2;
            if (ch == '+')
                goto LS3;
            if (ch == '-')
                goto LS4;
            if (ch == '*')
                goto LS5;
            if (ch == '/')
                goto LS6;
            if (ch == '<')
                goto LS7;
            if (ch == ';')
                goto LS8;
            if (ch == ':')
                goto LS9;
            if (ch == ',')
                goto LS10;
            if (ch == '.')
                goto LS11;
            if (ch == '=')
            {
                gotoflag = 12;
                goto LS11;
            }
            if (ch == '[')
            {
                gotoflag = 13;
                goto LS11;
            }
            if (ch == ']')
            {
                gotoflag = 14;
                goto LS11;
            }
            if (ch == '(')
            {
                gotoflag = 15;
                goto LS11;
            }
            if (ch == ')')
            {
                gotoflag = 16;
                goto LS11;
            }
            if (ch == ' ' || ch == '\n' || ch == '\r' || ch == '\t')
            {
                gotoflag = 17;
                goto LS11;
            }
            gotoflag = 18;
            goto LS11;
        }
        else
        {
            return -1;
        }
        LS1://标示符和关键字
        {
            tmpStr += ch;
            //ch = (char)fs.ReadByte();
            if (programindex == programtext.text.Length)
            {
                Debug.Log("2222222");
                ch = ' ';
                programindex++;
            }
            else
                ch = programtext.text[programindex++];

            if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9'))
                goto LS1;
            if (IsOther(ch))
            {
                //ungetc(ch, pf);//把读到的字符放回到文件流中
                //fs.WriteByte(Convert.ToByte(ch));
                programindex--;
                if (IsKeyWord(tmpStr))
                {
                    //保留字
                    pToken = new Token("$" + tmpStr, -1);
                    return 0;
                }
                else
                {
                    //不是保留字,是标示符
                    int pos = AddIdTable(tmpStr);
                    pToken = new Token("$id", pos);
                    return 0;
                }
            }
        }
        LS2://数字
        {
            tmpStr += ch;
            //ch = (char)fs.ReadByte();
            ch = programtext.text[programindex++];
            if (ch >= '0' && ch <= '9')
                goto LS2;
            /*if (IsOther(ch))*/
            if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z'))
            {
                error = 1;
                //Console.WriteLine("id is unavailable(the first should be a letter)\nthe column of error place is " + column);//打印错误信息
                resulttext.text = "id is unavailable(the first should be a letter)\nthe column of error place is " + column;
                return -1;
            }
            else
            {
                //ungetc(ch, pf);
                //fs.WriteByte(Convert.ToByte(ch));
                programindex--;
                int pos = AddNbTable(tmpStr);
                pToken = new Token("$INTC", pos);
                return 0;
            }
        }
        LS3://'+'
        {
            pToken = new Token("$+", -1);
            return 0;
        }
        LS4://'-'
        {
            pToken = new Token("$-", -1);
            return 0;
        }
        LS5://'*'
        {
            pToken = new Token("$*", -1);
            return 0;
        }
        LS6://'/'
        {
            pToken = new Token("$/", -1);
            return 0;
        }
        LS7://'<'
        {
            pToken = new Token("$<", -1);
            return 0;
        }
        LS8://';'
        {
            pToken = new Token("$;", -1);
            return 0;
        }
        LS9://':'
        {
            //ch = (char)fs.ReadByte();
            ch = programtext.text[programindex++];
            if (ch == '=')
            {
                pToken = new Token("$:=", -1);
                return 0;
            }
            else
            {
                error = 1;
                correct = false;
                //Console.WriteLine("there is no “=” after “:” \nthe column of error place is " + column);//打印错误信息
                resulttext.text = "there is no “=” after “:” \nthe column of error place is " + column;
                return -1;

            }
            /*if (IsOther(ch))
             {
             error = 1;
             printf("error\n");
             return NULL;
             }*/
        }
        LS10://','
        {
            pToken = new Token("$comma", -1);
            return 0;
        }
        LS11://'.'
        {
            if (gotoflag == 12)
                goto LS12;
            if (gotoflag == 13)
                goto LS13;
            if (gotoflag == 14)
                goto LS14;
            if (gotoflag == 15)
                goto LS15;
            if (gotoflag == 16)
                goto LS16;
            if (gotoflag == 17)
                goto LS17;
            if (gotoflag == 18)
                goto LS18;
            //ch = (char)fs.ReadByte();
            ch = programtext.text[programindex++];
            if (ch == '.')
            {
                pToken = new Token("$..", -1);
                return 0;
            }
            else
            {
                //ungetc(ch, pf);
                //fs.WriteByte(Convert.ToByte(ch));
                programindex--;
                pToken = new Token("$.", -1);
                return 0;

            }
            LS12://'='
            {
                pToken = new Token("$=", -1);
                return 0;
            }
            LS13://'['
            {
                pToken = new Token("$[", -1);
                return 0;
            }
            LS14://']'
            {
                pToken = new Token("$]", -1);
                return 0;
            }
            LS15://'('
            {
                pToken = new Token("$(", -1);
                return 0;
            }
            LS16://')'
            {
                pToken = new Token("$)", -1);
                return 0;
            }
            LS17://空白符
            {
                goto LS0;
            }
            LS18://other
            {
                error = 1;//设置全局变量error
                correct = false;
                          //printf("the error point is( %c )\nthe column of error place is %d\n", ch, column);//打印错误信息
                          //Console.WriteLine("the error point is( " + ch + " )\nthe column of error place is " + column);
                resulttext.text = "the error point is( " + ch + " )\nthe column of error place is " + column;
                return -1;
            }

        }
    }

    /*
     *主函数,如果argc大于等于2,argv[1]为源文件名，否则默认为snl.txt
     */
    public void OnStartLexAnalysis()
    {
        //设置源文件
        string filepath = "C:\\Users\\liangzx\\Desktop\\loop2.txt";
        programindex = 0;
        resulttext.text = "";
        error = 0;
        column = 0;
        correct = false;
        //打开源文件
        //FILE* pf = fopen(filename.c_str(), "r");

        //if (sr.Read)
        //{
        //    printf("打开文件失败!\n");
        //    return -1;
        //}

        List<Token> result = new List<Token>();
        IdTable = new List<string>();
        NbTable = new List<int>();

        pToken = new Token();
        //fs = new FileStream(filepath, FileMode.Open);
        while ((Scanner()) != -1)
        {
            result.Add(pToken);
            //delete pToken;
            //pToken = NULL;
        }

        //fclose(pf);

        if (error == 0)
        {
            correct = true;  //词法分析成功

            using (StreamWriter file = new StreamWriter(filepath + ".token", false))
            {
                foreach (Token iter in result)
                {
                    if (iter.type == "$id" || iter.type == "$INTC")
                    {
                        //printf("(%-10s, “[%1d]” )\n", iter.type, iter.pos);
                        //Console.WriteLine(iter.type + ",\t" + iter.pos);
                        //fprintf(pf, "(%s,[%d])\n", iter.type, iter.pos);
                        resulttext.text += iter.type + ",\t" + iter.pos + "\n";
                        file.WriteLine(iter.type + ",\t" + iter.pos);
                    }
                    else
                    {
                        //printf("(%-10s, \"   \" )\n", iter.type);
                        //fprintf(pf, "(%s,\"\")\n", iter.type);
                        //Console.WriteLine(iter.type + ",\t");
                        resulttext.text += iter.type + ",\t\n";
                        file.WriteLine(iter.type + ",\t");
                    }
                }
                file.Close();
            }

            using (StreamWriter file = new StreamWriter(filepath + ".idtable", false))
            {
                foreach (string iter in IdTable)
                {
                    file.WriteLine(iter);
                }
                file.Close();
            }

            using (StreamWriter file = new StreamWriter(filepath + ".nbtable", false))
            {
                foreach (int iter in NbTable)
                {
                    file.WriteLine(iter);
                }
                file.Close();
            }

            //pf = fopen((filename + ".idtable").c_str(), "w+");
            //for (int i = 0; i < IdTable.size(); i++)
            //{
            //    fprintf(pf, "%s\n", IdTable[i].c_str());
            //}
            //fclose(pf);

            //pf = fopen((filename + ".nbtable").c_str(), "w+");
            //for (int i = 0; i < NbTable.size(); i++)
            //{
            //    fprintf(pf, "%d\n", NbTable[i]);
            //}
            //fclose(pf);
            /*printf("%d",column);*/
        }
    }

}
