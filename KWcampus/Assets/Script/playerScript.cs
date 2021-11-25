using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    public float speed;
    public float rotatedSpeed;
    float hAxis; 
    float vAxis;
    bool wDown;
    bool jDown;
    bool isJump;
    bool csDown;

    Vector3 moveVec;
    Rigidbody rigid;

    //Animator anim;
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    void Awake(){
        //anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        thirdPersonCamera.enabled = true;
        firstPersonCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        cameraSwitch();
    }

    void GetInput(){
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        csDown = Input.GetButtonDown("CameraSwitch");
    }

    void Move(){
        moveVec = new Vector3(hAxis, 0,vAxis).normalized;
        if(firstPersonCamera.enabled){
            moveVec = firstPersonCamera.transform.TransformDirection(moveVec);
            moveVec.y = 0;
            //1인칭 이동
        }

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) *Time.deltaTime;
        //if(isCollision==0)
         //  thirdPersonCamera.transform.position += moveVec * speed * (wDown ? 0.3f : 1f) *Time.deltaTime;
        //anim.SetBool("isRun",moveVec != Vector3.zero);
        //anim.SetBool("isWalk",wDown);

    }

    void Turn(){
        if(hAxis == 0 && vAxis == 0) return;
        Quaternion newRotation = Quaternion.LookRotation(moveVec);
        rigid.rotation = Quaternion.Slerp(rigid.rotation,newRotation,rotatedSpeed*Time.deltaTime);
    }

    void Jump(){
        if(jDown&&!isJump){
            rigid.AddForce(Vector3.up * 5, ForceMode.Impulse);
            //anim.SetBool("isJump",true);
            //anim.SetTrigger("doJump");
            isJump = true;
        }
    }


    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag=="Floor"){
            //anim.SetBool("isJump",false);
            isJump = false;
        }
    }

    void cameraSwitch(){
        if(csDown){
            if(thirdPersonCamera.enabled==true){
                firstPersonCamera.enabled = true;
                thirdPersonCamera.enabled = false;
            }
            else {
                thirdPersonCamera.enabled = true;
                firstPersonCamera.enabled = false;

            }
        }
    }
}
