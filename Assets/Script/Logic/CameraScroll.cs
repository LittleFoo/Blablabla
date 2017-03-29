using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraScroll : MonoBehaviour {

	public Transform tf;
	private Tweener _curTween;
	void Start()
	{
		tf = transform;
	}

	public void notice(Vector3 deltaPos, Transform player, float scalex)
	{

			if(_curTween != null)
			{
				_curTween.Kill();
				_curTween = null;
			}
		tf.position += deltaPos;
	}

	public void setPos(Transform player)
	{
		float centerX =  player.position.x + ((player.lossyScale.x < 0)?-5:5);
		float centerY =  player.position.y +17;
		tf.position = new Vector3(centerX, centerY);
	}

	public void correct(Transform player)
	{
		if(_curTween != null)
		{
			return;
		}
		float centerX =  player.position.x + ((player.lossyScale.x < 0)?-5:5);
		float centerY =  player.position.y +17;
		_curTween = tf.DOMove(new Vector3(centerX, centerY, 0), 1.0f);
	}
}
