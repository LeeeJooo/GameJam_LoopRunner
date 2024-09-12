using System;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    private int currentScore = 0;
    public int CurrentScore { get { return currentScore; } }

    public int CurrentLevelGroup = -1;

    public static TestGameManager Instance { get; private set; }

    public event Action<int> OnScoreChanged;
    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnGameRestart;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OnGameRestart?.Invoke();

        Application.targetFrameRate = 60;
    }

    public void StartGame()
    {
        currentScore = 0;
        //GameObject.FindWithTag("Player").GetComponent<Ball>().Initialize();
        OnGameStart?.Invoke();
        OnScoreChanged?.Invoke(currentScore);
    }

    public void ChangeScore(int score)
    {
        currentScore += score;

        OnScoreChanged?.Invoke(currentScore);
    }

    public void SetGameOver()
    {
        OnGameOver?.Invoke();

        //SoundManager.Instance.PlaySFX(Constant.SFXNums.DIE);
    }

    public void SetGameRestart()
    {
        CurrentLevelGroup = -1;

        OnGameRestart?.Invoke();
    }
}
