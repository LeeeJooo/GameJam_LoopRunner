using Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public static TestManager Instance { get; private set; }

    [SerializeField] private GameObject pointPrefab;
    public bool IsTestMode = false;

    private void Awake()
    {
        if(Instance == null)
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
        if(IsTestMode)
        {
            GameManager.Instance.ChangeState(GameState.Game);
            Ball ball = GameObject.FindWithTag(Global.Strings.PLAYER).GetComponent<Ball>();

            if (ball != null)
            {
                ball.BallState = BallState.Move;
            }
            
            for(int i = 0; i < StatManager.Instance.StatDataSO.PointList.Count; i++)
            {
                Vector2 vector = StatManager.Instance.StatDataSO.PointList[i];

                vector.x -= StatManager.Instance.StatDataSO.X / 2;
                vector.y -= StatManager.Instance.StatDataSO.Y / 2;

                Point point = Instantiate(pointPrefab, vector, Quaternion.identity).GetComponent<Point>();
                MapManager.Instance.CurrentPointList.Add(point);
            }

            for(int i = 0; i < MapManager.Instance.CurrentPointList.Count; i++)
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

            ball.SetNewPath(MapManager.Instance.CurrentPointList[0], MapManager.Instance.CurrentPointList[1]);
            ball.transform.position = MapManager.Instance.CurrentPointList[0].transform.position;
        }
    }
}
