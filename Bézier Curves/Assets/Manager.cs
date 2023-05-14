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
    public GameObject CurvePoint;
    public GameObject BezierPointPrefab;

    [SerializeField] private SetX x;
    [SerializeField] private SetVector[] vectors;
    [SerializeField] private GameObject formular;
    [SerializeField] private GameObject instructions;

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
        if(Input.GetKeyDown("v") && !SimStarted)
        {
            startSim();
            canClick = false;
            SimStarted = true;
            Destroy(instructions);
            Destroy(formular);
        }
        if(Input.GetKeyDown("d") && !SimStarted)
        {
            drawLine();
            canClick = false;
            SimStarted = true;
            Destroy(instructions);
            Destroy(formular);
        }
        if(Input.GetKeyDown("f") && !SimStarted)
        {
            if(startPointCount == 4){
                startFormular();
                canClick = false;
                SimStarted = true;
                Destroy(instructions);
            }
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
        if(startPointCount <4){
            vectors[startPointCount].setVec(point.transform.position.x, point.transform.position.y);
        }else{
            Destroy(formular);
        }
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
    }
    public void drawLine()
    {
        for(int i=0; i< 1000; i++){
            BezierPoint = Instantiate(CurvePoint, BezierCurve(i*0.001f), Quaternion.identity);
        }
    }
    public void startFormular()
    {
        BezierPoint = Instantiate(BezierPointPrefab, startPointList[0].transform.position, Quaternion.identity);
        StartCoroutine(drawBezierCurve(0));
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
        if(formular != null){
            this.x.setX(x);
            vectors[4].setVec(BezierCurve(x).x, BezierCurve(x).y);
        }
        BezierPoint.transform.position = BezierCurve(x);
        yield return new WaitForSeconds(0.01f);
        if(x<0.999f){
            StartCoroutine(drawBezierCurve(x+0.001f));
        }else{
            BezierPoint.transform.position = startPointList[startPointCount-1].transform.position;
            if(formular != null){
                vectors[4].setVec(BezierCurve(1).x, BezierCurve(1).y);
                this.x.setX(1);
            }
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
