using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class CharactersAction : MonoBehaviour,IAwake
{
	public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();
	public List<CharactersActionData> actionDataList = new List<CharactersActionData>();
	public Transform tf;

	void Awake()
	{
		tf = transform;
	}

	void Start()
	{
		AwakeManager.instance.addEventListener(this);
	}

	public void onAwake()
	{
		trigger(Config.ActionTriggerCondition.Awake);
	}

	public void trigger(Config.ActionTriggerCondition condition)
	{
		CharactersActionData action;

		for(int i = 0; i < actionDataList.Count; i++)
		{
			action = actionDataList[i];
			if((condition != Config.ActionTriggerCondition.TriggerByOthers && action.condition != condition) || action.isTriggered)
				continue;

			action.isTriggered = true;
			if(action.loop <= 0)
			{
				action.loop = int.MaxValue;
			}

			if(action.pauseTime2 == -1)
				action.pauseTime2 = action.pauseTime;

			Sequence s;
			Tweener tweener;
			switch(action.actionType)
			{
				case Config.ColliderAction.Alpha:
					CharacterGroup cg = tf.GetComponent<CharacterGroup>();

					if(cg == null)
					{
						SpriteRenderer spr;
						Collider2D[] sprCol;
						spr = tf.GetComponent<SpriteRenderer>();
						sprCol = tf.GetComponents<Collider2D>();
						ColorUtil.toAlpha(spr, action.startAlpha);
//					ColorUtil.doFade(spr, action.endAlpha, action.duration).SetLoops(action.loop,LoopType.Yoyo).OnStepComplete(
//						()=>{
//							if(spr.color.a < 0.5f) 
//								sprCol.enabled = false;
//							else
//								sprCol.enabled = true;
//						});
						DoAlphaToEachSpr(spr, sprCol, action.endAlpha, action.duration, out tweener);
						if(action.delay > 0)
							tweener.SetDelay(action.delay);
						if(action.loop > 1 && action.pauseTime > 0)
						{
							s = DOTween.Sequence();
							s.Append(tweener);
							s.AppendInterval(action.pauseTime);
							if(action.loopType == LoopType.Yoyo)
							{
								DoAlphaToEachSpr(spr, sprCol, action.startAlpha, action.duration, out tweener);
								s.Append(tweener);
								s.AppendInterval(action.pauseTime2);
							}
							s.SetLoops(action.loop);
						} else
						{
							tweener.SetLoops(action.loop, action.loopType);
						}

					} else
					{
						Transform c;
						for(int j = 0; j < cg._character.Count; j++)
						{
							c = cg._character[j].tf;
							SpriteRenderer cspr = c.GetComponent<SpriteRenderer>();
							Collider2D[] csprCol = c.GetComponents<Collider2D>();

							ColorUtil.toAlpha(cspr, action.startAlpha);
							DoAlphaToEachSpr(cspr, csprCol, action.endAlpha, action.duration, out tweener);
							if(action.delay > 0)
								tweener.SetDelay(action.delay);
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
								if(action.loopType == LoopType.Yoyo)
								{
									DoAlphaToEachSpr(cspr, csprCol, action.startAlpha, action.duration, out tweener);
									s.Append(tweener);
									s.AppendInterval(action.pauseTime2);
								}
								s.SetLoops(action.loop);
							} else
							{
								tweener.SetLoops(action.loop, action.loopType);
							}

						}
					}
					break;

				case Config.ColliderAction.Scale:
					tf.localScale = action.startVal;
					tweener = tf.DOScale(action.endVal, action.duration);
					if(action.delay > 0)
						tweener.SetDelay(action.delay);
					if(action.loop > 1 && action.pauseTime > 0)
					{
						s = DOTween.Sequence();
						s.Append(tweener);
						s.AppendInterval(action.pauseTime);
						if(action.loopType == LoopType.Yoyo)
						{
							s.Append(tf.DOScale(action.startVal, action.duration));
							s.AppendInterval(action.pauseTime2);
						}
						s.SetLoops(action.loop);
					} else
					{
						tweener.SetLoops(action.loop, action.loopType);
					}
					break;

				case Config.ColliderAction.Movement:
					tf.localPosition = action.startVal;
					tweener = tf.DOLocalMove(action.endVal, action.duration);
					if(action.delay > 0)
						tweener.SetDelay(action.delay);
					if(action.loop > 1 && action.pauseTime > 0)
					{
						s = DOTween.Sequence();
						s.Append(tweener);
						s.AppendInterval(action.pauseTime);
						if(action.loopType == LoopType.Yoyo)
						{
							s.Append(tf.DOLocalMove(action.startVal, action.duration));
							s.AppendInterval(action.pauseTime2);
						}
						s.SetLoops(action.loop);
					} else
					{
						tweener.SetLoops(action.loop, action.loopType);
					}
					break;

				case Config.ColliderAction.Rotation:
					tf.localRotation = Quaternion.Euler(action.startVal);
					tweener = tf.DOLocalRotate(action.endVal, action.duration, false);
					if(action.delay > 0)
						tweener.SetDelay(action.delay);
					if(action.loop > 1 && action.pauseTime > 0)
					{
						s = DOTween.Sequence();
						s.Append(tweener);
						s.AppendInterval(action.pauseTime);
						if(action.loopType == LoopType.Yoyo)
						{
							s.Append(tf.DOLocalRotate(action.startVal, action.duration));
							s.AppendInterval(action.pauseTime2);
						}
						s.SetLoops(action.loop);
					} else
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
			trigger(Config.ActionTriggerCondition.OnCollider);
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


	private void DoAlphaToEachSpr(SpriteRenderer spr, Collider2D[] sprCols, float endVal, float duration, out Tweener tweener)
	{
		tweener = ColorUtil.doFade(spr, endVal, duration);
		if(sprCols.Length > 0)
			tweener.OnComplete(() =>{
					if(spr.color.a < 0.5f)
					for(int i = 0; i < sprCols.Length; i++)
						sprCols[i].enabled = false;
					else
					for(int i = 0; i < sprCols.Length; i++)
						sprCols[i].enabled = true;
				});
	}
}

[System.Serializable]
public class CharactersActionData
{
	public Config.ColliderAction actionType;
	public int loop = -1;
	public bool isTriggered = false;
	public Config.ActionTriggerCondition condition;
	public LoopType loopType = LoopType.Yoyo;
	public float delay;
	public float pauseTime;
	public float pauseTime2 = -1;
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