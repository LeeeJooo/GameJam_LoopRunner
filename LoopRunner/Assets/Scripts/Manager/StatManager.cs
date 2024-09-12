using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }

    [field:SerializeField] public StatDataSO StatDataSO { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector2.zero, StatDataSO.EnemyTargetRange);
        Gizmos.color = Color.blue;
        for(int i = - StatDataSO.X / 2; i <= StatDataSO.X / 2; i++)
        {
            for(int j =  - StatDataSO.Y / 2; j <= StatDataSO.Y /2; j++)
            {
                Gizmos.DrawWireCube(new Vector2(i, j), new Vector2(1, 1));
            }
        }
        Gizmos.color = Color.yellow;
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        Gizmos.DrawWireCube(Vector2.zero, new Vector2(-bottomLeft.x + topRight.x + StatDataSO.EnemySpawnOffset * 2, -bottomLeft.y + topRight.y + StatDataSO.EnemySpawnOffset * 2));

        Gizmos.color = Color.green;
        for (int i = 0; i < StatDataSO.PointList.Count; i++)
        {
            Vector2 vector = StatDataSO.PointList[i];

            vector.x -= StatDataSO.X / 2;
            vector.y -= StatDataSO.Y / 2;

            Gizmos.DrawWireSphere(vector, 0.1f);
        }

    }
}
