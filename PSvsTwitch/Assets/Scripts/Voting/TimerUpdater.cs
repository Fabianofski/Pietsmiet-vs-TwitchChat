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
    [SerializeField] LeanTweenType leanTweenType;

    private void Awake()
    {
        Timer = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateTimer(float time)
    {
        Timer.text = $"{Mathf.Round(time)}";
        int currentTime = Mathf.CeilToInt(time);
        if(currentTime != oldTime)
            LeanTween.value(0f, 1f, 1f).setEase(leanTweenType).setOnUpdate((float value) => 
            {TimerCircleImage.fillAmount = value; }) ;
        oldTime = currentTime;
    }


}
