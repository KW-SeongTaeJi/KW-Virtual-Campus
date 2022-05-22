using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
<<<<<<< HEAD:develop/Building_Inside/Assets/Scripts/About Building/Curriculum.cs

public class Curriculum : MonoBehaviour
=======
using UnityEngine.UI;

public class About_Building_UI : MonoBehaviour
>>>>>>> e7645a8df5ea0276289f8db2e3ff5ddd20220494:develop/Building_Inside/Assets/Scripts/Window_Shift/About_Building_UI.cs
=======

public class Curriculum : MonoBehaviour
>>>>>>> e7645a8df5ea0276289f8db2e3ff5ddd20220494
{
    [SerializeField]
    private Animator animator;

    public static string nickname;

    private void Awake()
    {
        animator = GetComponent<Animator>();

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
