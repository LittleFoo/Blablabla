﻿using UnityEngine;
using System.Collections;
using DG.Tweening;
using PathologicalGames;

public class Bullet : MonoBehaviour {

	Transform tf;
	SpriteRenderer spr;
	BulletData _data;
	private Tweener _curTween;
	public void init(Vector3 position, Config.Direction dir, BulletData d)
	{
		tf = transform;
		_data = d;
		spr = tf.GetComponent<SpriteRenderer>();
		ColorUtil.toAlpha(spr, 1);
	
		switch(dir)
		{
			case Config.Direction.Left:
				tf.position = new Vector2(position.x - GlobalController.instance.setting.gridSize, position.y + GlobalController.instance.setting.gridSize*0.5f);
				_curTween = tf.DOMoveX(position.x - GlobalController.instance.setting.screenWidth, GlobalController.instance.setting.screenWidth/d.speed);
				break;

			case Config.Direction.Right:
				tf.position = new Vector2(position.x + GlobalController.instance.setting.gridSize, position.y + GlobalController.instance.setting.gridSize*0.5f);
				_curTween = tf.DOMoveX(position.x + GlobalController.instance.setting.screenWidth, GlobalController.instance.setting.screenWidth/d.speed);
				break;
		}

		_curTween.OnComplete(()=>{
			GlobalController.instance.getCurPool().Despawn(tf);
		});
	}

	public void OnTriggerEnter2D(Collider2D coll)
	{
		explode();
		FeatureReactionBase fb = coll.GetComponent<FeatureReactionBase>();
		if(fb != null)
			fb.hurt(_data.power);
	}

	public void explode()
	{
		_curTween.Kill();
		_curTween = ColorUtil.doFade(spr, 0, 0.2f).OnComplete(()=>{
			GlobalController.instance.getCurPool().Despawn(tf);

		});
	}
}


public class BulletData
{
	public float speed;
	public int power = 1;
}