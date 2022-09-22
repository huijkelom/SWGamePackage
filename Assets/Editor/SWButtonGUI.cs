using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(SWButton))]
public class SWButtonGUI : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SWButton SWButton = (SWButton)target;

        //SWButton.Graphics = EditorGUILayout.PropertyField();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Graphics"), true);
        serializedObject.ApplyModifiedProperties();
        SWButton.Pixels = EditorGUILayout.IntField("Pixels", SWButton.Pixels);

    }
}