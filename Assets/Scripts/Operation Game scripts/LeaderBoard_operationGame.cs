using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

[System.Serializable]
public class OperationGameEntry
{
    public string userName;
    public string difficulty;
    public string operation;
    public int points;
    public int maxPossiblePoints;
    public string date; // Adding date for better tracking
}

[System.Serializable]
public class OperationGameLeaderboardData
{
    public List<OperationGameEntry> entries = new List<OperationGameEntry>();
}

public class LeaderBoard_operationGame : MonoBehaviour
{
    private string leaderboardFilePath;
    private OperationGameLeaderboardData leaderboardData;

    private void Awake()
    {
        // Set up file paths
        leaderboardFilePath = Path.Combine(Application.persistentDataPath, "leaderboard_operation_game.json");
        LoadLeaderboard();
    }

    public void AddEntry(string userName, string difficulty, string operation, int points, int maxPossiblePoints)
    {
        OperationGameEntry newEntry = new OperationGameEntry
        {
            userName = userName,
            difficulty = difficulty,
            operation = operation,
            points = points,
            maxPossiblePoints = maxPossiblePoints,
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
                leaderboardData = JsonUtility.FromJson<OperationGameLeaderboardData>(json);
                Debug.Log("Leaderboard loaded successfully.");
            }
            else
            {
                leaderboardData = new OperationGameLeaderboardData();
                Debug.Log("No leaderboard file found. Created new leaderboard.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading leaderboard: {e.Message}");
            leaderboardData = new OperationGameLeaderboardData();
        }
    }

    public void ExportToCSV()
    {
        try
        {
            string csvFilePath = Path.Combine(Application.persistentDataPath, "leaderboard_operation_game.csv");
            using (StreamWriter writer = new StreamWriter(csvFilePath))
            {
                writer.WriteLine("UserName,Difficulty,Operation,Points,MaxPossiblePoints,Date");
                foreach (OperationGameEntry entry in leaderboardData.entries)
                {
                    string line = $"{entry.userName},{entry.difficulty},{entry.operation},{entry.points},{entry.maxPossiblePoints},{entry.date}";
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

    // Method to get file paths for external access
    public string GetJSONPath()
    {
        return leaderboardFilePath;
    }

    public string GetCSVPath()
    {
        return Path.Combine(Application.persistentDataPath, "leaderboard_operation_game.csv");
    }

    // Method to get all entries (useful for displaying leaderboard)
    public List<OperationGameEntry> GetAllEntries()
    {
        LoadLeaderboard(); // Ensure we have the latest data
        return leaderboardData.entries;
    }
}