using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DotweenUtils{

    public static void PopScaleUp(Transform transform, Vector3 scale, float duration = 0.3f)
    {
        transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        transform.DOScale(scale, duration).SetEase(Ease.OutBack);
    }
}
