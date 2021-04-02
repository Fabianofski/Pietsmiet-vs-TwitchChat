using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityAtoms.BaseAtoms;

public class QuestionIndexUpdater : MonoBehaviour
{
    TextMeshProUGUI QuestionIndexText;
    [SerializeField] AtomList Questions;

    private void Awake()
    {
        QuestionIndexText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(int _index)
    {
        QuestionIndexText.text = $"Frage {_index + 1}/{Questions.Value.Count}";
    }
}
