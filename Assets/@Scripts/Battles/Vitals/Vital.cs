using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ComBots.Battles.Vitals
{
    [Serializable]
    [InlineProperty] // Ensures the fields are directly displayed instead of a dropdown
    [HideReferenceObjectPicker] // Hides the small reference picker button
    public class Vital
    {
        [HorizontalGroup("Stats", LabelWidth = 50, Width = 80)]
        [Range(0, 1000)]
        [SerializeField] private float _current;
        [HorizontalGroup("Stats", LabelWidth = 30, Width = 70)]
        [Range(0, 1000)]
        [SerializeField] private int _max;

        
        public float Current
        {
            get => _current;
            set => _current = Mathf.Clamp(value, 0, Max);
        }
        
        public int Max
        {
            get => _max;
            set => _max = value;
        }

        public virtual void Restore()
        {
            Current = Max;
        }
    }
}