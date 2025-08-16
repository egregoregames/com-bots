using UnityEngine;

namespace ComBots.Battles
{
    public abstract class AttackModifyingEffect : ScriptableObject
    {
        public abstract void ApplyEffect(Software attack);

    }
}
