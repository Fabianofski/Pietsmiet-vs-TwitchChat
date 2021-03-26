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


    public void SpawnOptions(int _index)
    {
        Question _currentQuestion = (Question)Questions.Value[_index];

        for (int i = 0; i < _currentQuestion.StringOptions.Count; i++)
        {
            GameObject stringOptionGO = Instantiate(StringOptionTemplate, transform);
            stringOptionGO.GetComponentInChildren<TextMeshProUGUI>().text = i + " " + _currentQuestion.StringOptions[i];
        }

        for (int i = 0; i < _currentQuestion.ImgOptions.Count; i++)
        {
            GameObject ImgOptionGO = Instantiate(ImgOptionTemplate, transform);
            ImgOptionGO.GetComponentInChildren<Image>().sprite = _currentQuestion.ImgOptions[i];
        }

    }

}
