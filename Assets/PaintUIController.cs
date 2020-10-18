using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PaintUIController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textRef;
    PlayerCharacterController playerContRef;
    PaintController paintContRef;

    // Start is called before the first frame update
    void Start()
    {
        textRef.enabled = false;
        playerContRef = PlayerCharacterController.request();
        paintContRef = PaintController.request();
        playerContRef.endGame.AddListener(runEnded);
    }

    private void runEnded()
    {
        textRef.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (textRef.enabled)
        {
            textRef.SetText("%" + (int)paintContRef.percentage);
        }
    }
}
