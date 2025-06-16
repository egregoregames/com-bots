using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game.World.NPC
{
    public class DialogueUtils : MonoBehaviour
    {
        /// <summary>
        /// Swaps the name and the icon properly based on the NPC Scriptable Object data container.
        /// </summary>
        /// <param name="npcSo"></param>
        public static void OverrideNpcDialogueDisplayData(NpcSo npcSo)
        {
            if (npcSo == null) return;

            var conversation = DialogueManager.masterDatabase.GetConversation(npcSo.conversationKey);
            if (conversation == null) return;

            var actor = DialogueManager.masterDatabase.actors.Find(a => a.id == conversation.ConversantID);
            if (actor == null) return;

            actor.Name = npcSo.name;
            if (npcSo.portrait != null)
                actor.portrait = npcSo.portrait;
        }
    }
}
