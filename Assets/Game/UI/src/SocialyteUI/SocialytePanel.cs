using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
        [SerializeField] List<MenuTab> connectionTabs;
        
    
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
    
        List<SocialyteTab> _socialyteTabs = new();
        int _currentCategoryIndex;

        void Awake()
        {
            _socialyteTabs = connectionTabs.Where(b => b is SocialyteTab).Cast<SocialyteTab>().ToList();
            SetAllTabActions();
        }

        public override void OpenMenu()
        {
            base.OpenMenu();
            EventSystem.current.SetSelectedGameObject(_socialyteTabs[0].gameObject);
            SetPlayerInfo();
            SetTabConnections();
            
            inputSO.OnLeft += HandleLeftInput;
            inputSO.OnRight += HandleRightInput;
        }

        public override void CloseMenu()
        {
            base.CloseMenu();
            inputSO.OnLeft -= HandleLeftInput;
            inputSO.OnRight -= HandleRightInput;
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
            SetButtonOnSelect(connectionTab, SetConnectionTabAction);
            SetButtonOnSelect(feedTab, SetFeedTabAction);
            SetButtonOnSelect(_socialyteTabs[0], SetPlayerInfo);
            for (var i = 1; i < _socialyteTabs.Count; i++)
            {
                var currentTab = _socialyteTabs[i];
                SetButtonOnSelect(currentTab, () => SetConnectionInfo(currentTab.connection));
            }
        }

        void SetConnectionTabAction()
        {
            scrollRect.GameObject().SetActive(true);
            connectionDescriptionPanel.SetActive(true);
            StartCoroutine(ResetScrollPositionAndSelect(scrollRect, connectionTabs, 0));
            EventSystem.current.SetSelectedGameObject(_socialyteTabs[0].gameObject);
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
            SetActiveSubButton(categoryButtons, -1, ref _currentCategoryIndex);
        }

        void HandleRightInput()
        {
            SetActiveSubButton(categoryButtons, 1, ref _currentCategoryIndex);
        }
        
        void SetActiveSubButton(List<MenuTab> tabs, int direction, ref int currentIndex)
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
