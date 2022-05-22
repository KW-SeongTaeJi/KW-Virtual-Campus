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

        if (keyboard.leftArrowKey.isPressed)
        {
            h = -1;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
            
        else if (keyboard.rightArrowKey.isPressed)
        {
            h = 1;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }


        var curPos = transform.position;

        curPos += new Vector3(h,v, 0) * speed * Time.deltaTime;
        //curPos.x = Clamp(curPos.x, -3.0f, 3.0f);
        //curPos.y = Clamp(curPos.y, -5.5f, 5.5f);

        animator.SetFloat("h", h);

        transform.position = curPos;


    }
}
