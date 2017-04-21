using UnityEngine;
using System.Collections;

public class MonsterBase : FeatureReactionBase, common.ITimerEvent, IDistanceTrigger
{
	public MonsterData mstData;
	private System.Action _movePattern;
	public float centerY;
	private float topY;
	private float lastSpeed;
	private float minX;
	private float maxX;
	private CharacterGroup root;
	private Collider2D lastCollistion;
	void Awake()
	{
		tf = transform;
		DistanceTriggerManager.instance.addEventListener(this);
	}

	public void Start()
	{
		init();
	}

	public override void init()
	{
	
		isDead = false;

		ani.copy(mstData.ani);
		ani.addSprToDic();
		ani.play(Config.CharcterAction.Idle);

		col.size = mstData.col.size;
		col.offset = mstData.col.offset;

		hp = mstData.hp;

		rb.gravityScale = 0;

		Setting s = GlobalController.instance.setting;
		if(GlobalController.instance.curScene.underWater)
		{
			_gravityScale = 0;
		} else
		{
			_gravityScale = s.playerG / s.g;
		}

		centerY = col.size.y * 0.5f;
		topY = col.size.y;
		lastSpeed = 0;
		if(mstData.isAvoidGap)
			_movePattern = recycleMove;
		else
			_movePattern = sillyMove;

	}

	public void OnDisable()
	{
#if UNITY_EDITOR
#else
		DistanceTriggerManager.instance.removeEventListener(this);
		common.TimerManager.instance.removeEventListeners(this);
#endif
	}

	public void onUpdate()
	{
		_movePattern();

		if(rb.velocity.y < 0 && tf.position.y < GlobalController.instance.curScene.boundsY.x)
		{
			dead();
		}
	}

	public void trigger()
	{
		if(tf.lossyScale.x > 0)
			lastSpeed = mstData.speedx;
		else
			lastSpeed = -mstData.speedx;
		rb.gravityScale = _gravityScale;
		common.TimerManager.instance.addEventListeners(this);
		rb.velocity = new Vector2(lastSpeed, 0);
		ani.play(Config.CharcterAction.Walk);
	}

	public Vector3 getPosition()
	{
		return tf.position;
	}

	public void OnCollisionEnter2D(Collision2D coll)
	{
		lastCollistion = coll.collider;

		GameObject obj = coll.gameObject;
		switch(obj.tag)
		{
		case Config.TAG_CHAR:
				
			if(isBottom > 0)
				return;
			float delta = coll.contacts[0].point.y - (tf.position.y + centerY);
			if(delta < 0)
			{

				CharacterCell cell = obj.GetComponent<CharacterCell>();
				if(cell.fontData.id == 95)
				{
					minX = 0;
					maxX = cell.fontData._actualWidth;
				} else
				{
					root = coll.transform.parent.GetComponent<CharacterGroup>();
					minX = -root.pivot.x * root.textWidth;
					maxX = minX + root.textWidth;

					int curCharIdx = root._character.IndexOf(cell);
					for(int i = curCharIdx-1; i > -1; i--)
					{
						if(root._character[i].fontData.id == 32 || root._character[i].fontData.id == 95)
						{
							minX = root._character[i].tf.localPosition.x + root._character[i].fontData._actualWidth;
							break;
						}
					}

					for(int i = curCharIdx+1; i < root._character.Count; i++)
					{
						if(root._character[i].fontData.id == 32 || root._character[i].fontData.id == 95)
						{
							maxX = root._character[i].tf.localPosition.x;
							break;
						}
					}
				}	
				onBottom();
				tf.SetParent(obj.transform.parent, true);
			} 
			break;
		}
	}

	public void onBottom()
	{
		isBottom = 1;
		rb.velocity = new Vector2(lastSpeed, 0);
//		ani.play(Config.CharcterAction.Walk);
		setScale();
	}

	private bool _justChange = false;

	private void recycleMove()
	{
		sillyMove();

		if(root != null)
		{
			if(_justChange)
			{
				_justChange = false;
			} else if(tf.localPosition.x > maxX || tf.localPosition.x < minX)
			{
				_justChange = true;
					lastSpeed = -lastSpeed;
				rb.velocity = new Vector2(lastSpeed, rb.velocity.y);
				setScale();
			}	
		}
	}

	private int correctTime = 0;

	private void sillyMove()
	{
		if(rb.velocity.y < -0.1f)
		{
			if(isBottom != 0)
			{
				rb.velocity = new Vector2(0, rb.velocity.y);
				isBottom = 0;
				root = null;
			}
		} else if(rb.velocity.x != lastSpeed)
		{
			correctTime++;
			correct();
		} else
		{
			correctTime = 0;
		}
	}

	private void RebounceMove()
	{

	}
		
	private void correct()
	{
		float cellbottomY;
		float celltopY;
		if(correctTime > 1)
		{
			lastSpeed = -lastSpeed;
			rb.velocity = new Vector2(lastSpeed, rb.velocity.y );

			setScale();
		} else
		{
			if(lastCollistion != null)
			{
				GameObject obj = lastCollistion.gameObject;

				cellbottomY = lastCollistion.bounds.center.y - lastCollistion.bounds.extents.y;
				celltopY = lastCollistion.bounds.center.y + lastCollistion.bounds.extents.y;

				if(cellbottomY > tf.position.y + topY || celltopY < tf.position.y)
				{
					rb.velocity = new Vector2(lastSpeed, rb.velocity.y);
				} else
				{
					lastSpeed = -lastSpeed;
					rb.velocity = new Vector2(lastSpeed, rb.velocity.y);
					setScale();
				}

			} else
			{
				rb.velocity = new Vector2(lastSpeed, rb.velocity.y);
				tf.localPosition += new Vector3(lastSpeed/Mathf.Abs(lastSpeed)*0.1f, 0, 0);
			}
		}
	}

	private void setScale()
	{
		if(lastSpeed > 0)
			tf.localScale = new Vector3(Mathf.Abs(tf.localScale.x), tf.localScale.y, 1);
		else
			tf.localScale = new Vector3(-Mathf.Abs(tf.localScale.x), tf.localScale.y, 1);
	}

	public override void dead()
	{
		base.dead();
		ColorUtil.doFade(tf.GetComponent<SpriteRenderer>(), 0, 0.5f);
		col.enabled = false;
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
		OnDisable();
	}
}


