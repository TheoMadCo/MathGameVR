using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

[System.Serializable]
public class LeaderboardEntry
{
    public string userName;
    public string gameType;
    public string difficulty;
    public float completionTime;
    public int tasksCompleted;
    public int totalTasks;
    public string date;
}


[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}

public class Leaderboard_CompleteOperation : MonoBehaviour
{
    public static Leaderboard_CompleteOperation Instance { get; private set; }

    private string leaderboardFilePath;
    private LeaderboardData leaderboardData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Set up file path
        leaderboardFilePath = Path.Combine(Application.persistentDataPath, "leaderboard_complete_operation.json");
        LoadLeaderboard();
    }

    public void AddEntry(string userName, string gameType, string difficulty, float completionTime, int tasksCompleted, int totalTasks)
    {
        LeaderboardEntry newEntry = new LeaderboardEntry
        {
            userName = userName,
            gameType = gameType,
            difficulty = difficulty, // Save difficulty
            completionTime = completionTime,
            tasksCompleted = tasksCompleted,
            totalTasks = totalTasks,
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        // Load latest data before adding new entry
        LoadLeaderboard();
        leaderboardData.entries.Add(newEntry);
        SaveLeaderboard();
        ExportToCSV(); // Automatically export to CSV after each new entry
    }


    public void SaveLeaderboard()
    {
        try
        {
            string json = JsonUtility.ToJson(leaderboardData, true);
            File.WriteAllText(leaderboardFilePath, json);
            Debug.Log($"Leaderboard saved successfully to: {leaderboardFilePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving leaderboard: {e.Message}");
        }
    }

    public void LoadLeaderboard()
    {
        try
        {
            if (File.Exists(leaderboardFilePath))
            {
                string json = File.ReadAllText(leaderboardFilePath);
                leaderboardData = JsonUtility.FromJson<LeaderboardData>(json);
                Debug.Log("Leaderboard loaded successfully.");
            }
            else
            {
                leaderboardData = new LeaderboardData();
                Debug.Log("No leaderboard file found. Created new leaderboard.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading leaderboard: {e.Message}");
            leaderboardData = new LeaderboardData();
        }
    }

    public void ExportToCSV()
    {
        try
        {
            string csvFilePath = Path.Combine(Application.persistentDataPath, "leaderboard_complete_operation.csv");
            using (StreamWriter writer = new StreamWriter(csvFilePath))
            {
                writer.WriteLine("UserName,GameType,Difficulty,CompletionTime,TasksCompleted,TotalTasks,Date"); // Updated header
                foreach (LeaderboardEntry entry in leaderboardData.entries)
                {
                    string line = $"{entry.userName},{entry.gameType},{entry.difficulty},{entry.completionTime:F2},{entry.tasksCompleted},{entry.totalTasks},{entry.date}";
                    writer.WriteLine(line);
                }
            }
            Debug.Log($"Leaderboard exported to CSV at: {csvFilePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error exporting to CSV: {e.Message}");
        }
    }


    public string GetJSONPath()
    {
        return leaderboardFilePath;
    }

    public string GetCSVPath()
    {
        return Path.Combine(Application.persistentDataPath, "leaderboard_complete_operation.csv");
    }

    public List<LeaderboardEntry> GetAllEntries()
    {
        LoadLeaderboard(); // Ensure we have the latest data
        return leaderboardData.entries;
    }
}