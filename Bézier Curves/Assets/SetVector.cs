using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVector : MonoBehaviour
{
    [SerializeField] Text[] vec;

    public void setVec(float x, float y){
        vec[0].text=(Mathf.Round(10.0f * x)/10).ToString();
        vec[1].text=(Mathf.Round(10.0f * y)/10).ToString();
    }
}
