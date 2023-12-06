using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PVPManager : MonoBehaviour
{
    public GameObject[] Cameras;
    public GameObject[] playerUI;
    public Transform[] players;
    public Transform[] startingPositions;
    private IEnumerator coroutineStartGame;
    private IEnumerator coroutinePlayer1;
    private IEnumerator coroutinePlayer2;
    private IEnumerator coroutinePlayer1returntoStartPos;
    [SerializeField] private float runSpeed = 5;
    [SerializeField] public bool hasPressedAttack = false;
    [SerializeField] public bool hasPressedAttackP2 = false;
    public GameObject[] healths;
    public AudioClip[] audioClipsAttackPlayer1;
    public AudioClip audioClipHurtPlayer1;
    public AudioClip audioClipHurtPlayer2;
    public AudioClip[] audioClipsAttackPlayer2;
    private AudioSource aud;
    private void Start()
    {
        Cameras[0].SetActive(true);
        Cameras[1].SetActive(false);
        Cameras[2].SetActive(false);
        playerUI[0].SetActive(false);
        playerUI[1].SetActive(false);
        coroutineStartGame = startGameCoroutine(3.0f);
        StartCoroutine(coroutineStartGame);
        aud = GetComponent<AudioSource>();
    }
    private IEnumerator startGameCoroutine(float waitTime)
    {
      
        yield return new WaitForSeconds(waitTime);
        Player1Turn();
        print("Coroutine for startGameCoroutine Ended " + Time.time + " seconds");
    }
    #region player1Stuff
    public void Player1Turn()
    {
        Cameras[0].SetActive(false);
        Cameras[1].SetActive(true);
        Cameras[2].SetActive(false);
        playerUI[0].SetActive(true);
        playerUI[1].SetActive(false);
    }
    public void AttackButtonPlayer1()
    {
        playerUI[0].SetActive(false);
        playerUI[1].SetActive(false);
        Cameras[0].SetActive(true);
        Cameras[1].SetActive(false);
        Cameras[2].SetActive(false);
        int randomIndex = Random.Range(0, audioClipsAttackPlayer1.Length);
        aud.clip = audioClipsAttackPlayer1[randomIndex];
        aud.Play();
        //   animPlayer1.SetTrigger("AttackP1");
        coroutinePlayer1 = player1Coroutine(8.0f);
        StartCoroutine(coroutinePlayer1);
        hasPressedAttack = true;
    }
    public void MoveTowardsPlayerAndAttackPlayer1()
    {
        if (hasPressedAttack)
        {
            Animator playerAnimator = players[0].GetComponent<Animator>();
            playerAnimator.SetBool("isRunning", true);;
            Vector3 player2Position = players[1].transform.position;
            float distanceToTarget = Vector3.Distance(players[0].transform.position, player2Position);
            Debug.Log(distanceToTarget);
         players[0].transform.position = Vector3.MoveTowards(players[0].transform.position, player2Position, Time.deltaTime * runSpeed);
            healths[0].gameObject.SetActive(false);
             if(distanceToTarget<2f)
            {
                players[0].transform.LookAt(players[1]);
                playerAnimator.SetTrigger("Attack");
                playerAnimator.SetBool("isRunning", false);
                hasPressedAttack = false;
                Debug.Log("Attacked");
                Animator playerAnimator2 = players[1].GetComponent<Animator>();
                playerAnimator2.SetTrigger("isHurt");
                aud.clip = audioClipHurtPlayer2;
                aud.Play();
                players[1].GetComponent<PlayerHealth>().LoseHealth();
                coroutinePlayer1returntoStartPos = player1CoroutineReturnToStartPos(1.0f);
                StartCoroutine(coroutinePlayer1returntoStartPos);

            }
        }
    }
    
    private void Update()
    {
        MoveTowardsPlayerAndAttackPlayer1();
        MoveTowardsPlayerAndAttackPlayer2();
    }
    private IEnumerator player1Coroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Player2Turn(); 
        print("Coroutine for player1 Ended " + Time.time + " seconds");
    }
    private IEnumerator player1CoroutineReturnToStartPos(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Transform startingPosition = startingPositions[0].transform;
        Vector3 targetPosition = startingPosition.position;
        float elapsedTime = 0f;

        while (elapsedTime < 3.0f) 
        {
            players[0].transform.position = Vector3.MoveTowards(players[0].transform.position, targetPosition, runSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Animator playerAnimator = players[0].GetComponent<Animator>();
        players[0].transform.position = targetPosition;
        playerAnimator.SetBool("isRunning", false);
        healths[0].gameObject.SetActive(true);
        print("Coroutine for player1 return to start position Ended " + Time.time + " seconds");
    }
    #endregion player1Stuff

    #region player2Stuff
    public void Player2Turn()
    {
        Cameras[0].SetActive(false);
        Cameras[1].SetActive(false);
        Cameras[2].SetActive(true);
        playerUI[0].SetActive(false);
        playerUI[1].SetActive(true);
    }
    public void AttackPlayer2()
    {
        playerUI[0].SetActive(false);
        playerUI[1].SetActive(false);
        Cameras[0].SetActive(true);
        Cameras[1].SetActive(false);
        Cameras[2].SetActive(false);
        //    animPlayer1.SetTrigger("AttackP1");
        coroutinePlayer2 = player2Coroutine(2.0f);
        hasPressedAttackP2 = true;
        int randomIndex = Random.Range(0, audioClipsAttackPlayer2.Length);
        aud.clip = audioClipsAttackPlayer2[randomIndex];
        aud.Play();
        StartCoroutine(coroutinePlayer2);
    }
    private IEnumerator player2Coroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
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
            float distanceToTarget = Vector3.Distance(players[0].transform.position, player2Position);
            Debug.Log(distanceToTarget);
            players[1].transform.position = Vector3.MoveTowards(players[1].transform.position, player2Position, Time.deltaTime * runSpeed);
            healths[1].gameObject.SetActive(false);
            if (distanceToTarget < 2f)
            {
                players[0].transform.LookAt(players[0]);
                playerAnimator.SetTrigger("Attack");
                playerAnimator.SetBool("isRunning", false);
                hasPressedAttackP2 = false;
                Debug.Log("Attacked");
                Animator playerAnimator2 = players[0].GetComponent<Animator>();
                playerAnimator2.SetTrigger("isHurt");
                aud.clip = audioClipHurtPlayer2;
                aud.Play();
                players[0].GetComponent<PlayerHealth>().LoseHealth();
                coroutinePlayer1returntoStartPos = player1CoroutineReturnToStartPos(1.0f);
                StartCoroutine(coroutinePlayer1returntoStartPos);

            }
        }
    }

    #endregion Player2Stuff
}
