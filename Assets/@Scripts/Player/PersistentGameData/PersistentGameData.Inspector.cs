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
                    Quests.Update(1, 0);
                    Quests.Update(2, 0);
                    Quests.Update(3, 100);
                    Quests.Update(4, 100);
                    Quests.Update(5, 0);
                    Quests.Update(17, 0);
                    Quests.Update(18, 0);
                    Quests.Update(21, 0);
                    Quests.Update(22, 0);
                    Quests.Update(1009, 100);
                    Quests.Update(1010, 100);
                    Quests.Update(1011, 0);
                    Quests.Update(1012, 0);
                    Quests.Update(1013, 0);
                    Quests.Update(1014, 0);
                }

                if (GUILayout.Button("Generate Socialyte Connections", buttonHeight))
                {
                    Socialyte.AddConnection(1);
                    Socialyte.AddConnection(2);
                    Socialyte.AddConnection(3);
                    Socialyte.AddConnection(4);
                    Socialyte.AddConnection(5);
                    Socialyte.AddConnection(6);
                    Socialyte.AddConnection(7);
                    Socialyte.AddConnection(8);
                    Socialyte.AddConnection(9);
                    Socialyte.AddConnection(10);
                }

                if (GUILayout.Button("Generate Inventory Items (Key Items)", buttonHeight))
                {
                    // Key items
                    Inventory.AddItem(1001, 1); // Student ID Card
                    Inventory.AddItem(1002, 1); // Bank Card
                    Inventory.AddItem(1003, 1); // V-pass
                    Inventory.AddItem(1004, 1); // Omni-phone
                    Inventory.AddItem(1005, 1); // Uplink glove
                    Inventory.AddItem(1006, 1); // Solex
                    Inventory.AddItem(1007, 1); // Backpack module
                    Inventory.AddItem(1008, 1); // Blueprint drive
                    Inventory.AddItem(1009, 1); // Software drive
                    Inventory.AddItem(1010, 1); // Medal Case
                    Inventory.AddItem(1011, 1); // Dynaboard
                    Inventory.AddItem(1012, 1); // AutoRecyclobot
                    Inventory.AddItem(1013, 1); // Antigrav part
                    Inventory.AddItem(1014, 1); // Statue of talos
                    Inventory.AddItem(1015, 1); // Smiley Mask
                }
            }
        }
    }
}
