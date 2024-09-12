using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreNumberText;
    [SerializeField] private TextMeshProUGUI NowGoalCntText;
    [SerializeField] private TextMeshProUGUI GoalCntText;
    [SerializeField] private Button touchButton;

    void Start()
    { 
        GameManager.Instance.OnScoreChanged += Ingame_UI_OnScoreChanged;


        EventTrigger touchButtonTrigger = touchButton.GetComponent<EventTrigger>();

        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) =>
        {
            OnPointerDown((PointerEventData)e);
        });

        touchButtonTrigger.triggers.Add(pointerDown);
    }

    private void OnPointerDown(PointerEventData e)
    {
        GameManager.Instance.PressMainButton();
    }

    public void SetScore(int currentScore)
    {
        scoreNumberText.text = string.Format($"{currentScore}"); ;
    }

    public void SetNowGoal(int currentNowGoal)
    {
        NowGoalCntText.text = string.Format($"{currentNowGoal}");
    }

    public void SetGoal(int currentGoal)
    {
        GoalCntText.text = string.Format($"{currentGoal}");
    }

    private void Ingame_UI_OnScoreChanged(int currentScore)
    {
        scoreNumberText.text = string.Format($"{currentScore}");
    }
}