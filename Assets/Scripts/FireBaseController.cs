using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Database;
using Firebase.Extensions;

//https://firebase.google.com/docs/database/unity/save-data

[Serializable]
public class PlayerScoreDetails
{
    public string _playerName;
    public PlayerResult _playerResult;

    public PlayerScoreDetails (string playerName, PlayerResult result)
    {
        this._playerName = playerName;
        this._playerResult = result;
    }
}


[Serializable]
public class PlayerResult
{
    public float _rt;
    public string _date;

    public PlayerResult(float rt, string date)
    {
        this._rt = rt;
        this._date = date;
    }
}


public class FireBaseController : MonoBehaviour
{

    private static DatabaseReference reference;
    public static List<PlayerScoreDetails> highscores = new List<PlayerScoreDetails>();

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.GetReference("Scores");
    }

    public static IEnumerator UploadScore(string playerName,  float rt)
    {
        yield return reference.Child(playerName).SetRawJsonValueAsync(
            JsonUtility.ToJson(new PlayerResult(rt,DateTime.Now.ToString()))
            );
    }

    public static IEnumerator GetScores()
    {
        yield return reference.OrderByChild("_rt").LimitToFirst(5).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception.Message);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                
                if ((snapshot != null) && (snapshot.ChildrenCount > 0))
                {

                    // Load highscores into list
                    foreach (var child in snapshot.Children)
                    {
                        highscores.Add(new PlayerScoreDetails(child.Key,
                            JsonUtility.FromJson<PlayerResult>(child.GetRawJsonValue())));
                    }

                    // Convert highscores into text
                    GameData.scores = "";

                    Debug.Log(highscores.Count);
                    foreach (PlayerScoreDetails score in highscores)
                    {
                        string tab = score._playerName.Length > 5 ? (score._playerName.Length > 10 ? "\t" : "\t\t") : "\t\t\t";
                        GameData.scores = GameData.scores + score._playerName + tab + score._playerResult._rt.ToString("N3") + "s\n";
                    }
                    SceneManager.LoadScene("HighScores");
                }
            }
        });
    }

}
