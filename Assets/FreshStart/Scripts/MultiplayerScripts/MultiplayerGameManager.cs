using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class MultiplayerGameManager : MonoBehaviour
{
    public static MultiplayerGameManager instance;

    public bool gameStarted;

    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject waitForHostText;
    [SerializeField] private GameObject healthBarsPanel;
    [Space(5)]
    [SerializeField] private Image winnerFaceSet;
    [SerializeField] private Image loserFaceSet;
    [SerializeField] private Image player1HealthImage;
    [SerializeField] private Image player2HealthImage;
    [Space(5)]
    [SerializeField] private Text winnerNickText;
    [SerializeField] private Text loserNickText;
    [SerializeField] private Text startText;
    [SerializeField] private Text player1NickText;
    [SerializeField] private Text player2NickText;
    [Space(5)]
    [SerializeField] private Sprite boiFaceset;
    [SerializeField] private Sprite daisyFaceset;
    [SerializeField] private Sprite lonelyBoyFaceset;
    [SerializeField] private Sprite oldManFaceset;

    [Header("Audio")]
    [Space(5)]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip chooseYourCharacterClip;
    [SerializeField] private AudioClip player1Clip;
    [SerializeField] private AudioClip player2Clip;
    [SerializeField] private AudioClip prepareYourselfClip;
    [Space(5)]
    [SerializeField] private AudioClip roundOneClip;
    [SerializeField] private AudioClip readyClip;
    [SerializeField] private AudioClip threeClip;
    [SerializeField] private AudioClip twoClip;
    [SerializeField] private AudioClip oneClip;
    [SerializeField] private AudioClip fightClip;
    [SerializeField] private AudioClip killHimClip;
    [SerializeField] private AudioClip killHerClip;
    [Space(5)]
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip looserClip;
    [SerializeField] private AudioClip winnerClip;
    [Space(5)]
    [SerializeField] private AudioClip selectionSoundTrackClip;
    [SerializeField] private AudioClip fightSoundTrackClip;
    [SerializeField] private AudioClip loserSoundTrackClip;
    [SerializeField] private AudioClip winnerSoundTrackClip;

    [SerializeField] private AudioSource SoundTrack;

    private float gameOverTimer;

    private bool setFinish;
    private string enemyNick;

    private Queue<AudioClip> playSoundsOnStart;

    private Mevement.Character characterType;
    private Mevement.Character enemyType;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playSoundsOnStart = new Queue<AudioClip>();

        audioSource.PlayOneShot(chooseYourCharacterClip);

        SoundTrack.clip = selectionSoundTrackClip;
        SoundTrack.Play();

        healthBarsPanel.SetActive(false);
    }

    private void Update()
    {
        if (enemyNick == null)
        {
            var playerList = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in playerList)
            {
                if (player.GetComponent<Mevement>().view.Owner.NickName != PhotonNetwork.NickName)
                {
                    enemyNick = player.GetComponent<Mevement>().view.Owner.NickName;
                    Debug.Log("Enemy Nick Catched");
                }

                if (player.GetComponent<Mevement>().view.IsMine)
                {
                    switch (player.GetComponent<Mevement>().characterDropdown)
                    {
                        case Mevement.Character.Boi:
                            characterType = Mevement.Character.Boi;
                            break;
                        case Mevement.Character.Daisy:
                            characterType = Mevement.Character.Daisy;
                            break;
                        case Mevement.Character.LonelyBoy:
                            characterType = Mevement.Character.LonelyBoy;
                            break;
                        case Mevement.Character.OldMan:
                            characterType = Mevement.Character.OldMan;
                            break;
                    }
                }
                else
                {
                    switch (player.GetComponent<Mevement>().characterDropdown)
                    {
                        case Mevement.Character.Boi:
                            enemyType = Mevement.Character.Boi;
                            break;
                        case Mevement.Character.Daisy:
                            enemyType = Mevement.Character.Daisy;
                            break;
                        case Mevement.Character.LonelyBoy:
                            enemyType = Mevement.Character.LonelyBoy;
                            break;
                        case Mevement.Character.OldMan:
                            enemyType = Mevement.Character.OldMan;
                            break;
                    }
                }
            }
        }

        if (!audioSource.isPlaying && playSoundsOnStart.Count > 0)
        {
            audioSource.clip = playSoundsOnStart.Dequeue();
            audioSource.Play();
            if (audioSource.clip == roundOneClip)
            {
                startText.gameObject.SetActive(true);
                startText.text = "ROUND - 1";

                SoundTrack.Stop();
            }
            else if (audioSource.clip == readyClip)
            {
                startText.text = "READY";
            }
            else if (audioSource.clip == threeClip)
            {
                startText.text = "3";
            }
            else if (audioSource.clip == twoClip)
            {
                startText.text = "2";
            }
            else if (audioSource.clip == oneClip)
            {
                startText.text = "1";
            }
            else if (audioSource.clip == fightClip)
            {
                startText.text = "FIGHT!";
                gameStarted = true;

                SoundTrack.clip = fightSoundTrackClip;
                SoundTrack.Play();

                healthBarsPanel.SetActive(true);
                SetHealthsPanel();
            }
        }
        else if(!audioSource.isPlaying)
        {
            startText.gameObject.SetActive(false);
            startText.text = "";
        }

        if (setFinish)
        {
            SetFinishPanel();
        }
    }

    void SetFinishPanel()
    {
        if (gameOverTimer < 3f)
        {
            gameOverTimer += Time.deltaTime;
            return;
        }
        if (!gameStarted)
        {
            return;
        }
        gameStarted = false;
        playSoundsOnStart.Enqueue(gameOverClip);

        gameOverPanel.SetActive(true);

        if (GameObject.FindObjectOfType<Mevement>().view.IsMine && !GameObject.FindObjectOfType<Health>().dead)
        {
            winnerNickText.text = PhotonNetwork.NickName;
            loserNickText.text = enemyNick;

            playSoundsOnStart.Enqueue(winnerClip);
            SoundTrack.clip = winnerSoundTrackClip;
            SoundTrack.Play();

            switch (characterType)
            {
                case Mevement.Character.Boi:
                    winnerFaceSet.sprite = boiFaceset;
                    break;
                case Mevement.Character.Daisy:
                    winnerFaceSet.sprite = daisyFaceset;
                    break;
                case Mevement.Character.LonelyBoy:
                    winnerFaceSet.sprite = lonelyBoyFaceset;
                    break;
                case Mevement.Character.OldMan:
                    winnerFaceSet.sprite = oldManFaceset;
                    break;
            }
            switch (enemyType)
            {
                case Mevement.Character.Boi:
                    loserFaceSet.sprite = boiFaceset;
                    break;
                case Mevement.Character.Daisy:
                    loserFaceSet.sprite = daisyFaceset;
                    break;
                case Mevement.Character.LonelyBoy:
                    loserFaceSet.sprite = lonelyBoyFaceset;
                    break;
                case Mevement.Character.OldMan:
                    loserFaceSet.sprite = oldManFaceset;
                    break;
            }
        }
        else if(!GameObject.FindObjectOfType<Mevement>().view.IsMine && !GameObject.FindObjectOfType<Health>().dead)
        {
            winnerNickText.text = enemyNick;
            loserNickText.text = PhotonNetwork.NickName;

            playSoundsOnStart.Enqueue(looserClip);
            SoundTrack.clip = loserSoundTrackClip;
            SoundTrack.Play();

            switch (enemyType)
            {
                case Mevement.Character.Boi:
                    winnerFaceSet.sprite = boiFaceset;
                    break;
                case Mevement.Character.Daisy:
                    winnerFaceSet.sprite = daisyFaceset;
                    break;
                case Mevement.Character.LonelyBoy:
                    winnerFaceSet.sprite = lonelyBoyFaceset;
                    break;
                case Mevement.Character.OldMan:
                    winnerFaceSet.sprite = oldManFaceset;
                    break;
            }
            switch (characterType)
            {
                case Mevement.Character.Boi:
                    loserFaceSet.sprite = boiFaceset;
                    break;
                case Mevement.Character.Daisy:
                    loserFaceSet.sprite = daisyFaceset;
                    break;
                case Mevement.Character.LonelyBoy:
                    loserFaceSet.sprite = lonelyBoyFaceset;
                    break;
                case Mevement.Character.OldMan:
                    loserFaceSet.sprite = oldManFaceset;
                    break;
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            restartButton.SetActive(true);
            waitForHostText.SetActive(false);
        }
        else
        {
            waitForHostText.SetActive(true);
            restartButton.SetActive(false);
        }

    }

    public void FinishPanelSet()
    {
        setFinish = true;
    }

    public void PlayerSelectedSound()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            audioSource.PlayOneShot(player1Clip);
            
        }
        else
        {
            audioSource.PlayOneShot(player2Clip);         
            
        }

        //IEnumerator WaitForSound()
        //{
        //    yield return new WaitForSeconds(player2Clip.length);
        //    audioSource.PlayOneShot(prepareYourselfClip);
        //}
    }

    public void GameStartingSounds()
    {
        playSoundsOnStart.Enqueue(roundOneClip);
        playSoundsOnStart.Enqueue(readyClip);
        playSoundsOnStart.Enqueue(threeClip);
        playSoundsOnStart.Enqueue(twoClip);
        playSoundsOnStart.Enqueue(oneClip);
        playSoundsOnStart.Enqueue(fightClip);
    }

    void SetHealthsPanel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            player1NickText.text = PhotonNetwork.NickName;

                switch (characterType)
                {
                    case Mevement.Character.Boi:
                        player1HealthImage.sprite = boiFaceset;
                        break;
                    case Mevement.Character.Daisy:
                        player1HealthImage.sprite = daisyFaceset;
                        break;
                    case Mevement.Character.LonelyBoy:
                        player1HealthImage.sprite = lonelyBoyFaceset;
                        break;
                    case Mevement.Character.OldMan:
                        player1HealthImage.sprite = oldManFaceset;
                        break;
                }
            
                switch (enemyType)
                {
                    case Mevement.Character.Boi:
                        player2HealthImage.sprite = boiFaceset;
                        break;
                    case Mevement.Character.Daisy:
                        player2HealthImage.sprite = daisyFaceset;
                        break;
                    case Mevement.Character.LonelyBoy:
                        player2HealthImage.sprite = lonelyBoyFaceset;
                        break;
                    case Mevement.Character.OldMan:
                        player2HealthImage.sprite = oldManFaceset;
                        break;
                }

            player2NickText.text = enemyNick;
        }
        else
        {
            player2NickText.text = PhotonNetwork.NickName;           
            player1NickText.text = enemyNick;

            switch (enemyType)
            {
                case Mevement.Character.Boi:
                    player1HealthImage.sprite = boiFaceset;
                    break;
                case Mevement.Character.Daisy:
                    player1HealthImage.sprite = daisyFaceset;
                    break;
                case Mevement.Character.LonelyBoy:
                    player1HealthImage.sprite = lonelyBoyFaceset;
                    break;
                case Mevement.Character.OldMan:
                    player1HealthImage.sprite = oldManFaceset;
                    break;
            }

            switch (characterType)
            {
                case Mevement.Character.Boi:
                    player2HealthImage.sprite = boiFaceset;
                    break;
                case Mevement.Character.Daisy:
                    player2HealthImage.sprite = daisyFaceset;
                    break;
                case Mevement.Character.LonelyBoy:
                    player2HealthImage.sprite = lonelyBoyFaceset;
                    break;
                case Mevement.Character.OldMan:
                    player2HealthImage.sprite = oldManFaceset;
                    break;
            }
        }
    }
}
