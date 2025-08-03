using System;
using ComBots.Battles.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace ComBots.Battles.Effects
{
   [CreateAssetMenu(menuName = "Effects/Simple Damage", fileName = "Simple Damage")]
   public class SimpleDamage : ScriptableObject
   {
      public VitalDefinition enduranceVital;

      public Condition nullifiedByCondition;
      
      string _basicBattleText;
      string _basicBattleTextMod = "";

      // base crit chance 5% heightened chance = 20%
      public void ApplyDamage(BotSo attackingBot, BotSo targettedBot, Software attack)
      {
         //todo
         // be specific about which condition prevented damage and how
         if (targettedBot.Conditions.Contains(nullifiedByCondition))
         {
            Debug.Log($"{targettedBot.name} guarded against the attack!");
            return;
         }
            
         targettedBot.GetHit(attackingBot, targettedBot, attack);
      }
      
      
      string GetBasicBattleText(BotSo targettedBot, Software attack)
      {
         return $"{targettedBot.name} was hit {_basicBattleTextMod} for: {attack.baseStats[SoftwareBaseStat.Power]}!";
      }

      public static int CalculateAttack(int attackMod, double rank, double attackBase)
      {
         double result;

         if (attackMod >= 0)
         {
            result = (1 + Math.Abs(attackMod) / 2.0) * rank * attackBase;
         }
         else
         {
            result = Math.Pow(1 + Math.Abs(attackMod) / 2.0, -1) * rank * attackBase;
         }

         return (int)Math.Ceiling(result); // Rounding up as specified
      }

      public static void Main()
      {
         // Example usage
         int attackMod = 3;  // Modifier stage
         double rank = 1.5;   // Example rank
         double attackBase = 100; // Base attack value

         int attackCurrent = CalculateAttack(attackMod, rank, attackBase);
         Console.WriteLine($"Calculated Attack: {attackCurrent}");
      }
   }

   
}



