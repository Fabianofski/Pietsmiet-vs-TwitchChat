using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityAtoms.BaseAtoms;

public class EndscreenDisplay : MonoBehaviour
{

    [Header("Endscreen")]
    [SerializeField] GameObject Game;
    [SerializeField] GameObject Confetti;
    [SerializeField] GameObject Endscreen;
    [SerializeField] TextMeshProUGUI WinnerText;

    [Header("Points")]
    [SerializeField] IntReference PSPoints;
    [SerializeField] IntReference ChatPoints;


    public void ActivateEndscreen()
    {
        Endscreen.SetActive(true);
        Game.SetActive(false);

        if (PSPoints.Value < ChatPoints.Value)
            WinnerText.text = "DER CHAT!";
        else if(ChatPoints.Value < PSPoints.Value)
            WinnerText.text = "TEAM PIETSMIET!";
        else
            WinnerText.text = "KEINER! UNENTSCHIEDEN!";
    }

    public void ActivateConfetti()
    {
        Confetti.SetActive(true);
    }

}
