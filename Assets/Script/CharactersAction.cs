using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharactersAction : MonoBehaviour 
{
	public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();
	public List<CharactersActionData> actionDataList = new List<CharactersActionData>();
	public Transform tf;
	void Awake()
	{
		tf = transform;
	}

	public void trigger()
	{
		CharactersActionData action;
		for(int i = 0; i < actionDataList.Count; i++)
		{
			action = actionDataList[i];
			switch(action.actionType)
			{
			case Config.ColliderAction.Alpha:
				SpriteRenderer[] sprs = tf.GetComponents<SpriteRenderer>();
				for(int j = 0; j < sprs.Length; j++)
				{
					ColorUtil.doFade(sprs[j], action.endAlpha, action.duration);
				}
				break;

			case Config.ColliderAction.Scale:
				break;

			case Config.ColliderAction.Movement:
				break;

			case Config.ColliderAction.Rotation:
				break;
			}
		}
	}
}

[System.Serializable]
public class CharactersActionData
{
	public Config.ColliderAction actionType;

	#region movement\scale\rotation
	public Vector3 startVal;
	public Vector3 endVal;
	public float duration;
	#endregion

	#region alpha
	public float startAlpha;
	public float endAlpha;
	#endregion
}