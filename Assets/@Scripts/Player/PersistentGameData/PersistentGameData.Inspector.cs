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
                    UpdateQuest(1, 0);
                    UpdateQuest(2, 0);
                    UpdateQuest(3, 100);
                    UpdateQuest(4, 100);
                    UpdateQuest(5, 0);
                    UpdateQuest(17, 0);
                    UpdateQuest(18, 0);
                    UpdateQuest(21, 0);
                    UpdateQuest(22, 0);
                    UpdateQuest(1009, 100);
                    UpdateQuest(1010, 100);
                    UpdateQuest(1011, 0);
                    UpdateQuest(1012, 0);
                    UpdateQuest(1013, 0);
                    UpdateQuest(1014, 0);
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
            }
        }
    }
}
