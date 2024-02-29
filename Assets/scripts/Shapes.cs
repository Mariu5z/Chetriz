using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is for finding pentomino shapes formed by pawns in board
//every specific shape has assigned number, if any shape on board is detected, function return assigned number
public class Shapes
{
    private static int[,] myTable = new int[9,9];
    private static int[,] EnTable = new int[9, 9];

    private static bool[] results = new bool[12];
    private static bool[] Enresults = new bool[12];

    private static int length;
    private static bool result2;
    private static bool[] result3 = new bool[8];
    private static bool[] result4 = new bool[2];
    private static int i, j, k;

    public static bool[] shapes(int[,] table)
    {
        myTable =GrowTable(table);
        Array.Fill(results, false);

        //seeking horizontal lines
        for (i = 2; i < 7; i++)
        {
            length = 0;
            for (j = 2; j < 7; j++)
            {
                //Debug.Log("i:"+i.ToString() + ", j:" + j.ToString()+ "  -> "+ table[i, j].ToString());
                if (myTable[i, j] > 22)
                {
                    length++;
                }
                else
                {
                    length = 0;
                }

                if (length >= 2)
                {
                    result2 = line2h(i, j-1,myTable);
                    if (result2)
                    {
                        //Debug.Log("znaleziono 12 dla " + i.ToString() + " " + j.ToString());
                        results[11]= true;
                    }
                }

                if (length >= 3)
                {
                    result3 = line3h(i, j - 2, myTable);
                    for (k = 0; k < 8; k++)
                    {
                        if (result3[k])
                        {
                            //Debug.Log("znaleziono "+ (k+4) + " dla " + i.ToString() + " " + j.ToString());
                            results[k+3] = true;
                        }
                    }

                }

                if (length >= 4)
                {
                    result4 = line4h(i, j-3, myTable);
                    if (result4[0])
                    {
                        //Debug.Log("znaleziono 2 dla "+ i.ToString() + " "+ j.ToString());
                        results[1] = true;
                    }
                    if (result4[1])
                    {
                        //Debug.Log("znaleziono 3 dla " + i.ToString() + " " + j.ToString());
                        results[2] = true;
                    }

                }

                if (length >= 5)
                {
                    //Debug.Log("znaleziono 1 dla " + i.ToString() + " " + j.ToString());
                    results[0] = true;
                }

            }
        }

        for (j = 2; j < 7; j++)                      //szukanie linii pionowych
        {
            length = 0;
            for (i = 2; i < 7; i++)
            {
                if (myTable[i, j] > 22)
                {
                    length++;
                }
                else
                {
                    length = 0;
                }

                if (length >= 3)
                {
                    result3 = line3v(i-2, j, myTable);
                    for (k = 0; k < 8; k++)
                    {
                        if (result3[k])
                        {
                            //Debug.Log("znaleziono " + (k + 4) + " dla " + i.ToString() + " " + j.ToString());
                            results[k + 3] = true;
                        }
                    }
                }

                if (length >= 4)
                {
                    result4 = line4v(i-3, j, myTable);
                    if (result4[0])
                    {
                        //Debug.Log("znaleziono 2 dla " + i.ToString() + " " + j.ToString());
                        results[1] = true;
                    }
                    if (result4[1])
                    {
                        //Debug.Log("znaleziono 3 dla " + i.ToString() + " " + j.ToString());
                        results[2] = true;
                    }
                }

                if (length >= 5)
                {
                    //Debug.Log("znaleziono 1 dla " + i.ToString() + " " + j.ToString());
                    results[0] = true;
                }

            }
        }

        return results;
    }


    public static bool[] EnShapes(int[,] table)
    {

        EnTable = ConvertEnTable(table);
        Array.Fill(Enresults, false);

        for (i = 2; i < 7; i++)               //szukanie linii poziomych
        {
            length = 0;
            for (j = 2; j < 7; j++)
            {
                //Debug.Log("i:"+i.ToString() + ", j:" + j.ToString()+ "  -> "+ table[i, j].ToString());
                if (EnTable[i, j] > 22)
                {
                    length++;
                }
                else
                {
                    length = 0;
                }

                if (length >= 2)
                {
                    result2 = line2h(i, j - 1, EnTable);
                    if (result2)
                    {
                        //Debug.Log("znaleziono 12 dla wroga");
                        Enresults[11] = true;
                    }
                }

                if (length >= 3)
                {
                    result3 = line3h(i, j - 2, EnTable);
                    for (k = 0; k < 8; k++)
                    {
                        if (result3[k])
                        {
                            //Debug.Log("znaleziono " + (k+4).ToString() + " dla wroga");
                            Enresults[k + 3] = true;
                        }
                    }

                }

                if (length >= 4)
                {
                    result4 = line4h(i, j - 3, EnTable);
                    if (result4[0])
                    {
                        //Debug.Log("znaleziono 2 dla wroga");
                        Enresults[1] = true;
                    }
                    if (result4[1])
                    {
                        //Debug.Log("znaleziono 3 dla wroga");
                        Enresults[2] = true;
                    }

                }

                if (length >= 5)
                {
                    //Debug.Log("znaleziono 1 dla wroga");
                    Enresults[0] = true;
                }

            }
        }


        for (j = 2; j < 7; j++)                      //szukanie linii pionowych
        {
            length = 0;
            for (i = 2; i < 7; i++)
            {
                if (EnTable[i, j] > 22)
                {
                    length++;
                }
                else
                {
                    length = 0;
                }

                if (length >= 3)
                {
                    result3 = line3v(i - 2, j, EnTable);
                    for (k = 0; k < 8; k++)
                    {
                        if (result3[k])
                        {
                            //Debug.Log("znaleziono " + (k + 4).ToString() + " dla wroga");
                            Enresults[k + 3] = true;
                        }
                    }
                }

                if (length >= 4)
                {
                    result4 = line4v(i - 3, j, EnTable);
                    if (result4[0])
                    {
                        //Debug.Log("znaleziono 2 dla wroga");
                        Enresults[1] = true;
                    }
                    if (result4[1])
                    {
                        //Debug.Log("znaleziono 3 dla wroga");
                        Enresults[2] = true;
                    }
                }

                if (length >= 5)
                {
                    //Debug.Log("znaleziono 1 dla wroga");
                    Enresults[0] = true;
                }

            }
        }

        return Enresults;

    }


    public static bool line2h(int x, int y, int[,] table)          //wykrywa ksztalt 12 dla poziomych lini 2, gdy to ta pozioma linia jest nizej w ksztalcie
    {
        bool detected = false;

        if (table[x + 1, y] > 22 && table[x + 1, y-1] > 22 && table[x + 2, y - 1] > 22)//NMI
        {
            //Debug.Log("NMI");
            detected = true;
        }
        else if (table[x + 1, y] > 22 && table[x + 1, y - 1] > 22 && table[x -1 , y + 1] > 22)//NMC
        {
            //Debug.Log("NMC");
            detected = true;
        }
        else if (table[x + 1, y +2] > 22 && table[x - 1, y] > 22 && table[x + 1, y + 1] > 22)//PBO
        {
            //Debug.Log("PBO");
            detected = true;
        }
        else if (table[x + 1, y + 2] > 22 && table[x + 2, y+2] > 22 && table[x + 1, y + 1] > 22)//PLO
        {
            //Debug.Log("PLO");
            detected = true;
        }

        return detected;   
    }

    public static bool[] line4h(int x, int y, int[,] table)
    {
        bool[] shapes4 = new bool[2];
        shapes4[0] = false; 
        shapes4[1] = false;

        if (table[x+1,y] > 22 || table[x + 1, y+3] > 22 || table[x - 1, y] > 22 || table[x - 1, y+3] > 22)
        {
            shapes4[0] = true;
        }

        if (table[x + 1, y+1] > 22 || table[x + 1, y + 2] > 22 || table[x - 1, y+1] > 22 || table[x - 1, y + 2] > 22)
        {
            shapes4[1] = true;
        }

        return shapes4;
    }

    public static bool[] line3h(int x, int y, int[,] table)
    {
        bool[] shapes3 = new bool[8];
        
        for (int i = 0;i<8; i++)
        {
            shapes3[i] = false;
        }

        if (table[x+1, y-1] > 22 && table[x+1, y] > 22)                     //kszta速 4
        {
            //Debug.Log("AE");
            shapes3[0] = true;
        }
        else if (table[x + 1, y + 2] > 22 && table[x + 1, y + 3] > 22)
        {
            //Debug.Log("GH");
            shapes3[0] = true;
        }
        else if (table[x - 1, y - 1] > 22 && table[x - 1, y] > 22)
        {
            //Debug.Log("IJ");
            shapes3[0] = true;
        }
        else if (table[x - 1, y + 2] > 22 && table[x - 1, y + 3] > 22)
        {
            //Debug.Log("LM");
            shapes3[0] = true;
        }
        


        if (table[x + 1, y + 1] > 22 && table[x + 2, y + 1] > 22)           //kszta速 5
        {
            //Debug.Log("CF");
            shapes3[1] = true;
        }
        else if (table[x - 1, y + 1] > 22 && table[x - 2, y + 1] > 22)
        {
            //Debug.Log("KO");
            shapes3[1] = true;
        }
        else if (table[x + 1, y] > 22 && table[x - 1, y] > 22)
        {
            //Debug.Log("EJ");
            shapes3[1] = true;
        }
        else if (table[x + 1, y+2] > 22 && table[x - 1, y+2] > 22)
        {
            //Debug.Log("GL");
            shapes3[1] = true;
        }


        if (table[x + 1, y] > 22 && table[x - 1, y + 1] > 22)           //kszta速 6
        {
            //Debug.Log("EK");
            shapes3[2] = true;
        }
        else if (table[x - 1, y] > 22 && table[x + 1, y + 1] > 22)
        {
            //Debug.Log("JF");
            shapes3[2] = true;
        }
        else if (table[x - 1, y + 2] > 22 && table[x + 1, y + 1] > 22)
        {
            //Debug.Log("LF");
            shapes3[2] = true;
        }
        else if (table[x + 1, y + 2] > 22 && table[x - 1, y + 1] > 22)
        {
            //Debug.Log("GK");
            shapes3[2] = true;
        }


        if (table[x + 1, y] > 22 && table[x + 1, y + 2] > 22)           //kszta速 7
        {
            //Debug.Log("EG");
            shapes3[3] = true;
        }
        else if (table[x - 1, y] > 22 && table[x - 1, y + 2] > 22)
        {
            //Debug.Log("JL");
            shapes3[3] = true;
        }

        if (table[x + 1, y+1] > 22 && table[x - 1, y + 1] > 22)           //kszta速 8
        {
            //Debug.Log("FK");
            shapes3[4] = true;
        }

        if (table[x + 1, y ] > 22 && table[x + 1, y + 1] > 22)           //kszta速 9
        {
            //Debug.Log("EF");
            shapes3[5] = true;
        }
        else if (table[x + 1, y+1] > 22 && table[x + 1, y + 2] > 22)
        {
            //Debug.Log("FG");
            shapes3[5] = true;
        }
        else if (table[x - 1, y ] > 22 && table[x - 1, y + 1] > 22)
        {
            //Debug.Log("JK");
            shapes3[5] = true;
        }
        else if (table[x - 1, y+1] > 22 && table[x - 1, y + 2] > 22)
        {
            //Debug.Log("KL");
            shapes3[5] = true;
        }

        if (table[x + 1, y] > 22 && table[x + 2, y ] > 22)           //kszta速 10
        {
            //Debug.Log("BE");
            shapes3[6] = true;
        }
        else if (table[x + 1, y+2 ] > 22 && table[x + 2, y+2] > 22)         
        {
            //Debug.Log("DG");
            shapes3[6] = true;
        }
        else if (table[x - 1, y] > 22 && table[x - 2, y] > 22)
        {
            //Debug.Log("JN");
            shapes3[6] = true;
        }
        else if (table[x - 1, y+2] > 22 && table[x - 2, y+2] > 22)
        {
            //Debug.Log("LP");
            shapes3[6] = true;
        }

        if (table[x + 1, y] > 22 && table[x - 1, y+2] > 22)           //kszta速 11
        {
            //Debug.Log("EL");
            shapes3[7] = true;
        }
        else if (table[x - 1, y] > 22 && table[x + 1, y + 2] > 22)
        {
            //Debug.Log("JG");
            shapes3[7] = true;
        }

        return shapes3;
    }

    public static bool[] line4v(int x, int y, int[,] table)
    {
        bool[] shapes4 = new bool[2];
        shapes4[0] = false;
        shapes4[1] = false;

        if (table[x+3, y-1] > 22 || table[x, y - 1] > 22 || table[x+3, y + 1] > 22 || table[x, y + 1] > 22)
        {
            shapes4[0] = true;
        }

        if (table[x + 2, y - 1] > 22 || table[x+1, y - 1] > 22 || table[x + 2, y + 1] > 22 || table[x+1, y + 1] > 22)
        {
            shapes4[1] = true;
        }

        return shapes4;
    }

    public static bool[] line3v(int x, int y, int[,] table)
    {
        bool[] shapes3 = new bool[8];

        for (int i = 0; i < 8; i++)
        {
            shapes3[i] = false;
        }

        if (table[x+3, y-1] > 22 && table[x+2, y - 1] > 22)                 //kszta速 4
        {
            //Debug.Log("GAV");
            shapes3[0] = true;
        } 
        else if (table[x + 3, y + 1] > 22 && table[x + 2, y + 1] > 22)
        {
            //Debug.Log("IDV");
            shapes3[0] = true;
        }
        else if (table[x -1, y + 1] > 22 && table[x , y + 1] > 22)
        {
            //Debug.Log("FJV");
            shapes3[0] = true;
        }
        else if (table[x - 1, y - 1] > 22 && table[x, y - 1] > 22)
        {
            //Debug.Log("CHV");
            shapes3[0] = true;
        }

        if (table[x + 2, y - 1] > 22 && table[x + 1, y + 1] > 22)                 //kszta速 6
        {
            //Debug.Log("AEV");
            shapes3[2] = true;
        }
        else if (table[x + 2, y + 1] > 22 && table[x + 1, y - 1] > 22)             
        {
            //Debug.Log("DBV");
            shapes3[2] = true;
        }
        else if (table[x, y - 1] > 22 && table[x + 1, y + 1] > 22)
        {
            //Debug.Log("CEV");
            shapes3[2] = true;
        }
        else if (table[x, y + 1] > 22 && table[x + 1, y - 1] > 22)
        {
            //Debug.Log("FBV");
            shapes3[2] = true;
        }

        if (table[x + 2, y - 1] > 22 && table[x, y - 1] > 22)                 //kszta速 7
        {
            //Debug.Log("ACV");
            shapes3[3] = true;
        }
        else if (table[x + 2, y + 1] > 22 && table[x, y + 1] > 22)           
        {
            //Debug.Log("DFV");
            shapes3[3] = true;
        }

        if (table[x + 2, y - 1] > 22 && table[x+1, y - 1] > 22)                 //kszta速 9
        {
            //Debug.Log("ABV");
            shapes3[5] = true;
        }
        else if (table[x + 1, y - 1] > 22 && table[x, y - 1] > 22)          
        {
            //Debug.Log("BCV");
            shapes3[5] = true;
        }
        else if (table[x + 2, y + 1] > 22 && table[x +1, y + 1] > 22)
        {
            //Debug.Log("DEV");
            shapes3[5] = true;
        }
        else if (table[x + 1, y + 1] > 22 && table[x, y + 1] > 22)
        {
            //Debug.Log("EFV");
            shapes3[5] = true;
        }

        if (table[x + 2, y - 1] > 22 && table[x , y + 1] > 22)                 //kszta速 11
        {
            //Debug.Log("AFV");
            shapes3[7] = true;
        }
        else if (table[x + 2, y + 1] > 22 && table[x, y - 1] > 22)           
        {
            //Debug.Log("CDV");
            shapes3[7] = true;
        }

        return shapes3;
    }

    //enlarge table with data two cells around (to avoid errors)
    public static int[,] GrowTable(int[,] table)
    {
        //clear all myTable (start with 0 element and clear all elements)
        Array.Clear(myTable, 0, myTable.Length);

        for (i = 0; i < 5; i++)
        {
            for (j = 0; j < 5; j++)
            {
                myTable[i+2,j+2]= table[i,j];
            }
        }
        return myTable;//myTable is 9x9 with data in 5x5 in the middle
    }

    public static int[,] ConvertEnTable(int[,] table)
    {
        Array.Clear(EnTable, 0, EnTable.Length);

        for (i = 0; i < 5; i++)
        {
            for (j = 0; j < 5; j++)
            {
                if (table[i, j] == 56)
                {
                    EnTable[i + 2, j + 2] = 16;
                }
                else if (table[i, j] == 16)
                {
                    EnTable[i + 2, j + 2] = 56;
                }
                else
                {
                    EnTable[i + 2, j + 2] = table[i, j];
                }           
            }
        }
        return EnTable;
    }



}


