using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotTest : MonoBehaviour
{
    public Vector3 destination;
    List<GameObject> dotsInMyLayer = new List<GameObject>();
    List<GameObject> dotsInPrevLayer;
    public int layer;
    public float lineLength;




    CharacterController cc;
    Vector2 toDestinationVector;
    public int numberInLayer;
    LineRenderer lr;
    public float speed;
    public GameObject linePrefab;
    Manager manager;
    GameObject[] dotArray;
    bool shouldStart;
    int i;
    public GameObject dotPrefab;
    GameObject mainDot;

    void Start()
    {
        Physics.IgnoreLayerCollision(0,0, true);
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        manager.allDots.Add(gameObject);
        cc = gameObject.GetComponent<CharacterController>();

        if(numberInLayer != 0)
        {
            DrawLine();
            dotsInPrevLayer = new List<GameObject>();
            foreach(GameObject dot1 in manager.allDots)
            {
                if(dot1.GetComponent<DotTest>().layer == this.layer )
                {
                    dotsInPrevLayer.Add(dot1);
                }
            }
            GameObject dot = Instantiate(dotPrefab, dotsInPrevLayer[numberInLayer -1].transform.position, Quaternion.identity);
            dot.GetComponent<DotTest>().startDot(transform.position, layer+1, numberInLayer -1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldStart)
        {
            if(numberInLayer != 0)
            {
                if(i < 1)
                {
                    DrawLine();
                }
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, dotsInMyLayer[numberInLayer - 1].transform.position);
            }
            if(layer > 0)
            {
                dotsInPrevLayer = new List<GameObject>();
                foreach(GameObject dot1 in manager.allDots)
                {
                    if(dot1.GetComponent<DotTest>().layer == this.layer -1)
                    {
                        dotsInPrevLayer.Add(dot1);
                    }
                }
                destination = dotsInPrevLayer[numberInLayer + 1].transform.position;
                lineLength = (destination - dotsInPrevLayer[numberInLayer ].transform.position).magnitude;
                speed = (layer +1) * 0.75f;
            }
            toDestinationVector = (destination - transform.position).normalized;

            if((destination - transform.position).magnitude > 0.01f)
            {
                cc.Move(toDestinationVector * lineLength * speed * Time.deltaTime * 0.1f);
            }else
            {
                cc.Move(destination - transform.position);
            }

            foreach(GameObject dot2 in manager.allDots)
            {
                if(dot2.GetComponent<DotTest>().layer > this.layer)
                {
                    mainDot = dot2;
                }
            }
            if(mainDot != null){
                mainDot.GetComponent<TrailRenderer>().enabled = true;
                manager.mainDot = mainDot;
            }
        }
    }

    public void startDot(Vector3 des, int l, int nil)
    {
        StartCoroutine(laterStartDot(des,l,nil));
        
    }

    IEnumerator laterStartDot(Vector3 des, int l, int nil)
    {
        numberInLayer = nil;
        destination = des;
        layer = l;
        yield return new WaitForSeconds(0.001f);

        foreach(GameObject dot in manager.allDots)
        {
            if(dot.GetComponent<DotTest>().layer == this.layer)
            {
                dotsInMyLayer.Add(dot);
            }
        }
        lineLength = (destination - dotsInMyLayer[numberInLayer].transform.position).magnitude;
        shouldStart = true;
    }
    public void DrawLine()
    {
        GameObject line = Instantiate(linePrefab, transform.position, Quaternion.identity);
        lr = line.GetComponent<LineRenderer>();
        line.transform.position = new Vector3(0,0,0);
        i++;
    }
}
