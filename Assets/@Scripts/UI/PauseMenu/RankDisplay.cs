using R3;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Automatically displays rank information on the in-game HUD
/// </summary>
public class RankDisplay : MonoBehaviourR3
{
    [field: SerializeField]
    private TextMeshProUGUI Text { get; set; }

    [field: SerializeField]
    private Slider Slider { get; set; }

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(

            // Udpate the UI elements when the rank xp is updated
            PersistentGameData.GameEvents.OnRankXpUpdated(UpdateAll),

            // Update the UI elements when save data is loaded
            PersistentGameData.GameEvents.OnSaveDataLoaded(UpdateAll)
        );

        UpdateAll();
    }

    private void UpdateAll()
    {
        UpdateText();
        UpdateSlider();
    }

    private async void UpdateText()
    {
        Text.text = $"Rank {await PersistentGameData.GetPlayerRank()}";
    }

    private async void UpdateSlider()
    {
        Slider.value = await PersistentGameData.GetProgressToNextRank();
    }
}
