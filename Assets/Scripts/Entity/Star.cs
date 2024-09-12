using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public bool IsSpread = false;

    [SerializeField] private float spreadSpeed;

    public void SetSpreadEffect()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        IsSpread = true;
    }

    public void ChangeColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    private void Update()
    {
        if(IsSpread)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(55, 55, 55), spreadSpeed * Time.deltaTime);
            GetComponent<SpriteRenderer>().sortingOrder = 10;
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingOrder = 8;
        }

        if (transform.localScale.x > 40)
        {
            GameManager.Instance.IsStarSpreadComplete = true;
            Destroy(gameObject, 0.1f);
        }
    }
}
