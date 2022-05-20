using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int Id { get; set; }
    public string Name { get; set; }

    #region Custermization Info
    HairType _hairType;
    FaceType _faceType;
    JacketType _jacketType;
    HairColor _hairColor;
    float _faceColorX;
    float _faceColorY;
    float _faceColorZ;

    public HairType HairType 
    {
        get { return _hairType; }
        set
        {
            if (_hairs.Count == 0)
                Init();
            if (_hairType != HairType.NoHair)
                _hairs[_hairType].SetActive(false);
            _hairType = value;
            if (_hairType != HairType.NoHair)
                _hairs[_hairType].SetActive(true);
        } 
    }
    public FaceType FaceType
    {
        get { return _faceType; }
        set
        {
            if (_bodyMaterial == null)
                Init();
            _faceType = value;
            _bodyMaterial.SetTexture("_MainTex", Managers.Resource.Load<Texture>($"Textures/Player/Body/{_faceType.ToString()}"));
        }
    }
    public JacketType JacketType
    {
        get { return _jacketType; }
        set
        {
            if (_jacketMaterial == null)
                Init();
            _jacketType = value;
            _jacketMaterial.SetTexture("_MainTex", Managers.Resource.Load<Texture>($"Textures/Player/Jacket/{_jacketType.ToString()}"));
        }
    }
    public HairColor HairColor
    {
        get { return _hairColor; }
        set
        {
            if (_hairColors.Count == 0 || _hairs.Count == 0)
                Init();
            _hairColor = value;
            if (_hairType != HairType.NoHair)
                _hairs[_hairType].GetComponent<Renderer>().material.color = _hairColors[_hairColor];
        }
    }
    public float FaceColor_X
    {
        get { return _faceColorX; }
        set
        {
            if (_bodyMaterial == null)
                Init();
            _faceColorX = value;
            _bodyMaterial.SetFloat("_R", _faceColorX);
        }
    }
    public float FaceColor_Y
    {
        get { return _faceColorY; }
        set
        {
            if (_bodyMaterial == null)
                Init();
            _faceColorY = value;
            _bodyMaterial.SetFloat("_G", _faceColorY);
        }
    }
    public float FaceColor_Z
    {
        get { return _faceColorZ; }
        set
        {
            if (_bodyMaterial == null)
                Init();
            _faceColorZ = value;
            _bodyMaterial.SetFloat("_B", _faceColorZ);
        }
    }
    #endregion

    public Dictionary<string, FriendInfo> Friends { get; set; } = new Dictionary<string, FriendInfo>();

    Dictionary<HairType, GameObject> _hairs = new Dictionary<HairType, GameObject>();
    Dictionary<HairColor, Color> _hairColors = new Dictionary<HairColor, Color>();
    Material _bodyMaterial;
    Material _jacketMaterial;


    void Awake()
    {
        Init();
    }

    void Init()
    {
        // Get player prefab's hair objects
        foreach (HairType hairType in Enum.GetValues(typeof(HairType)))
        {
            if (hairType == HairType.NoHair)
                continue;
            _hairs.Add(hairType, gameObject.FindChild($"{hairType.ToString()}", recursive:true));
        }

        // Save hair color's data
        foreach (HairColor hairColor in Enum.GetValues(typeof(HairColor)))
        {
            if (hairColor == HairColor.NoColor)
                continue;
            switch (hairColor)
            {
                case HairColor.Red:
                    _hairColors.Add(hairColor, Color.red);
                    break;
                case HairColor.Blue:
                    _hairColors.Add(hairColor, Color.blue);
                    break;
                case HairColor.Purple:
                    _hairColors.Add(hairColor, Color.magenta);
                    break;
                case HairColor.Yellow:
                    _hairColors.Add(hairColor, Color.yellow);
                    break;
                case HairColor.Black:
                    _hairColors.Add(hairColor, Color.black);
                    break;
            }
        }

        // Get player prefab's body and jacket material
        _bodyMaterial = gameObject.FindChild<Renderer>("body", recursive:true).material;
        _jacketMaterial = gameObject.FindChild<Renderer>("jacket", recursive: true).material;
    }
}
