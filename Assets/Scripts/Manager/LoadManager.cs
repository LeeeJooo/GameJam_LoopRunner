using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelGroupState
{
    public int BallSpeed;
    public int GoalCnt;
    public int ObstacleBatchSize;
    public float ObstacleCoolTime;
    public int ObstacleSpeed;
}

public class LevelState
{
    public int LevelId;
    public int LevelGroup;
    public List<List<int>> Path;
    public int BallSpeed;
    public int GoalCnt;
    public int ObstacleBatchSize;
    public float ObstacleCoolTime;
    public int ObstacleSpeed;
}

public class StaticState
{
    public int Width;
    public int Height;
    public int MaxLevelGroup;
}

public class Coordinate
{
    public int X;
    public int Y;
}

public class LoadManager : MonoBehaviour
{
    public static LoadManager Instance;

    private LevelGroupState[] levelGroupStates;
    public int LevelGroupSize;
    public static List<List<LevelState>> LevelStates;
    public int LevelSize;
    public static StaticState StaticStates;
    public static List<Coordinate> Coordinates;

    private void Awake()
    {
        Instance = this;
        loadLevelGroupDataset();
        loadLevelDataset();
        loadStaticDataset();
        loadCoordinateDataset();
    }

    void Start()
    {
        //logCoordinate();
        //logLevelStates();
        //logStaticStatse();
    }

    private void loadCoordinateDataset()
    {
        TextAsset csvData = Resources.Load("CoordinateDataset") as TextAsset;
        Coordinates = new();

        if (csvData != null)
        {
            string[] data = csvData.text.Split(new char[] { '\n' });

            int size = data.Length;
            for (int i = 1; i < size; i++)
            {
                string[] elements = data[i].Split(new char[] { ',' });
                if (elements[0] != "")
                {
                    int idx = int.Parse(elements[0]);
                    Coordinate coordinate = new Coordinate()
                    {
                        X = int.Parse(elements[1]),
                        Y = int.Parse(elements[2])
                    };
                    Coordinates.Add(coordinate);
                }
            }

            Debug.Log("Success To Load CoordinateDataset");
        }
        else
        {
            Debug.Log("Fail To Load CoordinateDataset");
        }
    }

    private void loadLevelGroupDataset()
    {
        TextAsset csvData = Resources.Load("LevelGroupDataset") as TextAsset;

        if (csvData != null)
        {
            string[] data = csvData.text.Split(new char[] { '\n' });
            LevelGroupSize = data.Length - 1;
            levelGroupStates = new LevelGroupState[LevelGroupSize];

            for (int i = 0; i < LevelGroupSize; i++)
            {
                string[] elements = data[i + 1].Split(new char[] { ',' });
                if (elements[0] != "")
                {
                    int NowLevelGroup = int.Parse(elements[0]);
                    LevelGroupState NowLevelGroupState = new LevelGroupState
                    {
                        BallSpeed = int.Parse(elements[1]),
                        GoalCnt = int.Parse(elements[2]),
                        ObstacleBatchSize = int.Parse(elements[3]),
                        ObstacleCoolTime = float.Parse(elements[4]),
                        ObstacleSpeed = int.Parse(elements[5])
                    };

                    levelGroupStates[NowLevelGroup] = NowLevelGroupState;
                }
            }

            Debug.Log("Success To Load LevelGroupDataset");
        }
        else
        {
            Debug.LogError("Failed To Load LevelGroupDataset");
        }
    }

    private void loadLevelDataset()
    {
        TextAsset csvData = Resources.Load("LevelDataset") as TextAsset;

        if (csvData != null)
        {
            string[] data = csvData.text.Split(new char[] { '\n' });
            LevelSize = data.Length - 1;

            LevelStates = new List<List<LevelState>>();
            for (int j = 0; j < LevelGroupSize; j++)
            {
                LevelStates.Add(new List<LevelState>());
            }

            for (int i = 0; i < LevelSize; i++)
            {
                string[] elements = data[i + 1].Split(new char[] { ',' });
                if (elements[0] != "")
                {
                    int NowLevelGroup = int.Parse(elements[1]);

                    List<List<int>> NowPaths = new();
                    List<int> NowPath = new List<int>(elements[2]
                        .Split(new char[] { ';' })
                        .Select(s => int.TryParse(s, out int result) ? result : 0)
                        .ToArray());
                    NowPaths.Add(NowPath);
                    List<int> NowPathReverse = new List<int>(NowPath);
                    NowPathReverse.Reverse();

                    NowPaths.Add(NowPathReverse);
                    LevelState NowLevelState = new LevelState
                    {
                        LevelId = int.Parse(elements[0]),
                        LevelGroup = NowLevelGroup,
                        Path = NowPaths,
                        BallSpeed = levelGroupStates[NowLevelGroup].BallSpeed,
                        GoalCnt = levelGroupStates[NowLevelGroup].GoalCnt,
                        ObstacleBatchSize = levelGroupStates[NowLevelGroup].ObstacleBatchSize,
                        ObstacleCoolTime = levelGroupStates[NowLevelGroup].ObstacleCoolTime,
                        ObstacleSpeed = levelGroupStates[NowLevelGroup].ObstacleSpeed
                    };

                    LevelStates[NowLevelGroup].Add(NowLevelState);
                }
            }

            Debug.Log("Success To Load LevelDataset");
        }
        else
        {
            Debug.LogError("Failed To Load LevelDataset");
        }
    }

    private void loadStaticDataset()
    {
        TextAsset csvData = Resources.Load("StaticDataset") as TextAsset;

        if (csvData != null)
        {
            string[] data = csvData.text.Split(new char[] { '\n' });

            string[] elements = data[1].Split(new char[] { ',' });

            StaticStates = new StaticState
            {
                Width = int.Parse(elements[0]),
                Height = int.Parse(elements[1]),
                MaxLevelGroup = int.Parse(elements[2])
            };

            Debug.Log("Success To Load StaticDataset");
        }
        else
        {
            Debug.LogError("Failed To Load LevelGroupDataset");
        }
    }

    private void logCoordinate()
    {
        StringBuilder sb = new StringBuilder();

        int index = 0;
        bool isWidthLine = true;

        sb.Append("[Coordinates Information]");
        sb.AppendLine();

        while (index < Coordinates.Count)
        {
            int count = isWidthLine ? StaticStates.Width : StaticStates.Height;

            for (int i = 0; i < count && index < Coordinates.Count; i++)
            {
                sb.Append($"({index})");
                sb.Append($"({Coordinates[index].X}, {Coordinates[index].Y})");

                if (i < count - 1)
                {
                    sb.Append(", ");
                }

                index++;
            }

            sb.AppendLine();
            isWidthLine = !isWidthLine;
        }

        Debug.Log(sb.ToString());
    }

    private void logLevelStates()
    {
        for (int i = 0; i < LevelGroupSize; i++)
        {
            for (int j = 0; j < LevelStates[i].Count; j++)
            {
                Debug.Log($"[{LevelStates[i][j].LevelId}] LevelGroup: {LevelStates[i][j].LevelGroup}, Path: [{string.Join(", ", LevelStates[i][j].Path[0])}] | [{string.Join(", ", LevelStates[i][j].Path[1])}], BallSpeed: {LevelStates[i][j].BallSpeed}, ObstacleBatchSize: {LevelStates[i][j].ObstacleBatchSize}, ObstacleCoolTime: {LevelStates[i][j].ObstacleCoolTime}, ObstacleSpeed: {LevelStates[i][j].ObstacleSpeed}");
            }
        }
    }

    private void logStaticStatse()
    {
        Debug.Log($"Width : {StaticStates.Width}, Height : {StaticStates.Height}, MaxLevelGroup : {StaticStates.MaxLevelGroup}");
    }
}
