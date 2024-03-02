using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//Main program of the game
public class Testing : MonoBehaviour
{
    //gameobject assigned in inspector so they can be changed throuh script
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
    public GameObject recordTimeText;
    public GameObject timeText;

    //auxiliary classes
    public Grid grid;//manages matrix with values which defines pawns arrangment on board                    
    public Circles circle;//manage sprites of circles which are portaing pawns on board
    public Levels thisLevel;//defines data of every level of the game such as initial pawns position, current shape or force pattern on board
    
    //auxiliary variables
    private int x, y, value1, value2;               
    private bool first8 = true;//flag, if was something already selected after confirmation or restart then false
    private bool zatwierdz = false;//flag, if was already confirmed by confirm button then true
    private bool zaznaczone = false;//if something was already selected in adding mode on then true
    private bool zaznaczone32 = false;//if something was already selected in swapping mode on then true
    private bool zaznaczone56 = false;//if 2 pawns were already selected in swapping mode on then true
    private Vector3 worldPositionNew;//position in the board
    private int x8, y8, x32, y32, x56, y56;//variables for coordinates of pawns on the board
    private bool zamien = false;//if we are in swapping mode then true
    private bool dodaj = true;//if we are in adding mode then true

    public int count = 0;//counter of clicks
    private bool[] shapesFound = new bool[12];//table to store which shapes are arranged on board for player (from black-gray pawns)
    private bool[] EnshapesFound = new bool[12];//table to store which shapes are arranged on board for enemy (from white-gray pawns)

    public int Level;
    public int Difficulty;//number of black pawns on the board, deafult is 2
    public float timer;
    public float wholeSeconds;//timer rounded to whole seconds

    void Start()
    {
        //setting game for the first time
        Level = 1;
        Difficulty = 2;
        timer = 0f;
        wholeSeconds = 0f;

        Levels.DrawShapesOrder();//draw order of shapes in game
        thisLevel = new Levels(Difficulty, Level);
        grid = new Grid(7, 7, 8f);
        circle = new Circles(CirclesObject, grid.cellSize);
        Records.loadJSON();//load data of current records

        SetLeveL();//standard function to prepare board for new level 
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && HowToPlay.boardLockFlag == false)//if LMB is pressed and interaction isn't turned off
        {
            Vector3 mousePosition = Input.mousePosition;                      //get mouse position
                                                                              //mouse position is definied by position in screen
            worldPositionNew = Camera.main.ScreenToWorldPoint(mousePosition); //convert position on screen into position in unity,
                                                                              //then it would be easy to convert it to position on board 

            if (grid.GetValue(worldPositionNew) == 8 && dodaj)          //if you clicked on empty field on the board and you are in adding mode
            {
                if (!first8)//clean previoius selection (unless there was nothing selected earlier)                                            
                {
                    circle.HideSelected(CirclesObject, false);
                }
                grid.GetXY(worldPositionNew, out x, out y);      
                x8 = x;                                                        
                y8 = y;
                circle.ShowSelected(CirclesObject, false, x8, y8);//show selection circle in given coordinates
                first8 = false;                                          
                zaznaczone = true;                                      
            }

            if (grid.GetValue(worldPositionNew) > 8 && zamien)             //if pawn is pressed and we are in swapping mode
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
                }//if pressed pawn has been already first selected pawn then unselect it, second turn into the first
                else if (x == x56 && y == y56)                               
                {
                    circle.HideSelected(CirclesObject, false);
                    x56 =0;
                    y56=0;
                    zaznaczone56 = false;
                }//if pressed pawn has been already second selected pawn then unselect it
                else if (!zaznaczone32)                                         
                {
                    x32 = x;
                    y32 = y;
                    circle.ShowSelected(CirclesObject, true, x32, y32);
                    zaznaczone32 = true;
                }//if there are not selected pawns then select it 
                else if((!zaznaczone56))                                        
                {
                    x56 = x32;
                    y56 = y32;
                    x32 = x;
                    y32 = y;
                    circle.ShowSelected(CirclesObject, false, x56, y56);
                    circle.ShowSelected(CirclesObject, true, x32, y32);
                    zaznaczone56 = true;
                }//if only one pawn have been selected then select it as second, pressed one is first
                else                                                           
                {
                    x56 = x32;
                    y56 = y32;
                    x32 = x;
                    y32 = y;
                    circle.ShowSelected(CirclesObject, true, x32, y32);
                    circle.ShowSelected(CirclesObject, false, x56, y56);
                }//if there were two selected before, unselect second, first becomes second, pressed one is first
            }

        }

        if (zatwierdz && zaznaczone)                                    //confirm adding pawn
        {
            if (HowToPlay.step == 2) HowToPlay.taskDoneFlag = true;
            StartCoroutine(DelayedPawnMove1());
        }

        if (zatwierdz && zaznaczone32 && zaznaczone56)                  //confirm swapping pawns
        {
            if (HowToPlay.step == 4) HowToPlay.taskDoneFlag = true;
            StartCoroutine(DelayedPawnMove2());   
        }

        if (Buttons.RestartFlag || Buttons.RestartLeveLFlag)    //restart game or restart only level
        {
            if (Buttons.RestartFlag)//restart game
            {
                Level = 1;
                count = -1;
                timer = 0f;
                wholeSeconds = 0f;
                updateText(timeText, "Czas: " + wholeSeconds.ToString());
                changeScoreText(Level);
                Levels.DrawShapesOrder();
            }
            if (HowToPlay.step == 10) HowToPlay.taskDoneFlag = true;
            restartF();
            Buttons.RestartFlag = false;
            Buttons.RestartLeveLFlag = false;
        }

        if (Buttons.ChangeModeFlag)     //change mode of game (between adding and swapping)
        {
            if (HowToPlay.step == 3) HowToPlay.taskDoneFlag = true;
            Buttons.ChangeModeFlag = false;
            zmianatrybu();
        }

        if (Buttons.ConfirmFlag)    //button confirm actives this funtcion after pressing
        {
            Buttons.ConfirmFlag = false;
            if (zaznaczone || (zaznaczone32 && zaznaczone56))
            {
                zatwierdz = true;
            }
        }

        // Update the timer, timer stops when both modes are turned off (betweeen levels)
        timer += Time.deltaTime;
        if (timer >= wholeSeconds + 1f && (dodaj || zamien))
        {
            wholeSeconds = wholeSeconds + 1f;
            updateText(timeText, "Czas: " + wholeSeconds.ToString());
        }
    }

    private void SetLeveL()
    {
        circle.HideAllCircle(CirclesObject);
        circle.HideSelected(CirclesObject, true);
        circle.HideSelected(CirclesObject, false);

        grid.SetBoard(thisLevel.ForceArray, thisLevel.MarkerStartPosition);
        grid.AddPawns(thisLevel.PawnsTable);
        circle.AddPawns(thisLevel.PawnsTable, CirclesObject);
        ShapePosition.SetShapes(ShapesObject, Levels.ShapesOrder);
        ShapePosition.GetCurrentShapes(ShapesObject, Levels.ShapesOrder, Level);

        updateText(levelText, "Poziom: " + Level.ToString());
        changeScoreText(Level);
        timer = wholeSeconds;
        TrybDodaj();

        Array.Fill(shapesFound, false);
        Array.Fill(EnshapesFound, false);

        if(HowToPlay.tutorialFlag == false)
        {
            Buttons.activateButton(ModeButton);
            Buttons.activateButton(ConfirmButton);
            Buttons.activateButton(RestartButton);
            Buttons.deactivateButton(NextLevelButton);
        }

        DrawObject.SetActive(false);
        LvlWonText.SetActive(false);
        LvlLostText.SetActive(false);
        VictoryObject.SetActive(false);
    }//standard function to prepare board for new level 

    private void restartF()
    {
        thisLevel = new Levels(Difficulty, Level);
        SetLeveL();
        count++;
        updateText(counterText, "Licznik: " + count.ToString());
    }//function restarting the level

    private void LoadNextLeveL()
    {
        Level++;
        thisLevel = new Levels(Difficulty, Level);
        SetLeveL();
    }//function loading next level

    private void CheckResults()
    {
        if (shapesFound[thisLevel.ShapeNumber - 1] && !EnshapesFound[thisLevel.ShapeNumber - 1])//if you won
        {
            Records.updateRecords(Level, count, (int)wholeSeconds);//check if the score could be saved in records
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
            dodaj = false;
            zamien = false;
            Buttons.deactivateButton(ModeButton);
            Buttons.deactivateButton(ConfirmButton);
            Buttons.deactivateButton(RestartButton);
        }
        else if (!shapesFound[thisLevel.ShapeNumber - 1] && EnshapesFound[thisLevel.ShapeNumber - 1])//if you lost
        {
            LvlLostText.SetActive(true);
            Array.Fill(EnshapesFound, false);
            DrawObject.SetActive(false);
            dodaj = false;
            zamien = false;
            Buttons.deactivateButton(ModeButton);
            Buttons.deactivateButton(ConfirmButton);
        }
        else if (shapesFound[thisLevel.ShapeNumber - 1] && EnshapesFound[thisLevel.ShapeNumber - 1])//if it is draw
        {
            DrawObject.SetActive(true);
            Array.Fill(shapesFound, false);
            Array.Fill(EnshapesFound, false);
        }
        else //if nothing found
        {
            DrawObject.SetActive(false);
        }
    }//function checks if you won, lost, or if is draw and do accordingly to it

    private void zmianatrybu()
    {
        circle.HideSelected(CirclesObject, true);                       
        circle.HideSelected(CirclesObject, false);
        if (dodaj)                                                      //if there was adding mode
        {
            zamien = true;
            dodaj = false;
            updateText(modeText, "Tryb: Zamienianie");                      
            zaznaczone = false;
            first8 = true;
        }
        else                                                            //if there was swapping mode
        {
           zamien = false;                                         
           dodaj = true;
           updateText(modeText, "Tryb: Dodawanie");
           zaznaczone32 = false;
           zaznaczone56 = false;
        }
    }//function swapping the modes in game (between adding and swapping)

    private void TrybDodaj()
    {
        updateText(modeText, "Tryb: Dodawanie");
        zamien = false;
        dodaj = true;
        first8 = true;
    }//this function always turns on the adding mode

    private void changeScoreText(int level)
    {
        int[] bestClicks = Records.getBestResults(level, "clicks");
        int best = bestClicks[0];
        if (best >= Records.clicksLimit)
        {
            updateText(recordText, "Rekord: -");
        }
        else
        {
            updateText(recordText, "Rekord: " + best.ToString());
        }

        bestClicks = Records.getBestResults(level, "times");
        best = bestClicks[0];
        if (best >= Records.timesLimit)
        {
            updateText(recordTimeText, "Rekord: -");
        }
        else
        {
            updateText(recordTimeText, "Rekord: " + best.ToString());
        }
    }//update record texts in game to given level 

    private void updateText(GameObject gameObject, string newText)
    {
        Text textComponent = gameObject.GetComponent<Text>();
        textComponent.text = newText;
    }//general function for changing text in given gameObject

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
                count++;
                updateText(counterText, "Licznik: " + count.ToString());
                CheckResults();
            } 
        }

        yield break;
    }//procedure after confirmation in adding mode, IEnumerator was solution for fluent animation

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
                count++;
                updateText(counterText, "Licznik: " + count.ToString());
                CheckResults();
            }
        }

        yield break;
    }//procedure after confirmation in swapping mode, IEnumerator was solution for fluent animation

}
