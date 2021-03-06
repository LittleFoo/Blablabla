﻿using UnityEngine;
using System.Collections;
using DG.Tweening;
using PathologicalGames;

public class PhysicalPlayerController : FeatureReactionBase, common.ITimerEvent, IAwake
{
	
//	public CharacterAnimation ani;

	public CameraScroll cam;
	public bool _autoJump;
	public bool autoJump
	{
		get{ return _autoJump; }
		set
		{
//			_autoJump = value;
//			if(_autoJump)
//				jumpHandler = autoJumpHandler;
//			else
//				jumpHandler = spaceJumpHandler;
		}
	}

	private Config.Direction _playerDirection;
	private Config.Direction _arrowStatus = Config.Direction.None;
	private float initScaleX;
	private float height;

	private float _upSpeed;
	private float _moveSpeed;
	private int _jumpCount = 0;
	private int _restrictDirection;
	private bool _lockPosture;
	private float _idleTime;
	private bool _isSpaceDown = false;
	private bool _setSpeedBeforeBottom = false;
	public bool setSpeedBeforeBottom
	{
		set{
			_setSpeedBeforeBottom = value;
		}
	}
	private bool _isShooting = false;
	private float _bulletCD = 0;
	private System.Action jumpHandler;
	private BulletData bulletData;
	private float minX, minY, maxX, maxY;

	void Start()
	{
		cam = Camera.main.GetComponent<CameraScroll>();
		if(ani != null)
		{
			CharacterAnimation newAni = tf.gameObject.AddComponent<CharacterAnimation>();
			newAni.copy(ani);
			ani = newAni;
			ani.addSprToDic();
		}

		rb.gravityScale = 0;
		Setting s = GlobalController.instance.setting;
		if(GlobalController.instance.curScene.underWater)
		{
			_gravityScale = 0;
			_upSpeed = s.waterUpSpeed;
			_moveSpeed = s.waterMoveSpeed;
			jumpHandler = waterUpHandler;
		}
		else
		{
			_gravityScale = s.playerG/s.g;
			_upSpeed = s.smallUpSpeed;
			_moveSpeed = s.moveSpeed;
			jumpHandler = spaceJumpHandler;
		}

		col = tf.GetComponent<BoxCollider2D>();
		col.size = s.roleColliderSize;
		col.offset = new Vector2(0, col.size.y*0.5f);
		height = col.size.y*0.9f;


		reboundReleaseDelay = new WaitForSeconds(s.reboundProtectTime);

		ani.play(Config.CharcterAction.Idle);
		_lockPosture = true;
		initScaleX = Mathf.Abs(transform.lossyScale.x);

		_lastPostion = tf.position;


		bulletData = new BulletData();
		bulletData.speed = GlobalController.instance.setting.bigUpSpeed;

		AwakeManager.instance.addEventListener(this);
	}

	public void onAwake()
	{
		rb.gravityScale = _gravityScale;
		common.TimerManager.instance.addEventListeners(this);
		cam.setPos(tf);
		DistanceTriggerManager.instance.notice(tf.position);
	}

	private Vector3 _lastPostion;
	public void onUpdate()
	{
		if(isDead)
			return;

//		if(rb.velocity.y > 0.1f || rb.velocity.y < -0.1f)
//			isBottom = 0;

		if(tf.position.x - maxX > 10 || tf.position.y -maxY > 10 || tf.position.y - minY < -10)
			DistanceTriggerManager.instance.notice(tf.position);

		bool arrowChange = false;
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			_arrowStatus = Config.Direction.Right;
			arrowChange = true;
		} else if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			_arrowStatus = Config.Direction.Left;
			arrowChange = true;
		}

		if(Input.GetKeyUp(KeyCode.RightArrow) && _arrowStatus == Config.Direction.Right)
		{
			_arrowStatus = Config.Direction.None;
			arrowChange = true;
		}

		if(Input.GetKeyUp(KeyCode.LeftArrow) && _arrowStatus == Config.Direction.Left)
		{
			_arrowStatus = Config.Direction.None;
			arrowChange = true;
		}

		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			_isShooting = true;
		}

		if(Input.GetKeyUp(KeyCode.DownArrow))
		{
			_isShooting = false;
		}

		if(_isShooting)
		{
			if(_bulletCD <= 0)
				shooting();
		}

		_bulletCD -= Time.unscaledDeltaTime;
		jumpHandler();

	
		Config.Direction lastPlayerDirection = _playerDirection;
		if(_restrictDirection == 0 )
			_playerDirection = _arrowStatus;
			
		switch(_playerDirection)
		{
			case Config.Direction.None:
				rb.velocity = new Vector2(additionSpeed, rb.velocity.y);
				if(isBottom > 0)
				{
					if(lastPlayerDirection != _playerDirection && !_lockPosture)
						ani.play(Config.CharcterAction.Idle);
					_idleTime += Time.deltaTime;
					if(lastPlayerDirection !=  _playerDirection)
						cam.correct(tf);
				} else
				{
					_idleTime = 0;
					cam.notice(tf.position-_lastPostion, tf, tf.localScale.x);
				}
				break;

			case Config.Direction.Left:
				rb.velocity = new Vector2(-_moveSpeed+additionSpeed, rb.velocity.y);
				if(!_lockPosture && lastPlayerDirection != _playerDirection)
					ani.play(Config.CharcterAction.Walk);
				if(tf.lossyScale.x > 0)
					tf.localScale = new Vector3(-tf.localScale.x, tf.localScale.y, tf.localScale.z);
				_idleTime = 0;
				cam.notice(tf.position-_lastPostion, tf, tf.localScale.x);
				break;

			case Config.Direction.Right:
				rb.velocity = new Vector2(_moveSpeed+additionSpeed, rb.velocity.y);
				if(!_lockPosture && lastPlayerDirection != _playerDirection)
					ani.play(Config.CharcterAction.Walk);
				if(tf.lossyScale.x < 0)
					tf.localScale = new Vector3(-tf.localScale.x, tf.localScale.y, tf.localScale.z);
				_idleTime = 0;
				cam.notice(tf.position-_lastPostion, tf, tf.localScale.x);
				break;
		}

		if(_idleTime > 3)
		{
			ani.doRandomIdle();
			_idleTime = -2;
		}

		_lastPostion = tf.position;


	}

	public void OnCollisionEnter2D(Collision2D coll)
	{
		GameObject obj = coll.gameObject;
		switch(coll.gameObject.tag)
		{
			case Config.TAG_CHAR:
				float delta = coll.contacts[0].point.y-tf.position.y;
				CharacterCell cell = obj.GetComponent<CharacterCell>();
				if(delta < 2)
				{
					if(isBottom <= 0)
					{
						onBottom();
						tf.SetParent(obj.transform.parent, true);
					}
						cell.onPlayerLand(this);
				} else if(delta > height)
				{
					rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
					coll.transform.GetComponent<CharacterCell>().pushUp();
				}
				break;

			case Config.TAG_MST:
				MonsterBase mst = obj.GetComponent<MonsterBase>();

				if(tf.position.y > mst.tf.position.y + mst.centerY)
				{
					mst.hurt(999);
					rb.velocity = new Vector2(rb.velocity.x, - rb.velocity.y);
				}
				else
					hurt(1);
				break;
		}
	}

	public void OnCollisionExit2D(Collision2D coll)
	{
		if(coll.transform == tf.parent)
			isBottom = 0;
	}

	public override void hurt(int power)
	{
		base.hurt(power);
			col.isTrigger = true;
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
		if(isDead)
		{
			ColorUtil.doFade(tf.GetComponent<SpriteRenderer>(), 0, 0.1f).SetLoops(6, LoopType.Yoyo);
		}else
		{
			ColorUtil.doFade(tf.GetComponent<SpriteRenderer>(), 0, 0.1f).SetLoops(16, LoopType.Yoyo).OnComplete(()=>{
				col.isTrigger = false;
				rb.constraints = RigidbodyConstraints2D.FreezeRotation;
			});
		}
	}

	//obj that won't effect players action
	//like charactergroup( a blur determin)
	public void OnTriggerEnter2D(Collider2D coll)
	{
		switch(coll.gameObject.tag)
		{
//			case Config.TAG_GROUP:
//				tf.SetParent(coll.transform, true);
//				break;

			case Config.TAG_DANGER:
				dead();
				break;

			case Config.TAG_COIN:
				coll.GetComponent<Coin>().Eat();
				break;
		}
	}


//	public void OnTriggerExit2D(Collider2D coll)
//	{
//		if(coll.transform == tf.parent)
//			tf.SetParent(null, true);
//	}

	public override void dead()
	{
		tf.parent = null;
		isDead = true;
		tf.GetComponent<Collider2D>().enabled = false;
		rb.gravityScale = 0;
		rb.velocity = Vector2.zero;
		tf.DOMove(new Vector3(tf.position.x, tf.position.y+20), 0.3f).OnComplete(() =>
		{
			tf.DOMove(new Vector3(tf.position.x, -450), 1).SetDelay(1.0f);
		});
	}

	#region public call

	//character like > < will rebound player;
	public void Rebound(Config.Direction dir)
	{
		isBottom = 0;
		_playerDirection = dir;

		rb.velocity = GlobalController.instance.setting.angleBlanketReboundParam;
		_moveSpeed = GlobalController.instance.setting.angleBlanketReboundParam.x;
		ani.play(Config.CharcterAction.Jump);

		_restrictDirection++;
		releaseAfterReboundRoutine = StartCoroutine(releaseAfterRebound());
	}

	WaitForSeconds reboundReleaseDelay;
	Coroutine releaseAfterReboundRoutine;

	IEnumerator releaseAfterRebound()
	{
		yield return reboundReleaseDelay;
		_restrictDirection--;
	}

	//lock players move direction and let them move faster
	public override void onIce()
	{
		_restrictDirection++;
		if(_playerDirection == Config.Direction.None)
			_playerDirection = Config.Direction.Right;
		_moveSpeed = GlobalController.instance.setting.moveSpeed*2;
		ani.play(Config.CharcterAction.Crash);
		_lockPosture = true;
		if(lastTriggerType == Config.ReactionType.Null)
			lastTriggerType = Config.ReactionType.Ice;
		else 
			lastTriggerType = Config.ReactionType.Null;
	}

	private float additionSpeed = 0;
	public override void onScroll(Collision2D coll, FeatureScroll scroll)
	{
		float delta = coll.contacts[0].point.y-tf.position.y;
		if(delta < 2)
		{
			if(isBottom <= 0)
				onBottom();
			scroll.onPlayerLand(this);
			additionSpeed = scroll.speed;
		} else if(delta > height)
		{
			rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
		}
			
	}

	public override void leaveScroll(Collision2D coll, FeatureScroll scroll)
	{
		isBottom = 0;
	}

	//unlock players move direction
	public override void leaveIce()
	{
		_restrictDirection--;
		_lockPosture = false;
		isBottom = 0;
	}

	#endregion

	//unlock players move direction


	private void autoJumpHandler()
	{
		if(isBottom > 0)
		{
			isBottom = 0;
			rb.velocity = new Vector2(rb.velocity.x, _upSpeed);
			ani.play(Config.CharcterAction.Jump);
		}

		if(Input.GetKeyUp(KeyCode.Space))
		{
		}
	}

	private void spaceJumpHandler()
	{
		if(Input.GetKeyDown(KeyCode.Space) && _jumpCount < 2)
		{
			_jumpCount++;
			isBottom = 0;
			rb.velocity = new Vector2(rb.velocity.x, _upSpeed);
			ani.doJump();
		}
	}

	private void waterUpHandler()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			_isSpaceDown = true;
		}

		if(Input.GetKeyUp(KeyCode.Space))
		{
			_isSpaceDown = false;
			rb.velocity = new Vector2(rb.velocity.x, 0);
		}

		if(_isSpaceDown)
		{
			rb.velocity = new Vector2(rb.velocity.x, _upSpeed);
		}
		else
			rb.velocity = new Vector2(rb.velocity.x, -_upSpeed);
	}

	void onBottom()
	{
		isBottom = 1;
		_jumpCount = 0;

		//correct some attribute if no one else set it .
		if(lastTriggerType == Config.ReactionType.Null || lastTriggerType == Config.ReactionType.Normal)
		{
			lastTriggerType = Config.ReactionType.Normal;
			_moveSpeed = GlobalController.instance.setting.moveSpeed;
			additionSpeed = 0;
			_playerDirection = _arrowStatus;
			_lockPosture = false;
		}
		else
		{
			lastTriggerType = Config.ReactionType.Null;
		}

		if(!_lockPosture)
		{
			if(_arrowStatus == Config.Direction.None )
			{
				ani.play(Config.CharcterAction.Idle);

			} else
			{
				ani.play(Config.CharcterAction.Walk);
			}
		}
		cam.correct(tf);
		rb.velocity = new Vector2(rb.velocity.x, 0);
	}


	void shooting()
	{
		_bulletCD = GlobalController.instance.setting.bulletCD;

		Bullet bullet = GlobalController.instance.getCurPool().Spawn(GlobalController.instance.prefabSetting.bullet).GetComponent<Bullet>();
	
		Config.Direction dir = (initScaleX > 0)?Config.Direction.Right:Config.Direction.Left;
		Vector3 pos;
		if(dir == Config.Direction.Left)
		{
			pos = new Vector3(tf.position.x - GlobalController.instance.setting.gridSize, tf.position.y + col.offset.y, 0);
		}
		else
			pos = new Vector3(tf.position.x + GlobalController.instance.setting.gridSize, tf.position.y + col.offset.y, 0);
		bullet.init(pos, dir,bulletData);
	}
}
