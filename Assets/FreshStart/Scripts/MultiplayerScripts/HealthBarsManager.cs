using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthBarsManager : MonoBehaviour
{
    private Health player1;
    private Health player2;

    private HealthBar healthBar1;
    private HealthBar healthBar2;

    void Start()
    {
        
    }


    void Update()
    {
        if (healthBar1 == null)
        {
            healthBar1 = GameObject.FindGameObjectWithTag("Player1HealthBar").GetComponent<HealthBar>();
        }
        if (healthBar2 == null)
        {
            healthBar2 = GameObject.FindGameObjectWithTag("Player2HealthBar").GetComponent<HealthBar>();
        }

        var playerList = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in playerList)
        {
            if (player.GetComponent<Mevement>().view.IsMine)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    player1 = player.GetComponent<Health>();
                }
                else
                {

                }
            }
        }
    }
}
