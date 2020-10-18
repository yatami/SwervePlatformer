using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animRef;
    RespawnSystem respawnRef;
    PlayerCharacterController characterContRef;
    Rigidbody rb;

    private bool canDie = true;
    private RigidbodyConstraints previousConstraints;
    // Start is called before the first frame update
    void Start()
    {
        characterContRef = PlayerCharacterController.request();
        rb = gameObject.GetComponent<Rigidbody>();
        previousConstraints = rb.constraints;
        animRef = gameObject.GetComponent<Animator>();
        respawnRef = gameObject.GetComponent<RespawnSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("KillerObstacle") && canDie)
        {
            rb.isKinematic = true;
            canDie = false;
            respawnRef.respawnCharacter();
            stopRunAnim();
            animRef.SetBool("isDead", true);
            freeRbConstraints();
            StartCoroutine(deathTriggerCooldown());
        }
    }

    IEnumerator deathTriggerCooldown()
    {
        yield return new WaitForSeconds(3);
        rb.isKinematic = false;
        canDie = true;
        yield break;
    }

    public void playIdleToRun()
    {
        animRef.SetBool("shouldStartTheGame", true);
    }
    public void playRunToStop()
    {
        animRef.SetBool("shouldFinish", true);
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
        lockRbConstraints();
    }

    private void freeRbConstraints()
    {
        rb.constraints = RigidbodyConstraints.None;
    }

    private void lockRbConstraints()
    {
        rb.constraints = previousConstraints;
    }
}
