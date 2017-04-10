using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using PathologicalGames;

[CustomEditor(typeof(FeatureScroll))]
public class FeatureScrollInspector : Editor {

	public override void OnInspectorGUI()
	{
		FeatureScroll script = (FeatureScroll)target;

		GUILayout.Space(6);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button(new GUIContent("Recreate", "Click to recreate scroll"), EditorStyles.toolbarButton))
		{
			script.create();
		}

		if (GUILayout.Button(new GUIContent("Clear", "Press this before remove"), EditorStyles.toolbarButton))
		{
			script.clear();
		}
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(2);
		script.speed = EditorGUILayout.FloatField("speed", script.speed);
		if (GUI.changed)
			EditorUtility.SetDirty(target);
	}
}
