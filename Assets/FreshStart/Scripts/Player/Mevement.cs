using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Mevement : MonoBehaviourPunCallbacks
{
    public float playerDirection;

    [SerializeField] private float moveSpeed;
    [SerializeField] private TMP_Text nameText;

    private bool stopMove;

    Rigidbody2D rigidHero;
    Animator animHero;
    Health health;

    public PhotonView view;

    public Vector2 moveVector;

    public Character characterDropdown;
    public enum Character { Boi, Daisy, LonelyBoy, OldMan};

    void Start()
    {
        //Catching Components from player
        rigidHero = GetComponent<Rigidbody2D>();
        animHero = GetComponent<Animator>();
        view = GetComponent<PhotonView>();
        health = GetComponent<Health>();

        //Remove Gravity
        rigidHero.gravityScale = 0.0f;
    }


    void Update()
    {
        if (PhotonNetwork.IsConnected && !view.IsMine)
        {
            if (nameText.text == "")
            {
                nameText.text = view.Owner.NickName;
            }
            return;
        }
        if (!MultiplayerGameManager.instance.gameStarted)
        {
            return;
        }

        if (nameText.text == "") nameText.text = PhotonNetwork.NickName;

        if (health.dead)
        {
            stopMove = true;            
        }

        //Idle Direction Set
        SetIdleDirection();

        if (!stopMove)
        {
            //Vector Set
            moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }


        //Animator
        animHero.SetFloat("Horizontal", moveVector.x);
        animHero.SetFloat("Vertical", moveVector.y);
        animHero.SetFloat("Speed", moveVector.sqrMagnitude);


    }

    void FixedUpdate()
    {
        if (PhotonNetwork.IsConnected && !view.IsMine)
        {
            return;
        }

        //Move without velocity changings
        rigidHero.MovePosition(rigidHero.position + moveVector.normalized * Time.fixedDeltaTime * moveSpeed);
        //transform.position += (Vector3)moveVector.normalized * Time.fixedDeltaTime * moveSpeed;
    }

    void SetIdleDirection()
    {
        //Changing idle direction depends on to last pressed key
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            playerDirection = 1.0f;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            playerDirection = 0.66f;
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            playerDirection = 0.33f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            playerDirection = 0.0f;
        }

        animHero.SetFloat("IdleDir", playerDirection);
    }
}
