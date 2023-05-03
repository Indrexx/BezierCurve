using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public Vector3 startPoint;
    public List<GameObject> startPoints;
    public int layer;
    public int numberInLayer;
    public GameObject[] dots;
    public GameObject linePrefab;
    public LineRenderer lr;
    CharacterController cc;
    float lineLength;
    Vector2 toDestinationVector;
    public float speed;
    int n;
    Dot dotScript;


    void Start()
    {
        cc = transform.GetComponent<CharacterController>();
        dots = GameObject.Find("Manager").GetComponent<Manager>().dotArray;
        if(numberInLayer != 0)
        {
            GameObject line = Instantiate(linePrefab, transform.position, Quaternion.identity);
            lr = line.GetComponent<LineRenderer>();
            line.transform.position = new Vector3(0,0,0);
        }
    }


    void Update()
    {
        if(numberInLayer != 0)
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, dots[numberInLayer - 1].transform.position);
        }

        lineLength = (startPoint - startPoints[numberInLayer].transform.position).magnitude;
        toDestinationVector = (startPoint - transform.position).normalized;
        if((startPoint - transform.position).magnitude > 0.01f)
        {
            cc.Move(toDestinationVector * lineLength * speed * 0.001f);
        }

    }



}
