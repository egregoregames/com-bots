using ComBots.Utils.EntryPoints;
using TMPro;
using UnityEngine;

namespace ComBots.UI.OverheadWidgets
{
    public class OverheadWidget : Controllers.UIController
    {
        public override Dependency Dependency => Dependency.Independent;

        protected override string UserInterfaceName => "Overhead Widget";

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI labelText;

        protected override void Init() { }

        public override void Dispose()
        {
        }

        public void SetActive(string label)
        {
            if(labelText)
            labelText.text = label;
        }
    }
}