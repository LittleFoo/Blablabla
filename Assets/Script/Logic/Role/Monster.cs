using UnityEngine;
using System.Collections;
using DG.Tweening;
public class Monster : FeatureReactionBase {
	public float gap;
	private Coroutine _shootRoutine;
	void Start()
	{
		init();
		_shootRoutine = StartCoroutine(action());
	}

	public override void init()
	{
		base.init();
		Setting s = GlobalController.instance.setting;
		rb.gravityScale = s.playerG/s.g;
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
		Config.Direction dir = (tf.lossyScale.x > 0)?Config.Direction.Right:Config.Direction.Left;
		Vector3 pos;
		if(dir == Config.Direction.Left)
		{
			pos = new Vector3(tf.position.x - GlobalController.instance.setting.gridSize, tf.position.y + col.offset.y, 0);
		}
		else
			pos = new Vector3(tf.position.x + GlobalController.instance.setting.gridSize, tf.position.y + col.offset.y, 0);
		bullet.init(pos, dir,d);
	}

	public override void dead()
	{
			isDead = true;
			tf.GetComponent<Collider2D>().enabled = false;
			Rigidbody2D rb = GetComponent<Rigidbody2D>();
			rb.gravityScale = 0;
			rb.velocity = Vector2.zero;
			tf.DOMove(new Vector3(tf.position.x, tf.position.y+20), 0.3f).OnComplete(() =>
			{
				tf.DOMove(new Vector3(tf.position.x, -450), 1).SetDelay(1.0f);
			});
		StopCoroutine(_shootRoutine);
	}
}
