using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private int xPos;
    private int yPos;

    public Point firstNextPoint;
    public Point secondNextPoint;

    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private SpriteRenderer backgroundRenderer;

    public void SetBackgroundColor(Color color)
    {
        backgroundRenderer.color = color;
    }

    public void SetPointPosition(int xPos, int yPos)
    {
        this.xPos = xPos;
        this.yPos = yPos;
    }

    public void SetNextPoint(bool isFirstPath, Point point)
    {
        if (isFirstPath)
        {
            firstNextPoint = point;
        }
        else
        {
            secondNextPoint = point;
        }
    }

    public Point GetNextPoint(bool isFirstPath)
    {
        if(isFirstPath)
        {
            return firstNextPoint;
        }
        else
        {
            return secondNextPoint;
        }
    }

    public void SpawnGoal(Color color)
    {
        GameObject starObject = Instantiate(goalPrefab, transform.position, Quaternion.identity);
        DotweenUtils.PopScaleUp(starObject.transform, Global.Vectors.STARSCALE);
        starObject.GetComponent<SpriteRenderer>().color = color;
    }

}
