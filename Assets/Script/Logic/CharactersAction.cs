using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class CharactersAction : MonoBehaviour 
{
	public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();
	public List<CharactersActionData> actionDataList = new List<CharactersActionData>();
	public Transform tf;
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

			Sequence s;
			Tweener tweener;
			switch(action.actionType)
			{
			case Config.ColliderAction.Alpha:
				CharacterGroup cg = tf.GetComponent<CharacterGroup>();

				if(cg == null)
				{
					SpriteRenderer spr;
					Collider2D sprCol;
					spr = tf.GetComponent<SpriteRenderer>();
					sprCol = tf.GetComponent<Collider2D>();
					ColorUtil.toAlpha(spr, action.startAlpha);
//					ColorUtil.doFade(spr, action.endAlpha, action.duration).SetLoops(action.loop,LoopType.Yoyo).OnStepComplete(
//						()=>{
//							if(spr.color.a < 0.5f) 
//								sprCol.enabled = false;
//							else
//								sprCol.enabled = true;
//						});
					DoAlphaToEachSpr(spr, sprCol, action.endAlpha, action.duration, out tweener);
					if(action.loop > 1 && action.pauseTime > 0)
					{
						s = DOTween.Sequence();
						s.Append(tweener);
						s.AppendInterval(action.pauseTime);
						DoAlphaToEachSpr(spr, sprCol, action.startAlpha, action.duration, out tweener);
						s.Append(tweener);
						s.AppendInterval(action.pauseTime);
						s.SetLoops(action.loop);
					}
					else
					{
						tweener.SetLoops(action.loop, LoopType.Yoyo);
					}

				}
				else
				{
					Transform c;
					for(int j = 0; j < cg._character.Count; j++)
					{
						c = cg._character[j].tf;
						SpriteRenderer cspr = c.GetComponent<SpriteRenderer>();
						Collider2D csprCol = c.GetComponent<Collider2D>();

						ColorUtil.toAlpha(cspr, action.startAlpha);
						DoAlphaToEachSpr(cspr, csprCol, action.endAlpha, action.duration, out tweener);
//						ColorUtil.doFade(cspr, action.endAlpha, action.duration).SetLoops(action.loop,LoopType.Yoyo).OnStepComplete(
//							()=>{
//								if(cspr.color.a < 0.5f) 
//									csprCol.enabled = false;
//								else
//									csprCol.enabled = true;
//							});

						if(action.loop > 1 && action.pauseTime > 0)
						{
							s = DOTween.Sequence();
							s.Append(tweener);
							s.AppendInterval(action.pauseTime);
							DoAlphaToEachSpr(cspr, csprCol, action.startAlpha, action.duration, out tweener);
							s.Append(tweener);
							s.AppendInterval(action.pauseTime);
							s.SetLoops(action.loop);
						}
						else
						{
							tweener.SetLoops(action.loop, LoopType.Yoyo);
						}

					}
				}
				break;

			case Config.ColliderAction.Scale:
				tf.localScale = action.startVal;
				tweener = tf.DOScale(action.endVal, action.duration);
				if(action.loop > 1 && action.pauseTime > 0)
				{
					s = DOTween.Sequence();
					s.Append(tweener);
					s.AppendInterval(action.pauseTime);
					s.Append(tf.DOScale(action.startVal, action.duration));
					s.AppendInterval(action.pauseTime);
					s.SetLoops(action.loop);
				}
				else
				{
					tweener.SetLoops(action.loop, LoopType.Yoyo);
				}
				break;

			case Config.ColliderAction.Movement:
				tf.localPosition = action.startVal;
				tweener = tf.DOLocalMove(action.endVal, action.duration);//.SetLoops(action.loop,LoopType.Yoyo);
				if(action.loop > 1 && action.pauseTime > 0)
				{
					s = DOTween.Sequence();
					s.Append(tweener);
					s.AppendInterval(action.pauseTime);
					s.Append(tf.DOLocalMove(action.startVal, action.duration));
					s.AppendInterval(action.pauseTime);
					s.SetLoops(action.loop, LoopType.Yoyo);
				}
				else
				{
					tweener.SetLoops(action.loop);
				}
				break;

			case Config.ColliderAction.Rotation:
				tf.localRotation = Quaternion.Euler( action.startVal);
				tweener = tf.DOLocalRotate(action.endVal, action.duration,false).SetLoops(action.loop,LoopType.Yoyo);
				if(action.loop > 1 && action.pauseTime > 0)
				{
					s = DOTween.Sequence();
					s.Append(tweener);
					s.AppendInterval(action.pauseTime);
					s.Append(tf.DOLocalRotate(action.startVal, action.duration));
					s.AppendInterval(action.pauseTime);
					s.SetLoops(action.loop);
				}
				else
				{
					tweener.SetLoops(action.loop, LoopType.Yoyo);
				}
				break;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.gameObject.tag == "Player")
			trigger(Config.ActionTriggerType.onCollider);
//		else if(_curMoveTweener != null &&other.gameObject.tag == Config.TAG_CHAR)
//		{
//			_curMoveTweener.Kill();
//
//			float distance;
//			if(_curMoveTweener.CompletedLoops()%2 == 0)
//			{
//				distance = Vector3.Distance(tf.localPosition, _curMoveAction.startVal);
//
//				_curMoveTweener = tf.DOLocalMove(_curMoveAction.startVal,_curMoveAction.duration*distance/_curMoveAction.distance).SetEase(Ease.OutQuad);
//				_curMoveTweener.OnComplete(()=>{_curMoveTweener = tf.DOLocalMove(_curMoveAction.endVal, _curMoveAction.duration).SetLoops(_curMoveAction.loop,LoopType.Yoyo);});
//			}
//			else
//			{
//				distance = Vector3.Distance(tf.localPosition, _curMoveAction.endVal);
//				_curMoveTweener = tf.DOLocalMove(_curMoveAction.endVal, _curMoveAction.duration*distance/_curMoveAction.distance).SetEase(Ease.OutQuad);
//				_curMoveTweener.OnComplete(()=>{_curMoveTweener = tf.DOLocalMove(_curMoveAction.startVal, _curMoveAction.duration).SetLoops(_curMoveAction.loop,LoopType.Yoyo);});
//			}
//		}
	}


	private void DoAlphaToEachSpr(SpriteRenderer spr, Collider2D sprCol, float endVal, float duration, out Tweener tweener)
	{
		tweener = ColorUtil.doFade(spr, endVal, duration).OnComplete(
			()=>{
				if(spr.color.a < 0.5f) 
					sprCol.enabled = false;
				else
					sprCol.enabled = true;
			});
	}
}

[System.Serializable]
public class CharactersActionData
{
	public Config.ColliderAction actionType;
	public int loop = -1;
	public bool isTriggered = false;
	public Config.ActionTriggerType condition;
	public float pauseTime;
	#region movement\scale\rotation
	public Vector3 startVal;
	public Vector3 endVal;
	public float duration;
//	public float distance;
	#endregion

	#region alpha
	public float startAlpha;
	public float endAlpha;
	#endregion
}