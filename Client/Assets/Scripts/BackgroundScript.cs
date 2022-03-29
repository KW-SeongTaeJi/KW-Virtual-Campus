//[ref]https://srk911028.tistory.com/125
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;

public class BackgroundScript : MonoBehaviour
{
    public GameObject[] arrGameObjects;
    private Material[] arrMaterials;

    public GameObject skyDome;
    private Material skyDomeMaterial;
    private float offsetValueX = 0;

    private float elpasedTime;
    private float r = 1f;
    private float g = 1f;
    private float b = 1f;

    Light sunLight;

    void Awake(){
        sunLight= GameObject.FindWithTag("Light").GetComponent<Light>();
    }

    // Start is called before the first frame update
    void Start()
    {
        sunLight.intensity = 1;
        this.skyDomeMaterial = this.skyDome.GetComponent<Renderer>().material; 
        this.skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(this.offsetValueX,0));
 
        //this.isNight = false;
        this.arrMaterials = new Material[this.arrGameObjects.Length];
 
        for (int i = 0; i < this.arrGameObjects.Length; i++) {
            this.arrMaterials[i] = this.arrGameObjects[i].GetComponent<Renderer>().material;
            this.arrMaterials[i].EnableKeyword("_EMISSION");
            this.arrMaterials[i].globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
 
        }
        this.StartCoroutine(this.DayToNightImpl());
    }

    // Update is called once per frame
    void Update()
    {
        this.skyDome.transform.Rotate(Vector3.up* 3* Time.deltaTime);
    }

    //낮 -> 밤
    IEnumerator DayToNightImpl(){
        while (true){
            sunLight.intensity-=Time.deltaTime*(float)0.1;
            this.elpasedTime += Time.deltaTime;
            //this.text.text = this.elpasedTime.ToString();
            //this.state.text = "낮->밤";
            RenderSettings.ambientLight = new Color(this.r, this.g, this.b, 1);
            this.r -= r / 1000; this.g -= g / 1000; this.b -= b / 1000;
 
            if (this.r <= 0 || this.g <= 0 || this.b <= 0){
                this.r = 0; this.g = 0; this.b = 0;
            }
 
            this.offsetValueX += 0.05f * Time.deltaTime;
            this.skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(this.offsetValueX, 0));
 
            if (this.elpasedTime >= 10){
                this.offsetValueX = 0.5f;
                this.elpasedTime = 0;
                //this.text.text = this.elpasedTime.ToString();
                this.StartCoroutine(this.NightImpl());
                break;
            }
            yield return null;
        }
    }
 
    //밤
    IEnumerator NightImpl() {
        this.skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(this.offsetValueX, 0));
 
        for (int i = 0; i < this.arrMaterials.Length; i++) {
            this.arrMaterials[i].SetColor("_EmissionColor", Color.white);
        }
 
        while (true) {
            this.elpasedTime += Time.deltaTime;
            //this.text.text = this.elpasedTime.ToString();
            //this.state.text = "밤";
            
            if (this.elpasedTime>=10) {
                this.elpasedTime = 0;
                this.StartCoroutine(this.NightToDayImpl());
                break;
            }
 
            yield return null;
        }
 
    }
 
    //밤 -> 낮
    IEnumerator NightToDayImpl()
    {
        while (true) {
            sunLight.intensity+=Time.deltaTime*(float)0.1;
            this.elpasedTime += Time.deltaTime;
            //this.text.text = this.elpasedTime.ToString();
            //this.state.text = "밤->낮";
            RenderSettings.ambientLight = new Color(this.r, this.g, this.b, 1);
            this.r += r / 1000; this.g += g / 1000; this.b += b / 1000;
 
            if (this.r >= 1 || this.g >= 1 || this.b >= 1) {
                this.r = 1; this.g = 1; this.b = 1;
            }
 
            this.offsetValueX -= 0.05f*Time.deltaTime;
            this.skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(this.offsetValueX, 0));
 
            if (this.elpasedTime >= 10) {
                this.offsetValueX = 0;
                this.elpasedTime = 0;
                //this.text.text = this.elpasedTime.ToString();
                this.StartCoroutine(this.DayImpl());
                break;
            }
            yield return null;
        }
    }
 
    //낮
    IEnumerator DayImpl() {
        this.skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(this.offsetValueX, 0));
        for (int i = 0; i < this.arrMaterials.Length; i++){
            this.arrMaterials[i].SetColor("_EmissionColor", Color.black);
        }
 
        while (true) {
            this.elpasedTime += Time.deltaTime;
            //this.text.text = this.elpasedTime.ToString();
            //this.state.text = "낮";
 
            if (this.elpasedTime >= 10){
                this.StopAllCoroutines();
                this.elpasedTime = 0;
                this.StartCoroutine(this.DayToNightImpl());
                break;
            }
 
            yield return null;
        }
    }
}
