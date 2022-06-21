using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class IndoorPlayer : MonoBehaviour
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
            if (_face == null)
                Init();
            _faceType = value;
            _face.SetCategoryAndLabel("face", _faceType.ToString());
        }
    }
    public JacketType JacketType
    {
        get { return _jacketType; }
        set
        {
            if (_body == null || _upperArm == null || _lowerArm == null)
                Init();
            _jacketType = value;
            _body.SetCategoryAndLabel("jacket_body", _jacketType.ToString());
            _upperArm.SetCategoryAndLabel("jacket_upper_arm", _jacketType.ToString());
            _lowerArm.SetCategoryAndLabel("jacket_lower_arm", _jacketType.ToString());
        }
    }
    public HairColor HairColor
    {
        get { return _hairColor; }
        set
        {
            if (_hairs.Count == 0)
                Init();
            _hairColor = value;
            if (_hairType != HairType.NoHair)
                _hairs[_hairType].GetComponent<SpriteResolver>().SetCategoryAndLabel(_hairType.ToString(), _hairColor.ToString());
        }
    }
    public float FaceColor_X
    {
        get { return _faceColorX; }
        set { _faceColorX = value; }
    }
    public float FaceColor_Y
    {
        get { return _faceColorY; }
        set { _faceColorY = value; }
    }
    public float FaceColor_Z
    {
        get { return _faceColorZ; }
        set { _faceColorZ = value; }
    }
    #endregion

    public Dictionary<string, FriendInfo> Friends { get; set; } = new Dictionary<string, FriendInfo>();

    public Place Place { get; set; }

    Dictionary<HairType, GameObject> _hairs = new Dictionary<HairType, GameObject>();
    SpriteResolver _face;
    SpriteResolver _body;
    SpriteResolver _upperArm;
    SpriteResolver _lowerArm;
    SpriteRenderer _faceRenderer;
    SpriteRenderer _upperHandRenderer;
    SpriteRenderer _lowerHandRenderer;
    SpriteRenderer _upperLegRenderer;
    SpriteRenderer _lowerLegRenderer;
    SpriteRenderer _upperFootRenderer;
    SpriteRenderer _lowerFootRenderer;


    private void Awake()
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
            _hairs.Add(hairType, gameObject.FindChild($"{hairType.ToString()}", recursive: true));
        }

        // Get player prefab's SpriteResolver component
        _face = gameObject.FindChild<SpriteResolver>("face", recursive: true);
        _body = gameObject.FindChild<SpriteResolver>("body", recursive: true);
        _upperArm = gameObject.FindChild<SpriteResolver>("upper_arm", recursive: true);
        _lowerArm = gameObject.FindChild<SpriteResolver>("lower_arm", recursive: true);

        // Get player prefab's SpriteRenderer componet
        _faceRenderer = gameObject.FindChild<SpriteRenderer>("face", recursive: true);
        _upperHandRenderer = gameObject.FindChild<SpriteRenderer>("upper_hand", recursive: true);
        _lowerHandRenderer = gameObject.FindChild<SpriteRenderer>("lower_hand", recursive: true);
        _upperLegRenderer = gameObject.FindChild<SpriteRenderer>("upper_leg", recursive: true);
        _lowerLegRenderer = gameObject.FindChild<SpriteRenderer>("lower_leg", recursive: true);
        _upperFootRenderer = gameObject.FindChild<SpriteRenderer>("upper_foot", recursive: true);
        _lowerFootRenderer = gameObject.FindChild<SpriteRenderer>("lower_foot", recursive: true);
    }

    public void SetBodyColor()
    {
        if (_faceRenderer == null || _upperHandRenderer == null || _lowerHandRenderer == null ||
            _upperLegRenderer == null || _lowerLegRenderer == null ||
            _upperFootRenderer == null || _lowerFootRenderer == null)
            Init();

        _faceRenderer.color = new Color(_faceColorX, _faceColorY, _faceColorZ, 1);
        _upperHandRenderer.color = new Color(_faceColorX, _faceColorY, _faceColorZ, 1);
        _lowerHandRenderer.color = new Color(_faceColorX, _faceColorY, _faceColorZ, 1);
        _upperLegRenderer.color = new Color(_faceColorX, _faceColorY, _faceColorZ, 1);
        _lowerLegRenderer.color = new Color(_faceColorX, _faceColorY, _faceColorZ, 1);
        _upperFootRenderer.color = new Color(_faceColorX, _faceColorY, _faceColorZ, 1);
        _lowerFootRenderer.color = new Color(_faceColorX, _faceColorY, _faceColorZ, 1);
    }
}
