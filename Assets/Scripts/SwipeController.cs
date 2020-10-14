using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 currentPos;
    private float inputDistance;
    [SerializeField] float threshHold = 4;
    [HideInInspector] public float normalizedDistance;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDistance(Input.mousePosition);
        }
    }

    public void StartDistance(Vector3 inputPosition)
    {
        startPos = inputPosition;
    }

    public void CalculateDistance(Vector3 inputPosition)
    {
        currentPos = inputPosition;
        inputDistance = currentPos.x - startPos.x;
        var screenSize = (Screen.width / threshHold);
        var clampedDistance = Mathf.Clamp(inputDistance, -screenSize, screenSize);
        normalizedDistance = NormalizeDistance(clampedDistance);
    }

    private float NormalizeDistance(float distance)
    {
        return distance / (Screen.width / threshHold);
    }
}