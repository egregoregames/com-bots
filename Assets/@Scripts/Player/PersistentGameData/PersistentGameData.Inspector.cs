using UnityEditor;
using UnityEngine;

public partial class PersistentGameData
{
    [CustomEditor(typeof(PersistentGameData))]
    private class Inspector : Editor
    {
        private PersistentGameData _target;
        private bool _isTestAreaOpen;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_target == null)
                _target = (PersistentGameData)target;


            if (!Application.isPlaying) return;

            _isTestAreaOpen = EditorGUILayout.Foldout(
                _isTestAreaOpen, new GUIContent("Test area"));

            if (_isTestAreaOpen)
            {
                var buttonHeight = GUILayout.Height(40);

                if (GUILayout.Button("Save Data", buttonHeight))
                {
                    ComBotsSaveSystem.Save("TestSave1");
                }

                if (GUILayout.Button("Load Data", buttonHeight))
                {
                    ComBotsSaveSystem.Load("TestSave1");
                }

                if (GUILayout.Button("Generate Quests", buttonHeight))
                {
                    Quests.UpdateQuest(1, 0);
                    Quests.UpdateQuest(2, 0);
                    Quests.UpdateQuest(3, 100);
                    Quests.UpdateQuest(4, 100);
                    Quests.UpdateQuest(5, 0);
                    Quests.UpdateQuest(17, 0);
                    Quests.UpdateQuest(18, 0);
                    Quests.UpdateQuest(21, 0);
                    Quests.UpdateQuest(22, 0);
                    Quests.UpdateQuest(1009, 100);
                    Quests.UpdateQuest(1010, 100);
                    Quests.UpdateQuest(1011, 0);
                    Quests.UpdateQuest(1012, 0);
                    Quests.UpdateQuest(1013, 0);
                    Quests.UpdateQuest(1014, 0);
                }

                if (GUILayout.Button("Generate Socialyte Connections", buttonHeight))
                {
                    AddSocialyteConnection(1);
                    AddSocialyteConnection(2);
                    AddSocialyteConnection(3);
                    AddSocialyteConnection(4);
                    AddSocialyteConnection(5);
                    AddSocialyteConnection(6);
                    AddSocialyteConnection(7);
                    AddSocialyteConnection(8);
                    AddSocialyteConnection(9);
                    AddSocialyteConnection(10);
                }

                if (GUILayout.Button("Generate Inventory Items (Key Items)", buttonHeight))
                {
                    // Key items
                    AddInventoryItem(1001, 1); // Student ID Card
                    AddInventoryItem(1002, 1); // Bank Card
                    AddInventoryItem(1003, 1); // V-pass
                    AddInventoryItem(1004, 1); // Omni-phone
                    AddInventoryItem(1005, 1); // Uplink glove
                    AddInventoryItem(1006, 1); // Solex
                    AddInventoryItem(1007, 1); // Backpack module
                    AddInventoryItem(1008, 1); // Blueprint drive
                    AddInventoryItem(1009, 1); // Software drive
                    AddInventoryItem(1010, 1); // Medal Case
                    AddInventoryItem(1011, 1); // Dynaboard
                    AddInventoryItem(1012, 1); // AutoRecyclobot
                    AddInventoryItem(1013, 1); // Antigrav part
                    AddInventoryItem(1014, 1); // Statue of talos
                    AddInventoryItem(1015, 1); // Smiley Mask
                }
            }
        }
    }
}
