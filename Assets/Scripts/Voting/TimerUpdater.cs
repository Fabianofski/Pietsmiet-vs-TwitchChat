using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUpdater : MonoBehaviour
{
    TextMeshProUGUI Timer;
    int oldTime;
    [SerializeField] Image TimerCircleImage;
    [SerializeField] RectTransform TextRect;

    [Header("LeanTween Types")]
    [SerializeField] LeanTweenType CircleLeanType;
    [SerializeField] LeanTweenType TimeLeanType;
    [SerializeField] LeanTweenType CircleVanishLeanType;

    private void Awake()
    {
        Timer = GetComponent<TextMeshProUGUI>();
    }

    public void VotingsOpened(bool _state)
    {
        if (_state)
        {
            TextRect.localScale = Vector3.one;
            TimerCircleImage.rectTransform.localScale = Vector3.one;
        }
    }

    public void UpdateTimer(float time)
    {
        Timer.text = $"{Mathf.Round(time)}";
        int currentTime = Mathf.CeilToInt(time);

        if(currentTime != oldTime && currentTime != 0)
        {
            LeanTween.value(0f, 1f, 1f).setEase(CircleLeanType).setOnUpdate((float value) => 
            { TimerCircleImage.fillAmount = value;})           .setOnComplete(() =>
            { TimerCircleImage.rectTransform.localScale = Vector3.one; }); 

            TextRect.LeanScale(Vector3.zero, .5f).setEase(TimeLeanType).setOnComplete(() => 
            { TextRect.LeanScale(Vector3.one, .2f); });
        }
        if (currentTime == 0)
        {
            TimerCircleImage.rectTransform.LeanScale(Vector3.zero, 0.5f).setEase(CircleVanishLeanType);
            TextRect.LeanScale(Vector3.zero, 0.5f).setEase(CircleVanishLeanType);
        }

        oldTime = currentTime;
    }


}
