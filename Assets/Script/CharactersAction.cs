using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class CharactersAction : MonoBehaviour 
{
	public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();
	public List<CharactersActionData> actionDataList = new List<CharactersActionData>();
	public Transform tf;
	private Tweener _curMoveTweener;
	private CharactersActionData _curMoveAction;
	void Awake()
	{
		tf = transform;
		trigger(Config.ActionTriggerType.Awake);
	}

	public void trigger(Config.ActionTriggerType condition)
	{
		CharactersActionData action;

		for(int i = 0; i < actionDataList.Count; i++)
		{
			action = actionDataList[i];
			if(action.condition != condition || action.isTriggered)
				continue;

			action.isTriggered = true;
			if(action.loop <= 0)
			{
				action.loop = int.MaxValue;
			}
			switch(action.actionType)
			{
			case Config.ColliderAction.Alpha:
				SpriteRenderer[] sprs = tf.GetComponentsInChildren<SpriteRenderer>();
				for(int j = 0; j < sprs.Length; j++)
				{
					ColorUtil.toAlpha(sprs[j], action.startAlpha);
					ColorUtil.doFade(sprs[j], action.endAlpha, action.duration).SetLoops(action.loop,LoopType.Yoyo);
				}
				break;

			case Config.ColliderAction.Scale:
				tf.localScale = action.startVal;
				tf.DOScale(action.endVal, action.duration).SetLoops(action.loop,LoopType.Yoyo);
				break;

			case Config.ColliderAction.Movement:
				tf.localPosition = action.startVal;
				action.distance = Vector3.Distance(action.endVal, action.startVal);
				_curMoveTweener = tf.DOLocalMove(action.endVal, action.duration).SetLoops(action.loop,LoopType.Yoyo);
				_curMoveAction = action;
				break;

			case Config.ColliderAction.Rotation:
				tf.localRotation = Quaternion.Euler( action.startVal);
				tf.DOLocalRotate(action.endVal, action.duration,false).SetLoops(action.loop,LoopType.Yoyo);
				break;
			}
		}
	}


	void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.gameObject.tag == "Player")
			trigger(Config.ActionTriggerType.onCollider);
		else if(_curMoveTweener != null &&other.gameObject.tag == Config.TAG_CHAR)
		{
			_curMoveTweener.Kill();

			float distance;
			if(_curMoveTweener.CompletedLoops()%2 == 0)
			{
				distance = Vector3.Distance(tf.localPosition, _curMoveAction.startVal);

				_curMoveTweener = tf.DOLocalMove(_curMoveAction.startVal,_curMoveAction.duration*distance/_curMoveAction.distance).SetEase(Ease.OutQuad);
				_curMoveTweener.OnComplete(()=>{_curMoveTweener = tf.DOLocalMove(_curMoveAction.endVal, _curMoveAction.duration).SetLoops(_curMoveAction.loop,LoopType.Yoyo);});
			}
			else
			{
				distance = Vector3.Distance(tf.localPosition, _curMoveAction.endVal);
				_curMoveTweener = tf.DOLocalMove(_curMoveAction.endVal, _curMoveAction.duration*distance/_curMoveAction.distance).SetEase(Ease.OutQuad);
				_curMoveTweener.OnComplete(()=>{_curMoveTweener = tf.DOLocalMove(_curMoveAction.startVal, _curMoveAction.duration).SetLoops(_curMoveAction.loop,LoopType.Yoyo);});
			}
		}
	}
}

[System.Serializable]
public class CharactersActionData
{
	public Config.ColliderAction actionType;
	public int loop = -1;
	public bool isTriggered = false;
	public Config.ActionTriggerType condition;
	#region movement\scale\rotation
	public Vector3 startVal;
	public Vector3 endVal;
	public float duration;
	public float distance;
	#endregion

	#region alpha
	public float startAlpha;
	public float endAlpha;
	#endregion
}