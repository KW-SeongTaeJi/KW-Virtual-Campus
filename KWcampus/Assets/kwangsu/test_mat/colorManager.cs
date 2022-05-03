using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorManager : MonoBehaviour
{
    //Slider
    public Slider Slider_R;
    public Slider Slider_G;
    public Slider Slider_B;
    //slider val
    float _R = 1;
    float _G = 1;
    float _B = 1;
    //mat
    GameObject ks=null;
    GameObject ksbody = null;
    Renderer ksbodyrenderer = null;
    Material targetMat = null;


    // Start is called before the first frame update
    void Start()
    {
        ks = GameObject.Find("kwangsu06");
        ksbody = ks.transform.GetChild(0).gameObject;
        ksbodyrenderer = ksbody.GetComponent<Renderer>();
        targetMat = ksbodyrenderer.material;

        //default
        Slider_R.value = _R;
        Slider_G.value = _G;
        Slider_B.value = _B;
        //send default val to shader
        ksbodyrenderer.material.SetFloat("_R", _R);
        ksbodyrenderer.material.SetFloat("_B", _B);
        ksbodyrenderer.material.SetFloat("_G", _G);

        //set trigger
        Slider_R.onValueChanged.AddListener(delegate{Function_Slider_R(); });
        Slider_G.onValueChanged.AddListener(delegate{Function_Slider_G(); });
        Slider_B.onValueChanged.AddListener(delegate{Function_Slider_B(); });
    }

    public void Function_Slider_R()
    {
        //get slider value
        _R = Slider_R.value;
        //send to shader
        targetMat.SetFloat("_R", _R);
    }

    public void Function_Slider_G()
    {
        //get slider value
        _G = Slider_G.value;
        //send to shader
        targetMat.SetFloat("_G", _G);
    }

    public void Function_Slider_B()
    {
        //get slider value
        _B = Slider_B.value;
        //send to shader
        targetMat.SetFloat("_B", _B);
    }
}
