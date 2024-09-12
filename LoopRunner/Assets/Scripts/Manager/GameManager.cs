using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum GameState
{
    BeforeStart,
    BeforeGame,
    Game,
    Clear,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<int> OnScoreChanged;
    public event Action OnPathChanged;
    public event Action OnMainButtonPressed;

    [field:SerializeField] public StageController StageController {  get; private set; }
    
    public int CurrentScore { get; private set; }
    public int CurrentStarGet;
    public int CurrentLevel = -1;
    public int CurrentLevelId = 0;

    public bool CanPress = true;


    public GameState GameState { get; private set; }
    public int ColorIndex = -1;

    [Header("Enemy Spawn")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float enemySpawnCooltime;
    private float enemySpawnCooltimer;

    [Header("Star Get Directing")]
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private GameObject pointPrefab;
    public bool IsStarSpreadComplete = false;
    public float PointSpawnTime = 0.1f;
    public float LineSpawnTime = 0.5f;
    public float BallSpawnTime = 0.2f;
    public float BallMoveTime = 0.2f;

    [Header("Testing")]
    public int GoalStarGet = 3;
    private int amountOfEnemy = 1;
    private float speedOfEnemy = 3f;

    [Header("Main Screen Points Prefab")]
    [SerializeField] private GameObject mainScreenPointsPrefab;
    private GameObject mainScreenPointsObject;

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
        GameState = GameState.BeforeStart;

        enemySpawnCooltime = StatManager.Instance.StatDataSO.EnemySpawnCooltime;
        OnMainButtonPressed += GameManager_OnMainButtonPressed;

        SetMainScreen();

        Application.targetFrameRate = 60;
    }

    public void PressMainButton()
    {
        OnMainButtonPressed?.Invoke();
    }

    private void GameManager_OnMainButtonPressed()
    {
        switch (GameState)
        {
            case GameState.BeforeStart:
                
                GameObject ballObject = GameObject.FindWithTag(Global.Strings.PLAYER);
                Ball ball = ballObject.GetComponent<Ball>();
                if (ball == null) Debug.LogError("There is no Ball in Scene");

                if(ball.BallState == BallState.BeforeGame)
                    ball.BallState = BallState.Move;

                UIManager.Instance.SetOutGameUIFade(false, 0.8f);

                break;
            case GameState.BeforeGame:
                Debug.Log("Pressed2");
                break;
            case GameState.Game:
                Debug.Log("Pressed3");
                OnPathChanged?.Invoke();
                break;
            case GameState.Clear:
                Debug.Log("Pressed4");
                break;
            case GameState.GameOver:
                Debug.Log("Pressed5");

                break;
        }
    }

    private void Update()
    {
        switch (GameState)
        {
            case GameState.BeforeStart:
                BeforeStart();
                break;
            case GameState.BeforeGame:
                BeforeGame();
                break;
            case GameState.Game:
                Game();
                break;
            case GameState.Clear:
                Clear();
                break;
            case GameState.GameOver:
                GameOver();
                break;
        }
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnMainButtonPressed?.Invoke();
        }
    }

    private void GameOver()
    {
        
    }

    private void BeforeGame()
    {
        
    }

    private void BeforeStart()
    {
        
    }

    public void StartNewGame()
    {
        CurrentLevel = -1;
        CurrentLevelId = 0;

        IsStarSpreadComplete = false;
        ChangeState(GameState.BeforeGame);
        StartGame();
        OnScoreChanged?.Invoke(0);
    }

    private void Game()
    {
        enemySpawnCooltimer -= Time.deltaTime;
        if(enemySpawnCooltimer < 0f)
        {
            enemySpawnCooltimer = enemySpawnCooltime;
            SpawnEnemy(amountOfEnemy);
        }
    }

    private void Clear()
    {
        StartCoroutine(StarGetCoroutine());
        ChangeState(GameState.BeforeGame);
    }

    public void ChangeState(GameState state)
    {
        GameObject ballObject = GameObject.FindWithTag(Global.Strings.PLAYER);
        Ball ball = ballObject.GetComponent<Ball>();
        if (ball == null) Debug.LogError("There is no Ball in Scene");

        GameState = state;

        if (state == GameState.BeforeStart)
        {
            CurrentScore = 0;
            CurrentStarGet = 0;
            ball.BallState = BallState.BeforeGame;
        }
        else if (state == GameState.BeforeGame)
        {
            CurrentStarGet = 0;
        }
        else if(state == GameState.Clear)
        {
            IsStarSpreadComplete = false;
            enemySpawnCooltimer = enemySpawnCooltime;
        }
    }

    private void SetMainScreen()
    {
        PickRandomColorIndex();

        mainScreenPointsObject = Instantiate(mainScreenPointsPrefab);
        Ball ball = GameObject.FindWithTag(Global.Strings.PLAYER).GetComponent<Ball>();
        ball.SetMainBall(mainScreenPointsObject.transform.GetChild(0).GetComponent<Point>(),
            mainScreenPointsObject.transform.GetChild(1).GetComponent<Point>()
            );
        ball.BallState = BallState.BeforeGame;

        Star star = mainScreenPointsObject.transform.GetChild(3).GetComponent<Star>();
        if(star == null)
        {
            Debug.LogError("There is no Star");
            return;
        }
        star.ChangeColor(StatManager.Instance.StatDataSO.Colors[ColorIndex]);
        star.GetComponent<SpriteRenderer>().sortingOrder = 8;
    }

    public void ChangeScore(int score)
    {
        CurrentScore += score;

        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void SpawnEnemy(int amount)
    {
        float targetRange = StatManager.Instance.StatDataSO.EnemyTargetRange;
        for(int i = 0; i < amount; i++)
        {
            Vector2 randomTargetPos = new Vector2(UnityEngine.Random.Range(-targetRange, targetRange), UnityEngine.Random.Range(-targetRange, targetRange));
            
            Enemy enemyObject = Instantiate(enemyPrefab, Vector2.zero, Quaternion.identity).GetComponent<Enemy>();
            enemyObject.Initialize(speedOfEnemy, randomTargetPos);
        }
    }

    private IEnumerator StarGetCoroutine()
    {
        GameObject ballObject = GameObject.FindWithTag(Global.Strings.PLAYER);
        ballObject.GetComponent<Ball>().BallState = BallState.BeforeMove;

        MapManager.IsLineRenderFinished = false;
        MapManager.Instance.LastStarIndex = 0;

        Star star = GameObject.FindWithTag(Global.Strings.STAR).GetComponent<Star>();

        if (star == null) Debug.LogError("There is no Star in Current Map");

        star.SetSpreadEffect();
        GameObject[] lineObject = GameObject.FindGameObjectsWithTag(Global.Strings.LINE);
        GameObject[] enemyObject = GameObject.FindGameObjectsWithTag(Global.Strings.ENEMY);
        GameObject[] pointObject = GameObject.FindGameObjectsWithTag(Global.Strings.POINT);
        yield return new WaitUntil(() => IsStarSpreadComplete == true);
        
        Camera.main.backgroundColor = StatManager.Instance.StatDataSO.Colors[ColorIndex];
        foreach (var line in lineObject)
        {
            Destroy(line.gameObject);
        }
        foreach(var enemy in enemyObject)
        {
            Destroy(enemy.gameObject);
        }
        foreach(var point in pointObject)
        {
            Destroy(point.gameObject);
        }
        ballObject.SetActive(false);
        MapManager.Instance.CurrentPointList.Clear();
        yield return new WaitForSeconds(PointSpawnTime);

        PickRandomColorIndex();

        LevelState levelState = StageController.GetStage(CurrentLevel, CurrentLevelId);
        CurrentLevel = levelState.LevelGroup;
        CurrentLevelId = levelState.LevelId;
        GoalStarGet = levelState.GoalCnt;
        enemySpawnCooltime = levelState.ObstacleCoolTime;
        amountOfEnemy = levelState.ObstacleBatchSize;
        speedOfEnemy = levelState.ObstacleSpeed;

        UIManager.Instance.SetIngameUI(true);
        UIManager.Instance.GetComponentInChildren<InGameUI>().SetGoal(GoalStarGet);
        UIManager.Instance.GetComponentInChildren<InGameUI>().SetNowGoal(0);
        UIManager.Instance.GetComponentInChildren<InGameUI>().SetScore(CurrentScore);
        // Spawn Points
        for (int i = 0; i < levelState.Path[0].Count ; i++)
        {
            Vector2 vector = new Vector2(LoadManager.Coordinates[levelState.Path[0][i]].X, LoadManager.Coordinates[levelState.Path[0][i]].Y);
            Debug.Log($"{i} index: X: {LoadManager.Coordinates[levelState.Path[0][i]].X}, Y: {LoadManager.Coordinates[levelState.Path[0][i]].Y}");

            Point point = Instantiate(pointPrefab, vector, Quaternion.identity).GetComponent<Point>();
            point.SetBackgroundColor(Camera.main.backgroundColor);
            DotweenUtils.PopScaleUp(point.transform, Global.Vectors.POINTSCALE);
            MapManager.Instance.CurrentPointList.Add(point);
        }

        for (int i = 0; i < MapManager.Instance.CurrentPointList.Count; i++)
        {
            if (i < MapManager.Instance.CurrentPointList.Count - 1)
            {
                Point firstPoint = MapManager.Instance.CurrentPointList[i];
                Point secondPoint = MapManager.Instance.CurrentPointList[i + 1];

                firstPoint.SetNextPoint(true, secondPoint);
                secondPoint.SetNextPoint(false, firstPoint);
            }
            else
            {
                Point firstPoint = MapManager.Instance.CurrentPointList[i];
                Point secondPoint = MapManager.Instance.CurrentPointList[0];

                firstPoint.SetNextPoint(true, secondPoint);
                secondPoint.SetNextPoint(false, firstPoint);
            }
        }

        // Spawn Line
        MapManager.Instance.StartCoroutine(MapManager.Instance.SetNewPathRenderer(LineSpawnTime));
        yield return new WaitUntil(() => MapManager.IsLineRenderFinished == true);

        // Spawn Ball & Star
        yield return new WaitForSeconds(BallSpawnTime);
        ballObject.SetActive(true);
        ballObject.GetComponent<Ball>().SetNewPath(MapManager.Instance.CurrentPointList[0], MapManager.Instance.CurrentPointList[1]);
        ballObject.GetComponent<Ball>().MoveSpeed = levelState.BallSpeed;
        ballObject.transform.position = MapManager.Instance.CurrentPointList[0].transform.position;
        DotweenUtils.PopScaleUp(ballObject.transform, Global.Vectors.BALLSCALE);

        MapManager.Instance.SpawnNewStar();

        yield return new WaitForSeconds(BallMoveTime);
        ballObject.GetComponent<Ball>().BallState = BallState.Move;
        ballObject.GetComponent<Ball>().bPathDirection = true;
        ChangeState(GameState.Game);

    }

    private IEnumerator BackToMainCoroutine()
    {
        IsStarSpreadComplete = false;
        GameObject ballObject = GameObject.FindWithTag(Global.Strings.PLAYER);
        ballObject.GetComponent<Ball>().BallState = BallState.BeforeGame;

        Star star = GameObject.FindWithTag(Global.Strings.STAR).GetComponent<Star>();
        if (star != null)
        {
            Destroy(star.gameObject);
        }
        star = Instantiate(starPrefab, Vector2.zero, Quaternion.identity).GetComponent<Star>();
        star.ChangeColor(new Color(77 / 255f,77 / 255f,77 / 255f));
        star.SetSpreadEffect();
        GameObject[] lineObject = GameObject.FindGameObjectsWithTag(Global.Strings.LINE);
        GameObject[] enemyObject = GameObject.FindGameObjectsWithTag(Global.Strings.ENEMY);
        GameObject[] pointObject = GameObject.FindGameObjectsWithTag(Global.Strings.POINT);
        yield return new WaitUntil(() => IsStarSpreadComplete == true);

        Camera.main.backgroundColor = new Color(77 / 255f,77 / 255f,77 / 255f);
        foreach (var line in lineObject)
        {
            Destroy(line.gameObject);
        }
        foreach (var enemy in enemyObject)
        {
            Destroy(enemy.gameObject);
        }
        foreach (var point in pointObject)
        {
            Destroy(point.gameObject);
        }
        MapManager.Instance.CurrentPointList.Clear();
        ChangeState(GameState.BeforeStart);

        SetMainScreen();
        UIManager.Instance.SetOutGameUIFade(true, 1.5f);
        yield return new WaitForSeconds(1 / 1.5f);
        star = mainScreenPointsObject.transform.GetChild(3).GetComponent<Star>();
        star.GetComponent<SpriteRenderer>().sortingOrder = 10;
        UIManager.Instance.GetComponentInChildren<GameOverUI>().gameObject.SetActive(false);

    }

    public void StartGame()
    {
        //PickRandomColorIndex();
        //Star star = Instantiate(starPrefab, Vector2.zero, Quaternion.identity).GetComponent<Star>();
        //star.ChangeColor(StatManager.Instance.StatDataSO.Colors[ColorIndex]);
        StartCoroutine(StarGetCoroutine());

 
    }

    private void PickRandomColorIndex()
    {
        int randomIndex = UnityEngine.Random.Range(0, StatManager.Instance.StatDataSO.Colors.Count);
        if(randomIndex == ColorIndex)
        {
            randomIndex++;
            randomIndex %= StatManager.Instance.StatDataSO.Colors.Count;
        }

        ColorIndex = randomIndex;
    }

    public void SetGameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.OpenGameoverUI();
    }

    public void GoToMain()
    {
        StartCoroutine(BackToMainCoroutine());
        ChangeState(GameState.BeforeStart);
    }


}
