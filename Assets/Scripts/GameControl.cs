using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public Text gameText;
    public Image gameBG;

    private Coroutine timer;
    private float startTime = 0f;
    private bool isTicking = false;
    private bool isStoppable = false;
    private int gameStage = 0;
    
    void Start()
    {
        gameText.text = "Click to Start!";
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (!isTicking)
            {
                if (gameStage == 3)
                {
                    StartCoroutine(FireBaseController.GetScores());
                }
                else
                {
                    isTicking = true;
                    gameText.text = "Wait for Green";
                    gameBG.color = Color.red;
                    timer = StartCoroutine(ReactionTimer());
                } 
            }
            else
            {
                if (isStoppable)
                {
                    StopCoroutine(timer);
                    isTicking = false;
                    isStoppable = false;
                    gameBG.color = Color.black;
                    float reactionTime = (Time.time - startTime);
                    gameStage++;
                    gameText.text = GameData.name + "'s Reaction Time is " + reactionTime.ToString("N3") + "s\n" +
                    (reactionTime < 0.3f ?  " You are a Pro!" : "You are a Noob!") + "\nGame [" + gameStage + "/3]";
                    switch (gameStage)
                    {
                        case 1:
                            GameData.rt1 = reactionTime;
                            break;
                        case 2:
                            GameData.rt2 = reactionTime;
                            break;
                        case 3:
                            GameData.rt3 = reactionTime;
                            break;
                        default:
                            break;
                    }                    
                }
                else
                {
                    StopCoroutine(timer);
                    gameText.text = "Too early!\nClick to start again!";
                    isTicking = false;
                    isStoppable = false;
                    gameBG.color = Color.black;
                }
            }
        }

    }

    private IEnumerator ReactionTimer()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 5f));
        gameBG.color = Color.green;
        startTime = Time.time;
        isStoppable = true;
    }
}
