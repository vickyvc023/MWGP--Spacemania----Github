using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playButton;
    public GameObject playerShip;
    public GameObject enemySpawner;
    public GameObject GameOver;
    public GameObject scoreUIText;
    public GameObject TimeCounter;
    public GameObject GameTitle;

    public enum GameManagerState
    {
        Opening,
        Gameplay,
        GameOver,
    }

    GameManagerState GMState;

    // Start is called before the first frame update
    void Start()
    {
        GMState = GameManagerState.Opening;
    }

    void UpdateGameManagerState()
    {
        switch(GMState)
        {
            case GameManagerState.Opening:

                GameOver.SetActive(false);

                GameTitle.SetActive(true);

                playButton.SetActive(true);

                break;
            case GameManagerState.Gameplay:

                scoreUIText.GetComponent<GameScore>().Score = 0;

                playButton.SetActive(false);

                GameTitle.SetActive(false);

                playerShip.GetComponent<PlayerControl>().Init();

                enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();

                TimeCounter.GetComponent<TimeCounter>().StartTimeCounter();

                break;

            case GameManagerState.GameOver:
                TimeCounter.GetComponent<TimeCounter>().StopTimeCounter();

                enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();

                GameOver.SetActive(true);

                Invoke("ChangeToOpeningState", 8f);

                break;
        }
    }

    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    public void StartGamePlay()
    {
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();
    }

    public void ChangeToOpeningState()
    {
        SetGameManagerState(GameManagerState.Opening);
    }
}
