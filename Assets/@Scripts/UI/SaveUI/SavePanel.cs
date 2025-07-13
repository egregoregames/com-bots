using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.UI.src.SaveUI
{
    public class SavePanel : MenuPanel
    {
        [SerializeField] TextMeshProUGUI saveDescription;
        string _initialSaveText;

        void Awake()
        {
            SetButtonOnSelect(categoryButtons[0], Save);
            SetButtonOnSelect(categoryButtons[1], Cancel);
            _initialSaveText = saveDescription.text;
        }

        public override void CloseMenu()
        {
            ToggleButtonsActive(true);
            base.CloseMenu();
            RemoveButtonListeners();
        }

        void Save()
        {
            RemoveButtonListeners();
            categoryButtons[0].onClick.AddListener(StartSaving);
        }

        void StartSaving()
        {
            StartCoroutine(StartSavingCoRO());
        }

        IEnumerator StartSavingCoRO()
        {
            // TODO: CAll save here
            
            ToggleButtonsActive(false);
            saveDescription.text = "Saving...";
            
            //TODO: yield return wait save complete
            yield return new WaitForSeconds(2f); //placeholder
            saveDescription.text = "Your progress has been saved!";    
            
            yield return new WaitForSeconds(1.0f);
            saveDescription.text = _initialSaveText;
            inputSO.OnCancel?.Invoke();
        }

        void Cancel()
        {
            RemoveButtonListeners();
            categoryButtons[1].onClick.AddListener(() => inputSO.OnCancel?.Invoke());
        }

        void RemoveButtonListeners()
        {
            foreach (var categoryButton in categoryButtons)
            {
                categoryButton.onClick.RemoveAllListeners();
            }
        }

        void ToggleButtonsActive(bool isActive)
        {
            foreach (var categoryButton in categoryButtons)
            {
                categoryButton.gameObject.SetActive(isActive);
            }
        }
    }
}
