using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviourR3
{
    [field: SerializeField]
    private TextMeshProUGUI Text { get; set; }

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(

            // Udpate the text when the money is updated
            PersistentGameData.GameEvents.OnMoneyUpdated(UpdateText),

            // Update the text when save data is loaded
            PersistentGameData.GameEvents.OnSaveDataLoaded(UpdateText)
        );

        UpdateText();
    }

    private async void UpdateText()
    {
        var instance = await PersistentGameData.GetInstanceAsync();
        Text.text = $"{instance.PlayerMoney}";
    }
}