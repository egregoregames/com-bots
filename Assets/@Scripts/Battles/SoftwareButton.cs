using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ComBots.Battles.BattleUI
{
    public class SoftwareButton : Button
    {
        private Software _software;
        private TextMeshProUGUI _softwareLabel;

        protected override void Awake()
        {
            base.Awake();
            _softwareLabel = GetComponentInChildren<TextMeshProUGUI>();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            UpdateUIWithSoftwareInfo();
        }

        public void AssignSoftwareToButton(Software software, Action<Software> softwareSelected)
        {
            _software = software;
            _softwareLabel.text = _software.name;
            
            onClick.RemoveAllListeners();
            onClick.AddListener(() => softwareSelected?.Invoke(software));
        }

        void UpdateUIWithSoftwareInfo()
        {
            BattleText.Instance.UpdateBattleText(_software);
        }
    }
}
