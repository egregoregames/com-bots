using System.Collections.Generic;
using Game.Battles.Effects;
using Game.Battles.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battles
{
   [CreateAssetMenu(fileName = "Condition")]
   public class Condition : ScriptableObject
   {

      [Header("Documentation")] [TextArea(2, 5)]
      public string documentation;
   
   
      public string BattleMessage;
   
      public Result Result;
      public TestType Type;
      public Dissolves Dissolves;
      public DissolutionTime DissolutionTime;
      public float percentage;

      public bool preventsBattle;
   
      public List<SoftwareEffect> endOfTurnEffects;

      private int conditionDuration = 0;
      public void OnGiveCondition()
      {
         conditionDuration = 0;
      }
   
      public void OnEndOfTurn(BotSo bot)
      {

         if (DissolutionTime == DissolutionTime.EndOfTurn)
         {
            TryRemoveCondition(bot);
         }

         conditionDuration++;

         if (name == "Freeze")
         {
            bool remove = DiceRoll.Roll(.5f + (.25f * (conditionDuration - 1)));
            if (remove)
            {
               Debug.Log($"{bot.name} thawed out!");
               return;
            }
         }

         foreach (var e in endOfTurnEffects)
         {
            e.ApplyEffect(bot, bot, name);
         }

      }

      private void TryRemoveCondition(BotSo bot)
      {
         if (bot.Conditions.Contains(this))
         {
            bot.Conditions.Remove(this);
            Debug.Log($"{bot.name} was cured of {name}!");
         }
      }

      public bool CanBotBattle(out string message)
      {
         if (preventsBattle)
         {
            message = BattleMessage;
            return false;
         }
         else
         {
            message = "";
            return true;
         }
      }
   
   }
}
