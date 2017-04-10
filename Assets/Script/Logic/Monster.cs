using UnityEngine;
using System.Collections;

public class Monster : FeatureReactionBase {
	public float gap;
	private Coroutine _shootRoutine;
	void Start()
	{
		init();
		_shootRoutine = StartCoroutine(action());
	}

	IEnumerator action()
	{
		while(true)
		{
			yield return new WaitForSeconds(gap);
			shoot();
		}
	}

	public void shoot()
	{
		BulletData d = new BulletData();
		d.speed = GlobalController.instance.setting.bulletSpeed;

		Bullet bullet = GlobalController.instance.getCurPool().Spawn(GlobalController.instance.prefabSetting.bullet).GetComponent<Bullet>();
		bullet.init(tf.position, 
			(tf.lossyScale.x > 0)?Config.Direction.Right:Config.Direction.Left,
			d
		);
	}

	public override void dead()
	{
		base.dead();
		StopCoroutine(_shootRoutine);
	}
}
