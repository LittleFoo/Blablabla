using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PhysicalPlayerController : MonoBehaviour {
	public Rigidbody2D rb;
	public CharacterAnimation ani;
	public Transform tf;
	public int isBottom;


	public bool _autoJump;
	public bool autoJump
	{
		get{return _autoJump;}
		set{
			_autoJump = value;
			if(_autoJump)
				jumpHandler = autoJumpHandler;
			else
				jumpHandler = spaceJumpHandler;
		}
	}
	private Config.Direction _playerDirection;
	private Config.Direction _arrowStatus = Config.Direction.None;
	private float initScaleX;
	private float height;
	private bool isDead = false;
	private float _upSpeed;
	private float _moveSpeed;
	private int _jumpCount = 0;
	private bool _restrictDirection;

	private System.Action jumpHandler;

	void Start()
	{
		tf = transform;

		Setting s = GlobalController.instance.setting;
		rb.gravityScale = s.playerG/s.g;
		_upSpeed = s.smallUpSpeed;
		_moveSpeed = s.moveSpeed;

		BoxCollider2D col = tf.GetComponent<BoxCollider2D>();
		col.size = new Vector2(6,s.gridSize);
		col.offset = new Vector2(0,s.gridSize*0.5f);
		height = col.size.y*0.9f;

		ani.play(Config.CharcterAction.Jump);
		initScaleX = transform.lossyScale.x;

		if(_autoJump)
			jumpHandler = autoJumpHandler;
		else
			jumpHandler = spaceJumpHandler;
	}

	void Update()
	{
		if(isDead)
			return;
		
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			_arrowStatus =  Config.Direction.Right;
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			_arrowStatus =  Config.Direction.Left;
		}

		if(Input.GetKeyUp(KeyCode.RightArrow) && _arrowStatus ==  Config.Direction.Right)
		{
			_arrowStatus =  Config.Direction.None;
		}

		if(Input.GetKeyUp(KeyCode.LeftArrow) && _arrowStatus ==  Config.Direction.Left)
		{
			_arrowStatus =  Config.Direction.None;
		}

		jumpHandler();

		if((_restrictDirection&&_playerDirection == Config.Direction.None) || !_restrictDirection)
			_playerDirection = _arrowStatus;
			
		switch(_playerDirection)
		{
		case Config.Direction.None:
			rb.velocity = new Vector2(0, rb.velocity.y);
			if(isBottom > 0)
				ani.play(Config.CharcterAction.Idle);
			break;

		case Config.Direction.Left:
			rb.velocity = new Vector2(-_moveSpeed, rb.velocity.y);
			if(isBottom > 0)
				ani.play(Config.CharcterAction.Walk);
			if(tf.lossyScale.x == initScaleX)
				tf.localScale = new Vector3(-tf.localScale.x, tf.localScale.y, tf.localScale.z);
			break;

		case Config.Direction.Right:
			rb.velocity = new Vector2(_moveSpeed, rb.velocity.y);
			if(isBottom > 0)
				ani.play(Config.CharcterAction.Walk);
			if(tf.lossyScale.x != initScaleX)
				tf.localScale = new Vector3(-tf.localScale.x, tf.localScale.y, tf.localScale.z);
			break;
		}
	}

	public void OnCollisionEnter2D(Collision2D coll)
	{
		GameObject obj = coll.gameObject;
		switch(coll.gameObject.tag)
		{
		case Config.TAG_CHAR:
  			float delta = coll.contacts[0].point.y - tf.position.y;
			CharacterCell cell = obj.GetComponent<CharacterCell>();
 			if(delta < 2)
			{
				if(isBottom <= 0)
					onBottom();
				cell.onPlayerLand(this);
			}
			else if(delta > height)
			{
				rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
				coll.transform.GetComponent<CharacterCell>().pushUp();
			}
			break;
		}
	}

	//obj that won't effect players action
	//like charactergroup( a blur determin)
	public void OnTriggerEnter2D(Collider2D coll)
	{
		switch(coll.gameObject.tag)
		{
		case Config.TAG_GROUP:
			tf.SetParent(coll.transform, true);
			break;

		case Config.TAG_DANGER:
			dead();
			break;
		}
	}


	public void OnTriggerExit2D(Collider2D coll)
	{
		if(coll.transform == tf.parent)
			tf.SetParent(null, true);
	}

	public void dead()
	{
		isDead = true;
		tf.GetComponent<Collider2D>().enabled = false;
		rb.gravityScale = 0;
		rb.velocity = Vector2.zero;
		tf.DOMove(new Vector3(tf.position.x, tf.position.y + 20), 0.3f).OnComplete(()=>{
			tf.DOMove(new Vector3(tf.position.x, -450), 1).SetDelay(1.0f);
		});
	}

	#region public call

	//character like > < will rebound player;
	public void Rebound(Vector2 param)
	{
		isBottom = 0;
		_arrowStatus = Config.Direction.None;
		rb.velocity = param;

		ani.play(Config.CharcterAction.Jump);
	}

	//lock players move direction and let them move faster
	public void onIce()
	{
		_restrictDirection = true;
		_moveSpeed = GlobalController.instance.setting.moveSpeed*2;

	}

	//unlock players move direction
	public void leaveIce()
	{
		_restrictDirection = false;
		_moveSpeed = GlobalController.instance.setting.moveSpeed*1;
	}

	#endregion

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
			_jumpCount ++;
			isBottom = 0;
			rb.velocity = new Vector2(rb.velocity.x, _upSpeed);
			ani.play(Config.CharcterAction.Jump);
		}

		if(Input.GetKeyUp(KeyCode.Space))
		{
		}
	}

	void onBottom()
	{
		isBottom = 1;
		_jumpCount = 0;
		if(_arrowStatus == Config.Direction.None)
		{
			ani.play(Config.CharcterAction.Idle);
			rb.velocity = Vector2.zero;
		}
		else
		{
			if(_arrowStatus == Config.Direction.Left)
			{
				rb.velocity = new Vector2(-_moveSpeed, rb.velocity.y);
				if(tf.lossyScale.x == initScaleX)
					tf.localScale = new Vector3(-tf.localScale.x, tf.localScale.y, tf.localScale.z);
			}
			else
			{
				rb.velocity = new Vector2(_moveSpeed, rb.velocity.y);
				if(tf.lossyScale.x != initScaleX)
					tf.localScale = new Vector3(-tf.localScale.x, tf.localScale.y, tf.localScale.z);
			}
			ani.play(Config.CharcterAction.Walk);
		}
	}

}
