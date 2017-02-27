using UnityEngine;
using System.Collections;
using DG.Tweening;

[System.Serializable]
public class CharacterCell : MonoBehaviour {
	public FontData fontData;
	public Transform tf;
	public bool enter;
	public Tweener _curTween;
	public BoxCollider2D[] colList;
	public void Awake()
	{
		tf = transform;
	}

	public void pushUp()
	{
		if(_curTween != null && _curTween.IsPlaying())
			_curTween.Complete();
		tf.DOLocalMoveY(tf.localPosition.y + 5, 0.1f).SetLoops(2, LoopType.Yoyo);
	}

}
