using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class FIrebase2 : MonoBehaviour
{

    private const string DB_URL = "https://rt-score-db-default-rtdb.europe-west1.firebasedatabase.app/";

    [Serializable]
    public struct PlayerScore
    {
        public string playerName;
        public float playerReactionTime;
    };


    public static IEnumerator UploadPlayerScore(String name, float rt)
    {

        PlayerScore player;
        player.playerName = name;
        player.playerReactionTime = rt;

        UnityWebRequest www = new UnityWebRequest(DB_URL + name + ".json", UnityWebRequest.kHttpVerbPUT, new DownloadHandlerBuffer(), new UploadHandlerRaw(new System.Text.UTF8Encoding().GetBytes(JsonUtility.ToJson(player))));


        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Saved Player Reaction Time Successfully");
        }
        else
        {
            Debug.Log(www.error);
        }

    }


}
