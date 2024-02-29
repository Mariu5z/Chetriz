using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

//class to manage pawns of board 5x5, their visiblity, color, animation of pawn movement 
public class Circles
{
    private Vector3 localPosition;
    private GameObject gameObject;
    private Renderer renderer;          
    private float cellSize1;
    Color DarkGray = new Color(0.4f, 0.4f, 0.4f);

    //constructor, turns off visiblity of circles (pawns and selection circles) 
    public Circles(GameObject CirclesObject, float cellSize)
    {
        cellSize1 = cellSize;//assign field Size in board to computing in functions 

        //turns off visiblity of pawns, for 5x5 board
        for (int i = 1; i <= 5; i++)
        {
            for (int j = 1; j <= 5; j++)                                   
            {
                //find gameobject (within parent from parameter) with given name
                gameObject = CirclesObject.transform.Find("Circle " + i.ToString() + j.ToString()).gameObject;
                //deactive this gameobject
                gameObject.SetActive(false);
            }
        }

        //turns off visiblity of selection circles
        gameObject = CirclesObject.transform.Find("Selection Circle 1").gameObject;
        gameObject.SetActive(false);
        gameObject = CirclesObject.transform.Find("Selection Circle 2").gameObject;
        gameObject.SetActive(false);
    }

    //change color of pawn in selected place in board
    public void ChangeColorCircle(GameObject CirclesObject, int x, int y, string color)
    {
        //find object with given coordinates
        Transform childTransform = CirclesObject.transform.Find("Circle " + x.ToString() + y.ToString());
        //edit component within this object (to edit color of sprite)
        SpriteRenderer spriteRenderer = childTransform.GetComponent<SpriteRenderer>();
        //change color according to parameter
        if (color == "gray")
        {
            spriteRenderer.color = DarkGray;//color definied earlier
        }
        else if(color == "white")
        {
            spriteRenderer.color = Color.white;
        }
        else if (color == "black")
        {
            spriteRenderer.color = Color.black;
        }
        else//niepoprawny argument wejœciowy
        {
            UnityEngine.Debug.Log("Wrong Color");
        }
    }

    //hide pawns
    public void HideCircle(GameObject CirclesObject, int x, int y)
    {
        //find gameobject (within parent from parameter) with given name
        gameObject = CirclesObject.transform.Find("Circle " + x.ToString() + y.ToString()).gameObject;
        //deactive this gameobject
        gameObject.SetActive(false);
    }

    //unhide pawns
    public void ShowCircle(GameObject CirclesObject, int x, int y)
    {
        //find gameobject (within parent from parameter) with given name
        gameObject = CirclesObject.transform.Find("Circle " + x.ToString() + y.ToString()).gameObject;
        //active this gameobject
        gameObject.SetActive(true);
    }

    //show selection circle in given coordinates (isNeutral = false -> show selection circle 1)
    public void ShowSelected(GameObject CirclesObject, bool isNeutral, int x, int y)
    {
        if (!isNeutral)
        {
            gameObject = CirclesObject.transform.Find("Selection Circle 1").gameObject;
            //set location od circle
            localPosition.x = x * cellSize1 + cellSize1 / 2f;
            localPosition.y = y * cellSize1 + cellSize1 / 2f;
            gameObject.transform.localPosition = localPosition;
            //make visible
            gameObject.SetActive(true);
        }
        else
        {
            gameObject = CirclesObject.transform.Find("Selection Circle 2").gameObject;
            //set location od circle
            localPosition.x = x * cellSize1 + cellSize1 / 2f;
            localPosition.y = y * cellSize1 + cellSize1 / 2f;
            gameObject.transform.localPosition = localPosition;
            //make visible
            gameObject.SetActive(true);
        }
    }

    //hide selection cirlce (isNeutral = false -> show selection circle 1)
    public void HideSelected(GameObject CirclesObject, bool isNeutral)
    {
        if (!isNeutral)
        {
            gameObject = CirclesObject.transform.Find("Selection Circle 1").gameObject;
            gameObject.SetActive(false);
        }
        else
        {
            gameObject = CirclesObject.transform.Find("Selection Circle 2").gameObject;
            gameObject.SetActive(false);
        }
    }

    //show enemies pawns(white) in coordinates given in table
    public void AddEnemies(int[,] table, GameObject CirclesObject)
    {
        int enemyCount = table.GetLength(0);//how many pawns are to be added, how many rows have table
        int i;
        //unhide pawns and change color of pawn in selected place in board
        for (i = 0; i < enemyCount; i++)
        {
            ShowCircle(CirclesObject, table[i, 0], table[i, 1]);
            ChangeColorCircle(CirclesObject, table[i, 0], table[i, 1], "white");
        }
    }

    //show allies pawns(black) in coordinates given in table
    public void AddAllies(int[,] table, GameObject CirclesObject)
    {
        int allyCount = table.GetLength(0);
        int i;
        for (i = 0; i < allyCount; i++)
        {
            ShowCircle(CirclesObject, table[i, 0], table[i, 1]);
            ChangeColorCircle(CirclesObject, table[i, 0], table[i, 1], "black");
        }
    }

    //update visiblity of pawns for all 5x5 board
    //table has information what/if pawn is in specified coordinate
    public void AddPawns(int[,] table, GameObject CirclesObject)
    {
        for (int i=0; i<5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (table[i,j] == 16)//if is white pawn
                {
                    ShowCircle(CirclesObject, i+1, j+1);
                    ChangeColorCircle(CirclesObject, i+1, j+1, "white");
                }
                else if (table[i, j] == 56)//if is black pawn
                {
                    ShowCircle(CirclesObject, i+1, j+1);
                    ChangeColorCircle(CirclesObject, i+1, j+1, "black");
                }
                else if (table[i, j] == 32)//if is neutral pawn
                {
                    ShowCircle(CirclesObject, i + 1, j + 1);
                    ChangeColorCircle(CirclesObject, i + 1, j + 1, "gray");
                }
                else if (table[i, j] == 8)//if there is no pawn
                {
                    HideCircle(CirclesObject, i + 1, j + 1);
                }
            }
        }
    }

    //update visiblity of pawns in one row/column for all 5x5 board
    //which column/row is updating depend on map movement marker cordinate x, y
    //given as parameter table is current state of board matrix
    public void PawnsMove(GameObject CirclesObject, int x, int y, int[,] table)
    {
        int i;

        if (x == 0)                                                     //left side
        {
            for (i = 1; i <= 5; i++)
            {
                if (table[i, y] == 8)                 //empty  
                {
                    HideCircle(CirclesObject, i, y);
                }
                else if (table[i, y] == 16)           //enemy - white
                {
                    ShowCircle(CirclesObject, i, y);
                    ChangeColorCircle(CirclesObject, i, y, "white");
                }
                else if (table[i, y] == 32)           //neutral - gray
                {
                    ShowCircle(CirclesObject, i, y);
                    ChangeColorCircle(CirclesObject, i, y, "gray");
                }
                else if (table[i, y] == 56)           //ally - black
                {
                    ShowCircle(CirclesObject, i, y);
                    ChangeColorCircle(CirclesObject, i, y, "black");
                }
            }
        }
        else if (x == 6)                                                     //right side
        {
            for (i = 1; i <= 5; i++)
            {
                if (table[i, y] == 8)                   
                {
                    HideCircle(CirclesObject, i, y);
                }
                else if (table[i, y] == 16)               
                {
                    ShowCircle(CirclesObject, i, y);
                    ChangeColorCircle(CirclesObject, i, y, "white");
                }
                else if (table[i, y] == 32)                 
                {
                    ShowCircle(CirclesObject, i, y);
                    ChangeColorCircle(CirclesObject, i, y, "gray");
                }
                else if (table[i, y] == 56)                  
                {
                    ShowCircle(CirclesObject, i, y);
                    ChangeColorCircle(CirclesObject, i, y, "black");
                }
            }
        }
        else if (y == 6)                                                    //top side                                                 
        {
            for (i = 1; i <= 5; i++)
            {
                if (table[x, i] == 8)                  
                {
                    HideCircle(CirclesObject, x, i);
                }
                else if (table[x, i] == 16)           
                {
                    ShowCircle(CirclesObject, x, i);
                    ChangeColorCircle(CirclesObject, x, i, "white");
                }
                else if (table[x, i] == 32)          
                {
                    ShowCircle(CirclesObject, x, i);
                    ChangeColorCircle(CirclesObject, x, i, "gray");
                }
                else if (table[x, i] == 56)           
                {
                    ShowCircle(CirclesObject, x, i);
                    ChangeColorCircle(CirclesObject, x, i, "black");
                }
            }
        }
        else if (y == 0)                                                     //bottom side
        {
            for (i = 1; i <= 5; i++)
            {
                if (table[x, i] == 8)                
                {
                    HideCircle(CirclesObject, x, i);
                }
                else if (table[x, i] == 16)           
                {
                    ShowCircle(CirclesObject, x, i);
                    ChangeColorCircle(CirclesObject, x, i, "white");
                }
                else if (table[x, i] == 32)           
                {
                    ShowCircle(CirclesObject, x, i);
                    ChangeColorCircle(CirclesObject, x, i, "gray");
                }
                else if (table[x, i] == 56)           
                {
                    ShowCircle(CirclesObject, x, i);
                    ChangeColorCircle(CirclesObject, x, i, "black");
                }
            }

        }
    }

    //hide all pawns in board 5x5
    public void HideAllCircle(GameObject CirclesObject)
    {
        for (int x = 1; x <= 5; x++)
        {
            for (int y = 1; y <= 5; y++)
            {
                gameObject = CirclesObject.transform.Find("Circle " + x.ToString() + y.ToString()).gameObject;
                gameObject.SetActive(false);
            }
        }

    }

    //animate pawn movement on the board (for 5x5 board)
    //given map movement marker position (x, y) and its force (how far paws are moving)
    //given as parameter table is current state of board matrix
    public IEnumerator PawnsAnimation(GameObject CirclesObject, int x, int y, int force, int[,] table)
    {
        float desiredDistance = (float)force * cellSize1;   //distance to travel by pawns
        float velocityValue = 20f;                          //velocity of pawns
        bool verticalMove = false;                          //is movement is vertical (or horizontal)
        
        int i;                                              //counter
        Vector3[] targetPosition = new Vector3[5];          //target position (after movement)
        Vector3[] startPosition = new Vector3[5];           //start position (before movement

        //check if movement will be vertical or horizontal
        if (y == 0 || y == 6)                              
        {
            verticalMove = true;
        }
        else
        {
            verticalMove = false;
        }

        //loop defining start and target positions of moving pawns (5 pawns for 5x5 board)
        for (i = 0; i < 5; i++)              
        {
            if (y == 0)//bottom side
            {
                Transform childTransform = CirclesObject.transform.Find("Circle " + x.ToString() + (i + 1).ToString()); //find pawn by name
                startPosition[i] = childTransform.position;                                                             //its position is start position
                targetPosition[i] = startPosition[i] + new Vector3(0f, desiredDistance, 0f);                            //counting target position
            }
            else if (y == 6)//top side
            {
                Transform childTransform = CirclesObject.transform.Find("Circle " + x.ToString() + (i + 1).ToString());
                startPosition[i] = childTransform.position;
                targetPosition[i] = startPosition[i] - new Vector3(0f, desiredDistance, 0f);
            }
            else if (x == 0)//left side
            {
                Transform childTransform = CirclesObject.transform.Find("Circle " + (i + 1).ToString() + y.ToString());
                startPosition[i] = childTransform.position;
                targetPosition[i] = startPosition[i] + new Vector3(desiredDistance, 0f, 0f);
            }
            else if (x == 6)//right side
            {
                Transform childTransform = CirclesObject.transform.Find("Circle " + (i + 1).ToString() + y.ToString());
                startPosition[i] = childTransform.position;
                targetPosition[i] = startPosition[i] - new Vector3(desiredDistance, 0f, 0f);
            }

            //UnityEngine.Debug.Log("targetPosition: " + targetPosition[i].ToString());
        }

        float duration = desiredDistance / velocityValue;   //set animation time in seconds (approximately)
        float elapsedTime = 0f;                             //real animation time (counter)

        //animation last until time counter reach set animation time
        while (elapsedTime < duration)                      
        {
            float t = elapsedTime / duration;               //what fraction of time passed 
                                                            //this fraction is later used in calculations of current position
            //animation loop
            for (i = 1; i < 6; i++)                         
            {
                //find pawn to be moved by name
                Transform childTransform;
                if (verticalMove)
                {
                    childTransform = CirclesObject.transform.Find("Circle " + x.ToString() + i.ToString());
                 }  
                else
                {
                    childTransform = CirclesObject.transform.Find("Circle " + i.ToString() + y.ToString());
                }

                //calculate current position of pawn
                //lerp function calculate point between two points with set ratio
                childTransform.position = Vector3.Lerp(startPosition[i - 1], targetPosition[i - 1], t);

                //if pawn goes beyond 5x5 board it appears on the opposide side of board
                if (childTransform.position.x > 20f)                                                   
                {
                    childTransform.position = childTransform.position - new Vector3(40f, 0f, 0f);
                }
                else if (childTransform.position.x < -20f)
                {
                    childTransform.position = childTransform.position + new Vector3(40f, 0f, 0f);
                }
                else if (childTransform.position.y > 20f)
                {
                    childTransform.position = childTransform.position - new Vector3(0f, 40f, 0f);
                }
                else if (childTransform.position.y < -20f)
                {
                    childTransform.position = childTransform.position + new Vector3(0f, 40f, 0f);
                }
            }
            elapsedTime += Time.deltaTime;//add more time to counter which passed from the last frame
            yield return null;//this command update visually position of the pawns
        }
        
        ResetPawnsPosition(CirclesObject);//after loop pawns return to their start position, this function is only for animation
        PawnsMove(CirclesObject, x, y, table);//update visiblity of pawns in one row/column for all 5x5 board
        yield return null;//again and finally update visiblity of the pawns
        yield break;//end this function
    }

    //place all the pawns in their places respectively to their names
    public void ResetPawnsPosition(GameObject CirclesObject)
    {
        for (int i = 1; i < 6; i++)
            for (int j = 1; j < 6; j++)
            {
                {
                    Transform childTransform = CirclesObject.transform.Find("Circle " + i.ToString() + j.ToString());
                    childTransform.position = new Vector3(8f * i + 4f - 28f, 8f * j + 4f -28f, 0f);  
                }
            }
    }
}

