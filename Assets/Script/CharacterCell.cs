using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[System.Serializable]
public class CharacterCell : MonoBehaviour
{
	public FontData fontData;
	public Transform tf;
	public bool enter;
	public Tweener _curTween;
	public BoxCollider2D[] colList;
	private System.Action<PhysicalPlayerController, CharacterCell> _onPlayerEnter;

	private static Dictionary< int, System.Action<PhysicalPlayerController, CharacterCell> > handlers;

	public void Awake ()
	{
		tf = transform;

	}

	public void init()
	{
		handlers.TryGetValue(fontData.id, out _onPlayerEnter);
	}


	public void pushUp ()
	{
		if (_curTween != null && _curTween.IsPlaying ())
			_curTween.Complete ();
		tf.DOLocalMoveY (tf.localPosition.y + 5, 0.1f).SetLoops (2, LoopType.Yoyo);
	}

	public static void initHandler()
	{
		if(handlers != null)
			return;

		handlers = new Dictionary<int, System.Action<PhysicalPlayerController, CharacterCell>>();
		handlers.Add(60, angleBracketLeftTrigger);
		handlers.Add(62, angleBracketRightTrigger);
	}

	public void onPlayerLand(PhysicalPlayerController pp)
	{
		if(_onPlayerEnter != null)
			_onPlayerEnter(pp, this);
	}

	public static void angleBracketLeftTrigger (PhysicalPlayerController pp, CharacterCell charTf)
	{
		charTf.tf.DOLocalMoveY (charTf.tf.localPosition.y - 5, 0.1f).SetLoops (2, LoopType.Yoyo);
		Vector2 vec = GlobalController.instance.setting.angleBlanketReboundParam;
		pp.Rebound (new Vector2 (-vec.x, vec.y));
	}

	public static void angleBracketRightTrigger (PhysicalPlayerController pp, CharacterCell charTf)
	{
		charTf.tf.DOLocalMoveY (charTf.tf.localPosition.y - 5, 0.1f).SetLoops (2, LoopType.Yoyo);
		Vector2 vec = GlobalController.instance.setting.angleBlanketReboundParam;
		pp.Rebound (new Vector2 (vec.x, vec.y));
	}
}
