using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CharacterGroup))]
public class CharacterGroupInspector : Editor {
	SerializedObject serObj;
//	public bool expandPrefabs = true;
	public override void OnInspectorGUI()
	{
		CharacterGroup script = (CharacterGroup)target;

		script.contentStr = EditorGUILayout.TextField("contentStr", script.contentStr);
		script.fontObj = (Transform)EditorGUILayout.ObjectField("fontObj", script.fontObj, typeof(Transform)); 
		script.pivot = EditorGUILayout.Vector2Field("pivot", script.pivot);
		script.color = EditorGUILayout.ColorField("color",script.color);
//		this.expandPrefabs = PGEditorUtils.SerializedObjFoldOutList<CharacterCell>
//			(
//				"Per-Prefab Pool Options", 
//				script._character,
//				this.expandPrefabs,
//				ref script._editorListItemStates,
//				true
//			);
		
		// Flag Unity to save the changes to to the prefab to disk
		if (GUI.changed)
			EditorUtility.SetDirty(target);

	}
}
