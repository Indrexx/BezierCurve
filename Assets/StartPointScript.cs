using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointScript : MonoBehaviour
{
    public int startPointNumber;
    public List<GameObject> points;
    public GameObject linePrefab;
    public GameObject dotPrefab;
    LineRenderer lr;
    float lineLength;
    Vector2 toDestinationVector;
    GameObject dot;
    DotTest dotScript;


    void Start()
    {
        points = GameObject.Find("Manager").GetComponent<Manager>().startPointList;

        if(startPointNumber != 0)
        {
            GameObject line = Instantiate(linePrefab, transform.position, Quaternion.identity);
            lr = line.GetComponent<LineRenderer>();
            line.transform.position = new Vector3(0,0,0);
            if(startPointNumber != 0)
            {
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, points[startPointNumber-1].transform.position);
            }


        }

    }

    void Update()
    {

    }

    public void moveDots()
    {
        if(startPointNumber != 0)
        {
            dot = Instantiate(dotPrefab, points[startPointNumber-1].transform.position, Quaternion.identity);
            dotScript = dot.GetComponent<DotTest>();
            dotScript.startDot(transform.position, 0, startPointNumber -1);
        }
    }

}
