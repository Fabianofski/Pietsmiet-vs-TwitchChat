using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityAtoms.BaseAtoms;


public class VetoHost : MonoBehaviour
{

    [SerializeField] IntReference PSPoints;
    [SerializeField] IntReference ChatPoints;
    [SerializeField] bool VetoPietsmiet;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Veto()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        bool _correct = animator.GetBool("Correct");
        PhotonSync _photonSync = FindObjectOfType<PhotonSync>();

        if(VetoPietsmiet)
        {
            if (_correct)
            {
                PSPoints.Value--;
                _photonSync.PSAnswerDisplay(false);
            }
            else
            {
                PSPoints.Value++;
                _photonSync.PSAnswerDisplay(true);
            }
        }
        else
        {
            if (_correct)
            {
                ChatPoints.Value--;
                _photonSync.ChatAnswerDisplay(false);
            }
            else
            {
                ChatPoints.Value++;
                _photonSync.ChatAnswerDisplay(true);
            }
        }

        _photonSync.UpdateScore(PSPoints.Value, ChatPoints.Value);
    }

}
