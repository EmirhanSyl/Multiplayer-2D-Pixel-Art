using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject boi_PlayerPrefab;
    [SerializeField] private GameObject diana_playerPrefab;
    [SerializeField] private GameObject lonelyBoy_playerPrefab;
    [SerializeField] private GameObject oldMan_playerPrefab;

    [SerializeField] private GameObject ChooseYourCharacterPanel;

    [SerializeField] private GameObject selectBoiButton;
    [SerializeField] private GameObject selectDianaButton;
    [SerializeField] private GameObject selectLonelyBoyButton;
    [SerializeField] private GameObject selectOldManButton;

    [SerializeField] private Transform firstPlayerInstPos;
    [SerializeField] private Transform secondPlayerInstPos;

    private int playerCount;

    private GameObject player;
    private PhotonView view;

    private Vector2 initPos;

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            initPos = firstPlayerInstPos.position;
        }
        else
        {
            initPos = secondPlayerInstPos.position;
        }

        view = GetComponent<PhotonView>();
        ChooseYourCharacterPanel.SetActive(true);
        
    }

    public void RestartButton()
    {
        view.RPC("RestartLevel", RpcTarget.All);
    }

    [PunRPC]
    void RestartLevel()
    {
        PhotonNetwork.LoadLevel("FightArea");
    }

    #region CharacterChoose Buttons
    public void BoiButton()
    {
        PhotonNetwork.Instantiate(boi_PlayerPrefab.name, initPos, Quaternion.identity);
        if(selectBoiButton != null) selectBoiButton.SetActive(false);
        if (selectDianaButton != null) selectDianaButton.SetActive(false);
        if (selectLonelyBoyButton != null) selectLonelyBoyButton.SetActive(false);
        if (selectOldManButton != null) selectOldManButton.SetActive(false);

        view.RPC("SelectBoiButtonDisabler", RpcTarget.All);
        MultiplayerGameManager.instance.PlayerSelectedSound();
    }
    public void DianaButton()
    {
        PhotonNetwork.Instantiate(diana_playerPrefab.name, initPos, Quaternion.identity);
        if (selectBoiButton != null) selectBoiButton.SetActive(false);
        if (selectDianaButton != null) selectDianaButton.SetActive(false);
        if (selectLonelyBoyButton != null) selectLonelyBoyButton.SetActive(false);
        if (selectOldManButton != null) selectOldManButton.SetActive(false);

        view.RPC("SelectDianaButtonDisabler", RpcTarget.All);
        MultiplayerGameManager.instance.PlayerSelectedSound();
    }
    public void LonelyBoyButton()
    {
        PhotonNetwork.Instantiate(boi_PlayerPrefab.name, initPos, Quaternion.identity);
        if (selectBoiButton != null) selectBoiButton.SetActive(false);
        if (selectDianaButton != null) selectDianaButton.SetActive(false);
        if (selectLonelyBoyButton != null) selectLonelyBoyButton.SetActive(false);
        if (selectOldManButton != null) selectOldManButton.SetActive(false);

        view.RPC("SelectLonelyButtonDisabler", RpcTarget.All);
        MultiplayerGameManager.instance.PlayerSelectedSound();
    }
    public void OldManButton()
    {
        PhotonNetwork.Instantiate(boi_PlayerPrefab.name, initPos, Quaternion.identity);
        if (selectBoiButton != null) selectBoiButton.SetActive(false);
        if (selectDianaButton != null) selectDianaButton.SetActive(false);
        if (selectLonelyBoyButton != null) selectLonelyBoyButton.SetActive(false);
        if (selectOldManButton != null) selectOldManButton.SetActive(false);

        view.RPC("SelectOldManButtonDisabler", RpcTarget.All);
        MultiplayerGameManager.instance.PlayerSelectedSound();
    }
    #endregion

    #region SelectButtonDisablers
    [PunRPC]
    void SelectBoiButtonDisabler()
    {
        PhotonNetwork.Destroy(selectBoiButton);
        playerCount++;

        if (playerCount >= 2)
        {
            ChooseYourCharacterPanel.SetActive(false);
            MultiplayerGameManager.instance.GameStartingSounds();
        }
    }
    
    [PunRPC]
    void SelectDianaButtonDisabler()
    {
        PhotonNetwork.Destroy(selectDianaButton);
        playerCount++;

        if (playerCount >= 2)
        {
            ChooseYourCharacterPanel.SetActive(false);
            MultiplayerGameManager.instance.GameStartingSounds();
        }
    }
    
    [PunRPC]
    void SelectLonelyButtonDisabler()
    {
        PhotonNetwork.Destroy(selectLonelyBoyButton);
        playerCount++;

        if (playerCount >= 2)
        {
            ChooseYourCharacterPanel.SetActive(false);
            MultiplayerGameManager.instance.GameStartingSounds();
        }
    }
    
    [PunRPC]
    void SelectOldManButtonDisabler()
    {
        PhotonNetwork.Destroy(selectOldManButton);
        playerCount++;

        if (playerCount >= 2)
        {
            ChooseYourCharacterPanel.SetActive(false);
            MultiplayerGameManager.instance.GameStartingSounds();
        }
    }
    #endregion
}
