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
    }
}
