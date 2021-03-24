using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class Timer : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] FloatConstant VotingTime;
    [SerializeField] FloatReference VotingTimer;
    [SerializeField] BoolReference VotingsOpen;

    void Update()
    {
        if (VotingsOpen.Value)
            CountTimer();
    }

    void CountTimer()
    {
        VotingTimer.Value -= Time.deltaTime;

        if (VotingTimer.Value < 0)
        {
            VotingTimer.Value = 0;
            VotingsOpen.Value = false;
        }
    }
}
