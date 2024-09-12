using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    void Start()
    {
        //TestStart();
    }

    public LevelState GetStage(int PriorLevelGroup, int PriorLevelId)
    {
        int Size = 0;
        int Idx = 0;
        int NowLevelGroup = PriorLevelGroup + 1;

        if (PriorLevelGroup < LoadManager.StaticStates.MaxLevelGroup)
        {
            Size = LoadManager.LevelStates[NowLevelGroup].Count;
            Idx = Random.Range(0, Size);

            return LoadManager.LevelStates[NowLevelGroup][Idx];
        }
        else
        {
            Size = LoadManager.LevelStates[LoadManager.StaticStates.MaxLevelGroup].Count;
            NowLevelGroup = Mathf.Min(NowLevelGroup, LoadManager.StaticStates.MaxLevelGroup);
            Idx = getIdx(PriorLevelId, Size, NowLevelGroup);

            LevelState result = LoadManager.LevelStates[NowLevelGroup][Idx];
            return result;
        }
    }

    public int GetGoalIdx(int PriorGoalIdx, List<int> Path)
    {
        int Size = Path.Count;

        bool Flag = true;
        int Idx = 0;
        while (Flag)
        {
            Idx = Random.Range(0, Size);

            if (Path[Idx] != PriorGoalIdx)
            {
                Flag = false;
            }
        }

        return Path[Idx];
    }

    private int getIdx(int PriorLevelId, int Size, int NowLevelGroup)
    {
        bool Flag = true;
        int Idx = 0;
        while (Flag)
        {
            Idx = Random.Range(0, Size);

            if (LoadManager.LevelStates[NowLevelGroup][Idx].LevelId == PriorLevelId)
            {
                Flag = false;
            }
        }

        return Idx;
    }

    private void logNowStage(LevelState result)
    {
        Debug.Log($"[GetMap] LevelId : {result.LevelId}, LevelGroup: {result.LevelGroup}, Path: [{string.Join(", ", result.Path[0])}] | [{string.Join(", ", result.Path[1])}], BallSpeed: {result.BallSpeed}, ObstacleBatchSize: {result.ObstacleBatchSize}, ObstacleCoolTime: {result.ObstacleCoolTime}, ObstacleSpeed: {result.ObstacleSpeed}");
    }

    private void logGoalIdx(int PriorGoalIdx, List<int> Path, int GoalIdx)
    {
        Debug.Log($"[GetGoalIdx] Prior Goal Idx : {PriorGoalIdx}, Path : {string.Join(",", Path)}, Now Goal Idx : {GoalIdx}");
    }

    private void TestStart()
    {
        LevelState result = GetStage(-1, 0);
        logNowStage(result);

        int PriorGoalIdx = 1;
        List<int> Path = new List<int> { 1, 5, 8 };
        int GoalIdx = GetGoalIdx(PriorGoalIdx, Path);
        logGoalIdx(PriorGoalIdx, Path, GoalIdx);
    }
}
