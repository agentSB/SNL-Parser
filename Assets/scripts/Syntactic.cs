using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Syntactic : MonoBehaviour
{
    public Text result;
    public Text compilestatus;
    public static bool correct = false;

    public int[,] SYNTAX = new int[150, 2];
    public Queue<int> TOKENLIST = new Queue<int>();
    public int[,] ACTIONGOTO = new int[400, 80];

    // Use this for initialization
    void Start()
    {
        correct = false;
        ReadFile("syntax.txt", "syntax");       //读取语法
        ReadFile("action.txt", "actiongoto");   //读取action&goto表
        ReadFile("tokenlist.txt", "tokenlist"); //读取Token序列
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStartSynAnalysis()
    {
        correct = false;
        Stack<int> state = new Stack<int>();
        Stack<int> symbol = new Stack<int>();
        state.Push(0);
        int stacktop = 0;
        int ptr = 0;
        bool reduce = false;
        while (TOKENLIST.Peek() != 0)
        {
            //cout<<TOKENLIST.front()<<endl;
            stacktop = state.Peek();
            if (reduce)
            {
                ptr = ACTIONGOTO[stacktop, symbol.Peek()];
            }
            else
                ptr = ACTIONGOTO[stacktop, TOKENLIST.Peek()];
            if (ptr == 999)     //分析成功
            {
                //successful
                //Console.WriteLine("successful");
                correct = true;
                result.text = "successful";
                compilestatus.text = "Success";
                break;
            }
            else if (ptr == 0)  //分析失败
            {
                //wrong
                //Console.WriteLine("wrong");
                correct = false;
                break;
            }
            else if (ptr > 0)   //移入
            {
                state.Push(ptr);
                if (reduce)
                {
                    reduce = false;
                    continue;
                }
                symbol.Push(TOKENLIST.Peek());
                TOKENLIST.Dequeue();
            }
            else if (ptr < 0)   //归约
            {
                for (int i = 0; i < SYNTAX[0 - ptr, 1]; i++)
                {
                    state.Pop();
                    symbol.Pop();
                }
                symbol.Push(SYNTAX[0 - ptr, 0]);
                reduce = true;
            }
        }
    }

    public void ReadFile(string path, string filetype)
    {
        StreamReader sr = new StreamReader(path, Encoding.Default);
        string line;
        int row = 0;
        while ((line = sr.ReadLine()) != null)
        {
            string[] split = line.Split(' ');
            //Console.WriteLine(line.ToString());
            if (filetype.Equals("syntax"))
            {
                for (int i = 0; i < split.Length; i++)
                {
                    SYNTAX[row, i] = int.Parse(split[i]);
                    //Console.WriteLine(SYNTAX[row, i]);
                }
            }
            else if (filetype.Equals("actiongoto"))
            {
                for (int i = 0; i < split.Length; i++)
                {
                    ACTIONGOTO[row, i] = int.Parse(split[i]);
                    //Console.WriteLine(ACTIONGOTO[row, i]);
                }
            }
            else if (filetype.Equals("tokenlist"))
            {
                TOKENLIST.Enqueue(int.Parse(split[0]));
            }
            row++;
        }
        sr.Close();
    }

}
