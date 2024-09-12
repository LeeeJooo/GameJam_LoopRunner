using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject ingameUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject outgameUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    public void OpenGameoverUI()
    {
        StartCoroutine(FadeCoroutine(ingameUI.GetComponent<CanvasGroup>(), false, 2f));
        gameOverUI.SetActive(true);
        StartCoroutine(FadeCoroutine(gameOverUI.GetComponent<CanvasGroup>(), true, 2f));
        gameOverUI.GetComponentInChildren<Button>().interactable = true;
        gameOverUI.GetComponent<GameOverUI>().SetScoreText(GameManager.Instance.CurrentScore);

    }

    public void CloseGameoverUI()
    {
        gameOverUI.GetComponentInChildren<Button>().interactable = false;
        StartCoroutine(FadeCoroutine(gameOverUI.GetComponent<CanvasGroup>(), false, 2f));
        GameManager.Instance.GoToMain();
    }


    public void SetIngameUI(bool isActive)
    {
        ingameUI.SetActive(isActive);
        if (isActive)
        {
            ingameUI.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    public void SetOutGameUIFade(bool fadeIn, float fadeSpeed)
    {
        if (fadeIn)
        {
            Toggle[] toggles = outgameUI.GetComponentsInChildren<Toggle>();
            foreach (Toggle toggle in toggles)
            {
                toggle.interactable = true;
            }
        }
        else
        {
            Toggle[] toggles = outgameUI.GetComponentsInChildren<Toggle>();
            foreach (Toggle toggle in toggles)
            {
                toggle.interactable = false;
            }
        }
        StartCoroutine(FadeCoroutine(outgameUI.GetComponent<CanvasGroup>(), fadeIn, fadeSpeed));
    }

    public IEnumerator FadeCoroutine(CanvasGroup canvas, bool fadeIn, float fadeSpeed)
    {
        if (fadeIn)
        {
            canvas.alpha = 0;
            float a = 0;

            while (a <= 1)
            {
                a += Time.deltaTime * fadeSpeed;
                canvas.alpha = a;
                yield return null;
            }
        }
        else
        {
            canvas.alpha = 1;
            float a = 1;

            while (a >= 0)
            {
                a -= Time.deltaTime * fadeSpeed;
                canvas.alpha = a;
                yield return null;
            }
        }
    }

}