using System;
using Game.Battles.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battles.Effects
{
   [CreateAssetMenu(menuName = "Effects/Simple Damage", fileName = "Simple Damage")]
   public class SimpleDamage : ScriptableObject
   {
      public VitalDefinition enduranceVital;
      // base crit chance 5% heightened chance = 20%
      public string ApplyDamage(BotSo attackingBot, BotSo targettedBot, int damage)
      {
         targettedBot.Vitals[enduranceVital].Current -= damage;
         return $"{targettedBot.name} was hit for: {damage}!";
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



