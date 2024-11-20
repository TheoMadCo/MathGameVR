using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LeaderboardEntry
{
    public string userName;
    public string gameType;
    public float completionTime;  // In seconds
    public int tasksCompleted;
    public int totalTasks;
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
            DontDestroyOnLoad(gameObject);  // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject);  // Prevent duplicates
        }

        leaderboardFilePath = Path.Combine(Application.persistentDataPath, "leaderboard_complete_operation.json");
        LoadLeaderboard();
    }

    public void AddEntry(string userName, string gameType, float completionTime, int tasksCompleted, int totalTasks)
    {
        LeaderboardEntry newEntry = new LeaderboardEntry
        {
            userName = userName,
            gameType = gameType,
            completionTime = completionTime,
            tasksCompleted = tasksCompleted,
            totalTasks = totalTasks
        };

        leaderboardData.entries.Add(newEntry);
        SaveLeaderboard();
    }

    public void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboardData, true);
        File.WriteAllText(leaderboardFilePath, json);
        Debug.Log($"Leaderboard saved to: {leaderboardFilePath}");
    }

    public void LoadLeaderboard()
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
            Debug.Log("No leaderboard file found. Initialized empty leaderboard.");
        }
    }

    public void ExportToCSV()
    {
        string csvFilePath = Path.Combine(Application.persistentDataPath, "leaderboard_complete_operation.csv");
        using (StreamWriter writer = new StreamWriter(csvFilePath))
        {
            writer.WriteLine("UserName,GameType,CompletionTime,TasksCompleted,TotalTasks");
            foreach (LeaderboardEntry entry in leaderboardData.entries)
            {
                string line = $"{entry.userName},{entry.gameType},{entry.completionTime:F2},{entry.tasksCompleted},{entry.totalTasks}";
                writer.WriteLine(line);
            }
        }
        Debug.Log($"Leaderboard exported to CSV at: {csvFilePath}");
    }

    public string GetFilePath()
    {
        return leaderboardFilePath;  // JSON file location
    }
}
