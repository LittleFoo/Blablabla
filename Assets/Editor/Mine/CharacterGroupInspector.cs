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
	
		if (GUILayout.Button(new GUIContent("Alignment", "Click to alignment."), EditorStyles.toolbarButton))
		{
			SceneSetting scene = FindObjectOfType<SceneSetting>();
			CharacterGroup[] groups = FindObjectsOfType<CharacterGroup>();
			FontAnalyse font;
			FontData d;
			float HCenterY;
			float desY;
			float num;
			float gridSize = GlobalController.instance.setting.gridSize;
			for(int i = 0; i < groups.Length; i++)
			{
				if(groups[i].fontObj == null)
					continue;
				font = groups[i].fontObj.GetComponent<FontAnalyse>();
				if(font.fontDatas.TryGetValue( (int)'H', out d))
				{
					HCenterY = groups[i].transform.position.y -d._actualOffsetY + (1-groups[i].pivot.y)*font.lineHeight;
					num = Mathf.Floor(HCenterY /gridSize);
					desY = num*gridSize+gridSize*0.5f - HCenterY;
					groups[i].transform.position = new Vector3(groups[i].transform.position.x,groups[i].transform.position.y + desY,groups[i].transform.position.z);

					desY = groups[i].transform.position.x - groups[i].pivot.x* groups[i].textWidth;
					if(desY < scene.boundsX.x)
						scene.boundsX.x = desY;

					desY = groups[i].transform.position.x +( 1- groups[i].pivot.x)* groups[i].textWidth;
					if(desY > scene.boundsX.y)
						scene.boundsX.y = desY;

					desY = groups[i].transform.position.y - groups[i].pivot.y* groups[i].analyse.lineHeight;
					if(desY < scene.boundsY.x)
						scene.boundsY.x = desY;

					desY = groups[i].transform.position.y +( 1- groups[i].pivot.y)* groups[i].analyse.lineHeight;
					if(desY > scene.boundsY.y)
						scene.boundsY.y = desY;
						
				}
			}

			scene.boundsX.x -= GlobalController.instance.setting.gridSize;
			scene.boundsX.y += GlobalController.instance.setting.gridSize;
			scene.boundsY.x -= GlobalController.instance.setting.gridSize;
			scene.boundsY.y += GlobalController.instance.setting.gridSize;
		}

		script.contentStr = EditorGUILayout.TextField("contentStr", script.contentStr);
		script.fontObj = (Transform)EditorGUILayout.ObjectField("fontObj", script.fontObj, typeof(Transform)); 
		script.pivot = EditorGUILayout.Vector2Field("pivot", script.pivot);
		script.color = EditorGUILayout.ColorField("color",script.color);
		script.createColliderForChar = EditorGUILayout.Toggle("createCollider",script.createColliderForChar);
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
