using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;


public class playerCustom : MonoBehaviour
{
    GameObject body, upper_arm, lower_arm, face;
    GameObject hair1, hair2, hair3, hair4, hair5, hair;
    GameObject upper_hand, lower_hand, upper_foot, lower_foot;

    HairType _hairType;
    FaceType _faceType;
    JacketType _jacketType;
    HairColor _hairColor;
    float _faceColorX, _faceColorY, _faceColorZ;

    //enum 기존 정의 사용하도록 수정 필요//////////
    enum HairType
    {
        NO_HAIR = 0,
        HAIR_ONE = 1,
        HAIR_TWO = 2,
        HAIR_THREE = 3,
        HAIR_FOUR = 4,
        HAIR_FIVE = 5
    }

    enum FaceType
    {
        NO_HAIR = 0,
        FACE_ONE = 1,
        FACE_TWO = 2,
        FACE_THREE = 3,
        FACE_FOUR = 4,
        FACE_FIVE = 5
    }

    enum JacketType
    {
        NO_JACKET = 0,
        ELECTRONIC_CONVERGENCE_ENGINEERING = 1,
        ROBOT = 2,
        COMPUTER_ENGINEERING = 3,
        MATHMATICS = 4,
        CHEMISTRY = 5,
        ELECTRICAL_BIOLOGICAL_PHYSICS = 6,
        SPORTS = 7,
        BUSINESS = 8,
        ENGLISH = 9,
        LAW = 10
    }

    enum HairColor
    {
        NO_COLOR = 0,
        RED = 1,
        BLUE = 2,
        PURPLE = 3,
        YELLOW = 4,
        BLACK = 5
    }

    //////////////////////////////////////////////

    private void Start()
    {
        //////////////////////////////////////////////
        getCustomVal();     // 서버에서 커스텀값 불러옴
        //////////////////////////////////////////////
        
        getSpriteObject();   //find sprite to change

        applyFaceColor();  // change face, hands, feet color
        setHairActive();    //active 1 or 0 hair type
        spriteSwap();    // Sprite Swap - faceType, jacketType(body, upper arm, lower arm), hairColor
    }

    //////////////////////////////////////////////
    private void getCustomVal()
    {
        //임의지정
        _hairType = (HairType)      3;
        _faceType = (FaceType)      2;
        _jacketType = (JacketType)  1;
        _hairColor = (HairColor)    4;
        _faceColorX = 2.7f;
        _faceColorY = 1.2f;
        _faceColorZ = 0.4f;
    }
    //////////////////////////////////////////////

    private void getSpriteObject()
    {
        body = transform.GetChild(0).GetChild(0).gameObject;
        upper_arm = body.transform.GetChild(0).GetChild(0).gameObject;
        lower_arm = body.transform.GetChild(0).GetChild(1).gameObject;
        face = body.transform.GetChild(0).GetChild(2).gameObject;

        hair1 = face.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        hair2 = face.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        hair3 = face.transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
        hair4 = face.transform.GetChild(0).GetChild(0).GetChild(3).gameObject;
        hair5 = face.transform.GetChild(0).GetChild(0).GetChild(4).gameObject;

        upper_hand = upper_arm.transform.GetChild(0).GetChild(0).gameObject;
        lower_hand = lower_arm.transform.GetChild(0).GetChild(0).gameObject;
        upper_foot = body.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).gameObject;
        lower_foot = body.transform.GetChild(0).GetChild(3).GetChild(1).GetChild(0).GetChild(0).gameObject;

    }

    private void spriteSwap()
    {
        face.GetComponent<SpriteResolver>().SetCategoryAndLabel("face", _faceType.ToString());
        body.GetComponent<SpriteResolver>().SetCategoryAndLabel("jacket_body", _jacketType.ToString());
        upper_arm.GetComponent<SpriteResolver>().SetCategoryAndLabel("jacket_upper_arm", _jacketType.ToString());
        lower_arm.GetComponent<SpriteResolver>().SetCategoryAndLabel("jacket_lower_arm", _jacketType.ToString());
        if (hair) 
            hair.GetComponent<SpriteResolver>().SetCategoryAndLabel(_hairType.ToString(), _hairColor.ToString());
    }

        
    private void applyFaceColor()
    {
        face.GetComponent<SpriteRenderer>().color = new Color(_faceColorX, _faceColorY, _faceColorZ, 1);
        upper_hand.GetComponent<SpriteRenderer>().color = new Color(_faceColorX, _faceColorY, _faceColorZ, 1);
        lower_hand.GetComponent<SpriteRenderer>().color = new Color(_faceColorX, _faceColorY, _faceColorZ, 1);
        upper_foot.GetComponent<SpriteRenderer>().color = new Color(_faceColorX, _faceColorY, _faceColorZ, 1);
        lower_foot.GetComponent<SpriteRenderer>().color = new Color(_faceColorX, _faceColorY, _faceColorZ, 1);
    }


    private void setHairActive()
    {
        hair1.SetActive(false);
        hair2.SetActive(false);
        hair3.SetActive(false);
        hair4.SetActive(false);
        hair5.SetActive(false);

        switch (_hairType)
        {
            case HairType.NO_HAIR: hair = null; break;
            case HairType.HAIR_ONE: hair = hair1; break;
            case HairType.HAIR_TWO: hair = hair2; break;
            case HairType.HAIR_THREE: hair = hair3; break;
            case HairType.HAIR_FOUR: hair = hair4; break;
            case HairType.HAIR_FIVE: hair = hair5; break;
        }
        if (hair)
            hair.SetActive(true);
    }
}
