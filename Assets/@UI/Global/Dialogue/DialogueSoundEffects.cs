using UnityEngine;

namespace ComBots.UI.Dialogue
{
    /// <summary>
    /// Contains references for the dialogue sound effects.
    /// </summary>
    [System.Serializable]
    public class DialogueSoundEffects
    {
        // =============== Sound Effects =============== //
        [Header("Sound Effects")]
        public AudioClip ContinueDialogue;
        public AudioClip ChooseOption;
        public AudioClip NavigateOptions;
        public AudioClip EndDialogue;
    }
}