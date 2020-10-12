using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotater : MonoBehaviour
{

    public float rotationSpeed = 5f;
    public Vector3 movementAxis = Vector3.right;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
            transform.Rotate(movementAxis.x * rotationSpeed * Time.deltaTime, movementAxis.y * rotationSpeed * Time.deltaTime, movementAxis.z * rotationSpeed * Time.deltaTime);
        
       


    }

    void FixedUpdate()
    {
      
    }
}