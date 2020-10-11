using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] float forwardSpeed = 10f;
    [SerializeField] float lerpSpeed = 5f;
    [SerializeField] float playerXVal = 2f;
    [SerializeField] Vector2 clampVals = new Vector2(-2, 2);

    private Rigidbody rb;
    private float newXPos;
    private float startXPos;



    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        startXPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetButton("Horizontal"))
        {
            newXPos = Mathf.Clamp(transform.position.x + Input.GetAxisRaw("Horizontal") * playerXVal, startXPos + clampVals.x, startXPos + clampVals.y);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(new Vector3(Mathf.Lerp(transform.position.x, newXPos, lerpSpeed * Time.fixedDeltaTime),rb.velocity.y, transform.position.z + forwardSpeed * Time.fixedDeltaTime));
    }
}
