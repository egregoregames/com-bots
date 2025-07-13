using System.Collections.Generic;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.src.BotlinkUI
{
    public class BotlinkPanel : MenuPanel
    {
        [SerializeField] string[] categoryDescriptions;
        [SerializeField] string[] categoryNames;
        [SerializeField] Sprite[] categoryIcons;
        [SerializeField] TextMeshProUGUI categoryDescriptionText;
        [SerializeField] TextMeshProUGUI categoryNameText;
        [SerializeField] Image categoryIconImage;
        [SerializeField] SubMenuPanel[] subMenuPanels;

        protected void Awake()
        {
            SetAllButtonActions();
            SetupNavigationWrapHorizontal(categoryButtons);
        }
        
        void SetAllButtonActions()
        {
            for (int i = 0; i < categoryButtons.Count; i++)
            {
                var ii = i;
                SetButtonOnSelect(categoryButtons[i], () => SetButtonOnSelect(categoryButtons[ii], ii));
            }
        }

        public override void OpenMenu()
        {
            base.OpenMenu();
            categoryButtons[0].SelectEffect();
        }

        public override void CloseMenu()
        {
            base.CloseMenu();
            subMenuPanels.ForEach(menu  => menu.CloseMenu());
        }

        void SetButtonOnSelect(MenuTab menuTab,int index)
        {
            RemoveButtonListeners();
            menuTab.onClick.AddListener(() => subMenuPanels[index].OpenMenu());
            categoryDescriptionText.text = categoryDescriptions[index];
            categoryNameText.text = categoryNames[index];
            categoryIconImage.sprite = categoryIcons[index];
        }
        
        void RemoveButtonListeners()
        {
            foreach (var categoryTab in categoryButtons)
            {
                categoryTab.onClick.RemoveAllListeners();
            }
        }
        
        void SetupNavigationWrapHorizontal(List<MenuTab> buttons)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Navigation nav = buttons[i].navigation;
                nav.mode = Navigation.Mode.Explicit;

                int leftIndex = (i - 1 + buttons.Count) % buttons.Count;
                int rightIndex = (i + 1) % buttons.Count;

                nav.selectOnLeft = buttons[leftIndex];
                nav.selectOnRight = buttons[rightIndex];

                buttons[i].navigation = nav;
            }
        }
    }
}
