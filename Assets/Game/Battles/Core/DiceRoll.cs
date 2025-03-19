using UnityEngine;

namespace Game.Battles
{
    public static class DiceRoll 
    {
        public static bool Roll(float chance)
        {
            var roll = Random.Range(0f, 1f);

            return roll < chance;
        }
    }
}
