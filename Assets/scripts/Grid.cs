
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class to handling matrix of data placed in space, board is made from this matrix of data
//matrix of data has customizable row and columns number and size of cell
public class Grid
{
    public int width;//liczba kolumn siatki
    public int height;//liczba wierszy siatki
    public float cellSize;//wymiary jednej komorki siatki
    public int[,] gridArray;//tabela 2d wartoœci dla ka¿dej komórki w siatce
    public int[,] gridArray55;//tabela wartosci komórek siatki bez skrajnych kolumn i wierszy 

    //constructor creating 2d table and displaying it
    public Grid(int width, int height, float cellSize)         
    {
        //szerokoœæ, wysokoœæ i wymiar komórki w tworzonej siatce taka jak mówi¹ parametry konstruktora
        this.width = width;                                     
        this.height = height;
        this.cellSize = cellSize;

        //tabele 2d z wartoœciami siatki
        gridArray = new int[width, height];
        gridArray55 = new int[width-2, height-2];
                                     
        //podwójna pêtla wyœwietlaj¹ca wartoœci tabeli
        for (int x = 0; x < width; x++)                           
        {
            for (int y = 0; y < height; y++)
            {
                //wywo³anie funkcji wyœwietlaj¹cej wartoœci tabeli
                //komórka (0,0) bêdzie w lewym dolnym rogu siatki
                CreateWorldText("BoardGrid", " ", GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) / 2, 40, Color.white, TextAnchor.MiddleCenter, x.ToString(), y.ToString());
            }
        }
    }

    //create and return TextMesh object
    public TextMesh CreateWorldText(string parentName, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, string x, string y)
    {
        //find parent where you put in text object
        GameObject parent = GameObject.Find(parentName);

        //creating object, setting name of object and adding textmesh component
        GameObject gameObject = new GameObject("World text" + " " + x + y, typeof(TextMesh));
        
        //position of object is relative to parent with set local position                            
        gameObject.transform.SetParent(parent.transform, false);
        gameObject.transform.localPosition = localPosition;

        //Start Edit TextMesh component
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        //TextMesh component setting
        textMesh.anchor = textAnchor;                                             
        textMesh.text = text;
        textMesh.color = color;
        textMesh.fontSize = fontSize;

        return textMesh;
    }

    //return vector3 of cell anchor based on cell coordinates
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSize, y * cellSize);
    }

    //count coordinates of cell based on position, return coordinates as parameters
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition.x + 28f) / cellSize);//28 to przesuniêcie planszy wzglêdem kamery
        y = Mathf.FloorToInt((worldPosition.y + 28f) / cellSize);
    }

    //set value of specified cell (given coordinates of table)
    public void SetValue(int x, int y, int value)
    {
        gridArray[x, y] = value;

        //for specified cells (which are displaying value) overwritwe also text component
        if (x == 0 || y ==0 || x==width-1 || y == height-1)
        {
            GameObject targetObject = GameObject.Find("World text " + x.ToString() + y.ToString());
            TextMesh textMesh = targetObject.GetComponent<TextMesh>();
            textMesh.text = value.ToString();
        } 
        
        //setting value only for limited range of arguments 
        if (x >= 1 && y >= 1 && (x < (width - 1)) && (y < (height - 1)))
        {
            gridArray55[x - 1, y - 1] = value;
        }

    }

    //set value of specified cell (given position in world)
    public void ChangeValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);//count coordinates of cell based on position, return coordinates as parameters

        //set value only for limed range of arguments
        if (x >= 1 && y >= 1 && (x < (width - 1)) && (y < (height - 1)))
        {
            SetValue(x, y, value);
        }
    }

    //set color of text in specified cell (given coordiantes of cell in table)
    public void ChangeColor(int x, int y, Color color)
    {
        GameObject targetObject = GameObject.Find("World text " + x.ToString() + y.ToString());
        TextMesh textMesh = targetObject.GetComponent<TextMesh>();
        textMesh.color = color;
    }

    //Check if color of text in specified cell is red
    public bool IsRed(int x, int y)
    {
        GameObject targetObject = GameObject.Find("World text " + x.ToString() + y.ToString());
        TextMesh textMesh = targetObject.GetComponent<TextMesh>();

        if (textMesh.color == Color.red)
        {
            return true;
        }
        return false;
    }

    //Return value of cell(given position of cell in world)
    public int GetValue(Vector3 worldPosition)
    {
        int x, y, value;
        GetXY(worldPosition, out x, out y);//count coordinates of cell based on position, return coordinates as parameters
        value = GetValueXY(x, y);//Return value of cell(given coordinates of cell)
        return value;
  
    }

    //Return value of cell(given coordinates of cell)
    public int GetValueXY(int x, int y)
    {
        //return value if coordinate is in permissible range, if not return -1
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return -1;
        }
    }

    //Move marker (text in red color) counterclockwise
    public void MarkerMove()
    {
        int i;

            for (i = 1; i <= width - 2; i++)                //botttom side
            {
                if (IsRed(i, 0)) //Check if color of text in specified cell is red
            {
                    if (i == width - 2)//transition red text to next side
                    {
                        ChangeColor(width - 2, 0, Color.white);
                        ChangeColor(width - 1, 1, Color.red);
                        return;
                    }
                    else//color red next clockwise in the same side
                    {
                        ChangeColor(i, 0, Color.white);
                        ChangeColor(i + 1, 0,  Color.red);
                        return;
                    }
                }
            }

            for (i = 1; i <= height - 2; i++)                 //right side
            {
                if (IsRed(width - 1, i))
                {
                    if (i == height - 2)
                    {
                        ChangeColor(width - 1, height - 2, Color.white);
                        ChangeColor(width - 2, height - 1, Color.red);
                        return;
                    }
                    else
                    {
                        ChangeColor(width - 1, i, Color.white);
                        ChangeColor(width - 1, i + 1, Color.red);
                        return;
                    }
                }
            }

            for (i = width - 2; i >= 1; i--)                //top side
            {
                if (IsRed(i, height - 1))
                {
                    if (i == 1)
                    {
                        ChangeColor(1, height - 1, Color.white);
                        ChangeColor(0, height - 2, Color.red);
                        return;
                    }
                    else
                    {
                        ChangeColor(i, height - 1, Color.white);
                        ChangeColor(i - 1, height - 1, Color.red);
                        return;
                    }
                }
            }

            for (i = height - 2; i >= 1; i--)               //left side
            {
                if (IsRed(0, i))
                {
                    if (i == 1)
                    {
                        ChangeColor(0, 1, Color.white);
                        ChangeColor(1, 0, Color.red);
                        return;
                    }
                    else
                    {
                        ChangeColor(0, i, Color.white);
                        ChangeColor(0, i - 1, Color.red);
                        return;
                    }
                }
            }

    }

    //Set initial values of the board
    //Empty field on the board get value 8
    //map movevement markers get value according to Forcetable
    //marker position definies first active marker (red colored)
    public void SetBoard(int[,] ForceTable, int markerPosition)
    {
        int i, j;
        i = 0;
        j = 0;
        //Empty field on the board get value 8
        for (i = 1; i <= (width - 2); i++)             
        {
            for (j = 1; j <= (height - 2); j++)
            {
                SetValue(i, j, 8);
            }
        }

        //map movevement markers get value according to Forcetable, set all colours to white
        for (i = 1; i <= width - 2; i++)               //bottom
        {
            SetValue(i, 0, ForceTable[0, i - 1]);
            ChangeColor(i, 0, Color.white);
        }
        for (i = 1; i <= height - 2; i++)              //right
        {
            SetValue(width - 1, i, ForceTable[1, i - 1]);
            ChangeColor(width - 1, i, Color.white);
        }
        for (i = width - 2; i >= 1; i--)
        {
            SetValue(i, height - 1, ForceTable[2, i - 1]);//top
            ChangeColor(i, height - 1, Color.white);
        }
        for (i = height - 2; i >= 1; i--)
        {
            SetValue(0, i, ForceTable[3, i - 1]);       //left
            ChangeColor(0, i, Color.white);
        }

        //makrer position definies first active marker (red colored)
        //markerPosition 1-5 -> left side, markerPosition 6-10 -> bottom side e.t.c
        if (markerPosition < (height-1))//left side
        {
            i = 0;
            j = (height - 1) - markerPosition;
        }
        else if (markerPosition < (height+width - 3))//bottom side
        {
            i = (height + width - 3)-markerPosition;
            j = 0;
        }
        else if (markerPosition < (2*height + width - 5))//right side
        {
            i = 6;
            j = (2 * height + width - 5)- markerPosition;
        }
        else if (markerPosition < (2 * height + 2*width - 7))//top side
        {
            i = (2 * height + 2 * width - 7) - markerPosition;
            j = 6;
        }
        ChangeColor(i, j, Color.red);
    }
   
    //Set new value of cells after map movement
    public void PawnsMove(int[,] ForceArray)
    {
        int i; 
        int x, y;
        int sidecount;//how many are cells in main board (2 less than width/height)
        int[] value = new int[Math.Max(height - 2, width-2)];//helping array to store new values on moved cells 
        int force;//how many fields is moving

        MarkerPosition(out x, out y);//return as parametres coordinates of acitve map movement marker
        force = MarkerValue(ForceArray);//what says red number

        //find new values in row, overwrite values
        if (x == 0)                                                     //left side
        {
            sidecount = width - 2;
            //find new values in row
            for (i = 1; i <= sidecount; i++)
            {
                value[i-1] = gridArray55[(i-1 + sidecount - force) % sidecount, y-1];
            }
            //overwrite values
            for (i = 1; i <= sidecount; i++)
            {
                SetValue(i, y, value[i-1]);//set value of specified cell (given coordinates of table)
            }

        }
        if (x == width - 1)                                                     //right side
        {
            sidecount = width - 2;
            for (i = 1; i <= sidecount; i++)
            {
                value[i - 1] = gridArray55[(i - 1 + sidecount + force) % sidecount, y - 1];
            }
            for (i = 1; i <= sidecount; i++)
            {
                SetValue(i, y, value[i - 1]);
            }
        }
        if (y == height - 1)                                                     //top side
        {
            sidecount = height - 2;
            for (i = 1; i <= sidecount; i++)
            {
                value[i - 1] = gridArray55[x-1, (i - 1 + sidecount + force) % sidecount];
            }
            for (i = 1; i <= sidecount; i++)
            {
                SetValue(x, i, value[i - 1]);
            }

        }
        if (y == 0)                                                         //bottom side
        {
            sidecount = height - 2;
            for (i = 1; i <= sidecount; i++)
            {
                value[i - 1] = gridArray55[x - 1, (i - 1 + sidecount - force) % sidecount];
            }
            for (i = 1; i <= sidecount; i++)
            {
                SetValue(x, i, value[i - 1]);
            }

        }

    }

    //return how many fields pawns are moving (what says red number)
    public int MarkerValue(int[,] ForceArray)
    {
        int x, y; 
        int force = 0;
        MarkerPosition(out x, out y);

        if (x == 0)                                                    
        {
            force = ForceArray[3, y - 1];
        }
        if (x == width-1)                                                     
        {
            force = ForceArray[1, y - 1];
        }
        if (y ==height-1)                                                    
        {
            force = ForceArray[2, x - 1];
        }
        if (y == 0)                                                     
        {
            force = ForceArray[0, x - 1];
        }

        return force;
    }

    //return as parametres coordinates of acitve map movement marker
    public void MarkerPosition(out int x,  out int y)
    {
        int i;

        for (i = 1; i <= width - 2; i++)
        {
            if (IsRed(i, 0))
            {
                x = i; 
                y = 0;
                return;
            }
        }
        for (i = 1; i <= height - 2; i++)
        {
            if (IsRed(width-1, i))
            {
                x = width - 1;
                y = i;
                return;
            }
        }
        for (i = width - 2; i >= 1; i--)
        {
            if (IsRed(i, height - 1))
            {
                x = i;
                y = height - 1;
                return;
            }
        }
        for (i = height - 2; i >= 1; i--)
        {
            if (IsRed(0, i))
            {
                x = 0;
                y = i;
                return;
            }

        }
        x = -1;
        y = -1;

    }

    //set value 16 for specified cells (given table with coordinates)
    public void AddEnemies(int[,] table)
    {
        int enemyCount = table.GetLength(0);//how many enemies are to be added
        int i;

        for (i=0; i<enemyCount; i++)
        {
            SetValue(table[i,0], table[i, 1] , 16);
        }

    }

    //set value 56 for specified cells (given table with coordinates)
    public void AddAllies(int[,] table)
    {
        int allyCount = table.GetLength(0);//how many allies are to be added
        int i;

        for (i = 0; i < allyCount; i++)
        {
            SetValue(table[i, 0], table[i, 1], 56);
        }

    }

    //Set value in main board (given all table for main board ready to be copied)
    public void AddPawns(int[,] table)
    {
        for (int i = 1; i < width-1; i++)
        {
            for (int j = 1; j < height-1; j++)
            {
                SetValue(i, j, table[i-1, j-1]);
            }
        }
    }

}

