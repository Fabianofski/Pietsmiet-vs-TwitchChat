using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityAtoms.BaseAtoms;



public class NetworkManager : MonoBehaviourPunCallbacks
{

    public GameObject LobbyPanel;

    public TMP_InputField RoomCodeInput;
    [SerializeField] GameObjectEvent SetParentEvent;

    #region ConnectToServer

    private void Awake()
    {
        Connect();
    }

    public void Connect()
    {
        Debug.Log("Connect to Server");
        PhotonNetwork.GameVersion = "v0.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        LobbyPanel.SetActive(true);
        Debug.Log("Joined Lobby");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server: " + cause);
    }

    #endregion


    #region CreateRooms
    public void HostGame()
    {
        PhotonNetwork.CreateRoom(RoomCodeInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(RoomCodeInput.text);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Joining Room failed");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("QuizScene");
    }
    #endregion
}
