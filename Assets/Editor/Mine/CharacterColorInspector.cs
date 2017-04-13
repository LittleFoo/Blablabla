using UnityEngine;
using UnityEditor;
using System.Text;
using System.Collections.Generic;

[CustomEditor(typeof(CharacterColor))]
public class CharacterColorInspector : Editor {
	public bool expandPrefabs = true;

	public override void OnInspectorGUI()
	{
		var script = (CharacterColor)target;

		//		EditorGUI.indentLevel = 0;
		//		PGEditorUtils.LookLikeControls();



		this.expandPrefabs = SerializedObjFoldOutList
			(
				"Per-Prefab Pool Options", 
				script.characterList,
				this.expandPrefabs,
				ref script._editorListItemStates,
				true,
				script.transform
			);

		// Flag Unity to save the changes to to the prefab to disk
		if (GUI.changed)
			EditorUtility.SetDirty(target);
	}

	public bool SerializedObjFoldOutList(string label, 
		List<CharacterColorData> list, 
		bool expanded,
		ref Dictionary<object, bool> foldOutStates,
		bool collapseBools,
		Transform tf)  
	{
		// Store the previous indent and return the flow to it at the end
		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		int buttonSpacer = 6;

		#region Header Foldout
		// Use a Horizanal space or the toolbar will extend to the left no matter what
		EditorGUILayout.BeginHorizontal();
		EditorGUI.indentLevel = 0;  // Space will handle this for the header
		GUILayout.Space(indent * 6); // Matches the content indent

		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

		expanded = PGEditorUtils.Foldout(expanded, label);
		if (!expanded)
		{
			// Don't add the '+' button when the contents are collapsed. Just quit.
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel = indent;  // Return to the last indent
			return expanded;
		}

		// BUTTONS...
		EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(100));

		// Add expand/collapse buttons if there are items in the list
		bool masterCollapse = false;
		bool masterExpand = false;
		if (list.Count > 0)
		{
			GUIContent content;
			var collapseIcon = '\u2261'.ToString();
			content = new GUIContent(collapseIcon, "Click to collapse all");
			masterCollapse = GUILayout.Button(content, EditorStyles.toolbarButton);

			var expandIcon = '\u25A1'.ToString();
			content = new GUIContent(expandIcon, "Click to expand all");
			masterExpand = GUILayout.Button(content, EditorStyles.toolbarButton);
		}
		else
		{
			GUILayout.FlexibleSpace();
		}

		EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(50));
		// A little space between button groups
		GUILayout.Space(buttonSpacer);

		// Main Add button
		if (GUILayout.Button(new GUIContent("+", "Click to add"), EditorStyles.toolbarButton))
			list.Add(new CharacterColorData());
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
		#endregion Header Foldout


		#region List Items
		// Use a for, instead of foreach, to avoid the iterator since we will be
		//   be changing the loop in place when buttons are pressed. Even legal
		//   changes can throw an error when changes are detected
		for (int i = 0; i < list.Count; i++)
		{
			CharacterColorData item = list[i];

			#region Section Header
			// If there is a field with the name 'name' use it for our label
			string itemLabel = PGEditorUtils.GetSerializedObjFieldName<CharacterColorData>(item);
			if (itemLabel == "") itemLabel = string.Format("Element {0}", i);


			// Get the foldout state. 
			//   If this item is new, add it too (singleton)
			//   Singleton works better than multiple Add() calls because we can do 
			//   it all at once, and in one place.
			bool foldOutState;
			if (!foldOutStates.TryGetValue(item, out foldOutState))
			{
				foldOutStates[item] = true;
				foldOutState = true;
			}

			// Force states if master buttons were pressed
			if (masterCollapse) foldOutState = false;
			if (masterExpand) foldOutState = true;

			// Use a Horizanal space or the toolbar will extend to the start no matter what
			EditorGUILayout.BeginHorizontal();
			EditorGUI.indentLevel = 0;  // Space will handle this for the header
			GUILayout.Space((indent+3)*6); // Matches the content indent

			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			// Display foldout with current state
			foldOutState = PGEditorUtils.Foldout(foldOutState, itemLabel);
			foldOutStates[item] = foldOutState;  // Used again below

			PGEditorUtils.LIST_BUTTONS listButtonPressed = PGEditorUtils.AddFoldOutListItemButtons();

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndHorizontal();

			#endregion Section Header


			// If folded out, display all serialized fields
			if (foldOutState == true)
			{
				EditorGUI.indentLevel = indent + 3;

				// Display Fields for the list instance
				//				PGEditorUtils.SerializedObjectFields<T>(item, collapseBools);
				System.Type type = typeof(CharacterColorData);
				//				System.Reflection.FieldInfo[] fields = type.GetFields();

				// Display Fields Dynamically

				item.character = EditorGUILayout.TextField("contentStr", item.character);

				PGEditorUtils.FieldInfoField<CharacterColorData>(item, type.GetField("color"));
				if(item.character != null && item.character.Length != 0)
				{
					StringBuilder sb = new StringBuilder("AssicID: ");
					item.assicIds.Clear();
					for(int n = 0; n < item.character.Length; n++)
					{
						sb.Append((int)(item.character[n]));
						sb.Append(", ");
						item.assicIds.Add((int)(item.character[n]));
					}
					EditorGUILayout.LabelField(sb.ToString());
				}
				GUILayout.Space(2);
			}



			#region Process List Changes
			// Don't allow 'up' presses for the first list item
			switch (listButtonPressed)
			{
				case PGEditorUtils.LIST_BUTTONS.None: // Nothing was pressed, do nothing
					break;

				case PGEditorUtils.LIST_BUTTONS.Up:
					if (i > 0)
					{
						CharacterColorData shiftItem = list[i];
						list.RemoveAt(i);
						list.Insert(i - 1, shiftItem);
					}
					break;

				case PGEditorUtils.LIST_BUTTONS.Down:
					// Don't allow 'down' presses for the last list item
					if (i + 1 < list.Count)
					{
						CharacterColorData shiftItem = list[i];
						list.RemoveAt(i);
						list.Insert(i + 1, shiftItem);
					}
					break;

				case PGEditorUtils.LIST_BUTTONS.Remove:
					list.RemoveAt(i);
					foldOutStates.Remove(item);  // Clean-up
					break;

				case PGEditorUtils.LIST_BUTTONS.Add:
					list.Insert(i, new CharacterColorData());
					break;
			}
			#endregion Process List Changes

		}
		#endregion List Items


		EditorGUI.indentLevel = indent;

		return expanded;
	}


	//	drawRegion(()=>{item.startVal = target.localPosition;},
	//		()=>{target.localPosition = item.startVal;},
	//		()=>{PGEditorUtils.FieldInfoField<CharacterColorData>(item, type.GetField("startVal"));});

	public delegate T DOGetter<out T> ();
	public delegate void DOSetter<in T> (T pNewValue);

	private void drawRegion(DOSetter<Vector3> reset,
		Vector3 resetVal,
		DOSetter<Vector3> go,
		Vector3 goVal,
		CharacterColorData item,
		string lableName)
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(45); // Matches the content indent
		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
		GUILayout.FlexibleSpace();

		EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(100));

		if(GUILayout.Button(new GUIContent("reset", "重置当前数值"),
			EditorStyles.toolbarButton))
		{
			reset(resetVal);
		}
		if(GUILayout.Button(new GUIContent("goto", "将物体变成所设置的数值"),
			EditorStyles.toolbarButton))
		{
			go(goVal);
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
		PGEditorUtils.FieldInfoField<CharacterColorData>(item, typeof(CharacterColorData).GetField(lableName));
	}

	private void drawRegion(DOSetter<Vector3> reset,
		Vector3 resetVal,
		DOSetter<Quaternion> go,
		Quaternion goVal,
		CharacterColorData item,
		string lableName)
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(45); // Matches the content indent
		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
		GUILayout.FlexibleSpace();

		EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(100));

		if(GUILayout.Button(new GUIContent("reset", "重置当前数值"),
			EditorStyles.toolbarButton))
		{
			reset(resetVal);
		}
		if(GUILayout.Button(new GUIContent("goto", "将物体变成所设置的数值"),
			EditorStyles.toolbarButton))
		{
			go(goVal);
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
		PGEditorUtils.FieldInfoField<CharacterColorData>(item, typeof(CharacterColorData).GetField(lableName));
	}

}
