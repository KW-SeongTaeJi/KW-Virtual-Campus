using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstPersonCameraScript : MonoBehaviour
{
    public float sensitivity = 10000f; //감도
    float xRotation = 0.0f;  //x축 회전값
    float yRotation = 0.0f;  //z축 회전값
    
    void Update()
    {
        MouseSencer();
    }

    void MouseSencer()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        xRotation += x * sensitivity * Time.deltaTime;
        yRotation += y * sensitivity * Time.deltaTime;
 
        if(yRotation > 30) yRotation = 30;
        else if(yRotation < -30) yRotation = -30;
        transform.eulerAngles = new Vector3(-yRotation, xRotation, 0.0f);      
    }
}