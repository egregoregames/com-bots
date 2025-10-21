using TMPro;
using UnityEngine;

public class CreditsDisplay : MonoBehaviourR3
{
    [field: SerializeField]
    private TextMeshProUGUI Text { get; set; }

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(

            // Udpate the text when the credits are updated
            PersistentGameData.GameEvents.OnCreditsUpdated(UpdateText),

            // Update the text when save data is loaded
            PersistentGameData.GameEvents.OnSaveDataLoaded(UpdateText)
        );

        UpdateText();
    }

    private async void UpdateText()
    {
        var instance = await PersistentGameData.GetInstanceAsync();
        Text.text = $"{instance.PlayerCredits:0.0}";
    }
}