using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
public class CharacterHandler : MonoBehaviour {

	public  Dictionary< int, System.Action<PhysicalPlayerController, CharacterCell> > handlers;
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
	}

	public static void angleBracketLeftTrigger (PhysicalPlayerController pp, CharacterCell charTf)
	{
		charTf.tf.DOLocalMoveY (charTf.tf.localPosition.y - 5, 0.1f).SetLoops (2, LoopType.Yoyo);
		pp.Rebound (Config.Direction.Left);
	}

	public static void angleBracketRightTrigger (PhysicalPlayerController pp, CharacterCell charTf)
	{
		charTf.tf.DOLocalMoveY (charTf.tf.localPosition.y - 5, 0.1f).SetLoops (2, LoopType.Yoyo);
		pp.Rebound (Config.Direction.Right);
	}
}
