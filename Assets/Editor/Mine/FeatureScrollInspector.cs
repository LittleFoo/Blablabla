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


		if (GUILayout.Button(new GUIContent("Recreate", "Click to recreate scroll"), EditorStyles.toolbarButton))
		{
			script.create();
		}

		script.speed = EditorGUILayout.FloatField("speed", script.speed);
		script.isClockWise = EditorGUILayout.Toggle("clockWise", script.isClockWise);
		if (GUI.changed)
			EditorUtility.SetDirty(target);
	}
}
