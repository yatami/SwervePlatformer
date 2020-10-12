﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : Singleton<PlayerAnimationController>
{
    Animator animRef;
    RespawnSystem respawnRef;
    PlayerCharacterController characterContRef;

    private bool canDie = true;
    // Start is called before the first frame update
    void Start()
    {
        characterContRef = PlayerCharacterController.request();
        animRef = gameObject.GetComponent<Animator>();
        respawnRef = RespawnSystem.request();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("KillerObstacle") && canDie)
        {
            canDie = false;
            respawnRef.respawnCharacter();
            stopRunAnim();
            animRef.SetBool("isDead", true);
            StartCoroutine(deathTriggerCooldown());
        }
    }

    IEnumerator deathTriggerCooldown()
    {
        yield return new WaitForSeconds(3);
        canDie = true;
        yield break;
    }

    public void playRunAnim()
    {
        animRef.SetBool("shouldRun", true);
    }
    public void stopRunAnim()
    {
        animRef.SetBool("shouldRun", false);
    }

    public void stopDeadAnim()
    {
        animRef.SetBool("isDead", false);
    }
}
