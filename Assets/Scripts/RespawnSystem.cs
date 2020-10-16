using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera characterCam;
    [SerializeField] CinemachineVirtualCamera deathCam;

    PlayerCharacterController characterContRef;
    AICharacterController aiControllerRef;
    AnimationController animRef;

    private Vector3 respawnPoint;
    

    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = gameObject.transform.position;
        characterContRef = PlayerCharacterController.request();
        animRef = gameObject.GetComponent<AnimationController>();
        if(gameObject.CompareTag("AI"))
        {
            aiControllerRef = gameObject.GetComponent<AICharacterController>();
        }
        else
        {
            deathCam.Follow = null;
        }
    }

    // Update is called once per frame
   

    public void respawnCharacter()
    {
        if (gameObject.CompareTag("AI"))
        {
            aiControllerRef.deActivateNavMesh();
        }
        else
        {
            characterContRef.enabled = false;
            characterCam.Priority = 0;
            deathCam.Priority = 1;
        }
       
        StartCoroutine(respawnAfterDelay());
    }

    IEnumerator respawnAfterDelay()
    {
        yield return new WaitForSeconds(3f);
       
        gameObject.transform.position = respawnPoint;
        animRef.stopDeadAnim();
        animRef.playRunAnim();

        if (gameObject.CompareTag("AI"))
        {
            aiControllerRef.activateNavMesh();
        }
        else
        {
            characterContRef.enabled = true;
            characterCam.Priority = 1;
            deathCam.Priority = 0;
        }

      
        yield break;
    }
}
