using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ComBots.UI.src.BackpackUI
{
    public class BackpackPanel : MenuPanel
    {
        const int ColumnAmount = 5;
        const int FinalCategorySlot = 4;
        const int FirstBottomRowSlot = 24;
        const int FinalFirstRowSlot = 9;
        
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
        int _currentCategoryIndex;

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

            foreach (var categoryButton in categoryButtons)
            {
                categoryButton.isSelected = categoryButton == categoryTab;
                categoryButton.DeselectEffect();
            }
            
            //SetActivePocket(categoryIndex);
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

            _currentIndex = 0;
            _currentCategoryIndex = 0;
        }
        
        void HandleLeftInput()
        {
            if (_currentIndex <= FinalCategorySlot)
            {
                SetActiveSubButton(_allMenuTabs, -1, ref _currentIndex);
                _currentCategoryIndex = _currentIndex;
            }
            else
            {
                SetActiveSubButtonWrap(_allMenuTabs, -1, ref _currentIndex);
            }
        }

        void HandleRightInput()
        {
            switch (_currentIndex)
            {
                case < FinalCategorySlot:
                    SetActiveSubButton(_allMenuTabs, 1, ref _currentIndex);
                    _currentCategoryIndex = _currentIndex;
                    break;
                case FinalCategorySlot:
                    break;
                default:
                    SetActiveSubButtonWrap(_allMenuTabs, 1, ref _currentIndex);
                    break;
            }
        }

        void HandleUpInput()
        {
            if (_currentIndex <= FinalCategorySlot) return;
            if (_currentIndex <= FinalFirstRowSlot)
            {
                SetActiveSubButton(_allMenuTabs, 0, ref _currentCategoryIndex);
                _allMenuTabs[_currentIndex].DeselectEffect();
                _currentIndex = _currentCategoryIndex;
            }
            else
            {
                SetActiveSubButton(_allMenuTabs, -ColumnAmount, ref _currentIndex);
            }
        }

        void HandleDownInput()
        {
            if (_currentIndex >= FirstBottomRowSlot) return;
                
            if (_currentIndex <= FinalCategorySlot)
            {
                ResetSubButtons();
            }
            else
            {
                SetActiveSubButton(_allMenuTabs, ColumnAmount, ref _currentIndex);
            }
        }
        
        void ResetSubButtons()
        {
            _currentIndex = FinalCategorySlot;
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
        
        void SetActiveSubButtonWrap(List<MenuTab> tabs, int direction, ref int currentIndex)
        {
            int columns = 5;

            int row = currentIndex / columns;
            int col = currentIndex % columns;

            int newCol = col + direction;

            if (newCol < 0)
                newCol = columns - 1;
            else if (newCol >= columns)
                newCol = 0;

            int newIndex = row * columns + newCol;

            if (newIndex < 0 || newIndex >= tabs.Count || currentIndex == newIndex)
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

        protected override void DeselectAllButtons()
        {
            _allMenuTabs.ForEach(b => b.DeselectEffect());
        }
    }
}
