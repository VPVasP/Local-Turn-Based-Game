using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PVPManager : MonoBehaviour
{
    public GameObject[] Cameras;
    public GameObject[] playerUI;
    public Transform[] players;
    private IEnumerator coroutineStartGame;
    private IEnumerator coroutinePlayer1;
    private IEnumerator coroutinePlayer2;
    [SerializeField] private float runSpeed = 5;
    [SerializeField] public bool hasPressedAttack = false;
    private void Start()
    {
        Cameras[0].SetActive(true);
        Cameras[1].SetActive(false);
        Cameras[2].SetActive(false);
        playerUI[0].SetActive(false);
        playerUI[1].SetActive(false);
        coroutineStartGame = startGameCoroutine(2.0f);
        StartCoroutine(coroutineStartGame);
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
        //   animPlayer1.SetTrigger("AttackP1");
        coroutinePlayer1 = player1Coroutine(8.0f);
        StartCoroutine(coroutinePlayer1);
        hasPressedAttack = true;
    }
    public void MoveTowardsPlayerAndAttack()
    {
        if (hasPressedAttack)
        {
            Animator playerAnimator = players[0].GetComponent<Animator>();
            playerAnimator.SetBool("isRunning", true);

            Vector3 player2Position = players[1].transform.position;
            float distanceToTarget = Vector3.Distance(players[0].transform.position, player2Position);
            Debug.Log(distanceToTarget);
         players[0].transform.position = Vector3.MoveTowards(players[0].transform.position, player2Position, Time.deltaTime * runSpeed);
            
             if(distanceToTarget<2f)
            {
                players[0].transform.LookAt(players[1]);
                playerAnimator.SetTrigger("Attack");
                hasPressedAttack = false;
                Debug.Log("Attacked");          
            }
        }
    }

    private void Update()
    {
        MoveTowardsPlayerAndAttack();
    }
    private IEnumerator player1Coroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Player2Turn(); 
        print("Coroutine for player1 Ended " + Time.time + " seconds");
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
        StartCoroutine(coroutinePlayer2);
    }
    private IEnumerator player2Coroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Player1Turn();
        print("Coroutine for player1 Ended " + Time.time + " seconds");
    }
    #endregion Player2Stuff
}
