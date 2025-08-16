using System.Collections.Generic;
using UnityEngine;

namespace ComBots.Battles.Effects
{
    public abstract class SoftwareEffect : ScriptableObject, IAttackEffect
    {
        public abstract void ApplyEffect(BotSo attackingBot, BotSo targettedBot, string applierName, Software attack = null);

    }
    
    public interface IAttackEffect
    {
        public void ApplyEffect(BotSo attackingBot, BotSo targettedBot, string applierName, Software attack);
    }
}
