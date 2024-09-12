using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Stat Data")]
public class StatDataSO : ScriptableObject
{
    [Header("�� �̵� �ӵ�")]
    [Range(0, 10)]
    public float BallMoveSpeed;
    [Space(10)]
    [Header("��ֹ� ����")]
    [Range(0, 20)]
    public float EnemyTargetRange;
    [Header("��ֹ� ���� Offset")]
    [Range(0, 10)]
    public float EnemySpawnOffset;
    [Header("��ֹ� ���� ����")]
    public int EnemySpawnAmount = 1;

    [Space(10)]
    [Header("���� ���")]
    public List<Color> Colors;

    [Space(10)]
    [Header("�� Grid")]
    public int X;
    public int Y;

    [Space(30)]
    [Header("�׽�Ʈ")]
    [Header("�� ���� ��Ÿ��")]
    public float EnemySpawnCooltime;
    [Header("�� �̵� �ӵ�")]
    public float EnemyMoveSpeed;

    [Space(10)]
    [Header("�� ��ǥ ���")]
    public List<Vector2> PointList;
}
