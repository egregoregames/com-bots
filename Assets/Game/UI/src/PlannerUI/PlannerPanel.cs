using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

namespace Game.UI.src.PlannerUI
{
    public class PlannerPanel: MenuPanel
    {
        [SerializeField] List<PlannerTab> subPlannerTabs = new();
        [SerializeField] CategoryTab requirementsTab;
        [SerializeField] CategoryTab electivesTab;
        [SerializeField] TextMeshProUGUI questNameText;
        [SerializeField] TextMeshProUGUI questPointsText;
        [SerializeField] TextMeshProUGUI questDescriptionText; 
        [SerializeField] GameObject setQuestGameObject;
        [SerializeField] ScrollRect scrollRect;
        readonly List<MenuTab> _subMenuTabs = new();
        int _currentSubIndex;
        int _currentCategoryIndex;

        void Awake()
        {
            SetAllButtonActions();
            _subMenuTabs.AddRange(subPlannerTabs);
        }

        void SetAllButtonActions()
        {
            SetButtonOnSelect(requirementsTab, SetRequirementsAction);
            SetButtonOnSelect(electivesTab, SetElectivesAction);

            for (int i = 0; i < subPlannerTabs.Count; i++)
            {
                var i1 = i;
                SetButtonOnSelect(subPlannerTabs[i], () => SetQuestTabAction(i1));
            }
        }

        void SetRequirementsAction()
        {
            //TODO: Adjust sub tabs to only have active as many requirement quests as player has, set all quest names
            foreach (var subTab in subPlannerTabs) // pull whether quest is complete or brand new from quest data
            {
                subTab.SetQuestStatuses(true, false);
            }
        }

        void SetElectivesAction()
        {
            //TODO: Adjust sub tabs to only have active as many elective quests as player has, set all quest names
            foreach (var subTab in subPlannerTabs) // pull whether quest is complete or brand new from quest data
            {
                subTab.SetQuestStatuses(false, true);
            }
        }
        
        void SetQuestTabAction(int questIndex)
        {
            //TODO: Figure out requirements quest list vs. electives quest list
            //TODO: Set description panel values.  Quest name, quest description text, quest points, quest giver name, quest giver image
            //TODO: Set is new quest to false 
            
            setQuestGameObject.SetActive(!subPlannerTabs[questIndex].isSelected);
            
            RemoveButtonListeners();
            subPlannerTabs[questIndex].onClick.AddListener(() => SelectQuest(questIndex));
        }

        void SelectQuest(int index)
        {
            //TODO: Add whatever logic selecting a quest entails
            setQuestGameObject.SetActive(false);

            for (var i = 0; i < subPlannerTabs.Count; i++)
            {
                subPlannerTabs[i].selectedGameObject.SetActive(i == index);
                subPlannerTabs[i].isSelected = i == index;
            }
        }
        
        public override void OpenMenu()
        {
            base.OpenMenu();
            requirementsTab.SelectEffect();
            ResetSubButtons();
            EventSystem.current.SetSelectedGameObject(subPlannerTabs[0].gameObject);
            StartCoroutine(ResetScrollPositionAndSelect(scrollRect, _subMenuTabs, 0));
        
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
            
            _currentSubIndex = 0;
            _currentCategoryIndex = 0;
        }
        
        void HandleLeftInput()
        {
            SetActiveCategoryButton(categoryButtons, -1, ref _currentCategoryIndex);
            StartCoroutine(ResetScrollPositionAndSelect(scrollRect, _subMenuTabs, 0));
            ResetSubButtons();
        }

        void HandleRightInput()
        {
            SetActiveCategoryButton(categoryButtons, 1, ref _currentCategoryIndex);
            StartCoroutine(ResetScrollPositionAndSelect(scrollRect, _subMenuTabs, 0));
            ResetSubButtons();
        }

        void HandleUpInput()
        {
            SetActiveSubButton(_subMenuTabs, -1, ref _currentSubIndex);
        }

        void HandleDownInput()
        {
           SetActiveSubButton(_subMenuTabs, 1, ref _currentSubIndex);
        }
        
        void ResetSubButtons()
        {
            _currentSubIndex = -1;
            SetActiveSubButton(_subMenuTabs, 1, ref _currentSubIndex);
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
        
        void RemoveButtonListeners()
        {
            foreach (var plannerTab in subPlannerTabs)
            {
                plannerTab.onClick.RemoveAllListeners();
            }
        }
        
        protected override void DeselectAllButtons()
        {
            base.DeselectAllButtons();
            subPlannerTabs.ForEach(b => b.DeselectEffect());
        }
    }
}
