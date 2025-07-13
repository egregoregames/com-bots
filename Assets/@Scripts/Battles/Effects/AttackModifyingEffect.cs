using UnityEngine;

namespace Game.Battles
{
    public abstract class AttackModifyingEffect : ScriptableObject
    {
        public abstract void ApplyEffect(Software attack);

    }
}
