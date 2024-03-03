using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    public Button ModeButton;
    public Button ConfirmButton;
    public Button RestartButton;
    public Button NextLessonButton;
    public Text progressText;
    public Text lessonText;
    public static int step = 1;


    Dictionary<int, string> lessons = new Dictionary<int, string>
    {
        {1, "Welcome in tutorial\n" + "Press NEXT to start the training"},//ok
        {2, "To add a gray pawn click on any empty space on the board and then click CONFIRM"},//jednorazowe
        {3, "Apart from adding mode there is also a pawn swapping mode, to switch to it click CHANGE MODE"},//jednorazowe
        {4, "Select any two pawns by clicking on them, subsequently click CONFIRM to swap them"},//jednorazowe
        {5, "The player's task is to form the shape displayed below the board using pawns"},//dalej
        {6, "The player has to create this shape using black and gray pawns. If the white and gray pawns form the shape faster, the player loses"},//dalej
        {7, "If the black-gray and white-gray shapes are formed at the same time, it is a draw and game goes on"},//dalej
        {8, "The shape composed of the pawns can be oriented in any direction and mirrored"},//dalej
        {9, "With each subsequent level, the number of white pawns increases, so the the game gets harder"},//dalej
        {10, "You can restart the level by clicking the REPEAT button, it removes all gray pawns and randomizes new positions of black and white ones"},//jednorazowe
        {11, "The game is more difficult by the fact that the pawns on the map are movable, the movement occurs immediately after adding or replacing"},//dalej
        {12, "See the numbers around the board? One of them is red, it is the map movement marker"},//dalej
        {13, "The position of the marker determines in which row the pawns will be pushed, and its value to how far" },//dalej
        {14, "Add new pawns to the board to better understand the movement algorithm, notice that the marker moves counterclockwise" },//wielorazowy, sandbox
        {15, "I think you are ready to play, can you win 12 levels? EXIT the tutorial and check it out!" }
    };//information text for every step in tutorial
    HashSet<int> ModeButtonActive = new HashSet<int>  { 3, 14 };//in which steps mode button is active
    HashSet<int> ConfirmButtonActive = new HashSet<int> { 2, 4, 14 };
    HashSet<int> RestartButtonActive = new HashSet<int> { 10 };
    HashSet<int> NextLessonButtonDeactive = new HashSet<int> { 2, 3, 4, 10, 15 };
    HashSet<int> boardLock = new HashSet<int> { 2, 4, 14 };//in which steps board is selectable
    HashSet<int> moveLock = new HashSet<int> { 14 };//in which steps pawns are movins after confirmation
    public static bool boardLockFlag = false;
    public static bool moveLockFlag = false;
    public static bool taskDoneFlag = false;
    public static bool tutorialFlag = false;

    void Update()
    {
        if (Buttons.NextLessonFlag == true || taskDoneFlag == true)//go to the next lesson (next step of tutorial)
        {
            Buttons.NextLessonFlag = false;
            taskDoneFlag = false;
            step++;
            StartLesson();
        }
        if (Buttons.RestartTutorialFlag == true)//start tutorial
        {
            step = 1;
            StartLesson();
            tutorialFlag = true;
            Buttons.RestartTutorialFlag = false;
        }
    }

    public void StartLesson()//set everything for current step in tutorial
    {
        Text textComponent = progressText.GetComponent<Text>();
        textComponent.text = step.ToString() + " / " + lessons.Count.ToString();

        textComponent = lessonText.GetComponent<Text>();
        textComponent.text = lessons[step];

        if (ModeButtonActive.Contains(step)) Buttons.activateButton(ModeButton);
        else Buttons.deactivateButton(ModeButton);
        if (ConfirmButtonActive.Contains(step)) Buttons.activateButton(ConfirmButton);
        else Buttons.deactivateButton(ConfirmButton);
        if (RestartButtonActive.Contains(step)) Buttons.activateButton(RestartButton);
        else Buttons.deactivateButton(RestartButton);
        if (NextLessonButtonDeactive.Contains(step)) Buttons.deactivateButton(NextLessonButton);
        else Buttons.activateButton(NextLessonButton);

        if (boardLock.Contains(step)) boardLockFlag = false;
        else boardLockFlag = true;
        if (moveLock.Contains(step)) moveLockFlag = false;
        else moveLockFlag = true;
    }

}
