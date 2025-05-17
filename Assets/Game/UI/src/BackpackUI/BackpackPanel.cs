using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.src.BackpackUI
{
    public class BackpackPanel : MenuPanel
    {
        const int ColumnAmount = 5;
        const int RowAmount = 1;
        
        [SerializeField] List<BackpackItemTab> itemTabs;
        [SerializeField] Sprite[] categoryIcons;
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] Image categoryImage;
        [SerializeField] Image itemImage;
        [SerializeField] TextMeshProUGUI itemNameText;
        [SerializeField] TextMeshProUGUI itemAmountText;
        [SerializeField] TextMeshProUGUI itemDescriptionText;
        [SerializeField] GameObject useBanner;
        [SerializeField] GameObject openBanner;
        readonly List<MenuTab> _allMenuTabs = new();
        
        int _currentIndex;

        protected void Awake()
        {
            SetAllButtonActions();
            _allMenuTabs.AddRange(categoryButtons);
            _allMenuTabs.AddRange(itemTabs);
        }
        
        void SetAllButtonActions()
        {
            for (int i = 0; i < categoryButtons.Count; i++)
            {
                var i1 = i;
                SetButtonOnSelect(categoryButtons[i], () => SetButtonCategories(categoryButtons[i1], i1));
            }

            for (int i = 0; i < itemTabs.Count; i++)
            {
                var i1 = i;
                SetButtonOnSelect(itemTabs[i], () => SetItemSlot(itemTabs[i1], i1));
            }
        }

        void SetButtonCategories(MenuTab categoryTab, int categoryIndex)
        {
            categoryImage.sprite = categoryIcons[categoryIndex];
            
            //TODO: Decide if an onclick to set the current pocket is needed
            //RemoveButtonListeners(categoryButtons);
            //categoryTab.onClick.AddListener(() => SetActivePocket(categoryIndex));
            useBanner.SetActive(false);
            openBanner.SetActive(true);
        }

        void SetActivePocket(int categoryIndex)
        {
            //TODO: Get item amount and item image for display from current pocket inventory
            //itemTab.itemImage.enabled = true;
            //itemTab.circleImage.enabled = false;
            //itemTab.itemImage.sprite = itemIndex.Image;
            //itemTab.amountText.text = $"{item.amount}";
        }

        void SetItemSlot(BackpackItemTab itemTab, int itemIndex)
        {
            RemoveButtonListeners(itemTabs);
            itemTab.onClick.AddListener(() => UseItem(itemTab));
            
            // TODO: get description sub panel values from current item
            // itemImage.sprite = currentInventoryPocket[itemIndex].Image;
            // itemNameText.text = currentInventoryPocket[itemIndex].Name;
            // itemAmountText.text = currentInventoryPocket[itemIndex].Amount;
            // itemDescriptionText.text = currentInventoryPocket[itemIndex].Description;
            
            useBanner.SetActive(true);
            openBanner.SetActive(false);
        }

        void UseItem(BackpackItemTab itemTab)
        {
            //TODO: Add item.use or whatever we decide to use the items effect if it has one.
            // TODO: Subtract quantity

            //itemTab.amountText.text = $"{item.amount}";
        }
        
        public override void OpenMenu()
        {
            base.OpenMenu();
            categoryButtons[0].SelectEffect();
            ResetSubButtons();
            EventSystem.current.SetSelectedGameObject(itemTabs[0].gameObject);
        
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
            if (_currentIndex < 6)
            {
                SetActiveSubButton(_allMenuTabs, -RowAmount, ref _currentIndex);
                // StartCoroutine(ResetScrollPositionAndSelect(scrollRect, itemTabs, 0));
                // ResetSubButtons();
            }
            else
            {
                SetActiveSubButton(_allMenuTabs, -RowAmount, ref _currentIndex);
            }
        }

        void HandleRightInput()
        {
            if (_currentIndex < 4)
            {
                SetActiveSubButton(_allMenuTabs, 1, ref _currentIndex);
                // StartCoroutine(ResetScrollPositionAndSelect(scrollRect, itemTabs, 0));
                // ResetSubButtons();
            }
            else
            {
                SetActiveSubButton(_allMenuTabs, 1, ref _currentIndex);
            }
        }

        void HandleUpInput()
        {
            SetActiveSubButton(_allMenuTabs, -ColumnAmount, ref _currentIndex);
        }

        void HandleDownInput()
        {
           SetActiveSubButton(_allMenuTabs, ColumnAmount, ref _currentIndex);
        }
        
        void ResetSubButtons()
        {
            _currentIndex = 4;
            SetActiveSubButton(_allMenuTabs, 1, ref _currentIndex);
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
        
        IEnumerator ResetScrollPositionAndSelect(ScrollRect scrollRect, List<BackpackItemTab> tabs, int selectIndex)
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
        
        void RemoveButtonListeners<T>(List<T> tabs) where T : MenuTab
        {
            foreach (var tab in tabs)
            {
                tab.onClick.RemoveAllListeners();
            }
        }
    }
}
