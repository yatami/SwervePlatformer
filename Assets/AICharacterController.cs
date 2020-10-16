using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterController : MonoBehaviour
{
    NavMeshAgent navMeshRef;
    PlayerCharacterController playerRef;
    AnimationController animRef;

    private bool gameIsStarted;
    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        playerRef = PlayerCharacterController.request();
        animRef = gameObject.GetComponent<AnimationController>();
        playerRef.startGame.AddListener(gameStarted);
        targetPos = GameObject.FindGameObjectWithTag("FinishLine").transform.position;
        navMeshRef = gameObject.GetComponent<NavMeshAgent>();
    }

    private void gameStarted()
    {
        Debug.Log("Executed");
        animRef.playIdleToRun();
        gameIsStarted = true;
        activateNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void activateNavMesh()
    {
        if(gameIsStarted)
        {
            navMeshRef.enabled = true;
            navMeshRef.SetDestination(targetPos);
        }
    }

    public void deActivateNavMesh()
    {
        navMeshRef.enabled = false;
    }
}
