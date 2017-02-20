using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float initJumpSpeed;
	public float initMoveSpeed;
	public float g;
	public float height;
	public float halfWidth;
	private bool getBottom = false;
	private Test _enteredTrigger;
	private Collider2D _enteredCol;
	private float initY;
	private float y;
	private float x;
	private float _speedx;
	public float speedx{get{return _speedx;}}
	private float _speedy;
	public float speedy{get{return _speedy;}}
	private float lastSpeedY;
	private bool allowToPressSpace = true;
	private Config.Direction arrowStatus = Config.Direction.None;
	private bool isOnDrop;
	// Use this for initialization
	void Start () {
		initY = transform.position.y;
		x = transform.position.x;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(die)
			return;
		
		x = transform.position.x;
		y = transform.position.y;
		if(allowToPressSpace && Input.GetKeyDown(KeyCode.Space) && getBottom)
		{
			allowToPressSpace = false;
			_speedy = initJumpSpeed;
			getBottom = false;
		}

		if(Input.GetKeyUp(KeyCode.Space))
			allowToPressSpace = true;

			if(Input.GetKeyDown(KeyCode.RightArrow))
			{
					arrowStatus = Config.Direction.Right;
			}
			else if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
					arrowStatus =  Config.Direction.Left;
			}

			if(Input.GetKeyUp(KeyCode.RightArrow) && arrowStatus ==  Config.Direction.Right)
			{
				arrowStatus =  Config.Direction.None;
			}

			if(Input.GetKeyUp(KeyCode.LeftArrow) && arrowStatus ==  Config.Direction.Left)
			{
				arrowStatus =  Config.Direction.None;
			}

		if(getBottom || arrowStatus == Config.Direction.None)
		{
			switch(arrowStatus)
			{
			case Config.Direction.Left:
				_speedx = -initMoveSpeed;
				break;
			case Config.Direction.Right:
				_speedx = initMoveSpeed;
				break;

				default:
				_speedx = 0;
				break;
			}
		}
		float deltaX = _speedx*Time.deltaTime;

		if(_enteredTrigger != null && !_enteredTrigger.allowToMove(this, deltaX))
		{
			deltaX = 0;
		}
		x += deltaX;

		if(!getBottom)
		{
			lastSpeedY = _speedy;
			_speedy -= g*Time.deltaTime;
			y = transform.position.y + (_speedy+lastSpeedY)*0.5f*Time.deltaTime;
		}
		transform.position = new Vector3(x, y, 0);
		if(_enteredTrigger != null)
			_enteredTrigger.check(this);
		
//		if(_enteredTrigger != null)
//		{
//			getBottom = true;
//			speedy = 0;
//		}
	}

	private bool die = false;
	public void onDie()
	{
//		GetComponent<SpriteRenderer>().sprite = null;
		transform.localScale = Vector3.one*20;
		die = true;
	}

	public void onDrop(Collider2D other)
	{
//		OnTriggerExit2D(other);
		allowToPressSpace = false;
		getBottom = false;
		isOnDrop = true;
		//		GetComponent<SpriteRenderer>().sprite = null;
//		transform.localScale = Vector3.one*20;
//		die = true;
	}

	public void onTouch(Collider2D other, Config.Direction direction)
	{
		_enteredCol = other;
		switch(direction)
		{
			case Config.Direction.Bottom:
				getBottom = true;
				_speedy = 0;
				isOnDrop = false;
				allowToPressSpace = true;
				_enteredCol = other;
				_enteredTrigger = other.GetComponent<Test>();
				_enteredTrigger.enter(this);
				break;

			case Config.Direction.Top:
				_speedy = -_speedy;
				break;

			case Config.Direction.Left:
				break;

			case Config.Direction.Right:
				break;
		}
	}


	public void OnTriggerEnter2D(Collider2D other)
	{
		if(other.GetComponent<Test>() == null)
			return;
		_enteredTrigger = other.GetComponent<Test>();
		_enteredTrigger.enter(this);

//		if(speedy <= 0)
//		{
//			getBottom = true;
//			speedy = 0;
//			_enteredCol = other;
//			_enteredTrigger = other.GetComponent<Test>();
//			_enteredTrigger.enter(this);
//		}
//		else
//		{
//			if(other != _enteredCol)
//			{
//				speedy = -speedy;
//			}
//		}
	}



	void OnTriggerExit2D(Collider2D other) {

		if(other.GetComponent<Test>() == null)
			return;
		
//		if(_enteredCol == other)
//		{
//			_enteredCol = null;
//			_enteredTrigger = null;
//		}

		other.GetComponent<Test>().leave();
	}
}
