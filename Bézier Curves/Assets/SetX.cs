using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetX : MonoBehaviour
{
    [SerializeField] Text[] x;

    public void setX(float x){
        for(int i = 0; i<6; i++){
            this.x[i].text = (Mathf.Floor(10.0f * x)/10).ToString();
        }
    }
}
