using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BallState
{
    BeforeGame,
    BeforeMove,
    Move,
    Die,
}

public class Ball : MonoBehaviour
{
    public float MoveSpeed;

    public bool bPathDirection = true;

    public BallState BallState;

    private Point startPoint;
    private Point endPoint;

    private void Update()
    {
        switch(BallState)
        {
            case BallState.BeforeGame:
                BeforeGame();
                break;
            case BallState.BeforeMove:
                BeforeMove();
                break;
            case BallState.Move:
                Move();
                break;
            case BallState.Die:
                Die();
                break;
        }
    }

    private void Start()
    {
        bPathDirection = true;

        GameManager.Instance.OnPathChanged += Ball_OnPathChanged;
    }

    private void Ball_OnPathChanged()
    {
        bPathDirection = !bPathDirection;

        SetNewPath(endPoint, startPoint);
    }

    private void Die()
    {
    }

    private void Move()
    {
        Vector2 direction = (endPoint.transform.position - transform.position).normalized;
        float distance = (transform.position - endPoint.transform.position).magnitude;

        if (distance <= 0.1f)
        {
            SetNextPoint();
            return;
        }
        else
        {
            transform.position += MoveSpeed * (Vector3)direction * Time.deltaTime;
        }
    }

    private void BeforeMove()
    {
    }

    private void BeforeGame()
    {
    }

    public void SetNewPath(Point startPoint, Point endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }

    public void SetNextPoint()
    {
        if(bPathDirection)
        {
            startPoint = endPoint;
            endPoint = startPoint.firstNextPoint;
        }
        else
        {
            startPoint = endPoint;
            endPoint = startPoint.secondNextPoint;
        }
    }

    public void SetMainBall(Point startPoint, Point endPoint)
    {
        MoveSpeed = 5;

        transform.position = startPoint.transform.position;

        SetNewPath(startPoint, endPoint);
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Star star))
        {
            if (BallState != BallState.Move) return;

            if(GameManager.Instance.GameState != GameState.BeforeStart)
            {
                GameManager.Instance.ChangeScore(3);
            }
            else
            {
                GameManager.Instance.StartNewGame();
                return;
            }

            GameManager.Instance.CurrentStarGet++;
            UIManager.Instance.GetComponentInChildren<InGameUI>().SetNowGoal(GameManager.Instance.CurrentStarGet);
            if (GameManager.Instance.CurrentStarGet >= GameManager.Instance.GoalStarGet)
            {
                GameManager.Instance.ChangeState(GameState.Clear);
                BallState = BallState.BeforeMove;
            }
            else
            {
                Destroy(collision.gameObject);
                //bPathDirection = !bPathDirection;
                MapManager.Instance.SpawnNewStar();
            }
        }

        else if(collision.TryGetComponent(out Enemy enemy))
        {
            if (BallState != BallState.Move) return;

            GameManager.Instance.ChangeState(GameState.GameOver);
            BallState = BallState.Die;
            GetComponent<SpriteRenderer>().enabled = false;


            GameManager.Instance.SetGameOver();
        }
    }
}
