using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{

    [SerializeField] GameObject charRef;
    // Start is called before the first frame update
    void Start()
    {
        GameObject newChar =  Instantiate(charRef);
        newChar.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
