using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BounceBullet : MonoBehaviour, common.ITimerEvent {

	private Transform tf;
	private SpriteRenderer spr;
	private BulletData _data;
	private Tweener _curTween;
	private bool isExploded = false;
	private float _minY;
	public void init(Vector3 position, float gScale, Vector2 velocity, BulletData d, float minY)
	{
		tf = transform;
		_data = d;
		_minY = minY;
		spr = tf.GetComponent<SpriteRenderer>();
		ColorUtil.toAlpha(spr, 1);
		tf.position = position;

		Rigidbody2D rb = tf.GetComponent<Rigidbody2D>();
		rb.gravityScale = gScale;
		rb.velocity = velocity;

		isExploded = false;

		common.TimerManager.instance.addEventListeners(this);

	}

	public void explode()
	{
		if(isExploded)
			return;
		
		_curTween = ColorUtil.doFade(tf.GetComponent<SpriteRenderer>(), 0, 0.2f).OnComplete(()=>{
			GlobalController.instance.getCurPool().Despawn(tf);
		});

		isExploded = true;

		common.TimerManager.instance.removeEventListeners(this);
	}

	public void OnCollisionEnter2D(Collision2D coll)
	{
		GameObject obj = coll.gameObject;
	
		FeatureReactionBase fb = obj.GetComponent<FeatureReactionBase>();
		if(fb != null)
		{
			fb.hurt(_data.power);
			explode();
		}
	}

	public void onUpdate()
	{
		if(tf.position.y < _minY)
			explode();
	}
}
