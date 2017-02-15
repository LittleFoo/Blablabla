using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float initJumpSpeed;
	public float initMoveSpeed;
	public float g;
	private bool getBottom = false;
	private Test _enteredTrigger;
	private Collider2D _enteredCol;
	private float initY;
	private float y;
	private float x;
	private float speedx;
	private float speedy;
	private float lastSpeedY;
	private bool allowToPressSpace = true;
	private int arrowStatus = 0;
	// Use this for initialization
	void Start () {
		initY = transform.position.y;
		x = transform.position.x;
	}
	
	// Update is called once per frame
	void Update()
	{
		x = transform.position.x;
		y = transform.position.y;
		if(allowToPressSpace && Input.GetKeyDown(KeyCode.Space) && getBottom)
		{
			allowToPressSpace = false;
			speedy = initJumpSpeed;
			getBottom = false;
		}

		if(Input.GetKeyUp(KeyCode.Space))
			allowToPressSpace = true;

		x += speedx*Time.deltaTime;

			if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				speedx = initMoveSpeed;
				arrowStatus = 1;
			}
			else if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				speedx = -initMoveSpeed;
				arrowStatus = -1;
			}
			

		if(Input.GetKeyUp(KeyCode.RightArrow) && arrowStatus == 1)
		{
			speedx = 0;
			arrowStatus = 0;
		}

		if(Input.GetKeyUp(KeyCode.LeftArrow) && arrowStatus == -1)
		{
			speedx = 0;
			arrowStatus = 0;
		}

		if(!getBottom)
		{
			lastSpeedY = speedy;
			speedy -= g*Time.deltaTime;
			y = transform.position.y + (speedy+lastSpeedY)*0.5f*Time.deltaTime;
		}
		transform.position = new Vector3(x, y, 0);
		if(_enteredTrigger != null)
			_enteredTrigger.check(this);
		
		if(speedy < 0 && _enteredTrigger != null)
		{
			getBottom = true;
			speedy = 0;
		}
	}

	public void onDie()
	{
		GetComponent<SpriteRenderer>().sprite = null;
	}

	public void onGetBottom()
	{
		speedy = 0;
		getBottom = true;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if(other.GetComponent<Test>() == null)
			return;
		_enteredCol = other;
		_enteredTrigger = other.GetComponent<Test>();
		_enteredTrigger.enter();
	}



	void OnTriggerExit2D(Collider2D other) {
		
		if(_enteredCol == other)
		{
			_enteredCol = null;
			_enteredTrigger = null;
		}

		if(other.GetComponent<Test>() == null)
			return;

		other.GetComponent<Test>().leave();
	}
}
