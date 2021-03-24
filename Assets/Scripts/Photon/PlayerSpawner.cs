using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerSpawner : MonoBehaviour
{

    private void Awake()
    {
        if(!PhotonNetwork.IsMasterClient)
            PhotonNetwork.Instantiate("Player", Vector2.zero, Quaternion.identity);
        else
            PhotonNetwork.Instantiate("Host", Vector2.zero, Quaternion.identity);
    }

    public void SetParent(GameObject _child)
    {
        _child.GetComponent<RectTransform>().SetParent(transform.parent);
    }
}
