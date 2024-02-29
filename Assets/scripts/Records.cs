using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Linq;

//--------------------------------baza danych
//tabele sk³ada siê z poziomów
[System.Serializable]
public class RecordsData
{
    public List<LevelData> levels;
}
//ka¿dy poziom ma swój numer i zbiór wyników
[System.Serializable]
public class LevelData
{
    public int level;
    public List<int> clicks;
}

public static class Records
{
    static string filePath = Application.persistentDataPath + "/RecordsData.json";
    static public RecordsData recordsData;
    static int numberOfLevels = 12;
    static int numberOfBestScores = 5;

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
                clicks = new List<int>()
            };

            // Add default score of 1000 for each level
            for (int j = 0; j < numberOfBestScores; j++)
            {
                levelData.clicks.Add(1000);
            }

            recordsData.levels.Add(levelData);
        }

        // Save the initial data with default scores
        string json = JsonUtility.ToJson(recordsData);
        File.WriteAllText(filePath, json);
        //Debug.Log("file created and loaded");
    }

    static public void SaveGameData()
    {
        // Serialize the data to JSON
        string json = JsonUtility.ToJson(recordsData);

        // Write the JSON to the file
        File.WriteAllText(filePath, json);

        //Debug.Log("Game data saved.");
    }

    static public int[] getBestResult(int level)
    {
        if (recordsData == null)
        {
            loadJSON();
        }
        LevelData levelData = recordsData.levels.Find(l => l.level == level);
        return levelData.clicks.ToArray();
    }

    static public void updateRecords(int level, int score) 
    {
        if (level < 1 || level > numberOfLevels || score <= 0)
        {
            Debug.Log("wrong input");
            return;
        } 
        LevelData levelData = recordsData.levels.Find(l => l.level == level);
        for (int i = 0; i < numberOfBestScores; i++)
        {
            if (score < levelData.clicks[i])
            {
                int temp = levelData.clicks[i];
                levelData.clicks[i] = score;
                score = temp;
            }
        }
        SaveGameData();
    }

    static public void cleanRecords()
    {
        File.Delete(filePath);
        crateAndLoadJSON();
    }
}

