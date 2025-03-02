﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{

    public Sprite[] ToExperiment;
    public Sprite[] ToRampage;
    public Image[] Bars;

    // Start is called before the first frame update
    void Start()
    {
        Bars[0].sprite = ToExperiment[0];
        Bars[1].sprite = ToExperiment[1];
        if (SceneManager.GetActiveScene().name == "01_GrowScene")
        {
            this.gameObject.GetComponent<Animator>().Play("OpenInMainMenu");
        }
        else if (SceneManager.GetActiveScene().name == "02_RampageScene")
        {
            this.gameObject.GetComponent<Animator>().Play("OpenInRampage");
        }
    }
    private void Update()
    {
        //Temp Controls to Loads
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.gameObject.GetComponent<Animator>().Play("OpenInRampage");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.gameObject.GetComponent<Animator>().Play("OpenInMainMenu");
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.gameObject.GetComponent<Animator>().Play("ToMainMenu");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.gameObject.GetComponent<Animator>().Play("ToRampage");
        }
    }
    public void LoadingExperiment()
    {
        Bars[0].sprite = ToExperiment[0];
        Bars[1].sprite = ToExperiment[1];
    }

    public void LoadingRampage()
    {
        Bars[0].sprite = ToRampage[0];
        Bars[1].sprite = ToRampage[1];
    }

    public void GameActivate()
    {
        //activates controls OR Game and set self as inactive
        gameObject.SetActive(false);
    }

    public void GameDeactivate()
    {
        //deactivates controls OR Game (require another script to activate Loading Screen again (example: Button press)
        
    }

    public void GoToGrow()
    {
        SceneManager.LoadScene("01_GrowScene");
    }

    public void GoToRampage()
    {
        SceneManager.LoadScene("02_RampageScene");
    }
    

}
