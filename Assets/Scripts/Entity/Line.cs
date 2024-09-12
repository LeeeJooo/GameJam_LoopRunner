using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    private float renderTime;
    private float renderTimer = 0;
    private LineRenderer lineRenderer;
    private Vector2 currentEndPos;

    public bool IsFinished;

    public void Initialize(Vector2 startPos, Vector2 endPos, float renderTime)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.renderTime = renderTime;
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, startPos);
    }

    public void SetLineRenderer()
    {
        StartCoroutine(SetLineRendererCoroutine());
    }

    private IEnumerator SetLineRendererCoroutine()
    {
        while (renderTimer < renderTime)
        {
            renderTimer += Time.deltaTime;

            currentEndPos = Vector2.Lerp(startPos, endPos, renderTimer / renderTime);

            lineRenderer.SetPosition(1, currentEndPos);

            yield return null;
        }

        lineRenderer.SetPosition(1, endPos);
        IsFinished = true;
    }

}
