using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Linq;

//--------------------------------data base for records
//records data consist of levels
[System.Serializable]
public class RecordsData
{
    public List<LevelData> levels;
}
//in each level is nuber defining which level it refers and list of best results of clicks and times for that level
[System.Serializable]
public class LevelData
{
    public int level;
    public List<int> clicks;
    public List<int> times;
}
//-------------------------------------

//class managing records database
public static class Records
{
    static public string filePath = Application.persistentDataPath + "/RecordsData.json";//file for storing data
    static public RecordsData recordsData;//variable storing the all data
    static int numberOfLevels = 12;
    static int numberOfBestScores = 5;
    static public int clicksLimit = 1000;
    static public int timesLimit = 10000;

    //loading records into recordsData variable so it will be accesible later
    static public void loadJSON()
    {
        if (File.Exists(filePath))
        {
            //Debug.Log("file exists");
            string json = File.ReadAllText(filePath);
            // Deserialize the JSON to the RecordsData object
            recordsData = JsonUtility.FromJson<RecordsData>(json);
            //Debug.Log("Game data loaded.");
        }
        else
        {
            //Debug.Log("file doesn't exist");
            crateAndLoadJSON();
        }
    }

    //this creates file with deafult values and assigning recordsData 
    static public void crateAndLoadJSON()
    {
        recordsData = new RecordsData
        {
            levels = new List<LevelData>()
        };

        // Initialize default scores for each level
        for (int i = 1; i <= numberOfLevels; i++)
        {
            LevelData levelData = new LevelData
            {
                level = i,
                clicks = new List<int>(),
                times = new List<int>()
            };

            // Add default score of 1000 for each level
            for (int j = 0; j < numberOfBestScores; j++)
            {
                levelData.clicks.Add(clicksLimit);
                levelData.times.Add(timesLimit);
            }

            recordsData.levels.Add(levelData);
        }

        // Save the initial data with default scores
        string json = JsonUtility.ToJson(recordsData);
        File.WriteAllText(filePath, json);
        //Debug.Log("file created and loaded");
    }

    //updating json file
    static public void SaveGameData()
    {
        // Serialize the data to JSON
        string json = JsonUtility.ToJson(recordsData);

        // Write the JSON to the file
        File.WriteAllText(filePath, json);

        //Debug.Log("Game data saved.");
    }

    //returns array of best results of given type for given level
    static public int[] getBestResults(int level, string type)
    {
        if (recordsData == null)
        {
            loadJSON();
        }
        LevelData levelData = recordsData.levels.Find(l => l.level == level);
        if (type == "clicks")
        {
            return levelData.clicks.ToArray();
        }
        else if (type == "times")
        {
            return levelData.times.ToArray();
        }
        else
        {
            Debug.Log("Wrong input");
            return new int[0];
        }
    }

    //update recordsData, there is one new score for clicks and time for given level, it may jump into recordsData in this function
    static public void updateRecords(int level, int Clickscore, int TimeScore) 
    {
        int[] bestResults = getBestResults(level, "clicks");
        for (int i = 0; i < numberOfBestScores; i++)
        {
            if (Clickscore < bestResults[i])
            {
                int temp = bestResults[i];
                bestResults[i] = Clickscore;
                Clickscore = temp;
            }
        }
        recordsData.levels.Find(l => l.level == level).clicks = bestResults.ToList();

        bestResults = getBestResults(level, "times");
        for (int i = 0; i < numberOfBestScores; i++)
        {
            if (TimeScore < bestResults[i])
            {
                int temp = bestResults[i];
                bestResults[i] = TimeScore;
                TimeScore = temp;
            }
        }
        recordsData.levels.Find(l => l.level == level).times = bestResults.ToList();

        SaveGameData();
    }

    //delete json and creates new with deafult values
    static public void cleanRecords()
    {
        File.Delete(filePath);
        crateAndLoadJSON();
    }
}

