using UnityEngine;
using UnityEngine.UI;

public class OutGameUI : MonoBehaviour
{
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle vibrationToggle;
    [SerializeField] private Button touchButton;

    void Start()
    {
        touchButton.onClick.AddListener(() =>
        {
            GameManager.Instance.PressMainButton();
        });

        soundToggle.onValueChanged.AddListener((bool isOn) =>
        {
            SoundManager.Instance.CanPlaySound = !isOn;
            if (isOn)
            {
                SoundManager.Instance.StopAllBGM();
            }
            else
            {
                SoundManager.Instance.PlayBGM(0);
            }
        });

        //vibrationToggle.onValueChanged.AddListener((bool isOn) =>
        //{
        //    Vibration.CanVibrate = !isOn;
        //});
    }
}