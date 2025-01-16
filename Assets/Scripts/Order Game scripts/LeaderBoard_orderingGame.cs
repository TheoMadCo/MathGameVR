using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

[System.Serializable]
public class OrderingGameLeaderboardEntry
{
    public string userName;
    public string difficulty;
    public int pointsEarned;
    public int totalPoints;
    public string date;
}

[System.Serializable]
public class OrderingGameLeaderboardData
{
    public List<OrderingGameLeaderboardEntry> entries = new List<OrderingGameLeaderboardEntry>();
}

public class LeaderBoard_orderingGame : MonoBehaviour
{
    public static LeaderBoard_orderingGame Instance { get; private set; }

    private string leaderboardFilePath;
    private OrderingGameLeaderboardData leaderboardData;

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

        leaderboardFilePath = Path.Combine(Application.persistentDataPath, "leaderboard_ordering_game.json");
        LoadLeaderboard();
    }

    public void AddEntry(string playerName, string difficulty, int pointsEarned, int totalPoints)
    {
        OrderingGameLeaderboardEntry newEntry = new OrderingGameLeaderboardEntry
        {
            userName = playerName, // Save the player name
            difficulty = difficulty,
            pointsEarned = pointsEarned,
            totalPoints = totalPoints,
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        LoadLeaderboard();
        leaderboardData.entries.Add(newEntry);
        SaveLeaderboard();
        ExportToCSV();
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
                leaderboardData = JsonUtility.FromJson<OrderingGameLeaderboardData>(json);
                Debug.Log("Leaderboard loaded successfully.");
            }
            else
            {
                leaderboardData = new OrderingGameLeaderboardData();
                Debug.Log("No leaderboard file found. Created new leaderboard.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading leaderboard: {e.Message}");
            leaderboardData = new OrderingGameLeaderboardData();
        }
    }

    public void ExportToCSV()
    {
        try
        {
            string csvFilePath = Path.Combine(Application.persistentDataPath, "leaderboard_ordering_game.csv");
            using (StreamWriter writer = new StreamWriter(csvFilePath))
            {
                writer.WriteLine("UserName,Difficulty,PointsEarned,TotalPoints,Date");
                foreach (OrderingGameLeaderboardEntry entry in leaderboardData.entries)
                {
                    string line = $"{entry.userName},{entry.difficulty},{entry.pointsEarned},{entry.totalPoints},{entry.date}";
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
}
