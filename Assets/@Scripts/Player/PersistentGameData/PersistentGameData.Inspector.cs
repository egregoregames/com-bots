using UnityEditor;
using UnityEngine;

public partial class PersistentGameData
{
    [CustomEditor(typeof(PersistentGameData))]
    private class Inspector : Editor
    {
        private PersistentGameData _target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_target == null)
                _target = (PersistentGameData)target;


            if (!Application.isPlaying) return;

            if (GUILayout.Button("Save Data", GUILayout.Height(40)))
            {
                ComBotsSaveSystem.Save("TestSave1");
            }

            if (GUILayout.Button("Load Data", GUILayout.Height(40)))
            {
                ComBotsSaveSystem.Load("TestSave1");
            }
        }
    }
}
