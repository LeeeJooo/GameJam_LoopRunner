using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float moveSpeed;
    private Vector2 direction;
    private Vector3 bottomLeft;
    private Vector3 topRight;
    private float spawnDistance;

    [SerializeField] private float rotateSpeed;

    public void Initialize(float moveSpeed, Vector2 randomPos)
    {
        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        spawnDistance = StatManager.Instance.StatDataSO.EnemySpawnOffset;
        Vector3 spawnPosition = Vector3.zero;

        int randomSide = Random.Range(0, 2);
        float yOffset = StatManager.Instance.StatDataSO.Y / 2f;
        switch (randomSide)
        {
            case 0:
                spawnPosition = new Vector3(bottomLeft.x - spawnDistance, Random.Range(-yOffset, yOffset), 0);
                break;
            case 1:
                spawnPosition = new Vector3(topRight.x + spawnDistance, Random.Range(-yOffset, yOffset), 0);
                break;
        }

        this.moveSpeed = moveSpeed;
        transform.position = spawnPosition;
        direction = (randomPos - (Vector2)spawnPosition).normalized;
    }

    private void Update()
    {
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

        if(transform.position.x < bottomLeft.x - spawnDistance || transform.position.x > topRight.x + spawnDistance
            || transform.position.y < bottomLeft.y - spawnDistance || transform.position.y > topRight.y + spawnDistance)
        {
            Destroy(gameObject);
        }
    }

}
