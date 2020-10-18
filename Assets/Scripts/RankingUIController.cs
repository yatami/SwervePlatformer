using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;

public class RankingUIController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textRef;
    GameObject playerRef;
    GameObject finish;
    List<Transform> racers;
    Animator animRef;

    private int totalRacers = 0;
    private int playerRank = 1;
    private int  prevRank = 1;
    // Start is called before the first frame update
    void Start()
    {
        animRef = gameObject.GetComponent<Animator>();
        racers = new List<Transform>();
        finish = GameObject.FindGameObjectWithTag("FinishLine");
        int index = 0;
        foreach(GameObject opponent in GameObject.FindGameObjectsWithTag("AI"))
        {
            racers.Add(opponent.transform);
            index++;
        }
        totalRacers = index + 1;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        racers.Add(playerRef.transform);
        playerRef.GetComponent<PlayerCharacterController>().endGame.AddListener(runEnded);
    }

    private void runEnded()
    {
        textRef.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        findPlayerRanking();
        int index = 1;
     
        foreach (Transform racer in racers)
        {
            if(racer.CompareTag("Player"))
            {
                playerRank = index;
                if(prevRank != playerRank)
                {
                    animRef.SetBool("shouldPlayRankUp", true);
                    StartCoroutine(stopAnimAfterPlay());
                }
                prevRank = playerRank;
                break;
            }
            index++;
        }

        textRef.SetText(playerRank +  "/" + totalRacers);
    }

 

    private void findPlayerRanking()
    {
        racers.Sort(delegate (Transform a, Transform b)
        {
            return Mathf.Abs(finish.transform.position.z - a.transform.position.z).CompareTo(Mathf.Abs(finish.transform.position.z - b.transform.position.z));
        });
    }

    IEnumerator stopAnimAfterPlay()
    {
        yield return new WaitForSeconds(0.1f);
        animRef.SetBool("shouldPlayRankUp", false);
        yield break;
    }

}
