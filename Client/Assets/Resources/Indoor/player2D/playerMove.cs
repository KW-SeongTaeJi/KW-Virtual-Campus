using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Mathf;

public class playerMove : MonoBehaviour
{
    public float speed = 3.0f;
    Animator animator;

    Keyboard keyboard;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    private void Start()
    {
        keyboard = Keyboard.current;
    }

    private void Update()
    {
        float h = 0;
        float v = 0;
        bool isWalk = false;

        if (keyboard.leftArrowKey.isPressed)
        {
            h = -1;
            transform.localScale = new Vector3(-1f, 1f, 1f);
            isWalk = true;
        }
            
        else if (keyboard.rightArrowKey.isPressed)
        {
            h = 1;
            transform.localScale = new Vector3(1f, 1f, 1f);
            isWalk = true;
        }


        var curPos = transform.position;

        curPos += new Vector3(h,v, 0) * speed * Time.deltaTime;

        animator.SetBool("isWalk", isWalk);

        transform.position = curPos;


    }
}
