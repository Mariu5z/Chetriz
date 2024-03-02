using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class returns values defining level and its start moment
//level is definied by pawns arrangement, current shape, map movement pattern and position of map movement marker
public class Levels
{
    public static int[] ShapesOrder = new int[12];
    static System.Random rng = new System.Random(); // Create a random number generator
    public int[,] PawnsTable = new int[5, 5];
    public int[,] ForceArray = new int[4, 5];
    public int ShapeNumber;
    public int MarkerStartPosition;

    //constructor preparing game for new level
    public Levels(int difficulty, int level)
    {
        PawnsTable = DrawPawns(difficulty, level);//draw arrangments of pawns in the start of level (based on difficulty and level)
        ForceArray = ForceArrays(level);//chose force patern of map movement
        ShapeNumber = GetShapeNumber(level);//chose drawn shape 
        MarkerStartPosition = DrawMarkerPosition();//draw start position of map marker movement
    }

    //draw random order of number from 1 to 12
    //to define random order of seeked shapes in 12 levels
    //Fisher-Yates shuffle algorithm is used to obtain random order of numbers
    public static void DrawShapesOrder()
    {
        int i, j, temp;
        int[] numbers = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};

        // Fisher-Yates shuffle algorithm
        for (i=numbers.Length - 1; i > 0; i--)      
        {                              
            j = rng.Next(i + 1);// Generate a random index between 0 and i (inclusive)

            temp = numbers[i];//replace values between elements i and j (temp is for temporary storing) 
            numbers[i] = numbers[j];
            numbers[j] = temp;
        }
        ShapesOrder = numbers;
    }

    //this function returns 2d table defining how to fill 5x5 board with pawns in random fields
    //difficulty - number of black pawns, ;level - number of white pawns
    public int[,] DrawPawns(int difficulty, int level)
    {
        int i, j, temp;
        int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24};
        int[,] table = new int[5, 5];
        int quotient;//integer part from division
        int remainder;//the rest from division

        // Fisher-Yates shuffle algorithm generates random order of numbers
        for (i = numbers.Length - 1; i > 0; i--)     
        {
            j = rng.Next(i + 1);                
            temp = numbers[i];                  
            numbers[i] = numbers[j];
            numbers[j] = temp;
        }

        //add specified number of black pawns
        for (i=0; i<difficulty; i++)
        {
            //e.g. if number[0] = 8
            //quotient = 8 / 5 = 1  - first row of board
            //remainder = 8 % 5 = 3 - third column of board
            quotient = numbers[i]/5;
            remainder = numbers[i] % 5;
            table[quotient, remainder] = 56;//assign value (refered to black pawns) in drawn field of table
        }
        //add specified number of white pawns
        for (i = difficulty; i < (difficulty+level); i++)
        {
            quotient = numbers[i] / 5;
            remainder = numbers[i] % 5;
            table[quotient, remainder] = 16;
        }
        //rest of fields leave empty
        for (i = (difficulty + level); i < numbers.Length; i++)
        {
            quotient = numbers[i] / 5;
            remainder = numbers[i] % 5;
            table[quotient, remainder] = 8;
        }
        return table;
    } 

    //this function returns table deining how to fill map movement fields in board
    //each level has defining pattern of map movement
    public int[,] ForceArrays(int level)
    {
        int[,] tableOut = new int[4, 5];

        if (level == 1)
        {
            int[,] table =
            {
                    { 1, 1, 2, 1, 1 },   //dolny bok
                    { 1, 1, 2, 1, 1 },   //prawy bok
                    { 1, 1, 2, 1, 1 },   //górny bok
                    { 1, 1, 2, 1, 1 }    //lewy bok
            };
            tableOut = table;
        }
        else if (level == 2)
        {
            int[,] table =
                    {
                    { 1, 2, 2, 2, 1 },   //dolny bok
                    { 1, 2, 2, 2, 1 },   //prawy bok
                    { 1, 2, 2, 2, 1 },   //górny bok
                    { 1, 2, 2, 2, 1 }    //lewy bok
            };
            tableOut = table;
        }
        else if (level == 3)
        {
            int[,] table =
                    {
                    { 1, 2, 3, 2, 1 },   //dolny bok
                    { 1, 2, 3, 2, 1 },   //prawy bok
                    { 1, 2, 3, 2, 1 },   //górny bok
                    { 1, 2, 3, 2, 1 }    //lewy bok
            };
            tableOut = table;
        }
        else if (level == 4)
        {
            int[,] table =
                    {
                    { 2, 2, 2, 2, 2 },   //dolny bok
                    { 1, 1, 1, 1, 1 },   //prawy bok
                    { 4, 4, 4, 4, 4 },   //górny bok
                    { 3, 3, 3, 3, 3 }    //lewy bok
            };
            tableOut = table;
        }
        else if (level == 5)
        {
            int[,] table =
                    {
                    { 1, 2, 3, 4, 5 },   //dolny bok
                    { 5, 4, 3, 2, 1 },   //prawy bok
                    { 1, 2, 3, 4, 5 },   //górny bok
                    { 5, 4, 3, 2, 1 }    //lewy bok
            };
            tableOut = table;
        }
        else if (level == 6)
        {
            int[,] table =
                    {
                    { 2, 3, 2, 3, 2 },   //dolny bok
                    { 3, 2, 3, 2, 3 },   //prawy bok
                    { 2, 3, 2, 3, 2 },   //górny bok
                    { 3, 2, 3, 2, 3 }    //lewy bok
            };
            tableOut = table;
        }
        else if (level == 7)
        {
            int[,] table =
                    {
                    { 5, 4, 3, 2, 1 },   //dolny bok
                    { 1, 2, 3, 4, 5 },   //prawy bok
                    { 5, 4, 3, 2, 1 },   //górny bok
                    { 1, 2, 3, 4, 5 }    //lewy bok
            };
            tableOut = table;
        }
        else if (level == 8)
        {
            int[,] table =
                    {
                    { 2, 4, 2, 4, 2 },   //dolny bok
                    { 4, 2, 4, 2, 4 },   //prawy bok
                    { 2, 4, 2, 4, 2 },   //górny bok
                    { 4, 2, 4, 2, 4 }    //lewy bok
            };
            tableOut = table;
        }
        else if (level == 9)
        {
            int[,] table =
                    {
                    { 1, 3, 1, 1, 1 },   //dolny bok
                    { 1, 1, 3, 1, 1 },   //prawy bok
                    { 1, 1, 1, 3, 1 },   //górny bok
                    { 3, 1, 1, 1, 1 }    //lewy bok
            };
            tableOut = table;
        }
        else if (level == 10)
        {
            int[,] table =
                    {
                    { 1, 2, 5, 2, 1 },   //dolny bok
                    { 1, 2, 5, 2, 1 },   //prawy bok
                    { 1, 2, 5, 2, 1 },   //górny bok
                    { 1, 2, 5, 2, 1 }    //lewy bok
            };
            tableOut = table;
        }
        else if (level == 11)
        {
            int[,] table =
                    {
                    { 4, 1, 2, 1, 4 },   //dolny bok
                    { 4, 1, 2, 1, 4 },   //prawy bok
                    { 4, 1, 2, 1, 4 },   //górny bok
                    { 4, 1, 2, 1, 4 }    //lewy bok
            };
            tableOut = table;
        }
        else if (level == 12)
        {
            int[,] table =
                    {
                    { 1, 3, 5, 2, 4 },   //dolny bok
                    { 4, 2, 5, 3, 1 },   //prawy bok
                    { 1, 3, 5, 2, 4 },   //górny bok
                    { 4, 2, 5, 3, 1 }    //lewy bok
            };
            tableOut = table;
        }

        return tableOut;
    }

    //return number defining shape to look for in given level
    public int GetShapeNumber(int level)
    {
        int number;  
        number = ShapesOrder[level - 1];
        return number;
    }

    //return random number defining map movement marker position 
    public int DrawMarkerPosition()
    {
        int i = rng.Next(20) +1 ;//losowanie od 1 do 20 w³¹cznie
        return i;
    }

}

