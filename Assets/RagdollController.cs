using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : Singleton<RagdollController>
{
    Rigidbody rb;
    Collider collider;
    PlayerCharacterController characterContRef;
    Animator animRef;
    RespawnSystem respawnRef;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        collider = gameObject.GetComponent<Collider>();
        characterContRef = PlayerCharacterController.request();
        animRef = gameObject.GetComponent<Animator>();
        respawnRef = RespawnSystem.request();

        deActivateRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if(collision.gameObject.CompareTag("KillerObstacle"))
        {
            activateRagdoll();
            respawnRef.respawnCharacter();
        }*/
    }

    public void activateRagdoll()
    {
        foreach (Rigidbody rbs in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            rbs.isKinematic = false;
            rbs.AddForce(new Vector3(0, 0, -2f), ForceMode.Impulse);
        }
        foreach (Collider col in gameObject.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
        rb.isKinematic = true;
        collider.enabled = false;
        characterContRef.enabled = false;
        animRef.enabled = false;
    }
    public void deActivateRagdoll()
    {
        foreach (Rigidbody rbs in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            rbs.isKinematic = true;
        }
        foreach (Collider col in gameObject.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
        rb.isKinematic = false;
        collider.enabled = true;
        characterContRef.enabled = true;
        animRef.enabled = true;
    }
}
