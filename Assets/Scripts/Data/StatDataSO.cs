using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Stat Data")]
public class StatDataSO : ScriptableObject
{
    [Header("공 이동 속도")]
    [Range(0, 10)]
    public float BallMoveSpeed;
    [Space(10)]
    [Header("장애물 범위")]
    [Range(0, 20)]
    public float EnemyTargetRange;
    [Header("장애물 스폰 Offset")]
    [Range(0, 10)]
    public float EnemySpawnOffset;
    [Header("장애물 스폰 개수")]
    public int EnemySpawnAmount = 1;

    [Space(10)]
    [Header("색상 목록")]
    public List<Color> Colors;

    [Space(10)]
    [Header("맵 Grid")]
    public int X;
    public int Y;

    [Space(30)]
    [Header("테스트")]
    [Header("적 생성 쿨타임")]
    public float EnemySpawnCooltime;
    [Header("적 이동 속도")]
    public float EnemyMoveSpeed;

    [Space(10)]
    [Header("점 좌표 목록")]
    public List<Vector2> PointList;
}
