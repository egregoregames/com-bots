using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InGameDateTimeDisplay : MonoBehaviourR3
{
    [field: SerializeField]
    private TextMeshProUGUI Text { get; set; }

    [field: SerializeField]
    private string DateTimeFormat { get; set; } = "ddd hh:mm tt";

    private Dictionary<Term, string> TermNames { get; set; } = new();
    
    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(

            // Update the text no more than every second for performance reasons
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ => UpdateText()),

            // Udpate the text when the term is updated
            PersistentGameData.GameEvents.OnTermUpdated(UpdateText),

            // Update the text when save data is loaded
            PersistentGameData.GameEvents.OnSaveDataLoaded(UpdateText)
        );

        CacheTermDescriptions();
        UpdateText();
    }

    private void UpdateText()
    {
        string term = TermNames[PersistentGameData.Instance.CurrentTerm];
        Text.text = $"{DateTime.Now.ToString(DateTimeFormat)} ({term})";
    }

    private void CacheTermDescriptions()
    {
        foreach (Term term in Enum.GetValues(typeof(Term)).Cast<Term>())
        {
            TermNames[term] = term.GetDescription();
        }
    }
}
