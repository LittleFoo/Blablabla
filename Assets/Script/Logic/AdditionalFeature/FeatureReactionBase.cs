using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FeatureReactionBase : MonoBehaviour {
	public int hp;
	public BoxCollider2D col;
	public CharacterAnimation ani;
	[HideInInspector]
	public Transform tf;
	[HideInInspector]
	public int isBottom;
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
		isDead = false;

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
	}


}
