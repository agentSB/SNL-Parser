using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Project : MonoBehaviour
{

    public Text compilestatus;
    public Text programtext;
    public Text outputtext;
    public InputField fd;
    Syntactic syn;
    Lexical lex;

    // Use this for initialization
    void Start()
    {
        syn = gameObject.GetComponent<Syntactic>();
        lex = gameObject.GetComponent<Lexical>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Lexical.correct && !Syntactic.correct)
        {
            compilestatus.text = "Normal";
        }
        else if (Lexical.correct && !Syntactic.correct)
        {
            compilestatus.text = "Lexical Correct";
        }
        else if (Syntactic.correct)
        {
            compilestatus.text = "Syntactic Correct";
        }
        if (Lexical.correct)
        {
            GameObject.Find("LightBulb").GetComponent<RawImage>().color = new Color(1f,1f,0.75f);
        }
        else
        {
            GameObject.Find("LightBulb").GetComponent<RawImage>().color = new Color(1f, 1f, 1f);
        }
        if(outputtext.text.Equals("Syntax Error"))
        {
            compilestatus.text = "Syntax Error";
        }
    }

    public void onNewProject()
    {
        programtext.text = "";
        fd.text = "";
        outputtext.text = "";
        compilestatus.text = "Normal";
        Lexical.correct = false;
        Syntactic.correct = false;
    }

    public void onAnalysize()
    {
        if (!Lexical.correct)
            lex.OnStartLexAnalysis();
        else
            syn.OnStartSynAnalysis();
    }

    public void onLightBulb()
    {
        if(Lexical.correct)
            SceneManager.LoadScene("Tree");
    }

    public void onBackButton()
    {
        SceneManager.LoadScene("main");
    }

    public void onExit()
    {
        Application.Quit();
    }

    public void onExam()
    {
        outputtext.text = "Syntax Error";
        //compilestatus.text = "Syntactic Error";
    }

}
