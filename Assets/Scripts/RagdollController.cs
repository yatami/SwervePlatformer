using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RagdollController : Singleton<RagdollController>
{
    Rigidbody rb;
    Collider collider;
    PlayerCharacterController characterContRef;
    Animator animRef;
    RespawnSystem respawnRef;

    private GameObject pelvis;
    private Vector3 hitPos;
    private Vector3 curretNormal;
    private Vector3[] rigPos;
    private Vector3[] rigRot;
    private bool triggered;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        collider = gameObject.GetComponent<Collider>();
        characterContRef = PlayerCharacterController.request();
        animRef = gameObject.GetComponent<Animator>();
        respawnRef = RespawnSystem.request();
        pelvis = GameObject.FindGameObjectWithTag("PelvisBone");
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
        if (collision.gameObject.CompareTag("Rotator"))
        {
            curretNormal = collision.contacts[0].normal;
            hitPos = pelvis.transform.position;
            activateRagdoll();
            triggered = true;
            Transform[] trans = gameObject.GetComponentsInChildren<Transform>();
            rigPos = new Vector3[trans.Length];
            rigRot = new Vector3[trans.Length];
            int index = 0;
            foreach (Transform tran in trans)
            { 
                rigPos[index] = tran.position;
                rigRot[index] = tran.rotation.eulerAngles;
                index++;
            }
        }
        
    }

    public void activateRagdoll()
    {
        Debug.Log(curretNormal);
        foreach (Rigidbody rbs in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            rbs.isKinematic = false;
            rbs.AddForce(new Vector3(curretNormal.x, curretNormal.y, curretNormal.z) * 5, ForceMode.Impulse);
        }
        foreach (Collider col in gameObject.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
        rb.isKinematic = true;
        collider.enabled = false;
        characterContRef.enabled = false;
        animRef.enabled = false;

       StartCoroutine(triggerGetUp());
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

    IEnumerator triggerGetUp()
    {
        
        yield return new WaitForSeconds(3f);
        Vector3 currentPos = pelvis.transform.position;
        Vector3 differenceInPos = currentPos - hitPos;
        differenceInPos.y = 0;

        Transform[] trans = gameObject.GetComponentsInChildren<Transform>();
        int index = 0;

        rigPos[0] = new Vector3(currentPos.x, gameObject.transform.position.y, currentPos.z);

        DOTween.Sequence()
                 .Append(gameObject.transform.DORotate(rigRot[0], 2).SetEase(Ease.InSine));

        DOTween.Sequence().Join(gameObject.transform.DOMove(rigPos[0] , 2).SetEase(Ease.InSine));

        foreach (Transform tran in trans)
        {
            if(index != 0)
            {
                DOTween.Sequence().Join(tran.DOMove(rigPos[index] + differenceInPos, 2).SetEase(Ease.InSine));
            }
            
            
            DOTween.Sequence().Join(tran.DORotate(rigRot[index], 2).SetEase(Ease.InSine));
            index++;
        }

        // gameObject.transform.position = currentPos;

        yield return new WaitForSeconds(2f);
        triggered = false;
        deActivateRagdoll();
        yield break;
    }
}
