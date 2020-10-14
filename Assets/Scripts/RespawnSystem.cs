using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSystem : Singleton<RespawnSystem>
{
    [SerializeField] CinemachineVirtualCamera characterCam;
    [SerializeField] CinemachineVirtualCamera deathCam;

    PlayerCharacterController characterContRef;
    PlayerAnimationController animRef;

    private Vector3 respawnPoint;
    

    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = gameObject.transform.position;
        deathCam.Follow = null;
        characterContRef = PlayerCharacterController.request();
        animRef = PlayerAnimationController.request();
    }

    // Update is called once per frame
   

    public void respawnCharacter()
    {
        characterContRef.enabled = false;
        characterCam.Priority = 0;
        deathCam.Priority = 1;
        StartCoroutine(respawnAfterDelay());
    }

    IEnumerator respawnAfterDelay()
    {
        yield return new WaitForSeconds(3f);
       
        gameObject.transform.position = respawnPoint;
        animRef.stopDeadAnim();
        animRef.playRunAnim();
        characterContRef.enabled = true;
        characterCam.Priority = 1;
        deathCam.Priority = 0;
        yield break;
    }
}
