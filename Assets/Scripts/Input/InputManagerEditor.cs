using UnityEditor;
using UnityEngine;

namespace ControllerInputCreator
{
    [CustomEditor(typeof(InputCreator))]
    public class InputCreatorManager : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Update"))
            {
                (target as InputCreator).Init();
            }
        }
    }
}