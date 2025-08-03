using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace ComBots.Battles.Effects.AttackMultiplierEffect
{
    [CreateAssetMenu(menuName = "Effects/Attack Multiplier Effect", fileName = "Attack Multiplier Effect")]
    public class AttackMultiplierEffect : AttackModifyingEffect
    {
        [MinMaxSlider(1, 10, true)] public Vector2Int AttacksRange;

        public override void ApplyEffect(Software attack)
        {
            var strikeCount = Random.Range(AttacksRange.x, AttacksRange.y);
            //attack.strikeCount = strikeCount;

            for (int i = 1; i < strikeCount; i++)
            {
                attack.internalDamageList.Add(attack.damage);
            }
            Debug.Log($"{attack.name} will strike {strikeCount} times!");
        }
    }

    
}