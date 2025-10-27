using System.Collections.Generic;
using ComBots.TimesOfDay;
using UnityEngine;
using UnityEngine.UI;

namespace ComBots.World.NPCs
{
    [CreateAssetMenu(fileName = "NPC", menuName = "World/NPC/NPC Config")]
    public class NPC_Config : ScriptableObject
    {
        // =============== Active State Config =============== //
        [Header("Active State Config")]
        public NPC_ActiveStateConfig ActiveStateConfig;

        // =============== Conversations =============== //
        [Header("Conversations")]
        public List<NPC_ConversationConfig> Conversations;

        public NPC_ConversationConfig GetValidConversation(Term term, TimeOfDay timeOfDay)
        {
            foreach (var conversation in Conversations)
            {
                if (conversation.IsValid(term, timeOfDay))
                {
                    return conversation;
                }
            }
            return null;
        }

        // =============== Conditions =============== //
        [System.Serializable]
        public class QuestCondition
        {
            public string QuestName;
            public int MinQuestStage;

            public bool IsStatisfied()
            {
                // TODO: Once the quest system is implemented, create a real implementation here.
                return true;
            }
        }

        [System.Serializable]
        public class TimeCondition
        {
            // =============== Term =============== //
            [Header("Term")]
            public Term[] Terms = new Term[] { Term.FirstTerm, Term.SecondTerm, Term.ThirdTerm, Term.FourthTerm, Term.FifthTerm, Term.SixthTerm };
            // =============== Time of Day =============== //
            [Header("Time of Day")]
            public TimeOfDay[] TimesOfDay = new TimeOfDay[] { TimeOfDay.Day, TimeOfDay.Morning, TimeOfDay.Evening, TimeOfDay.Night };

            public bool IsStatisfied(Term term, TimeOfDay timeOfDay)
            {
                bool termSatisfied = false;
                foreach (var t in Terms)
                {
                    if (t == term)
                    {
                        termSatisfied = true;
                        break;
                    }
                }
                if (!termSatisfied)
                {
                    return false;
                }

                bool timeSatisfied = false;
                foreach (var tod in TimesOfDay)
                {
                    if (tod == timeOfDay)
                    {
                        timeSatisfied = true;
                        break;
                    }
                }
                if (!timeSatisfied)
                {
                    return false;
                }
                return true;
            }
        }
    }

    /// <summary>
    /// Configuration for an NPC's conversation, including time and quest conditions.
    /// </summary>
    [System.Serializable]
    public class NPC_ConversationConfig
    {
        /// <summary>
        /// The name of the conversation in the PixelCrushers database.
        /// </summary>
        public string NameInDatabase;
        // =============== Time Condition =============== //
        [Header("Time Condition")]
        public NPC_Config.TimeCondition TimeCondition;
        // =============== Quest =============== //
        [Header("Quest Condition")]
        public NPC_Config.QuestCondition[] QuestConditions;

        public bool IsValid(Term term, TimeOfDay day)
        {
            // Check Time Conditions
            if (!TimeCondition.IsStatisfied(term, day))
            {
                return false;
            }
            // Check quest Conditions
            foreach (var questCondition in QuestConditions)
            {
                if (!questCondition.IsStatisfied())
                {
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Configuration for when an NPC is active in the world.
    /// </summary>
    [System.Serializable]
    public struct NPC_ActiveStateConfig
    {
        // =============== Time Condition =============== //
        [Header("Time Condition")]
        public NPC_Config.TimeCondition TimeCondition;
        // =============== Quest Condition =============== //
        [Header("Quest Condition")]
        public List<NPC_Config.QuestCondition> QuestConditions;
    }
}