using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacterController : Singleton<PlayerCharacterController>
{
    [HideInInspector] public UnityEvent startGame;
    [HideInInspector] public UnityEvent endGame;
    
    [SerializeField] float forwardSpeed = 10f;
    [SerializeField] float lerpSpeed = 5f;
    [SerializeField] float rotationLerpSpeed = 5f;
    [SerializeField] float xSpeed = 2f;
    [SerializeField] Vector2 clampVals = new Vector2(-2, 2);

    PaintController paintContRef;
    SwipeController swipeContRef;
    AnimationController animationController;
    GameObject tutorialUIRef;

    private Rigidbody rb;
    private float newXPos;
    private float startXPos;
    private float rotationLerper;
    private float startYRot;
    private bool gameHasStarted;
   

    // Start is called before the first frame update
    void Start()
    {
        if(startGame == null)
        {
            startGame = new UnityEvent();
        }
        if (endGame == null)
        {
            endGame = new UnityEvent();
        }
        tutorialUIRef = GameObject.FindGameObjectWithTag("TutorialUI");
        swipeContRef = gameObject.GetComponent<SwipeController>();
        animationController = gameObject.GetComponent<AnimationController>();
        paintContRef = PaintController.request();
        rb = gameObject.GetComponent<Rigidbody>();
        startXPos = transform.position.x;
        startYRot = transform.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(!gameHasStarted)
            {
                startGame.Invoke();
                gameHasStarted = true;
                animationController.playIdleToRun();
                tutorialUIRef.SetActive(false);
            }
        }

            if (Input.GetMouseButton(0))
        {
            
            swipeContRef.CalculateDistance(Input.mousePosition);
            newXPos = Mathf.Clamp(transform.position.x + swipeContRef.normalizedDistance * xSpeed, startXPos + clampVals.x, startXPos + clampVals.y);
           
            //Rotate
            if ((transform.rotation.y >= 0 && (swipeContRef.normalizedDistance >= 0.1f)) || (transform.rotation.y <= 0 && swipeContRef.normalizedDistance <= -0.1f))
            {
               
                rotationLerper += Time.deltaTime * rotationLerpSpeed;
                if (rotationLerper > 1)
                {
                    rotationLerper = 1;
                }
                RotateCharacter(swipeContRef.normalizedDistance >= 0.1f, rotationLerper, swipeContRef.normalizedDistance * 30);
            }
            else
            {
               
                rotationLerper -= Time.deltaTime * rotationLerpSpeed;
                if (rotationLerper < 0)
                {
                    rotationLerper = 0;
                }
                RotateCharacter(transform.rotation.y > 0, rotationLerper, swipeContRef.normalizedDistance * 30);
            }
        }
        else
        {
            newXPos = transform.position.x;
            
            //Rotate
            rotationLerper -= Time.deltaTime * rotationLerpSpeed;
            if (rotationLerper < 0)
            {
                rotationLerper = 0;
            }
            RotateCharacter(transform.rotation.y > 0, rotationLerper, swipeContRef.normalizedDistance * 30);
        }
    }

    private void FixedUpdate()
    {
       if(gameHasStarted)
        {
            rb.MovePosition(new Vector3(Mathf.Lerp(transform.position.x, newXPos, lerpSpeed * Time.fixedDeltaTime), rb.velocity.y, transform.position.z + forwardSpeed * Time.fixedDeltaTime));
        }

     
          //  transform.rotation = Quaternion.Euler(0, 30, 0);
        
    }
      
       


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("FinishLine"))
        {
            rb.isKinematic = true;
            endGame.Invoke();
            paintContRef.activatePaintGame();
            animationController.playRunToStop();
            //play finish anim and show effects
        }
    }

    // find a workaround for characters rotation

    private void RotateCharacter(bool toRight, float rotator , float degree)
    {
        transform.rotation = Quaternion.Euler(0, Mathf.Lerp(startYRot, degree, rotator), 0);
    }

  
}
