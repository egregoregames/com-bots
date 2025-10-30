using ComBots.Utils.EntryPoints;
using TMPro;
using UnityEngine;

namespace ComBots.UI.OverheadWidgets
{
    public class OverheadWidget : Controllers.UIController
    {
        // =============== UIController Implementation =============== //
        public override Dependency Dependency => Dependency.Independent;
        protected override string UserInterfaceName => "Overhead Widget";
        // =============== UI =============== //
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI labelText;

        #region Public API
        // ----------------------------------------
        // Public API 
        // ----------------------------------------

        public void SetActive(string label)
        {
            if (labelText)
            {
                labelText.text = label;
            }
        }

        #endregion

        #region UIController Implementation
        // ----------------------------------------
        // UIController Implementation 
        // ----------------------------------------

        protected override void Init() { }

        public override void Dispose()
        {
            labelText = null;
        }

        #endregion
    }
}