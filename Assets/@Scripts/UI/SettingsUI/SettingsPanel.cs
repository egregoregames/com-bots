using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ComBots.UI.src.SettingsUI
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
        
        public override void OpenMenu()
        {
            base.OpenMenu();
        
            inputSO.OnLeft += HandleLeftInput;
            inputSO.OnRight += HandleRightInput;
        }
        public override void CloseMenu()
        {
            base.CloseMenu();
        
            inputSO.OnLeft -= HandleLeftInput;
            inputSO.OnRight -= HandleRightInput;
        }

        void HandleLeftInput()
        {
            var selected = EventSystem.current.currentSelectedGameObject;
            if (selected != null && selected.TryGetComponent(out MenuTab tab))
            {
                tab.HandleHorizontalInput(-1);
            }
        }

        void HandleRightInput()
        {
            var selected = EventSystem.current.currentSelectedGameObject;
            if (selected != null && selected.TryGetComponent(out MenuTab tab))
            {
                tab.HandleHorizontalInput(1);
            }
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
            //TODO: Set Text speed
        }

        void SetMusicVolumeSlider()
        {
            //TODO: Set Music volume to slider value
        }

        void SetSfxVolumeSlider()
        {
            //TODO: Set sfx volume to slider value
        }
    }
}
