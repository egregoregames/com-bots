using Sirenix.OdinInspector;
using System;
using ComBots.Battles.Vitals;
using UnityEngine;

namespace ComBots.Battles.Stats
{
    [Serializable]
    [InlineProperty] // Display values directly in the dictionary
    [HideReferenceObjectPicker] // Removes the reference picker
    public class Stat : Vital
    {
        [SerializeField, Range(-5, 5)]  private int _stage;

        [HorizontalGroup("Stats", LabelWidth = 50)]
        [PropertyOrder(2)]
        public int Stage
        {
            get => _stage;
            set => _stage = Mathf.Clamp(value, -5, 5);
        }

        public override void Restore()
        {
            base.Restore();
            Stage = 0;
        }
    }
}