using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//max 22 fuer formelansatz
//max 11 fuer visuellen ansatz

public class Manager : MonoBehaviour
{
    public GameObject startPoint;
    public List<GameObject> startPointList = new List<GameObject>();
    public GameObject[] dotArray;
    public List<GameObject> allDots = new List<GameObject>();
    public int startPointCount;
    public int currentLayer;
    public GameObject mainDot;
    public GameObject BezierPoint;
    public GameObject BezierPointPrefab;
    GameObject point;
    int n;
    bool canClick;
    bool SimStarted;

    void Start()
    {
        canClick = true;
    }


    void Update()
    {
        if(Input.GetButtonDown("Fire1") && canClick)
        {
            click();
        }
        if(Input.GetButtonDown("Jump") && !SimStarted)
        {
            startSim();
            canClick = false;
        }
        if(Input.GetKeyDown("r"))
        {
            Restart();
        }
        if(mainDot != null){
            if(mainDot.transform != point.transform)
            {
                SimStarted = true;
            }
        }
    }

    void click()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point = Instantiate(startPoint, mouse, Quaternion.identity);
        point.GetComponent<StartPointScript>().startPointNumber = startPointCount;
        startPointList.Add(point);
        startPointCount++;
    }

    public void startSim()
    {
        if(startPointCount< 8){
            for(n = 0; n < startPointList.Count; n++)
            {
                startPointList[n].GetComponent<StartPointScript>().moveDots();
            }
        }
        //BezierPoint = Instantiate(BezierPointPrefab, startPointList[0].transform.position, Quaternion.identity);
        //StartCoroutine(drawBezierCurve(0)); help
        for(int i=0; i< 10000; i++){
            BezierPoint = Instantiate(BezierPointPrefab, BezierCurve(i*0.0001f), Quaternion.identity);
        }
    }

    Vector2 BezierCurve(float x){
        Vector2 ret = new Vector2();
        int n = startPointCount-1;
        float a = 1-x;
        float b = x;
        double temp;
        Vector2[] vectors = new Vector2[n+1];
        for(int i = 0; i<=n; i++){
            vectors[i].x= startPointList[i].transform.position.x;
            vectors[i].y= startPointList[i].transform.position.y;
        }
        for(int k = 0; k<=n; k++){
            temp =  Binomial(n,k) * Mathf.Pow(a,n-k)*Mathf.Pow(b,k);
            ret.x += (float)temp*vectors[k].x;
            ret.y += (float)temp*vectors[k].y;
        }
        return ret;
    }

    float Binomial(int n, int k){
        if(k == 0 || k == n){
            return 1;
        }
        return faculty(n)/(faculty(k)*faculty(n-k));
    }
    ulong faculty(int x){
        ulong ret = 1;
        for(ulong i = 1; (int)i<=x; i++){
            ret *= i;
        }
        return ret;
    }
    IEnumerator drawBezierCurve(float x){
        BezierPoint.transform.position = BezierCurve(x);
        yield return new WaitForEndOfFrame();
        if(x<0.99f){
            StartCoroutine(drawBezierCurve(x+0.01f));
        }else{
            BezierPoint.transform.position = startPointList[startPointCount-1].transform.position;
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
