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
    public TMP_Dropdown myDropdown; 


    //turn on start menu
    //turn off other game modes
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

    //turn on solo game mode
    public void StartGame()
    {
        StartMenuObject.SetActive(false);
        BoardGridObject.SetActive(true);
        CirclesObject.SetActive(true);
        ShapesObject.SetActive(true);
        TestingObject.SetActive(true);
    }

    public void showRecords()
    {
        StartMenuObject.SetActive(false);
        RecordsObject.SetActive(true);
        DisplayRecords(myDropdown.value);
    }

    public void StartHowToPlay()
    {
        HowToPlayObject.SetActive(true);
        resultBar.SetActive(false);
        nextLevelButton.SetActive(false);
        StartGame();
    }

    //end game - exit from apllication
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

    public void EndTutorial()
    {
        HowToPlay.boardLockFlag = false;
        HowToPlay.moveLockFlag = false;
        HowToPlay.tutorialFlag = false;
        HowToPlay.step = 1;
        resultBar.SetActive(true);
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

    // Modify the color of button to white if color is "white", in other cases turn button into gray
    public static void ChangeButtonColor(Button button, string color)
    {
        //getting into button component in button object from parameter
        Button buttonComponent = button.GetComponent<Button>();
        //getting into colorblock class when you can modify color of button
        ColorBlock colorBlock = buttonComponent.colors;

        if (color == "white")
        {
            colorBlock.normalColor = Color.white;//apply white color
            buttonComponent.colors = colorBlock;
        }
        else
        {
            colorBlock.normalColor = new Color(0.5f, 0.5f, 0.5f);//apply gray color
            buttonComponent.colors = colorBlock;
        }
    }

    public void CleanRecords()
    {
        Records.cleanRecords();
        DisplayRecords(myDropdown.value);
    }

    public void DisplayRecords(int index)
    {
        int[] levelScores = Records.getBestResult(index+1);
        string recordsAll = "";
        for (int i = 0; i < levelScores.Length; i++)
        {
            if (levelScores[i] == 1000)
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
    }
}
