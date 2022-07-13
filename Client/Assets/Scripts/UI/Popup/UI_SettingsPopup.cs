using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_SettingsPopup : UI_Popup
{
    // UI component name to bind
    enum Buttons
    {
        CloseButton
    }
    enum Dropdowns
    {
        ResolutionDropdown
    }
    enum Sliders
    {
        SoundSlider
    }
    enum Toggles
    {
        FullscreenToggle,
        SoundToggle
    }


    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TMP_Dropdown>(typeof(Dropdowns));
        Bind<Slider>(typeof(Sliders));
        Bind<Toggle>(typeof(Toggles));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton, Define.UIEvent.Click);

        // Set resolution dropdown
        TMP_Dropdown resolutionDropdown = GetDropdown((int)Dropdowns.ResolutionDropdown);
        resolutionDropdown.options.Clear();
        for (int i = 0; i < Managers.SceneLoad.Resolutions.Count; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            Resolution resolution = Managers.SceneLoad.Resolutions[i];
            option.text = $"{resolution.width} x {resolution.height}";
            resolutionDropdown.options.Add(option);

            if (resolution.width == Screen.width && resolution.height == Screen.height)
                resolutionDropdown.value = i;
        }
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(OnValueChangedResolutionDropdown);

        // Set fullscreen toggle
        Toggle fullscreenToggle = GetToggle((int)Toggles.FullscreenToggle);
        fullscreenToggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
        fullscreenToggle.onValueChanged.AddListener(OnValueChangedFullscreenToggle);

        // Set sound slider
        Slider soundSlider = GetSlider((int)Sliders.SoundSlider);
        soundSlider.onValueChanged.AddListener(OnValueChangedSoundSlider);

        // Set sound toggle
        Toggle soundToggle = GetToggle((int)Toggles.SoundToggle);
        soundToggle.onValueChanged.AddListener(OnValueChangedSoundToggle);
    }

    public void OnClickCloseButton(PointerEventData evt)
    {
        ClosePopup();
    }

    public void OnValueChangedResolutionDropdown(int option)
    {
        Resolution resolution = Managers.SceneLoad.Resolutions[option];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }
    public void OnValueChangedFullscreenToggle(bool isOn)
    {
        FullScreenMode screenMode = isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(Screen.width, Screen.height, screenMode);
    }

    public void OnValueChangedSoundSlider(float value)
    {

    }
    public void OnValueChangedSoundToggle(bool isOn)
    {

    }
}
