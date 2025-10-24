using System.Collections.Generic;
using ComBots.TimesOfDay;
using UnityEngine;
using UnityEngine.UI;

namespace ComBots.World.NPCs
{
    [CreateAssetMenu(fileName = "NPC", menuName = "World/NPC/NPC Config")]
    public class NPC_Config : ScriptableObject
    {
        [Header("Visibility Config")]
        public NPC_VisibilityConfig VisibilityConfig;

        // ============ Legacy Variables ============ //
        public Image portrait;
        public Image imageOverWorld;
        public bool potentialTeammate;
        public int bond;
        public string occupation;
        public string origin;
        public string bio;
    }

    [System.Serializable]
    public struct NPC_VisibilityConfig
    {
        [Header("Term")]
        public Term[] Terms;
        
        [Header("Time of Day")]
        public TimeOfDay[] TimesOfDay;

        [Header("Quest")]
        public List<NPC_VisibilityConfig_Quest> Quests;
    }

    [System.Serializable]
    public struct NPC_VisibilityConfig_Quest
    {
        public string QuestName;
        public int MinQuestStage;
    }
}