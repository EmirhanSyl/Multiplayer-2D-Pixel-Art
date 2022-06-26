using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class MultiPlayerManager : MonoBehaviour
{
    public GameObject PlayerUiPrefab;

    public PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();

        if (PlayerUiPrefab != null)
        {
            GameObject _uiGo = Instantiate(PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            
        }
    }

   
    void Update()
    {
        
    }
    
}
