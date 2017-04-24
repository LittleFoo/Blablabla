using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraScroll : MonoBehaviour {

	public Transform tf;
	private Tweener _curTween;
	private bool isLock = false;
	void Awake()
	{
		tf = transform;
	}

	public void notice(Vector3 deltaPos, Transform player, float scalex)
	{
		if(isLock)
			return;
			if(_curTween != null)
			{
				_curTween.Kill();
				_curTween = null;
			}
		tf.position += new Vector3(deltaPos.x, deltaPos.y*0.5f ,0);
	}

	public void setPos(Transform player)
	{
		if(isLock)
			return;
		float centerX =  player.position.x + ((player.lossyScale.x < 0)?-5:5);
		float centerY =  player.position.y +17;
		tf.position = new Vector3(centerX, centerY);
	}

	public void correct(Transform player)
	{
		if(isLock)
			return;
		if(_curTween != null && _curTween.IsPlaying())
		{
			_curTween.Kill();
		}

		float centerX =  player.position.x + ((player.lossyScale.x < 0)?-5:5);
		float centerY =  player.position.y +17;
		float differ = tf.position.x - centerX;
		if(differ > -2 && differ < 2)
		{
			centerX = tf.position.x;
		}

		differ = tf.position.y - centerY;
		if(differ > -2 && differ < 2)
		{
			centerY = tf.position.y;
		}

		_curTween = tf.DOMove(new Vector3(centerX, centerY, 0), 1.0f).SetEase(Ease.OutQuad);
	}

	public void move(Transform player, Vector2 playerLocation, float duration)
	{
		if(_curTween != null)
			_curTween.Kill();
		Vector3 endPos = new Vector3((0.5f-playerLocation.x)*GlobalController.instance.setting.screenWidth,
			(0.5f-playerLocation.y)*GlobalController.instance.setting.screenHeight, 0);

		tf.DOMove(endPos + player.position, duration);
	}

	public void lockCamera()
	{
		isLock = true;
	}

	public void unlockCamera()
	{
		isLock = false;
	}
}
