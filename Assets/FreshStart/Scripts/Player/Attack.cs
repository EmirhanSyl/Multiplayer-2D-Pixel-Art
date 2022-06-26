using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Attack : MonoBehaviour
{
    private Animator animator;

    private Health health;
    private Mevement movement;
    private SwordAttack swordAttack;

    private void Awake()
    {
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        movement = GetComponent<Mevement>();
        swordAttack = GetComponentInChildren<SwordAttack>();
    }
    
    void Update()
    {
        if (health.dead || (!movement.view.IsMine && PhotonNetwork.IsConnected))
        {
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Attack");
            animator.SetFloat("AttackDir", movement.playerDirection);
            swordAttack.SelectTransform(movement.playerDirection);
        }
    }
}
