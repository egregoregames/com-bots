using System.Collections.Generic;
using UnityEngine;

namespace Game.Battles.Effects
{
    public abstract class SoftwareEffect : ScriptableObject, IAttackEffect
    {
        public abstract string ApplyEffect(BotSo attackingBot, BotSo targettedBot);

    }
    
    public interface IAttackEffect
    {
        public string ApplyEffect(BotSo attackingBot, BotSo targettedBot);
    }
}
