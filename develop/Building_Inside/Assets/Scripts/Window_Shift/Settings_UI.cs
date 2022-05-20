using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Settings_UI : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public static string nickname;

    private void Awake()
    {
        animator = GetComponent < Animator>();

    }
    
    public void Close()
    {
        StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay()
    {
        animator.SetTrigger("close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        animator.ResetTrigger("close");
    }

}
