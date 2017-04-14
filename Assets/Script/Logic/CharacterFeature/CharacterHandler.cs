using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
public class CharacterHandler : MonoBehaviour {

	public  Dictionary< int, System.Action<PhysicalPlayerController, CharacterCell> > handlers;
	public  Dictionary< int, System.Action<CharacterCell> > inits;

	private static CharacterHandler _instance;
	public static CharacterHandler instance
	{
		get{
			return _instance;
		}
	}

	void Awake () {
		if(_instance != null) throw new System.Exception("GlobalController is a singleton");
		_instance = this;
		init();
	}

	private void init()
	{
		if(handlers != null)
			return;

		handlers = new Dictionary<int, System.Action<PhysicalPlayerController, CharacterCell>>();
		handlers.Add(60, angleBracketLeftTrigger);
		handlers.Add(62, angleBracketRightTrigger);
		handlers.Add(94, angleBracketUpTrigger);
		handlers.Add(95, underLineTriggerHandler);
		inits = new Dictionary<int, System.Action<CharacterCell>>();
		for(int i = 0; i < 9; i++)
		{
			inits.Add(49 +i, numberInit);
		}
		inits.Add(95, underLineInit);
		inits.Add(123, leftBraceInit);
		inits.Add(125, rightBraceInit);
		inits.Add(38, atInit);
	}

	public static void angleBracketLeftTrigger (PhysicalPlayerController pp, CharacterCell cell)
	{
		cell.tf.DOLocalMoveY (cell.tf.localPosition.y - 5, 0.1f).SetLoops (2, LoopType.Yoyo);
		pp.Rebound (Config.Direction.Left);
	}

	public static void angleBracketRightTrigger (PhysicalPlayerController pp, CharacterCell cell)
	{
		cell.tf.DOLocalMoveY (cell.tf.localPosition.y - 5, 0.1f).SetLoops (2, LoopType.Yoyo);
		pp.Rebound (Config.Direction.Right);
	}

	public static void angleBracketUpTrigger (PhysicalPlayerController pp, CharacterCell cell)
	{
		cell.tf.DOLocalMoveY (cell.tf.localPosition.y - 5, 0.1f).SetLoops (2, LoopType.Yoyo);
		pp.Rebound (Config.Direction.None);
	}

	public static void numberInit(CharacterCell cell)
	{
		CharactersAction action = cell.tf.gameObject.AddComponent<CharactersAction>();
		CharactersActionData d = new CharactersActionData();
		d.actionType = Config.ColliderAction.Alpha;
		d.condition = Config.ActionTriggerCondition.Awake;
		d.duration = 1.0f;
		d.startAlpha = 1;
		d.endAlpha = 0;
		d.pauseTime = GlobalController.instance.setting.numberDisappearTime;
		d.pauseTime2 = int.Parse( cell.fontData.Name);
		d.loopType = LoopType.Yoyo;
		d.delay = d.pauseTime2;
		d.loop = -1;
		action.actionDataList.Add(d);

		action.trigger(Config.ActionTriggerCondition.Awake);
	}

	public static void leftBraceInit(CharacterCell cell)
	{
		BraceFeature b = cell.tf.gameObject.AddComponent<BraceFeature>();
		b.direction = Config.Direction.Left;
	}

	public static void rightBraceInit(CharacterCell cell)
	{
		BraceFeature b = cell.tf.gameObject.AddComponent<BraceFeature>();
		b.direction = Config.Direction.Right;
	}

	public static void underLineInit(CharacterCell cell)
	{
		cell.gameObject.AddComponent<UnderLineMove>();
	}

	public static void atInit(CharacterCell cell)
	{
		cell.gameObject.AddComponent<AtShootFeature>();
	}

	public static void underLineTriggerHandler(PhysicalPlayerController pp, CharacterCell cell)
	{
		pp.tf.SetParent(cell.tf);
	}
}
