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

	public void Awake ()
	{
		tf = transform;

	}

	public void init()
	{
		CharacterHandler.instance.handlers.TryGetValue(fontData.id, out _onPlayerEnter);

		CharacterColorData d;
		if(CharacterColor.instance.colorDic.TryGetValue(fontData.id, out d))
		{
			GetComponent<SpriteRenderer>().color = d.color;
		}

		System.Action<CharacterCell> initHandler;
		if(CharacterHandler.instance.inits.TryGetValue(fontData.id, out initHandler))
		{
			initHandler(this);
		}


	}


	public void pushUp ()
	{
		if (_curTween != null && _curTween.IsPlaying ())
			_curTween.Complete ();
		_curTween = tf.DOLocalMoveY (tf.localPosition.y + 5, 0.1f).SetLoops (2, LoopType.Yoyo);
	}

	public void onPlayerLand(PhysicalPlayerController pp)
	{
//		CharacterHandler.instance.handlers.TryGetValue(fontData.id, out _onPlayerEnter);
		if(_onPlayerEnter != null)
			_onPlayerEnter(pp, this);
	}


}
