using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdPersonCameraScript : MonoBehaviour
{
    float hAxis; 
    float vAxis;
    Vector3 moveVec;
    public float speed;
    bool wDown;

    void Update()
    {
        GetInput();
        Move();
    }

    void GetInput(){
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
    }
    void Move(){
        moveVec = new Vector3(hAxis, 0,vAxis).normalized;
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) *Time.deltaTime;
    }

}