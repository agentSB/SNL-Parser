﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnNewProject()
    {
        SceneManager.LoadScene("ProjectScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
