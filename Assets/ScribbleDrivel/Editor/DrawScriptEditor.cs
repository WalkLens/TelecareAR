using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace LylekGames
{
    [CustomEditor(typeof(DrawScript))]
    public class DrawScriptEditor : Editor
    {
        DrawScript myDrawScript;
        SerializedObject serialObject;
        SerializedProperty optimizeProperty;
        
        public void OnEnable()
        {
            myDrawScript = (DrawScript)target;
            serialObject = new SerializedObject(myDrawScript);
            optimizeProperty = serialObject.FindProperty("optimize");

            myDrawScript.GetDefaultSettings();
        }
        public override void OnInspectorGUI()
        {
            if (myDrawScript.optimize)
            {
                if (myDrawScript.canvas && myDrawScript.canvas.renderMode == RenderMode.WorldSpace)
                {
                    EditorGUILayout.HelpBox("Optimization should only be used with a Screen Space Canvas!", MessageType.Warning);
                    if (GUILayout.Button("Fix"))
                        myDrawScript.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    GUI.enabled = false;
                }
                else if (myDrawScript.transform.rotation != Quaternion.identity)
                {
                    EditorGUILayout.HelpBox("Optimization is not compatible with rotated/askew Canvas!", MessageType.Error);
                    if (GUILayout.Button("Fix"))
                        myDrawScript.transform.rotation = Quaternion.identity;
                    GUI.enabled = false;
                }
                else
                {
                    EditorGUILayout.HelpBox("Optimization will increase performance, but may cause blurriness to the image.", MessageType.Info);
                }
            }
            DrawDefaultInspector();
        }
    }
}
