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
    public Animator animPlayer1;
    public Animator animPlayer2;
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
    public void Attack()
    {
        playerUI[0].SetActive(false);
        playerUI[1].SetActive(false);
        Cameras[0].SetActive(true);
        Cameras[1].SetActive(false);
        Cameras[2].SetActive(false);
    //    animPlayer1.SetTrigger("AttackP1");
        coroutinePlayer1=player1Coroutine(2.0f);
        StartCoroutine(coroutinePlayer1);
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
