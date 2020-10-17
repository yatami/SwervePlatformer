using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PaintController : Singleton<PaintController>
{
    
    [SerializeField] GameObject brush;
    [SerializeField] float brushSize = 0.1f;
    [SerializeField] CinemachineVirtualCamera characterCam;
    [SerializeField] CinemachineVirtualCamera paintCam;

    private bool isActive = false;
    private Dictionary<Vector2, bool> wallPoints ;
    private int paintCounter = 0;


    // Start is called before the first frame update
    void Start()
    {
            wallPoints = new Dictionary<Vector2, bool>();
            for(int i = -13; i < 18; i ++)
            {
            
                for(int j = 4; j < 33; j ++)
                {
                    Vector2 point = new Vector2(i, j);
                    wallPoints[point] = false;
                
            }
            }
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray,out hit))
                {
                    if(hit.transform.gameObject.CompareTag("PaintingWall"))
                    {
                       // Debug.Log(hit.point);
                        GameObject paint = Instantiate(brush);
                        paint.transform.rotation = Quaternion.Euler(-90, 0, 0);
                        paint.transform.position = hit.point + Vector3.back * 0.1f;
                        paint.transform.localScale = Vector3.one * brushSize;


                        // calculate percentage
                        
                        for (int i = (int)(hit.point.x*10) - 3; i <= (int)(hit.point.x * 10) + 3; i ++)
                        {
                            for(int  j = (int)(hit.point.y*10) - 3; j <= (int)(hit.point.y*10) + 3; j ++)
                            {
                                Vector2 point = new Vector2(i, j);
                                
                                if (wallPoints.ContainsKey(point))
                                {
                                    if (wallPoints[point] == false)
                                    {
                                        wallPoints[point] = true;
                                        paintCounter++;
                                        
                                    }
                                }
                            }
                        }
                        Debug.Log(" %" + ((float)paintCounter / 899) * 100 + "  " + paintCounter);
                    }
                }
            }
        }
    }

    public void activatePaintGame()
    {
        isActive = true;
        paintCam.Priority = 1;
        characterCam.Priority = 0;

    }
}
