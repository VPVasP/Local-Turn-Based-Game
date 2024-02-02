using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PVPManager : MonoBehaviour
{
    public static PVPManager instance;
    public GameObject[] Cameras; //our cameras refrence
    public GameObject[] playerUI; //our player ui gameobject refrence
    public Transform[] players; //our players transform
    public Transform[] startingPositions; //the startingposition transforms
    //coroutines refrences
    private IEnumerator coroutineStartGame;
    private IEnumerator coroutinePlayer1;
    private IEnumerator coroutinePlayer2;
    private IEnumerator coroutinePlayer1returntoStartPos;
    private IEnumerator coroutineHealPlayer1;
    private IEnumerator coroutineHealPlayer2;

    [SerializeField] private float runSpeed = 5; //the runspeed for both players
    //bools to handle attack
    [SerializeField] private bool hasPressedAttack = false;
    [SerializeField] private bool hasPressedAttackP2 = false;
    private bool player1Attacking = false;
    private bool player2Attacking = false;
    public GameObject[] healths; //health game objects array
    //audioclips and audiosources variables
   [SerializeField] private AudioClip[] audioClipsAttackPlayer1;
   [SerializeField] private AudioClip audioClipHurtPlayer1;
    [SerializeField] private AudioClip audioClipHurtPlayer2;
    [SerializeField]private AudioClip[] audioClipsAttackPlayer2;
    [SerializeField] private AudioClip healAudioClip;
    private AudioSource aud;
    [SerializeField ] private AudioSource mainMusic;
    public GameObject winnerScreen; //winner screen gameobject
    public GameObject[] healthPotions; //health potion game object array
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //we set the correct cameras to true and others to false through the array
        Cameras[0].SetActive(true);
        Cameras[1].SetActive(false);
        Cameras[2].SetActive(false);
        Cameras[3].SetActive(false);
        Cameras[4].SetActive(false);
        //we set the playerui to false at the start
        playerUI[0].SetActive(false);
        playerUI[1].SetActive(false);
        coroutineStartGame = startGameCoroutine(3.0f); //we call the coroutine 
        StartCoroutine(coroutineStartGame);
        aud = GetComponent<AudioSource>();
        winnerScreen.SetActive(false);
        healthPotions[0].SetActive(false);
        healthPotions[1].SetActive(false);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

    }
    //coroutine for delaying the start of the game
    private IEnumerator startGameCoroutine(float waitTime)
    {
      
        yield return new WaitForSeconds(waitTime);
        Player1Turn();
        print("Coroutine for startGameCoroutine Ended " + Time.time + " seconds");
    }
    #region player1Stuff
    //method that activates player 1 ui and the player 1 camera
    public void Player1Turn()
    {
        Cameras[0].SetActive(false);
        Cameras[1].SetActive(true);
        Cameras[2].SetActive(false);
        Cameras[3].SetActive(false);
        Cameras[4].SetActive(false);
        playerUI[0].SetActive(true);
        playerUI[1].SetActive(false);
        players[1].transform.LookAt(startingPositions[0].transform);
    }
    //method for handling player1 attack
    public void AttackButtonPlayer1()
    {
       // activates player 1 ui and the player 1 camera to attack
        playerUI[0].SetActive(false);
        playerUI[1].SetActive(false);
        Cameras[0].SetActive(true);
        Cameras[1].SetActive(false);
        Cameras[2].SetActive(false);
        Cameras[3].SetActive(false);
        Cameras[4].SetActive(false);
        //plays a random attack sound from the array
        int randomIndex = Random.Range(0, audioClipsAttackPlayer1.Length);
        mainMusic.volume = 0.3f;
        aud.clip = audioClipsAttackPlayer1[randomIndex];
        aud.Play();
        coroutinePlayer1 = player1Coroutine(8.0f);
        StartCoroutine(coroutinePlayer1); 
        hasPressedAttack = true;
    }
    // method for moving player 1 towards player 2 and attacking
    public void MoveTowardsPlayerAndAttackPlayer1()
    {
        if (hasPressedAttack)
        {
            //we set the target position and make player 1 look at it
            Transform lookatPosition = startingPositions[1].transform;
            Vector3 targetPositionlookat = lookatPosition.position;
            players[0].transform.LookAt(lookatPosition);
            //we get the Animator component of player 1
            Animator playerAnimator = players[0].GetComponent<Animator>();
            playerAnimator.SetBool("isRunning", true);;
            //get the position of player 2
            Vector3 player2Position = players[1].transform.position;
            //we calculate the distance to the player 2
            float distanceToTarget = Vector3.Distance(players[0].transform.position, player2Position);
          Debug.Log(distanceToTarget);
            //move player 1 towards player 2
         players[0].transform.position = Vector3.MoveTowards(players[0].transform.position, player2Position, Time.deltaTime * runSpeed);
            healths[0].gameObject.SetActive(false); //disable player 1 health when he is going towards player 2

            //checcks if the distance bettween player 1 and 2 is less than a value
             if(distanceToTarget<1.4)
            {
                //we look at player 2 and we trigger the attack animation
                players[0].transform.LookAt(players[1]);
                playerAnimator.SetTrigger("Attack");
                playerAnimator.SetBool("isRunning", false);
                hasPressedAttack = false;
                //Debug.Log("Attacked");
                //we get the animator of player2 and play the hurt animation
                Animator playerAnimator2 = players[1].GetComponent<Animator>();
                playerAnimator2.SetTrigger("isHurt");
                //we play the hurt audio of player2
                aud.clip = audioClipHurtPlayer2;
                aud.Play();
                player1Attacking = true;
                //begin the coroutine to return player 1 to the starting position
                coroutinePlayer1returntoStartPos = player1CoroutineReturnToStartPos(1.0f);
                StartCoroutine(coroutinePlayer1returntoStartPos);

            }
        }
    }
    //heal player1
    public void HealPlayer1()
    {
        //we handle the ui and cameras 
        playerUI[0].SetActive(false);
        playerUI[1].SetActive(false);
        Cameras[0].SetActive(false);
        Cameras[1].SetActive(false);
        Cameras[2].SetActive(false);
        Cameras[3].SetActive(true);
        Cameras[4].SetActive(false);
        //we get the player 1 animator 
        Animator playerAnimator = players[0].GetComponent<Animator>();
        playerAnimator.SetTrigger("Heal");
        //we call the gain health function of player1
        players[0].GetComponent<PlayerHealth>().GainHealth();
        mainMusic.volume = 0.3f;
        coroutinePlayer1 = player1Coroutine(8.0f);
        Invoke("PlayHealSound", 3f);
        //we set the health potion at array of 0 to true and 1 to false just in case
        healthPotions[0].SetActive(true);
        healthPotions[1].SetActive(false);
        StartCoroutine(coroutinePlayer1);
    }
  
    private void Update()
    {
        MoveTowardsPlayerAndAttackPlayer1();
        MoveTowardsPlayerAndAttackPlayer2();
    }
    private IEnumerator player1Coroutine(float waitTime)
    {
        //deactivate health potions for both players
        yield return new WaitForSeconds(waitTime);
        player1Attacking = false;
        mainMusic.volume = 1f;
        healthPotions[0].SetActive(false);
        healthPotions[1].SetActive(false);
        StartCoroutine(coroutinePlayer1);
        Player2Turn(); //transition to player 2 turn
        print("Coroutine for player1 Ended " + Time.time + " seconds");
    }
    private IEnumerator player1CoroutineReturnToStartPos(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Transform startingPositionLookat = startingPositions[1].transform;//get the transform of the look-at position for player 1
        Vector3 targetPositionlookat = startingPositionLookat.position;//Get the target position for looking at the opponent
        mainMusic.volume = 1f;
        Transform startingPosition = startingPositions[0].transform; //set the starting position transform 
        Vector3 targetPosition = startingPosition.position;

        float elapsedTime = 0f;
        //get player 1 animator and play the animation
        Animator playerAnimator = players[0].GetComponent<Animator>();
        playerAnimator.SetBool("isRunning", true);
       
        while (elapsedTime < 3.0f) 
        {
            players[0].transform.position = Vector3.MoveTowards(players[0].transform.position, targetPosition, runSpeed * Time.deltaTime);
            players[0].transform.LookAt(targetPosition);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    
        players[0].transform.position = targetPosition;
        playerAnimator.SetBool("isRunning", false);
        healths[0].gameObject.SetActive(true);
        //look at the target position during the movement
        players[0].transform.LookAt(targetPositionlookat);
        print("Coroutine for player1 return to start position Ended " + Time.time + " seconds");
    }
    #endregion player1Stuff

    #region player2Stuff
    public void Player2Turn()
    {
        Cameras[0].SetActive(false);
        Cameras[1].SetActive(false);
        Cameras[2].SetActive(true);
        Cameras[3].SetActive(false);
        Cameras[4].SetActive(false);
        playerUI[0].SetActive(false);
        playerUI[1].SetActive(true);
        mainMusic.volume = 1f;
    }
    public void AttackPlayer2()
    {
        playerUI[0].SetActive(false);
        playerUI[1].SetActive(false);
        Cameras[0].SetActive(true);
        Cameras[1].SetActive(false);
        Cameras[2].SetActive(false);
        Cameras[3].SetActive(false);
        Cameras[4].SetActive(false);
        //    animPlayer1.SetTrigger("AttackP1");
        coroutinePlayer2 = player2Coroutine(8.0f);
        hasPressedAttackP2 = true;
        int randomIndex = Random.Range(0, audioClipsAttackPlayer2.Length);
        aud.clip = audioClipsAttackPlayer2[randomIndex];
        aud.Play();
        mainMusic.volume = 0.3f;
        StartCoroutine(coroutinePlayer2);
    }
    public void HealPlayer2()
    {
        playerUI[0].SetActive(false);
        playerUI[1].SetActive(false);
        Cameras[0].SetActive(false);
        Cameras[1].SetActive(false);
        Cameras[2].SetActive(false);
        Cameras[3].SetActive(false);
        Cameras[4].SetActive(true);
        Animator playerAnimator = players[1].GetComponent<Animator>();
        playerAnimator.SetTrigger("Heal");
        players[1].GetComponent<PlayerHealth>().GainHealth();
        Invoke("PlayHealSound", 3f);
        mainMusic.volume = 0.3f;
        healthPotions[0].SetActive(false);
        healthPotions[1].SetActive(true);
        coroutinePlayer2 = player2Coroutine(8.0f);
        StartCoroutine(coroutinePlayer2);
    }
    private void PlayHealSound()
    {
        aud.clip = healAudioClip;
        aud.Play();
    }
    private IEnumerator player2Coroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        mainMusic.volume = 1f;
        healthPotions[0].SetActive(false);
        healthPotions[1].SetActive(false);
        Player1Turn();
        print("Coroutine for player1 Ended " + Time.time + " seconds");
    }
    public void MoveTowardsPlayerAndAttackPlayer2()
    {
        if (hasPressedAttackP2)
        {
            Animator playerAnimator = players[1].GetComponent<Animator>();
            playerAnimator.SetBool("isRunning", true); ;
            Vector3 player2Position = players[0].transform.position;
            float distanceToTarget = Vector3.Distance(players[1].transform.position, player2Position);
            players[1].transform.position = Vector3.MoveTowards(players[1].transform.position, player2Position, Time.deltaTime * runSpeed);
            healths[1].gameObject.SetActive(false);
            if (distanceToTarget < 1.6f)
            {
                players[1].transform.LookAt(players[0]);
                playerAnimator.SetTrigger("Attack");
                playerAnimator.SetBool("isRunning", false);
                hasPressedAttackP2 = false;
                Animator playerAnimator2 = players[0].GetComponent<Animator>();
                playerAnimator2.SetTrigger("isHurt");
                aud.clip = audioClipHurtPlayer2;
                aud.Play();
                coroutinePlayer1returntoStartPos = player2CoroutineReturnToStartPos(1.0f);
                StartCoroutine(coroutinePlayer1returntoStartPos);

            }
        }
    }
    private IEnumerator player2CoroutineReturnToStartPos(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Transform startingPositionLookat = startingPositions[0].transform;

        Vector3 targetPositionlookat = startingPositionLookat.position;
        Transform startingPosition = startingPositions[1].transform;
        Vector3 targetPosition = startingPosition.position;
        float elapsedTime = 0f;
        Animator playerAnimator = players[1].GetComponent<Animator>();
        playerAnimator.SetBool("isRunning",true);
        mainMusic.volume = 1f;
        while (elapsedTime < 3.0f)
        {
            players[1].transform.position = Vector3.MoveTowards(players[1].transform.position, targetPosition, runSpeed * Time.deltaTime);
            players[1].transform.LookAt(targetPosition);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Animator playerAnimator2 = players[1].GetComponent<Animator>();
        players[1].transform.position = targetPosition;
        //playerAnimator.SetBool("isRunning", false);
        healths[1].gameObject.SetActive(true);
        playerAnimator.SetBool("isRunning", false);
        players[1].transform.LookAt(targetPositionlookat);
        print("Coroutine for player1 return to start position Ended " + Time.time + " seconds");
    }

    #endregion Player2Stuff

    public void EndOfGame()
    {
        winnerScreen.SetActive(true);
        Invoke("RestartingGame", 6f);
    }
    private void RestartingGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
