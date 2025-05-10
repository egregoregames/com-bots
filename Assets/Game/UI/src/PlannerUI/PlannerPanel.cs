using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.src.PlannerUI
{
    public class PlannerPanel: MenuPanel
    {
        [SerializeField] List<MenuTab> subTabs = new();
        [SerializeField] PlannerCategoryTab requirementsTab;
        [SerializeField] PlannerCategoryTab electivesTab;
        [SerializeField] TextMeshProUGUI questNameText;
        [SerializeField] TextMeshProUGUI questPointsText;
        [SerializeField] TextMeshProUGUI questDescriptionText;
        [SerializeField] TextMeshProUGUI questGiverNameText;
        [SerializeField] Image questGiverImage;
        [SerializeField] ScrollRect scrollRect;
        int _currentSubIndex;
        int _currentCategoryIndex;

        void Awake()
        {
            SetButtonOnSelect(requirementsTab, SetRequirementsAction);
            SetButtonOnSelect(electivesTab, SetElectivesAction);
        }

        void SetRequirementsAction()
        {
            //TODO: Adjust sub tabs to only have active as many requirement quests as player has, set all quest names
        }

        void SetElectivesAction()
        {
            //TODO: Adjust sub tabs to only have active as many elective quests as player has, set all quest names
        }
        
        public override void OpenMenu()
        {
            base.OpenMenu();
            ResetSubButtons();
            SetQuestInfo(0);
        
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
        
        void HandleLeftInput()
        {
            SetActiveSubButton(categoryButtons, -1, ref _currentCategoryIndex);
            StartCoroutine(ResetScrollPositionAndSelect(scrollRect, subTabs, 0));
            ResetSubButtons();
        }

        void HandleRightInput()
        {
            SetActiveSubButton(categoryButtons, 1, ref _currentCategoryIndex);
            StartCoroutine(ResetScrollPositionAndSelect(scrollRect, subTabs, 0));
            ResetSubButtons();
        }

        void HandleUpInput()
        {
            SetActiveSubButton(subTabs, -1, ref _currentSubIndex);
            SetQuestInfo(_currentSubIndex);
        }

        void HandleDownInput()
        {
           SetActiveSubButton(subTabs, 1, ref _currentSubIndex);
           SetQuestInfo(_currentSubIndex);
        }
        
        void ResetSubButtons()
        {
            _currentSubIndex = -1;
            SetActiveSubButton(subTabs, 1, ref _currentSubIndex);
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
            // Step 1: Force layout to be updated
            Canvas.ForceUpdateCanvases();

            // Step 2: Reset scroll
            scrollRect.normalizedPosition = new Vector2(0, 1);

            // Step 3: Clear selection and wait a frame to let Unity finish UI layout
            EventSystem.current.SetSelectedGameObject(null);
            yield return null; // let one frame pass

            // Step 4: Re-assign the selected UI element
            if (selectIndex >= 0 && selectIndex < tabs.Count)
            {
                var target = tabs[selectIndex].gameObject;
                EventSystem.current.SetSelectedGameObject(target);
            }
        }

        void SetQuestInfo(int questIndex)
        {
            //TODO: Figure out requirements quest list vs. electives quest list
            //TODO: Set description panel values.  Quest name, quest description text, quest points, quest giver name, quest giver image
        }
    }
}
