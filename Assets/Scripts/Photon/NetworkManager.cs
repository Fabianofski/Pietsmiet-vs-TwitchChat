using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityAtoms.BaseAtoms;



public class NetworkManager : MonoBehaviourPunCallbacks
{

    [SerializeField] GameObject LobbyPanel;
    [SerializeField] GameObject LoadingPanel;

    [SerializeField] TMP_InputField NickNameInput;
    [SerializeField] TMP_InputField RoomCodeInput;

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
        LoadingPanel.SetActive(false);
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
        PhotonNetwork.NickName = NickNameInput.text;
        PhotonNetwork.CreateRoom(RoomCodeInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.NickName = NickNameInput.text;
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
