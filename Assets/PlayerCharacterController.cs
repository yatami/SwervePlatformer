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

    private Rigidbody rb;
    private float newXPos;
    private float startXPos;
    private float rotationLerper;
    private float startYRot;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        startXPos = transform.position.x;
        startYRot = transform.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rotationLerper + "  axisraw : " + Input.GetAxisRaw("Horizontal"));
        if (Input.GetButton("Horizontal"))
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
        }
    }

    private void FixedUpdate()
    {
      
       // rb.MovePosition(new Vector3(Mathf.Lerp(transform.position.x, newXPos, lerpSpeed * Time.fixedDeltaTime), rb.velocity.y, transform.position.z + forwardSpeed * Time.fixedDeltaTime));
        //rb.MovePosition(new Vector3(newXPos, rb.velocity.y, transform.position.z + forwardSpeed * Time.fixedDeltaTime));
        //rb.MovePosition(new Vector3(transform.position.x - 0.05f * Time.fixedDeltaTime, transform.position.y, transform.position.z));

    }

    private void RotateCharacter(bool toRight, float rotator)
    {
        if(toRight)
        {
            transform.rotation = Quaternion.Euler(0, Mathf.Lerp(startYRot, 30, rotator), 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, Mathf.Lerp(startYRot, -30, rotator), 0);
        }
    }
}
