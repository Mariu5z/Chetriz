using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class Testing : MonoBehaviour
{
    //tworzenie zmiennych typu obiekty do których mo¿na siê potam odwo³ywaæ w inspektorze
    public GameObject LvlWonText;
    public GameObject LvlLostText;
    public GameObject DrawObject;
    public GameObject VictoryObject;
    public GameObject CirclesObject;
    public GameObject ShapesObject;    
    public Button RestartButton;
    public Button ModeButton;
    public Button ConfirmButton;
    public Button NextLevelButton;
    public GameObject recordText;
    public GameObject levelText;
    public GameObject counterText;
    public GameObject modeText;

    //tworzenie obiektów stworzonych klas pomocnicznych
    public Grid grid;//zawiera wartoœci tabeli wskazuj¹ce jak u³o¿one s¹ pionki, równie¿ rozmiary tabeli                     
    public Circles circle;//zawiera graficzne kó³ka przedstawiaj¹ce pionki i steruje nimi
    public Levels thisLevel;//zawiera dane dla ka¿dego poziomu, losuje siê w nim ustawienia na pocz¹tku poziomu i kszta³t
    
    //tworzenie pomocniczych zmiennych, w tym flag
    private int x, y, value1, value2;               
    private bool first8 = true;//czy coœ ju¿ by³o postawione w trybie dodawania
    private bool zatwierdz = false;//czy zatwierdzono
    private bool zaznaczone = false;//czy ju¿ coœ w trybie dodawanie
    private bool zaznaczone32 = false;//czy zaznaczono pierwszy pion w trybie zamienianie
    private bool zaznaczone56 = false;
    private Vector3 worldPositionNew;//zmienna do pozycji naciœniêcia
    private int x8, y8, x32, y32, x56, y56;//zmienne do wspó³rzêdnych zaznaczonych
    private bool zamien = false;//czy jesteœmy w trybie zamien
    private bool dodaj = true;

    private int count = 0;//licznik klikniec
    private bool[] shapesFound = new bool[12];//tabela trafieñ kszta³tów dla gracza
    private bool[] EnshapesFound = new bool[12];//tabela trafieñ kszta³tów dla przeciwnika

    private int Level;//zmienna mówi który level
    public int Difficulty;//ilosc pionkow gracza, 1-³atwy, 2-œredni, 3-trudny

    void Start()
    {
        Level = 1;//ustawienia pocz¹tkowe gry
        Difficulty = 2;

        Levels.DrawShapesOrder();//losuj kolejnoœæ kszta³tów w grze
        thisLevel = new Levels(Difficulty, Level);
        grid = new Grid(7, 7, 8f);
        circle = new Circles(CirclesObject, grid.cellSize);
        ShapePosition.SetShapes(ShapesObject, Levels.ShapesOrder);
        Records.loadJSON();
        SetLeveL();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && HowToPlay.boardLockFlag == false)                                                    //GDY LPM  jest nacisniety
        {
            Vector3 mousePosition = Input.mousePosition;                      //pobierz pozycje myszy do lokalnej zmiennej
                                                                              //pozycja myszy jest wstêpnie okreœlona wed³ug po³o¿enia na ekranie
            worldPositionNew = Camera.main.ScreenToWorldPoint(mousePosition); //konwersja pozycji myszy na pozycje w œwiecie w unity

            if (grid.GetValue(worldPositionNew) == 8 && dodaj)          //jesli klikna³eœ na puste pole i dodajesz pionka
            {
                if (!first8)                                            //wyczyœæ zaznaczenie ale nie dla pierwszego zaznaczenia ani zaraz po zatwierdzeniu 
                {
                    circle.HideSelected(CirclesObject, false);
                }
                grid.GetXY(worldPositionNew, out x, out y);             //wspó³rzêdne zaznaczonego pola
                x8 = x;                                                 //zapisz wspó³rzêdne        
                y8 = y;
                circle.ShowSelected(CirclesObject, false, x8, y8);

                first8 = false;                                          //juz nie pierwsze zaznaczenie
                zaznaczone = true;                                      //zaznaczenie odnotowane
            }

            if (grid.GetValue(worldPositionNew) > 8 && zamien)             //jesli zaznaczono pionek
            {
                grid.GetXY(worldPositionNew, out x, out y);
                if ( x == x32 && y == y32 )                                 
                {
                    x32 = x56;
                    y32 = y56;
                    x56 = 0;
                    y56 = 0;
                    if (zaznaczone56)
                    {
                        circle.ShowSelected(CirclesObject, true, x32, y32);
                        circle.HideSelected(CirclesObject, false);
                    }
                    else
                    {
                        circle.HideSelected(CirclesObject, true);
                    }
                    zaznaczone32 = zaznaczone56;
                    zaznaczone56 = false;
                }//jesli zaznaczony pionek byl juz zaznaczony to odznacz, drugi staje siê pierwszym
                else if (x == x56 && y == y56)                               
                {
                    circle.HideSelected(CirclesObject, false);
                    x56 =0;
                    y56=0;
                    zaznaczone56 = false;
                }//jesli zaznaczony pionek byl juz zaznaczony (ten drugi) to odznacz
                else if (!zaznaczone32)                                         
                {
                    x32 = x;
                    y32 = y;
                    circle.ShowSelected(CirclesObject, true, x32, y32);
                    zaznaczone32 = true;
                }//jesli nie ma zanznaczonych pionków to zaznacz
                else if((!zaznaczone56))                                        
                {
                    x56 = x32;
                    y56 = y32;
                    x32 = x;
                    y32 = y;
                    circle.ShowSelected(CirclesObject, false, x56, y56);
                    circle.ShowSelected(CirclesObject, true, x32, y32);
                    zaznaczone56 = true;
                }//jesli tylko jeden pionek byl zaznaczony to juz bêd¹ dwa, pierwszy staje siê drugim
                else                                                           
                {
                    x56 = x32;
                    y56 = y32;
                    x32 = x;
                    y32 = y;
                    circle.ShowSelected(CirclesObject, true, x32, y32);
                    circle.ShowSelected(CirclesObject, false, x56, y56);
                }//jesli by³y ju¿ dwa zaznaczone inne pionki to odznacz drugiego, przesuñ pierwszego na drugie, zaznacz nowy
            }

        }

        if (zatwierdz && zaznaczone)                                    //zatwierdzanie dodania pionka
        {
            if (HowToPlay.step == 2) HowToPlay.taskDoneFlag = true;
            StartCoroutine(DelayedPawnMove1());
        }

        if (zatwierdz && zaznaczone32 && zaznaczone56)                  //zatwierdzanie zamiany
        {
            if (HowToPlay.step == 4) HowToPlay.taskDoneFlag = true;
            StartCoroutine(DelayedPawnMove2());   
        }

        if (Buttons.RestartFlag || Buttons.RestartLeveLFlag)
        {
            if (Buttons.RestartFlag)
            {
                Level = 1;
                count = -1;
                changeScoreText(Level);
                Levels.DrawShapesOrder();
            }
            if (HowToPlay.step == 10) HowToPlay.taskDoneFlag = true;
            restartF();
            Buttons.RestartFlag = false;
            Buttons.RestartLeveLFlag = false;
        }

        if (Buttons.ChangeModeFlag)
        {
            if (HowToPlay.step == 3) HowToPlay.taskDoneFlag = true;
            Buttons.ChangeModeFlag = false;
            zmianatrybu();
        }

        if (Buttons.ConfirmFlag)
        {
            Buttons.ConfirmFlag = false;

            if (zaznaczone || (zaznaczone32 && zaznaczone56))
            {
                zatwierdz = true;
            }
        }
    }

    private void SetLeveL()
    {
        grid.SetBoard(thisLevel.ForceArray, thisLevel.MarkerStartPosition);
        grid.AddPawns(thisLevel.PawnsTable);
        circle.AddPawns(thisLevel.PawnsTable, CirclesObject);
        ShapePosition.GetCurrentShapes(ShapesObject, Levels.ShapesOrder, Level);

        changeScoreText(Level);
        TrybDodaj();

        Array.Fill(shapesFound, false);//czyszczenie tablicy
        Array.Fill(EnshapesFound, false);

        if(HowToPlay.tutorialFlag == false)
        {
            Buttons.activateButton(ModeButton);
            Buttons.activateButton(ConfirmButton);
            Buttons.activateButton(RestartButton);
            Buttons.deactivateButton(NextLevelButton);
        }
        DrawObject.SetActive(false);
    }

    private void restartF()
    {
        LvlWonText.SetActive(false);
        LvlLostText.SetActive(false);
        VictoryObject.SetActive(false);

        circle.HideAllCircle(CirclesObject);
        circle.HideSelected(CirclesObject, true);
        circle.HideSelected(CirclesObject, false);

        ShapePosition.SetShapes(ShapesObject, Levels.ShapesOrder);

        ChangeLevelWriting();
        
        thisLevel = new Levels(Difficulty, Level);
        SetLeveL();

        ChangeCount();
    }

    private void LoadNextLeveL()
    {
        Level++;
        ChangeLevelWriting();

        LvlWonText.SetActive(false);
        LvlLostText.SetActive(false);
        DrawObject.SetActive(false);

        circle.HideAllCircle(CirclesObject);
        circle.HideSelected(CirclesObject, true);
        circle.HideSelected(CirclesObject, false);

        thisLevel = new Levels(Difficulty, Level);
        SetLeveL();
    }

    private void CheckResults()
    {
        if (shapesFound[thisLevel.ShapeNumber - 1] && !EnshapesFound[thisLevel.ShapeNumber - 1])
        {
            Records.updateRecords(Level, count);//sprawdz i ewentualnie nadpisz rekord
            if (Level == 12)
            {
                VictoryObject.SetActive(true);
            }
            else
            {
                LvlWonText.SetActive(true); 
                Buttons.activateButton(NextLevelButton);
            }
            Array.Fill(shapesFound, false);
            DrawObject.SetActive(false);
            dodaj = false;                          //nieaktywne zaznaczanie
            zamien = false;
            Buttons.deactivateButton(ModeButton);
            Buttons.deactivateButton(ConfirmButton);
            Buttons.deactivateButton(RestartButton);
        }//wygrales
        else if (!shapesFound[thisLevel.ShapeNumber - 1] && EnshapesFound[thisLevel.ShapeNumber - 1])
        {
            LvlLostText.SetActive(true);
            Array.Fill(EnshapesFound, false);
            DrawObject.SetActive(false);
            //Debug.Log("Przegrales");
            dodaj = false;
            zamien = false;
            Buttons.deactivateButton(ModeButton);
            Buttons.deactivateButton(ConfirmButton);
        }//przegrales
        else if (shapesFound[thisLevel.ShapeNumber - 1] && EnshapesFound[thisLevel.ShapeNumber - 1])
        {
            DrawObject.SetActive(true);
            Array.Fill(shapesFound, false);
            Array.Fill(EnshapesFound, false);
        }//remis
        else
        {
            DrawObject.SetActive(false);
        }
    }

    private void zmianatrybu()                                           //obs³uga przycsku zmiany trybu gry po nacisnieciu
    {
        Text textComponent = modeText.GetComponent<Text>();

        circle.HideSelected(CirclesObject, true);                                           //schowaj zaznaczenia
        circle.HideSelected(CirclesObject, false);

        if (dodaj)                                                      //jesli byl tryb dodawania
        {
            zamien = true;
            dodaj = false;
            textComponent.text = "Tryb: Zamienianie";                         
            zaznaczone = false;
            first8 = true;
        }
        else                                                            //jesli jest tryb zamiany
        {
           zamien = false;                                         
           dodaj = true;
           textComponent.text = "Tryb: Dodawanie";
           zaznaczone32 = false;
           zaznaczone56 = false;
        }
    }

    private void TrybDodaj()
    {
        Text textComponent = modeText.GetComponent<Text>();
        zamien = false;
        dodaj = true;
        textComponent.text = "Tryb: Dodawanie";
        first8 = true;
    }

    private void ChangeCount()
    {
        count++;
        Text textComponent = counterText.GetComponent<Text>();
        textComponent.text = "Licznik: " + count.ToString();
    }

    private void ChangeLevelWriting()
    {
        Text textComponent = levelText.GetComponent<Text>();
        textComponent.text = "Poziom: " + Level.ToString();
    }

    private void changeScoreText(int level)
    {
        int[] levelScores = Records.getBestResult(level);
        int best = levelScores[0];

        Text textComponent = recordText.GetComponent<Text>();
        if (best >= 1000)
        {
            textComponent.text = "Rekord: -";
        }
        else
        {
            textComponent.text = "Rekord: " + best.ToString();
        }
    }

    private IEnumerator DelayedPawnMove1()
    {
        zatwierdz = false;
        first8 = true;
        zaznaczone = false;
        yield return null;

        circle.HideSelected(CirclesObject, false);

        grid.SetValue(x8, y8, 32);
        circle.ShowCircle(CirclesObject, x8, y8);
        circle.ChangeColorCircle(CirclesObject, x8, y8, "gray");

        if (HowToPlay.moveLockFlag == false)
        {
            yield return new WaitForSeconds(0.5f);

            grid.MarkerPosition(out x, out y);
            grid.PawnsMove(thisLevel.ForceArray);
            StartCoroutine(circle.PawnsAnimation(CirclesObject, x, y, grid.MarkerValue(thisLevel.ForceArray), grid.gridArray));
            grid.MarkerMove();
            if (HowToPlay.tutorialFlag == false)
            {
                shapesFound = Shapes.shapes(grid.gridArray55);
                EnshapesFound = Shapes.EnShapes(grid.gridArray55);
                ChangeCount();
                CheckResults();
            } 
        }

        yield break;
    }

    private IEnumerator DelayedPawnMove2()
    {
        circle.HideSelected(CirclesObject, true);
        circle.HideSelected(CirclesObject, false);
        value1 = grid.GetValueXY(x32, y32);
        value2 = grid.GetValueXY(x56, y56);
        grid.SetValue(x32, y32, value2);
        grid.SetValue(x56, y56, value1);

        if (value2 == 16)
        {
            circle.ChangeColorCircle(CirclesObject, x32, y32, "white");
        }
        else if (value2 == 32)
        {
            circle.ChangeColorCircle(CirclesObject, x32, y32, "gray");
        }
        else if (value2 == 56)
        {
            circle.ChangeColorCircle(CirclesObject, x32, y32, "black");
        }
        if (value1 == 16)
        {
            circle.ChangeColorCircle(CirclesObject, x56, y56, "white");
        }
        else if (value1 == 32)
        {
            circle.ChangeColorCircle(CirclesObject, x56, y56, "gray");
        }
        else if (value1 == 56)
        {
            circle.ChangeColorCircle(CirclesObject, x56, y56, "black");
        }

        zatwierdz = false;
        zaznaczone32 = false;
        zaznaczone56 = false;
        x32 = 0; y32 = 0;
        x56 = 0; y56 = 0;
        yield return null;

        if (HowToPlay.moveLockFlag == false)
        {
            yield return new WaitForSeconds(0.5f);

            grid.MarkerPosition(out x, out y);
            grid.PawnsMove(thisLevel.ForceArray);
            StartCoroutine(circle.PawnsAnimation(CirclesObject, x, y, grid.MarkerValue(thisLevel.ForceArray), grid.gridArray));
            grid.MarkerMove();
            if (HowToPlay.tutorialFlag == false)
            {
                shapesFound = Shapes.shapes(grid.gridArray55);
                EnshapesFound = Shapes.EnShapes(grid.gridArray55);
                ChangeCount();
                CheckResults();
            }
        }

        yield break;
    }



}
