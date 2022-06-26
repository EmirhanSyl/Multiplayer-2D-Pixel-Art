using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text createInput;
    [SerializeField] TMP_Text JoinInput;

    [SerializeField] TMP_Text nicknameText;

    RoomOptions roomOptions;

    public void CreateRoom()
    {
        //roomOptions = new RoomOptions();

        //if (maxPlayerText.text != "0" || maxPlayerText.text != "1" || maxPlayerText.text != "9")
        //{
        //    roomOptions.MaxPlayers = (byte)int.Parse(maxPlayerText.text);
        //}
        //else
        //{
        //    return;
        //}

        PhotonNetwork.NickName = nicknameText.text;
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.NickName = nicknameText.text;
        PhotonNetwork.JoinRoom(JoinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("FightArea");
    }

}
