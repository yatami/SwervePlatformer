using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class RagdollController : MonoBehaviour
{
    Rigidbody rb;
    Collider collider;
    PlayerCharacterController characterContRef;
    AICharacterController aiControllerRef;
    Animator animRef;
    RespawnSystem respawnRef;
    NavMeshAgent navMeshRef;

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

        Transform[] trans = gameObject.GetComponentsInChildren<Transform>();
        foreach(Transform t in trans)
        {
            if(t.CompareTag("PelvisBone"))
            {
                pelvis = t.gameObject;
            }
        }
        if(gameObject.CompareTag("AI"))
        {
            navMeshRef = gameObject.GetComponent<NavMeshAgent>();
            aiControllerRef = gameObject.GetComponent<AICharacterController>();
        }

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

        if (gameObject.CompareTag("Player"))
        {
            characterContRef.enabled = false;
        }
        else if (gameObject.CompareTag("AI"))
        {
            aiControllerRef.deActivateNavMesh();
        }
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
        animRef.enabled = true;
        if (gameObject.CompareTag("Player"))
        {
            characterContRef.enabled = true;
        }
        else if (gameObject.CompareTag("AI"))
        {
            aiControllerRef.activateNavMesh();
        }

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
                 .Append(gameObject.transform.DORotate(rigRot[0], 1.6f).SetEase(Ease.InSine));

        DOTween.Sequence().Join(gameObject.transform.DOMove(rigPos[0] , 1.6f).SetEase(Ease.InSine));

        foreach (Transform tran in trans)
        {
            if(index != 0)
            {
                DOTween.Sequence().Join(tran.DOMove(rigPos[index] + differenceInPos, 1.6f).SetEase(Ease.InSine));
            }
            
            
            DOTween.Sequence().Join(tran.DORotate(rigRot[index], 1.6f).SetEase(Ease.InSine));
            index++;
        }

        // gameObject.transform.position = currentPos;

        yield return new WaitForSeconds(1.6f);
        triggered = false;
        deActivateRagdoll();
        yield break;
    }
}
