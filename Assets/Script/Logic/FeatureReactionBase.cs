using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FeatureReactionBase : MonoBehaviour {
	public int hp;
	public CharacterAnimation ani;
	[HideInInspector]
	public Transform tf;
	public Rigidbody2D rb;
	protected bool isDead = false;
	protected Config.ReactionType lastTriggerType = Config.ReactionType.Null;

	void Awake()
	{
		tf = transform;
		rb = GetComponent<Rigidbody2D>();

	}

	public virtual void init()
	{
		Setting s = GlobalController.instance.setting;
		rb.gravityScale = s.playerG/s.g;
		if(ani != null)
		{
			CharacterAnimation newAni = tf.gameObject.AddComponent<CharacterAnimation>();
			newAni.copy(ani);
			ani = newAni;
		}
		ani.play(Config.CharcterAction.Idle);
	}

	public virtual void onIce()
	{
		
	}

	public virtual void onScroll(Collision2D coll, FeatureScroll scroll)
	{
		
	}

	public virtual void leaveScroll(Collision2D coll, FeatureScroll scroll)
	{

	}

	public virtual void leaveIce()
	{
		
	}

	public virtual void hurt(int power)
	{
		if(isDead)
			return;
		
		hp -= power;
		if(hp <= 0)
			dead();
	}

	public virtual void dead()
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
	}
}
