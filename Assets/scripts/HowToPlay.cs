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
        {1, "Witaj w Samouczku\n" + "Kliknij DALEJ aby zacz�� szkolenie"},//ok
        {2, "Aby doda� szarego pionka kliknij w dowolne, wolne pole na planszy, a nast�pnie kliknij ZATWIERD�"},//jednorazowe
        {3, "Jest jeszcze tryb zamieniania miejscami pionk�w, aby na niego przej�� kliknij ZMIE� TRYB"},//jednorazowe
        {4, "Kliknij na dowolne dwa pionki i kliknij ZATWIERD�, aby zamieni� je miejscami"},//jednorazowe
        {5, "Zadaniem gracza jest u�o�enie kszta�tu na dole ekranu. W Pasku poni�ej s� pozosta�e kszta�ty do u�o�enia w kolejnych poziomach"},//dalej
        {6, "Gracz ma u�o�y� ten kszta�t z czarnych i szarych pionk�w. Je�li kszta�t u�o�y si� szybciej z bia�ych i szarych pionk�w gracz przegrywa"},//dalej
        {7, "Je�li kszta�t czarno-szary i bia�o-szary u�o�� si� jednocze�nie, gra trwa a� kt�ra� strona przewa�y"},//dalej
        {8, "Ksza�t z pionk�w na planszy mo�e by� dowolnie zorientowany oraz lustrzanie odbity"},//dalej
        {9, "Z ka�dym kolejnym poziomem liczba bia�ych pionk�w ro�nie, wi�c ro�nie r�wnie� trudno��"},//dalej
        {10, "Mo�esz zrestartowa� poziom klikaj�c przycisk POWT�RZ, usuwa on wszystkie szare pionki i losuje nowe pozycje czarnych i bia�ych pionk�w"},//jednorazowe
        {11, "Gr� utrudnia to, �e pionki na mapie s� ruchome, ruch nast�puje od razu po dodaniu lub zamianie pionk�w"},//dalej
        {12, "Widzisz liczby dooko�a planszy? Jeden z nich jest czerwony, to znacznik ruchu mapy"},//dalej
        {13, "Pozycja znacznika okre�la w kt�rym rz�dzie pionki b�d� odpychane, a jego warto�� na jak daleko" },//dalej
        {14, "Dodaj nowe pionki na plansze, aby lepiej zrozumie� algorytm ruchu, zauwa� �e znacznik przesuwa si� przeciwnie do wskaz�wek zegara" },//wielorazowy, sandbox
        {15, "My�l� �e jeste� gotowy na gr�, czy uda ci si� wygra� 12 poziom�w? ile b�dziesz potrzebowa� klikni��? WYJD� z samouczka i sprawd�!" }
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
