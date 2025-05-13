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
        [SerializeField] TextMeshProUGUI playerNameText;
        [SerializeField] TextMeshProUGUI playerOccupationText;
        [SerializeField] TextMeshProUGUI playerConnectionText;
        [SerializeField] TextMeshProUGUI playerRankText;
        [SerializeField] TextMeshProUGUI playerBlueprintsText;
        [SerializeField] TextMeshProUGUI playerSoftwareText;
    
        [Header("Connection Profile")]
        [SerializeField] GameObject connectionProfileGameObject;
        [SerializeField] Image connectionProfileImage;
        [SerializeField] TextMeshProUGUI connectionNameText;
        [SerializeField] TextMeshProUGUI connectionOccupationText;
        [SerializeField] TextMeshProUGUI connectionOriginText;
        [SerializeField] TextMeshProUGUI connectionLocationText;
        [SerializeField] TextMeshProUGUI connectionBioText;
    
        List<SocialyteTab> _socialyteTabs = new();

        void Awake()
        {
            _socialyteTabs = categoryButtons.Where(b => b is SocialyteTab).Cast<SocialyteTab>().ToList();
            SetAllTabActions();
        }

        public override void OpenMenu()
        {
            base.OpenMenu();
            SetPlayerInfo();
            SetTabConnections();
        }

        void SetTabConnections()
        {
            if (playerData.KnownConnections.Count == 0) return;
            
            for (var i = 1; i < _socialyteTabs.Count; i++)
            {
                if (i > playerData.KnownConnections.Count)
                {
                    _socialyteTabs[i].gameObject.SetActive(false);
                    continue;
                }
                
                _socialyteTabs[i].gameObject.SetActive(true);
                var connection = playerData.KnownConnections[i - 1];
                _socialyteTabs[i].connection = connection;
                _socialyteTabs[i].SetConnectionStatus();
            }
        }

        void SetAllTabActions()
        {
            SetButtonOnSelect(_socialyteTabs[0], SetPlayerInfo);
            for (var i = 1; i < _socialyteTabs.Count; i++)
            {
                var currentTab = _socialyteTabs[i];
                SetButtonOnSelect(currentTab, () => SetConnectionInfo(currentTab.connection));
            }
        }

        void SetPlayerInfo()
        {
            //TODO: Get player data to set profile information. Using placeholder variables
        
            playerProfileGameObject.SetActive(true);
            connectionProfileGameObject.SetActive(false);

            //playerProfileImage.sprite = playerData.playerSprite;
            playerNameText.text = playerData.playerName;
            playerOccupationText.text = $"{playerData.playerOccupation}";
            playerConnectionText.text = $"{playerData.KnownConnections.Count} Connections";
            playerRankText.text = $"I'm a Rank {playerData.rank} Meister!";
            playerBlueprintsText.text = $"I've collected {playerData.ownedBlueprints} Blueprints!";
            playerSoftwareText.text = $"I've collected {playerData.CollectedSoftware.Count} Software!";
        }

        void SetConnectionInfo(dataHolderSocialyteProfile connection)
        {
            playerProfileGameObject.SetActive(false);
            connectionProfileGameObject.SetActive(true);

            //connectionProfileImage.sprite = connection.imagePortrait.GetComponent<Image>().sprite; TODO: Decide if this should be a GameObject or Image in the dataHolder
            //connectionLocationText.text = $"Checked In at: {connection.location}"; TODO: Get location information
            connectionNameText.text = connection.profileName;
            connectionOccupationText.text = connection.occupation; 
            connectionOriginText.text = connection.origin;
            connectionBioText.text = connection.bio;
        }
    }
}
