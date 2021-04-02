using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;
using TMPro;

public class OptionSpawner : MonoBehaviour
{
    [SerializeField] AtomList Questions;

    [Header("Option Templates")]
    [SerializeField] GameObject StringOptionTemplate;
    [SerializeField] GameObject ImgOptionTemplate;
    RectTransform rect;
    [SerializeField] float padding;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SpawnOptions(int _index)
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);

        Question _currentQuestion = (Question)Questions.Value[_index];

        for (int i = 0; i < _currentQuestion.StringOptions.Count; i++)
        {
            GameObject stringOptionGO = Instantiate(StringOptionTemplate, transform);
            stringOptionGO.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _currentQuestion.StringOptions[i];
            stringOptionGO.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = i + 1 + "";
        }

        for (int i = 0; i < _currentQuestion.ImgOptions.Count; i++)
        {
            GameObject ImgOptionGO = Instantiate(ImgOptionTemplate, transform);
            Image image = ImgOptionGO.GetComponentInChildren<Image>();
            image.sprite = _currentQuestion.ImgOptions[i];
            SetImageSize(image);
        }

    }

    private void SetImageSize(Image image)
    {
        image.SetNativeSize();
        float height = image.rectTransform.sizeDelta.y;
        if (height > rect.sizeDelta.y)
        {
            float modifier = (rect.sizeDelta.y - padding) / height;
            image.rectTransform.sizeDelta = image.rectTransform.sizeDelta * modifier;
        }
    }
}
