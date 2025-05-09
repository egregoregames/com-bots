using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.src.SettingsUI
{
    public class SettingsPanel : MenuPanel
    {
        [SerializeField] SettingsTab windowedButton;
        [SerializeField] SettingsTab fullscreenButton;
        [SerializeField] SettingsTab slowTextButton;
        [SerializeField] SettingsTab mediumTextButton;
        [SerializeField] SettingsTab fastTextButton;
        [SerializeField] Slider musicVolumeSlider;
        [SerializeField] Slider sfxVolumeSlider;

        void Awake()
        {
            SetButtonOnSelect(windowedButton, SetWindowed);
            SetButtonOnSelect(fullscreenButton, SetFullscreen);
            SetButtonOnSelect(slowTextButton, SetTextSpeed);
            SetButtonOnSelect(mediumTextButton, SetTextSpeed);
            SetButtonOnSelect(fastTextButton, SetTextSpeed);
            
            musicVolumeSlider.onValueChanged.AddListener(_ => SetMusicVolumeSlider());
            musicVolumeSlider.onValueChanged.AddListener(_ => SetSfxVolumeSlider());
        }

        void SetButtonOnSelect(SettingsTab settingsTab, Action buttonAction)
        {
            settingsTab.onSelect += buttonAction;
        }
        
        void SetFullscreen()
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Screen.fullScreen = true;
        }

        void SetWindowed()
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.fullScreen = false;
        }

        void SetTextSpeed()
        {
            //Set Text speed
        }

        void SetMusicVolumeSlider()
        {
            //Set Music volume to slider value
        }

        void SetSfxVolumeSlider()
        {
            //Set sfx volume to slider value
        }
    }
}
