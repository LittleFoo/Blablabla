using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using PathologicalGames;

[CustomEditor(typeof(FontAnalyse))]
public class FontDataAnalyseInspector : Editor {
	public bool expandPrefabs = true;
	// Use this for initialization
	public override void OnInspectorGUI()
	{
		FontAnalyse script = (FontAnalyse)target;

		EditorGUI.indentLevel = 0;
		PGEditorUtils.LookLikeControls();


		Rect sfxPathRect = EditorGUILayout.GetControlRect();
		// 用刚刚获取的文本输入框的位置和大小参数，创建一个文本输入框，用于输入特效路径 
		EditorGUI.TextField (sfxPathRect, "ImgPath", script.ImgPath);
		// 判断当前鼠标正拖拽某对象或者在拖拽的过程中松开了鼠标按键 
		// 同时还需要判断拖拽时鼠标所在位置处于文本输入框内 
		if ((Event.current.type == EventType.DragUpdated
			|| Event.current.type == EventType.DragExited)
			&& sfxPathRect.Contains (Event.current.mousePosition)) {
			// 判断是否拖拽了文件 
			if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0) {
				string sfxPath = DragAndDrop.paths [0];
				// 拖拽的过程中，松开鼠标之后，拖拽操作结束，此时就可以使用获得的 sfxPath 变量了 
				#if UNITY_EDITOR_OSX
					if (!string.IsNullOrEmpty (sfxPath) && Event.current.type == EventType.DragExited) {
				#else
					if (!string.IsNullOrEmpty (sfxPath) && Event.current.type == EventType.DragUpdated) {
				#endif
					DragAndDrop.AcceptDrag ();

					script.ImgPath = common.TextUtil.cut( sfxPath, "Resources/", false, false);
				}
			}
		}

		sfxPathRect = EditorGUILayout.GetControlRect();
		// 用刚刚获取的文本输入框的位置和大小参数，创建一个文本输入框，用于输入特效路径 
		EditorGUI.TextField (sfxPathRect, "DataPath", script.DataPath);
		// 判断当前鼠标正拖拽某对象或者在拖拽的过程中松开了鼠标按键 
		// 同时还需要判断拖拽时鼠标所在位置处于文本输入框内 
		if ((Event.current.type == EventType.DragUpdated
			|| Event.current.type == EventType.DragExited)
			&& sfxPathRect.Contains (Event.current.mousePosition)) {
			// 判断是否拖拽了文件 
			if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0) {
				string sfxPath = DragAndDrop.paths [0];
				// 拖拽的过程中，松开鼠标之后，拖拽操作结束，此时就可以使用获得的 sfxPath 变量了 
				#if UNITY_EDITOR_OSX
				if (!string.IsNullOrEmpty (sfxPath) && Event.current.type == EventType.DragExited) {
				#else
				if (!string.IsNullOrEmpty (sfxPath) && Event.current.type == EventType.DragUpdated) {
				#endif
					DragAndDrop.AcceptDrag ();

					script.DataPath = common.TextUtil.cut( sfxPath, "Resources/", false, false);
				}
			}
		}
//
//		script.matchPoolScale = EditorGUILayout.Toggle("Match Pool Scale", script.matchPoolScale);
//		script.matchPoolLayer = EditorGUILayout.Toggle("Match Pool Layer", script.matchPoolLayer);
//
//		script.dontReparent = EditorGUILayout.Toggle("Don't Reparent", script.dontReparent);
//
//		script._dontDestroyOnLoad = EditorGUILayout.Toggle("Don't Destroy On Load", script._dontDestroyOnLoad);
//
//		script.logMessages = EditorGUILayout.Toggle("Log Messages", script.logMessages);
		EditorGUILayout.BeginHorizontal();   // 1/2 the item button width
		GUILayout.Space(0);

		// Master add at end button. List items will insert
		if (GUILayout.Button(new GUIContent("Generate", "Click to generate data"), EditorStyles.toolbarButton))
		{
			if(script.ImgPath != null && script.DataPath != null)
			script.slice(script.ImgPath, script.DataPath);
		}

		if (GUILayout.Button(new GUIContent("Clear", "Click to generate data"), EditorStyles.toolbarButton))
		{
			script.clear();
		}
		EditorGUILayout.EndHorizontal();

		this.expandPrefabs = PGEditorUtils.SerializedObjFoldOutList<FontData>
			(
				"Per-Prefab Pool Options", 
				script.fontDataList,
				this.expandPrefabs,
				ref script._editorListItemStates,
				true
			);

		// Flag Unity to save the changes to to the prefab to disk
		if (GUI.changed)
			EditorUtility.SetDirty(target);
	}
}
