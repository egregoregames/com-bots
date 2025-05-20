using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.UI.src.SocialyteUI
{
    public class SocialytePanel : MenuPanel
    {
        [SerializeField] PlayerData playerData;
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] GameObject connectionDescriptionPanel;
        [SerializeField] SocialyteCategoryTab connectionTab;
        [SerializeField] SocialyteCategoryTab feedTab;
        [SerializeField] List<SocialyteTab> socialyteTabs;
        
        [Header("Player Profile")]
        [SerializeField] GameObject playerProfileGameObject;
        [SerializeField] TextMeshProUGUI playerNameText;
        [SerializeField] TextMeshProUGUI playerOccupationText;
        [SerializeField] TextMeshProUGUI playerConnectionText;
        [SerializeField] TextMeshProUGUI playerRankText;
        [SerializeField] TextMeshProUGUI playerBlueprintsText;
        [SerializeField] TextMeshProUGUI playerSoftwareText;
    
        [Header("Connection Profile")]
        [SerializeField] GameObject connectionProfileGameObject;
        [SerializeField] TextMeshProUGUI connectionNameText;
        [SerializeField] TextMeshProUGUI connectionOccupationText;
        [SerializeField] TextMeshProUGUI connectionOriginText;
        [SerializeField] TextMeshProUGUI connectionLocationText;
        [SerializeField] TextMeshProUGUI connectionBioText;
        [SerializeField] List<GameObject> connectionBondIcons;

        readonly List<MenuTab> _connectionTabs = new();
        int _currentCategoryIndex;
        int _currentSubIndex;

        void Awake()
        {
            _connectionTabs.AddRange(socialyteTabs);
            SetAllTabActions();
        }

        public override void OpenMenu()
        {
            base.OpenMenu();
            EventSystem.current.SetSelectedGameObject(socialyteTabs[0].gameObject);
            connectionTab.SelectEffect();
            SetPlayerInfo();
            SetTabConnections();
            
            inputSO.OnLeft += HandleLeftInput;
            inputSO.OnRight += HandleRightInput;
            inputSO.OnUp += HandleUpInput;
            inputSO.OnDown += HandleDownInput;
        }

        public override void CloseMenu()
        {
            base.CloseMenu();
            inputSO.OnLeft -= HandleLeftInput;
            inputSO.OnRight -= HandleRightInput;
            inputSO.OnUp -= HandleUpInput;
            inputSO.OnDown -= HandleDownInput;
        }

        void SetTabConnections()
        {
            for (var i = 1; i < socialyteTabs.Count; i++)
            {
                if (i > playerData.KnownConnections.Count)
                {
                    socialyteTabs[i].gameObject.SetActive(false);
                    continue;
                }
                
                socialyteTabs[i].gameObject.SetActive(true);
                var connection = playerData.KnownConnections[i - 1];
                socialyteTabs[i].connection = connection;
                socialyteTabs[i].SetConnectionStatus();
            }
        }

        void SetAllTabActions()
        {
            SetButtonOnSelect(connectionTab, SetConnectionTabAction);
            SetButtonOnSelect(feedTab, SetFeedTabAction);
            SetButtonOnSelect(socialyteTabs[0], SetPlayerInfo);
            for (var i = 1; i < socialyteTabs.Count; i++)
            {
                var currentTab = socialyteTabs[i];
                SetButtonOnSelect(currentTab, () => SetConnectionInfo(currentTab.connection));
            }
        }

        void SetConnectionTabAction()
        {
            SetTabConnections();
            scrollRect.GameObject().SetActive(true);
            connectionDescriptionPanel.SetActive(true);
            StartCoroutine(ResetScrollPositionAndSelect(scrollRect, _connectionTabs, 0));
            EventSystem.current.SetSelectedGameObject(socialyteTabs[0].gameObject);
        }

        void SetFeedTabAction()
        {
            scrollRect.GameObject().SetActive(false);
            connectionDescriptionPanel.SetActive(false);
            
            // TODO: Add whatever functionality the feed has.
        }

        void SetPlayerInfo()
        {
            //TODO: Get player data to set profile information. Using placeholder variables
        
            playerProfileGameObject.SetActive(true);
            connectionProfileGameObject.SetActive(false);
            
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
            
            //connectionLocationText.text = $"Checked In at: {connection.location}"; TODO: Get location information
            connectionNameText.text = connection.profileName;
            connectionOccupationText.text = connection.occupation; 
            connectionOriginText.text = connection.origin;
            connectionBioText.text = connection.bio;

            for (int i = 0; i < connectionBondIcons.Count; i++)
            {
                connectionBondIcons[i].SetActive(i < connection.bond);
            }
        }
        
        void HandleLeftInput()
        {
            SetActiveCategoryButton(categoryButtons, -1, ref _currentCategoryIndex);
            StartCoroutine(ResetScrollPositionAndSelect(scrollRect, _connectionTabs, 0));
            ResetSubButtons();
        }

        void HandleRightInput()
        {
            SetActiveCategoryButton(categoryButtons, 1, ref _currentCategoryIndex);
            StartCoroutine(ResetScrollPositionAndSelect(scrollRect, _connectionTabs, 0));
            ResetSubButtons();
        }
        
        void HandleUpInput()
        {
            SetActiveSubButton(_connectionTabs, -1, ref _currentSubIndex);
        }

        void HandleDownInput()
        {
            SetActiveSubButton(_connectionTabs, 1, ref _currentSubIndex);
        }
        
        void SetActiveCategoryButton(List<MenuTab> tabs, int direction, ref int currentIndex)
        {
            int newIndex = Mathf.Clamp(currentIndex + direction, 0, tabs.Count - 1);

            if (currentIndex == newIndex)
                return;

            currentIndex = newIndex;

            for (int i = 0; i < tabs.Count; i++)
            {
                if (!tabs[i].isActiveAndEnabled) break;

                if (i == currentIndex)
                {
                    tabs[i].SelectEffect();
                }
                else
                {
                    tabs[i].DeselectEffect();
                }
            }
        }
        
        void SetActiveSubButton(List<MenuTab> tabs, int direction, ref int currentIndex)
        {
            int lastActiveTab = tabs.Count(t => t.isActiveAndEnabled) - 1;
            int newIndex = Mathf.Clamp(currentIndex + direction, 0, lastActiveTab);
            
            currentIndex = newIndex;

            for (int i = 0; i < lastActiveTab; i++)
            {
                if (i == currentIndex)
                {
                    EventSystem.current.SetSelectedGameObject(tabs[i].gameObject);
                }
            }
        }
        
        void ResetSubButtons()
        {
            _currentSubIndex = -1;
            SetActiveSubButton(_connectionTabs, 1, ref _currentSubIndex);
        }
        
        IEnumerator ResetScrollPositionAndSelect(ScrollRect scrollRect, List<MenuTab> tabs, int selectIndex)
        {
            Canvas.ForceUpdateCanvases();
            
            scrollRect.normalizedPosition = new Vector2(0, 1);
            
            EventSystem.current.SetSelectedGameObject(null);
            yield return null;
            
            if (selectIndex >= 0 && selectIndex < tabs.Count)
            {
                var target = tabs[selectIndex].gameObject;
                EventSystem.current.SetSelectedGameObject(target);
            }
        }
    }
}
