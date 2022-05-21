
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CustermizePopup : UI_Popup
{
    // each name of components to bind
    enum Sliders
    {
        BodyRSlider,
        BodyGSlider,
        BodyBSlider
    }
    enum Buttons
    {
        CloseButton,
        SaveButton,
        ResetButton,
        HairOneButton,
        HairTwoButton,
        HairThreeButton,
        HairFourButton,
        HairFiveButton,
        HairZeroButton,
        HairRedButton,
        HairBlueButton,
        HairYellowButton,
        HairPurpleButton,
        HairBlackButton,
        FaceOneButton,
        FaceTwoButton,
        FaceThreeButton,
        FaceFourButton,
        FaceFiveButton,
        BusinessButton,
        ChemistryButton,
        ComputerEngineeringButton,
        ElectricalBiologicalPhysicsButton,
        ElectronicConvergenceEngineeringButton,
        EnglishButton,
        LawButton,
        MathmaticsButton,
        RobotButton,
        SportsButton
    }

    UI_AlertPopup _alertPopup;
    UI_LoadingCirclePopup _loadingPopup;
 
    PlayerInfo playerInfo;

    HairType _realHairType;
    FaceType _realFaceType;
    JacketType _realJacketType;
    HairColor _realHairColor;
    float _realFaceColorX;
    float _realFaceColorY;
    float _realFaceColorZ;



    public override void Init()
    {
        base.Init();

        // bind each components
        Bind<Slider>(typeof(Sliders));
        Bind<Button>(typeof(Buttons));

        // bind each event
        #region bind event
        GetSlider((int)Sliders.BodyRSlider).onValueChanged.AddListener(OnValueChangedBodyRSlider);
        GetSlider((int)Sliders.BodyGSlider).onValueChanged.AddListener(OnValueChangedBodyGSlider);
        GetSlider((int)Sliders.BodyBSlider).onValueChanged.AddListener(OnValueChangedBodyBSlider);
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton, Define.UIEvent.Click);
        GetButton((int)Buttons.SaveButton).gameObject.BindEvent(OnClickSaveButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ResetButton).gameObject.BindEvent(OnClickResetButton, Define.UIEvent.Click);
        GetButton((int)Buttons.HairZeroButton).gameObject.BindEvent(OnClickHairZeroButton, Define.UIEvent.Click);
        GetButton((int)Buttons.HairOneButton).gameObject.BindEvent(OnClickHairOneButton, Define.UIEvent.Click);
        GetButton((int)Buttons.HairTwoButton).gameObject.BindEvent(OnClickHairTwoButton, Define.UIEvent.Click);
        GetButton((int)Buttons.HairThreeButton).gameObject.BindEvent(OnClickHairThreeButton, Define.UIEvent.Click);
        GetButton((int)Buttons.HairFourButton).gameObject.BindEvent(OnClickHairFourButton, Define.UIEvent.Click);
        GetButton((int)Buttons.HairFiveButton).gameObject.BindEvent(OnClickHairFiveButton, Define.UIEvent.Click);
        GetButton((int)Buttons.HairRedButton).gameObject.BindEvent(OnClickHairRedButton, Define.UIEvent.Click);
        GetButton((int)Buttons.HairBlueButton).gameObject.BindEvent(OnClickHairBlueButton, Define.UIEvent.Click);
        GetButton((int)Buttons.HairYellowButton).gameObject.BindEvent(OnClickHairYellowButton, Define.UIEvent.Click);
        GetButton((int)Buttons.HairPurpleButton).gameObject.BindEvent(OnClickHairPurpleButton, Define.UIEvent.Click);
        GetButton((int)Buttons.HairBlackButton).gameObject.BindEvent(OnClickHairBlackButton, Define.UIEvent.Click);
        GetButton((int)Buttons.FaceOneButton).gameObject.BindEvent(OnClickFaceOneButton, Define.UIEvent.Click);
        GetButton((int)Buttons.FaceTwoButton).gameObject.BindEvent(OnClickFaceTwoButton, Define.UIEvent.Click);
        GetButton((int)Buttons.FaceThreeButton).gameObject.BindEvent(OnClickFaceThreeButton, Define.UIEvent.Click);
        GetButton((int)Buttons.FaceFourButton).gameObject.BindEvent(OnClickFaceFourButton, Define.UIEvent.Click);
        GetButton((int)Buttons.FaceFiveButton).gameObject.BindEvent(OnClickFaceFiveButton, Define.UIEvent.Click);
        GetButton((int)Buttons.BusinessButton).gameObject.BindEvent(OnClickJacketBusinessButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ChemistryButton).gameObject.BindEvent(OnClickJacketChemistyButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ComputerEngineeringButton).gameObject.BindEvent(OnClickJacketCEButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ElectricalBiologicalPhysicsButton).gameObject.BindEvent(OnClickJacketEBPButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ElectronicConvergenceEngineeringButton).gameObject.BindEvent(OnClickJacketECEButton, Define.UIEvent.Click);
        GetButton((int)Buttons.EnglishButton).gameObject.BindEvent(OnClickJacketEnglishButton, Define.UIEvent.Click);
        GetButton((int)Buttons.LawButton).gameObject.BindEvent(OnClickJacketLawButton, Define.UIEvent.Click);
        GetButton((int)Buttons.MathmaticsButton).gameObject.BindEvent(OnClickJacketMathmaticButton, Define.UIEvent.Click);
        GetButton((int)Buttons.RobotButton).gameObject.BindEvent(OnClickJacketRobotButton, Define.UIEvent.Click);
        GetButton((int)Buttons.SportsButton).gameObject.BindEvent(OnClickJacketSportButton, Define.UIEvent.Click);
        #endregion

        // get lobby player custermize infomation 
        playerInfo = ((LobbyScene)Managers.SceneLoad.CurrentScene).PlayerInfo;

        // save previous player custermize infomation 
        SavePlayerInfo();

        // set sliders value
        GetSlider((int)Sliders.BodyRSlider).value = playerInfo.FaceColor_X;
        GetSlider((int)Sliders.BodyGSlider).value = playerInfo.FaceColor_Y;
        GetSlider((int)Sliders.BodyBSlider).value = playerInfo.FaceColor_Z;
    }

    void SavePlayerInfo()
    {
        _realHairType = playerInfo.HairType;
        _realFaceType = playerInfo.FaceType;
        _realJacketType = playerInfo.JacketType;
        _realHairColor = playerInfo.HairColor;
        _realFaceColorX = playerInfo.FaceColor_X;
        _realFaceColorY = playerInfo.FaceColor_Y;
        _realFaceColorZ = playerInfo.FaceColor_Z;
    }
    void ResetPlayerInfo()
    {
        playerInfo.HairType    = _realHairType  ;
        playerInfo.FaceType    = _realFaceType  ;
        playerInfo.JacketType  = _realJacketType;
        playerInfo.HairColor   = _realHairColor ;
        playerInfo.FaceColor_X = _realFaceColorX;
        playerInfo.FaceColor_Y = _realFaceColorY;
        playerInfo.FaceColor_Z = _realFaceColorZ;

        GetSlider((int)Sliders.BodyRSlider).value = playerInfo.FaceColor_X;
        GetSlider((int)Sliders.BodyGSlider).value = playerInfo.FaceColor_Y;
        GetSlider((int)Sliders.BodyBSlider).value = playerInfo.FaceColor_Z;
    }

    public void OnValueChangedBodyRSlider(float value)
    {
        playerInfo.FaceColor_X = value;
    }
    public void OnValueChangedBodyGSlider(float value)
    {
        playerInfo.FaceColor_Y = value;
    }
    public void OnValueChangedBodyBSlider(float value)
    {
        playerInfo.FaceColor_Z = value;
    }

    public void OnClickCloseButton(PointerEventData evt)
    {
        ResetPlayerInfo();
        ClosePopup();
    }
    public void OnClickResetButton(PointerEventData evt)
    {
        ResetPlayerInfo();
    }

    public void OnClickSaveButton(PointerEventData evt)
    {
        // Show Loading UI
        _loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingCirclePopup>();
        _loadingPopup.SetMessageText("커스터마이징 저장 중");

        // Send save packet to server
        B_SaveCustermize packet = new B_SaveCustermize();
        {
            packet.HairType = playerInfo.HairType;
            packet.FaceType = playerInfo.FaceType;
            packet.JacketType = playerInfo.JacketType;
            packet.HairColor = playerInfo.HairColor;
            packet.FaceColor = new Vector3D();
            packet.FaceColor.X = playerInfo.FaceColor_X;
            packet.FaceColor.Y = playerInfo.FaceColor_Y;
            packet.FaceColor.Z = playerInfo.FaceColor_Z;
        }
        Managers.Network.Send(packet);
    }
    public void OnRecvCustermizePacket()
    {
        // Update Player infomation
        SavePlayerInfo();
        
        // Close loading UI 
        _loadingPopup.ClosePopup();

        // Show alert UI
        _alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
        _alertPopup.SetMessageText("커스터마이징 저장 완료");
    }

    #region Button Click Handler (Custermization)
    public void OnClickHairZeroButton(PointerEventData evt)
    {
        playerInfo.HairType = HairType.NoHair;
    }
    public void OnClickHairOneButton(PointerEventData evt)
    {
        playerInfo.HairType = HairType.HairOne;
    }
    public void OnClickHairTwoButton(PointerEventData evt)
    {
        playerInfo.HairType = HairType.HairTwo;
    }
    public void OnClickHairThreeButton(PointerEventData evt)
    {
        playerInfo.HairType = HairType.HairThree;
    }
    public void OnClickHairFourButton(PointerEventData evt)
    {
        playerInfo.HairType = HairType.HairFour;
    }
    public void OnClickHairFiveButton(PointerEventData evt)
    {
        playerInfo.HairType = HairType.HairFive;
    }

    public void OnClickHairRedButton(PointerEventData evt)
    {
        playerInfo.HairColor = HairColor.Red;
    }
    public void OnClickHairBlueButton(PointerEventData evt)
    {
        playerInfo.HairColor = HairColor.Blue;
    }
    public void OnClickHairYellowButton(PointerEventData evt)
    {
        playerInfo.HairColor = HairColor.Yellow;
    }
    public void OnClickHairPurpleButton(PointerEventData evt)
    {
        playerInfo.HairColor = HairColor.Purple;
    }
    public void OnClickHairBlackButton(PointerEventData evt)
    {
        playerInfo.HairColor = HairColor.Black;
    }

    public void OnClickFaceOneButton(PointerEventData evt)
    {
        playerInfo.FaceType = FaceType.FaceOne;
    }
    public void OnClickFaceTwoButton(PointerEventData evt)
    {
        playerInfo.FaceType = FaceType.FaceTwo;
    }
    public void OnClickFaceThreeButton(PointerEventData evt)
    {
        playerInfo.FaceType = FaceType.FaceThree;
    }
    public void OnClickFaceFourButton(PointerEventData evt)
    {
        playerInfo.FaceType = FaceType.FaceFour;
    }
    public void OnClickFaceFiveButton(PointerEventData evt)
    {
        playerInfo.FaceType = FaceType.FaceFive;
    }

    public void OnClickJacketBusinessButton(PointerEventData evt)
    {
        playerInfo.JacketType = JacketType.Business;
    }
    public void OnClickJacketChemistyButton(PointerEventData evt)
    {
        playerInfo.JacketType = JacketType.Chemistry;
    }
    public void OnClickJacketCEButton(PointerEventData evt)
    {
        playerInfo.JacketType = JacketType.ComputerEngineering;
    }
    public void OnClickJacketEBPButton(PointerEventData evt)
    {
        playerInfo.JacketType = JacketType.ElectricalBiologicalPhysics;
    }
    public void OnClickJacketECEButton(PointerEventData evt)
    {
        playerInfo.JacketType = JacketType.ElectronicConvergenceEngineering;
    }
    public void OnClickJacketEnglishButton(PointerEventData evt)
    {
        playerInfo.JacketType = JacketType.English;
    }
    public void OnClickJacketLawButton(PointerEventData evt)
    {
        playerInfo.JacketType = JacketType.Law;
    }
    public void OnClickJacketMathmaticButton(PointerEventData evt)
    {
        playerInfo.JacketType = JacketType.Mathmatics;
    }
    public void OnClickJacketRobotButton(PointerEventData evt)
    {
        playerInfo.JacketType = JacketType.Robot;
    }
    public void OnClickJacketSportButton(PointerEventData evt)
    {
        playerInfo.JacketType = JacketType.Sports;
    }
    #endregion
}
