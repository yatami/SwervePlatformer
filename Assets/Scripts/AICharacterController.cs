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
    Rigidbody rb;

    private bool gameIsStarted;
    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        playerRef = PlayerCharacterController.request();
        animRef = gameObject.GetComponent<AnimationController>();
        playerRef.startGame.AddListener(gameStarted);
        targetPos = GameObject.FindGameObjectWithTag("FinishLine").transform.position;
        targetPos.x = gameObject.transform.position.x;
        navMeshRef = gameObject.GetComponent<NavMeshAgent>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void gameStarted()
    {
        animRef.playIdleToRun();
        gameIsStarted = true;
        activateNavMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
       
    }
    private void LateUpdate()
    {
        if (gameIsStarted)
        {
            navMeshRef.velocity = new Vector3(navMeshRef.desiredVelocity.x, 0, navMeshRef.speed);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("FinishLine"))
        {
            rb.isKinematic = true;
            deActivateNavMesh();
            animRef.playRunToStop();
           
        }
    }
}
