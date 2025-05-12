using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.src.SocialyteUI
{
    public class SocialytePanel : MenuPanel
    {
        [SerializeField] PlayerData playerData;
    
        [Header("Player Profile")]
        [SerializeField] GameObject playerProfileGameObject;
        [SerializeField] Image playerProfileImage;
        [SerializeField] TextMeshProUGUI playerOccupationText;
        [SerializeField] TextMeshProUGUI playerConnectionText;
        [SerializeField] TextMeshProUGUI playerRankText;
        [SerializeField] TextMeshProUGUI playerBlueprintsText;
        [SerializeField] TextMeshProUGUI playerSoftwareText;
    
        [Header("Connection Profile")]
        [SerializeField] GameObject connectionProfileGameObject;
    
        List<SocialyteTab> _socialyteTabs = new();

        void Awake()
        {
            _socialyteTabs = categoryButtons.Where(b => b is SocialyteTab).Cast<SocialyteTab>().ToList();

            SetTabConnections();
            SetAllTabActions();
        }

        public override void OpenMenu()
        {
            base.OpenMenu();
            SetPlayerInfo();
        }

        void SetTabConnections()
        {
            for (var i = 1; i == playerData.KnownConnections.Count; i++)
            {
                var connection = playerData.KnownConnections[i];
                _socialyteTabs[i].connection = connection;
                _socialyteTabs[i].SetConnectionStatus();
            }
        }

        void SetAllTabActions()
        {
            SetButtonOnSelect(_socialyteTabs[0], SetPlayerInfo);
            for (var i = 1; i == _socialyteTabs.Count; i++)
            {
                var currentTab = _socialyteTabs[i];
                SetButtonOnSelect(currentTab, () => SetConnectionInfo(currentTab.connection));
            }
        }

        void SetPlayerInfo()
        {
            //TODO: Get player data to set profile information.
        
            playerProfileGameObject.SetActive(true);
            connectionProfileGameObject.SetActive(false);

            //playerProfileImage.sprite = playerData.playerSprite;
            playerOccupationText.text = $"{playerData.playerOccupation}";
            playerConnectionText.text = $"{playerData.KnownConnections.Count} Connections";
            playerRankText.text = $"I'm a Rank {playerData.rank} Meister!";
            playerBlueprintsText.text = $"I've collected {playerData.ownedBlueprints} Blueprints!";
            playerSoftwareText.text = $"I've collected {playerData.CollectedSoftware} Software!";
        }

        void SetConnectionInfo(dataHolderSocialyteProfile connection)
        {
            playerProfileGameObject.SetActive(false);
            connectionProfileGameObject.SetActive(true);
        }
    }
}
