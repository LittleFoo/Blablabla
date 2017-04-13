using UnityEngine;
using System.Collections;

public class MonsterBase : FeatureReactionBase, common.ITimerEvent
{
	public MonsterData mstData;
	private System.Action _movePattern;
	private float centerY;
	private float topY;
	private float lastSpeed;
	private float minX;
	private float maxX;
	private CharacterGroup root;
	private Collision2D lastCollistion;
	public void Start()
	{
		init();
	}

	public override void init()
	{
		isDead = false;

		ani.copy(mstData.ani);
		ani.play(Config.CharcterAction.Idle);

		col.size = mstData.col.size;
		col.offset = mstData.col.offset;

		Setting s = GlobalController.instance.setting;
		if(GlobalController.instance.curScene.underWater)
		{
			rb.gravityScale = 0;
		} else
		{
			rb.gravityScale = s.playerG/s.g;
		}

		centerY = col.size.y*0.5f;
		topY = col.size.y;
		if(tf.lossyScale.x > 0)
			lastSpeed = mstData.speedx;
		else
			lastSpeed = -mstData.speedx;
		_movePattern = recycleMove;

		trigger();
	}

	public void onUpdate()
	{
		_movePattern();
	}

	public void trigger()
	{
		common.TimerManager.instance.addEventListeners(this);
		rb.velocity = new Vector2(lastSpeed, 0);
	}

	public void OnCollisionEnter2D(Collision2D coll)
	{
		lastCollistion = coll;
		switch(coll.gameObject.tag)
		{
			case Config.TAG_CHAR:
				
				if(isBottom > 0)
					return;
				float delta = coll.contacts[0].point.y-(tf.position.y+centerY);
				if(delta < 0)
				{
					root = coll.transform.parent.GetComponent<CharacterGroup>();
					minX = root.transform.position.x-root.pivot.x*root.textWidth;
					maxX = minX+root.textWidth;
					onBottom();
				} 
				break;
		}
	}

	public void onBottom()
	{
		isBottom = 1;
		rb.velocity = new Vector2(lastSpeed, 0);
		ani.play(Config.CharcterAction.Walk);
		setScale();
	}

	public void OnDestroy()
	{
		common.TimerManager.instance.removeEventListeners(this);
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
			} else if(tf.position.x > maxX || tf.position.x < minX)
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
		}
		else
		{
			correctTime = 0;
		}
	}
		
	private void correct()
	{
		float cellbottomY;
		float celltopY ;
		if(correctTime > 1)
		{
			lastSpeed = -lastSpeed;
			rb.velocity = new Vector2(lastSpeed, rb.velocity.y);
			setScale();
		}
		else
		{
			if(lastCollistion != null)
			{
				GameObject obj = lastCollistion.gameObject;
				BoxCollider2D objCol = obj.GetComponent<BoxCollider2D>();

				cellbottomY = obj.transform.position.y-objCol.size.y*0.5f + objCol.offset.y;
				celltopY = obj.transform.position.y+objCol.size.y*0.5f+ objCol.offset.y;

				if(cellbottomY > tf.position.y + topY || celltopY < tf.position.y)
				{
					rb.velocity = new Vector2(lastSpeed, rb.velocity.y);
				} else
				{
					lastSpeed = -lastSpeed;
					rb.velocity = new Vector2(lastSpeed, rb.velocity.y);
					setScale();
				}

			}
			else
			{
				rb.velocity = new Vector2(lastSpeed, rb.velocity.y);
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
}


