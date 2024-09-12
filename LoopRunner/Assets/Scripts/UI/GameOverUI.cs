using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button restartButton;

    private void Start()
    {
        restartButton.onClick.AddListener(() =>
        {
            Debug.Log("Pressed");

            UIManager.Instance.CloseGameoverUI();
        });
    }

    public void SetScoreText(int resultScore)
    {
        scoreText.text = string.Format($"{resultScore}");
    }
}
