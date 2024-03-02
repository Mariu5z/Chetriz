using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;//handling with UI comoponents e.g buttons requires this library
using System.IO;
using TMPro;

//class to with buttons functions and their flags
//this class also manages visibilty of canvas and its objects for different game modes
public class Buttons : MonoBehaviour
{
    //public object to assign in unity inspector
    //each refers to diffrent screen window in game
    public GameObject TestingObject;                              
    public GameObject StartMenuObject;
    public GameObject BoardGridObject;
    public GameObject RecordsObject;
    public GameObject CirclesObject;
    public GameObject ShapesObject;
    public GameObject HowToPlayObject;
    public GameObject resultBar;
    public GameObject resultTimeBar;
    public GameObject nextLevelButton;
    //Flags indicating event when the buttons are clicked are very useful
    public static bool RestartFlag = false;                        
    public static bool ChangeModeFlag = false;
    public static bool ConfirmFlag = false;
    public static bool RestartLeveLFlag = false;
    public static bool NextLessonFlag = false;
    public static bool RestartTutorialFlag = true;
    //another public objects to assing in unity inspector
    public GameObject RecordsClicks;
    public GameObject RecordsTimes;
    public TMP_Dropdown myDropdown; 


    //turn on start menu
    //turn off game mode, tutorial and records tab
    public void StartMenu()
    {
        EndTutorial();
        StartMenuObject.SetActive(true);                                
        TestingObject.SetActive(false);
        BoardGridObject.SetActive(false);
        RecordsObject.SetActive(false);
        CirclesObject.SetActive(false);
        ShapesObject.SetActive(false);
        HowToPlayObject.SetActive(false);
        RestartFlag = true;
        RestartTutorialFlag = true;
    }

    //turn on game mode
    public void StartGame()
    {
        StartMenuObject.SetActive(false);
        BoardGridObject.SetActive(true);
        CirclesObject.SetActive(true);
        ShapesObject.SetActive(true);
        TestingObject.SetActive(true);
    }

    //turn on records tab
    public void showRecords()
    {
        StartMenuObject.SetActive(false);
        RecordsObject.SetActive(true);
        DisplayRecords(myDropdown.value);
    }

    //turn on tutorial (tutotial uses game mode but turn off some funcionality within it)
    public void StartHowToPlay()
    {
        HowToPlayObject.SetActive(true);
        resultBar.SetActive(false);
        resultTimeBar.SetActive(false);
        nextLevelButton.SetActive(false);
        StartGame();
    }

    //exit application
    public void EndGame()
    {
        Application.Quit();                                       
    }

    //functions called when buttons are pressed, they just turn on flag 
    public void Confirm()
    {
        ConfirmFlag = true;
    }
    public void ChangeMode()
    {
        ChangeModeFlag = true;
    }
    public void RestartGame()
    {
        RestartLeveLFlag = true;
    }
    public void NextLesson()
    {
        NextLessonFlag = true;
    }

    //when tutorial ends this function is used 
    public void EndTutorial()
    {
        HowToPlay.boardLockFlag = false;
        HowToPlay.moveLockFlag = false;
        HowToPlay.tutorialFlag = false;
        HowToPlay.step = 1;
        resultBar.SetActive(true);
        resultTimeBar.SetActive(true);
        nextLevelButton.SetActive(true);
    }

    //deactivate button
    public static void deactivateButton(Button button)
    {
        button.interactable = false;
    }
    //activate button
    public static void activateButton(Button button)
    {
        button.interactable = true;
    }

    public void CleanRecords()
    {
        Records.cleanRecords();
        DisplayRecords(myDropdown.value);
    }//clean all the saved records

    public void DisplayRecords(int index)
    {
        //clicks update display -----------------------------------------
        int[] levelScores = Records.getBestResults(index+1, "clicks");
        string recordsAll = "";
        for (int i = 0; i < levelScores.Length; i++)
        {
            if (levelScores[i] == Records.clicksLimit)
            {
                recordsAll += "-" + "\n";
            }
            else
            {
                recordsAll += levelScores[i].ToString() + "\n";
            }
        }
        Text textComponent = RecordsClicks.GetComponent<Text>();
        textComponent.text = recordsAll;

        //times update display -----------------------------------------
        levelScores = Records.getBestResults(index + 1, "times");
        recordsAll = "";
        for (int i = 0; i < levelScores.Length; i++)
        {
            if (levelScores[i] == Records.timesLimit)
            {
                recordsAll += "-" + "\n";
            }
            else
            {
                recordsAll += levelScores[i].ToString() + "\n";
            }
        }
        textComponent = RecordsTimes.GetComponent<Text>();
        textComponent.text = recordsAll;
    }//update displaying records using dropdown menu
}
