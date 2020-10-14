using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : Singleton<PlayerCharacterController>
{
    [SerializeField] float forwardSpeed = 10f;
    [SerializeField] float lerpSpeed = 5f;
    [SerializeField] float rotationLerpSpeed = 5f;
    [SerializeField] float xSpeed = 2f;
    [SerializeField] Vector2 clampVals = new Vector2(-2, 2);

    PaintController paintContRef;
    SwipeController swipeContRef;

    private Rigidbody rb;
    private float newXPos;
    private float startXPos;
    private float rotationLerper;
    private float startYRot;


    // Start is called before the first frame update
    void Start()
    {
        swipeContRef = gameObject.GetComponent<SwipeController>();
        paintContRef = PaintController.request();
        rb = gameObject.GetComponent<Rigidbody>();
        startXPos = transform.position.x;
        startYRot = transform.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {
       

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


        // Debug.Log(rotationLerper + "  axisraw : " + Input.GetAxisRaw("Horizontal"));
        /*if (Input.GetButton("Horizontal"))
        {
            newXPos = Mathf.Clamp(transform.position.x + Input.GetAxisRaw("Horizontal") * xSpeed, startXPos + clampVals.x, startXPos + clampVals.y);
           
            //Rotate
            if ((transform.rotation.y >= 0 && (Input.GetAxisRaw("Horizontal") == 1)) || (transform.rotation.y <= 0 && Input.GetAxisRaw("Horizontal") == -1))
            {
                Debug.Log("executes");
               rotationLerper += Time.deltaTime * rotationLerpSpeed;
                if (rotationLerper > 1)
                {
                    rotationLerper = 1;
                }
                RotateCharacter(Input.GetAxisRaw("Horizontal") == 1, rotationLerper);
            }
            else
            {
                rotationLerper -= Time.deltaTime * rotationLerpSpeed;
                if (rotationLerper < 0)
                {
                    rotationLerper = 0;
                }
                RotateCharacter(transform.rotation.y > 0, rotationLerper);
            }
           
        }
        else
        {
            newXPos = transform.position.x;
             //Rotate
             rotationLerper -= Time.deltaTime * rotationLerpSpeed;
            if(rotationLerper < 0)
            {
                rotationLerper = 0;
            }
            RotateCharacter(transform.rotation.y > 0, rotationLerper);
        }*/
    }

    private void FixedUpdate()
    {
        //rb.velocity = new Vector3(newXPos ,rb.velocity.y, forwardSpeed);
     
        rb.MovePosition(new Vector3(Mathf.Lerp(transform.position.x, newXPos, lerpSpeed * Time.fixedDeltaTime), rb.velocity.y, transform.position.z + forwardSpeed * Time.fixedDeltaTime));
        //rb.MovePosition(new Vector3(newXPos, rb.velocity.y, transform.position.z + forwardSpeed * Time.fixedDeltaTime));
        //rb.MovePosition(new Vector3(transform.position.x - 0.05f * Time.fixedDeltaTime, transform.position.y, transform.position.z));

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("FinishLine"))
        {
            rb.isKinematic = true;
            paintContRef.activatePaintGame();
            //play finish anim and show effects
        }
    }

    private void RotateCharacter(bool toRight, float rotator , float degree)
    {
        if(toRight)
        {
            transform.rotation = Quaternion.Euler(0, Mathf.Lerp(startYRot, degree, rotator), 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, Mathf.Lerp(startYRot, degree, rotator), 0);
        }
    }
}
