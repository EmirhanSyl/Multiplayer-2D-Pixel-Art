using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
    //public static Health instance;

    [HideInInspector] public bool dead;

    [SerializeField] private float maxHealth;
    [SerializeField] private float godModeDuraiton = 1f;

    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private Image winnerFaceSet;
    [SerializeField] private Image loserFaceSet;

    [SerializeField] private Text winnerNickText;
    [SerializeField] private Text loserNickText;

    [SerializeField] private Sprite boiFaceset;
    [SerializeField] private Sprite daisyFaceset;
    [SerializeField] private Sprite lonelyBoyFaceset;
    [SerializeField] private Sprite oldManFaceset;


    private float currentHealth;

    private bool godMode;
    private Animator animator;
    private PhotonView view;
    private MultiplayerGameManager gameManager;

    private HealthBar player1HealthBar;
    private HealthBar player2HealthBar;

    private void Awake()
    {
        //instance = this;
        animator = GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MultiplayerGameManager>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected && !view.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
               if(player2HealthBar != null) player2HealthBar.SetHealth(currentHealth);
            }
            else
            {
                if (player1HealthBar != null) player1HealthBar.SetHealth(currentHealth);
            }

            return;
        }

        if (player1HealthBar == null)
        {
            player1HealthBar = GameObject.FindGameObjectWithTag("Player1HealthBar").GetComponent<HealthBar>();
        }
        if (player2HealthBar == null)
        {
            player2HealthBar = GameObject.FindGameObjectWithTag("Player2HealthBar").GetComponent<HealthBar>();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (player1HealthBar != null) player1HealthBar.SetHealth(currentHealth);
        }
        else
        {
            if (player2HealthBar != null) player2HealthBar.SetHealth(currentHealth);
        }


        if (currentHealth <= 0f)
        {
            PlayerDead();
            return;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerHitted(6);
        }
    }

    [PunRPC]
    public void PlayerHitted(float damageAmount)
    {
        if (dead || godMode)
        {
            return;
        }

        godMode = true;
        currentHealth -= damageAmount;
        StartCoroutine(DamageTakenCoroutine());

    }

    void PlayerDead()
    {
        dead = true;
        animator.SetBool("Dead", true);
        StartCoroutine(DeadCoroutine());
        view.RPC("GameOverRPC", RpcTarget.All);
    }

    IEnumerator DamageTakenCoroutine()
    {
        animator.SetLayerWeight(1, 1);
        yield return new WaitForSeconds(godModeDuraiton);
        animator.SetLayerWeight(1, 0);
        godMode = false;
    }

    IEnumerator DeadCoroutine()
    {
        yield return new WaitForSeconds(2f);
        PhotonNetwork.Destroy(gameObject);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            SwordAttack sword = collision.gameObject.GetComponent<SwordAttack>();
            //PlayerHitted(Random.Range(sword.minDamageAmount, sword.maxDamageAmount));
            view.RPC("PlayerHitted", RpcTarget.Others, Random.Range(sword.minDamageAmount, sword.maxDamageAmount));
        }
    }

    [PunRPC]
    void GameOverRPC()
    {
        gameManager.FinishPanelSet();
        Debug.Log("GameOverRPC");
    }

}
