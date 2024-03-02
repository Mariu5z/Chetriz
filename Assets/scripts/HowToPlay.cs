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
        {1, "Witaj w Samouczku\n" + "Kliknij DALEJ aby zaczπÊ szkolenie"},//ok
        {2, "Aby dodaÊ szarego pionka kliknij w dowolne, wolne pole na planszy, a nastÍpnie kliknij ZATWIERDè"},//jednorazowe
        {3, "Jest jeszcze tryb zamieniania miejscami pionkÛw, aby na niego przejúÊ kliknij ZMIE— TRYB"},//jednorazowe
        {4, "Kliknij na dowolne dwa pionki i kliknij ZATWIERDè, aby zamieniÊ je miejscami"},//jednorazowe
        {5, "Zadaniem gracza jest u≥oøenie kszta≥tu na dole ekranu. W Pasku poniøej sπ pozosta≥e kszta≥ty do u≥oøenia w kolejnych poziomach"},//dalej
        {6, "Gracz ma u≥oøyÊ ten kszta≥t z czarnych i szarych pionkÛw. Jeúli kszta≥t u≥oøy siÍ szybciej z bia≥ych i szarych pionkÛw gracz przegrywa"},//dalej
        {7, "Jeúli kszta≥t czarno-szary i bia≥o-szary u≥oøπ siÍ jednoczeúnie, gra trwa aø ktÛraú strona przewaøy"},//dalej
        {8, "Ksza≥t z pionkÛw na planszy moøe byÊ dowolnie zorientowany oraz lustrzanie odbity"},//dalej
        {9, "Z kaødym kolejnym poziomem liczba bia≥ych pionkÛw roúnie, wiÍc roúnie rÛwnieø trudnoúÊ"},//dalej
        {10, "Moøesz zrestartowaÊ poziom klikajπc przycisk POWT”RZ, usuwa on wszystkie szare pionki i losuje nowe pozycje czarnych i bia≥ych pionkÛw"},//jednorazowe
        {11, "GrÍ utrudnia to, øe pionki na mapie sπ ruchome, ruch nastÍpuje od razu po dodaniu lub zamianie pionkÛw"},//dalej
        {12, "Widzisz liczby dooko≥a planszy? Jeden z nich jest czerwony, to znacznik ruchu mapy"},//dalej
        {13, "Pozycja znacznika okreúla w ktÛrym rzÍdzie pionki bÍdπ odpychane, a jego wartoúÊ na jak daleko" },//dalej
        {14, "Dodaj nowe pionki na plansze, aby lepiej zrozumieÊ algorytm ruchu, zauwaø øe znacznik przesuwa siÍ przeciwnie do wskazÛwek zegara" },//wielorazowy, sandbox
        {15, "MyúlÍ øe jesteú gotowy na grÍ, czy uda ci siÍ wygraÊ 12 poziomÛw? ile bÍdziesz potrzebowa≥ klikniÍÊ? WYJDè z samouczka i sprawdü!" }
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
