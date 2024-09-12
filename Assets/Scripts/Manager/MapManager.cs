using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    public static bool IsLineRenderFinished = false;

    public Point[,] Grids;
    public List<Point> CurrentPointList;
    public int LastStarIndex = 0;

    [SerializeField] private GameObject linePrefab;


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
        int x = StatManager.Instance.StatDataSO.X;
        int y = StatManager.Instance.StatDataSO.Y;

        Grids = new Point[x, y];

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Point");
        foreach (GameObject obj in objects)
        {
            CurrentPointList.Add(obj.GetComponent<Point>());
        }

        SetNewPathRenderer(GameManager.Instance.LineSpawnTime);
    }

    public IEnumerator SetNewPathRenderer(float time)
    {
        float durationTime = time / CurrentPointList.Count;

        foreach (Point point in CurrentPointList)
        {
            Line line = Instantiate(linePrefab, transform.position, Quaternion.identity).GetComponent<Line>();

            line.Initialize(point.transform.position, point.firstNextPoint.transform.position, durationTime);
            line.SetLineRenderer();
            yield return new WaitUntil(() => line.IsFinished == true);
        }

        IsLineRenderFinished = true;
    }

    public void SpawnNewStar()
    {
        int randomIndex = Random.Range(0, CurrentPointList.Count);

        if(randomIndex == LastStarIndex)
        {
            randomIndex++;
            randomIndex %= CurrentPointList.Count;
        }

        LastStarIndex = randomIndex;
        CurrentPointList[randomIndex].SpawnGoal(StatManager.Instance.StatDataSO.Colors[GameManager.Instance.ColorIndex]);
    }

}
