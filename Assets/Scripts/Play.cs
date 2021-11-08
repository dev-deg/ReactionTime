using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public TMPro.TMP_InputField playerName;
    public TMPro.TMP_Text playerRt;
    public TMPro.TMP_Text playerScores;

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "Lobby" && playerName && GameData.name != "")
        {
            playerName.text = GameData.name;
        }else if (SceneManager.GetActiveScene().name == "HighScores" && playerRt)
        {
            GameData.rt = ((GameData.rt1 + GameData.rt2 + GameData.rt3) / 3);
            playerRt.text = "YOUR AVG TIME: " + GameData.rt.ToString("N3") + "s";
            playerScores.text = GameData.scores;
            StartCoroutine(FireBaseController.UploadScore(GameData.name, GameData.rt));
        }
    }

    public void StartGame()
    {
        if (playerName.text != "")
        {
            GameData.name = playerName.text.Substring(0, 1).ToUpper() + playerName.text.Substring(1);
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
        
    }
    public void GoToLobby()
    {
            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);

    }

}
