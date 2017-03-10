﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PhysicalPlayerController : MonoBehaviour {
	public Rigidbody2D rb;
	public CharacterAnimation ani;
	public int isBottom;
	public float moveForce;
	public float jumpForce;
	public float hitForce;
	public Transform tf;
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
	private bool allowToPressSpace = true;
	private Config.Direction arrowStatus = Config.Direction.None;
	private Config.Direction curArrowStatus = Config.Direction.None;
	private float initScaleX;
	private float height;
	private bool isDead = false;
	private System.Action jumpHandler;


	void Start()
	{
		ani.play(Config.CharcterAction.Jump);
		initScaleX = transform.lossyScale.x;
		tf = transform;
		height = GetComponent<BoxCollider2D>().size.y * transform.localScale.y;

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
			arrowStatus =  Config.Direction.Right;
			if(isBottom > 0)
			{
				if(curArrowStatus != arrowStatus)
				{
					ani.play(Config.CharcterAction.Walk);
					curArrowStatus = arrowStatus;
					if(tf.lossyScale.x != initScaleX)
						tf.localScale = new Vector3(-tf.localScale.x, tf.localScale.y, tf.localScale.z);
				}
			}
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			arrowStatus =  Config.Direction.Left;
			if(isBottom > 0)
			{
				if(curArrowStatus != arrowStatus)
				{
					curArrowStatus = arrowStatus;
					if(tf.lossyScale.x == initScaleX)
						tf.localScale = new Vector3(-tf.localScale.x, tf.localScale.y, tf.localScale.z);
					ani.play(Config.CharcterAction.Walk);
				}
			}
		}

		if(Input.GetKeyUp(KeyCode.RightArrow) && arrowStatus ==  Config.Direction.Right)
		{
			arrowStatus =  Config.Direction.None;
			curArrowStatus = arrowStatus;
			rb.velocity = new Vector2(0, rb.velocity.y);
			if(isBottom > 0)
			{
				ani.play(Config.CharcterAction.Idle);
			}
		}

		if(Input.GetKeyUp(KeyCode.LeftArrow) && arrowStatus ==  Config.Direction.Left)
		{
			arrowStatus =  Config.Direction.None;
			curArrowStatus = arrowStatus;
			rb.velocity = new Vector2(0, rb.velocity.y);
			if(isBottom > 0)
			{
				ani.play(Config.CharcterAction.Idle);
			}
		}

		switch(curArrowStatus)
		{
		case Config.Direction.Left:

			if(rb.velocity.x >= -moveForce)
				rb.AddForce(Vector2.left * moveForce);
			print("left:"+rb.velocity.x);
			break;

		case Config.Direction.Right:
			if(rb.velocity.x <= moveForce)
				rb.AddForce(Vector2.right * moveForce);
			print("Right:"+rb.velocity.x);
			break;
		}

		if(allowToPressSpace && Input.GetKeyDown(KeyCode.Space) && isBottom > 0)
		{
			isBottom = 0;
			allowToPressSpace = false;
			rb.AddForce(Vector2.up * jumpForce);
			ani.play(Config.CharcterAction.Jump);
		}

		if(Input.GetKeyUp(KeyCode.Space))
			allowToPressSpace = true;

		jumpHandler();
	}

	void onBottom()
	{
		isBottom = 1;
		curArrowStatus = arrowStatus;
		if(curArrowStatus == Config.Direction.None)
		{
			ani.play(Config.CharcterAction.Idle);
			rb.velocity = Vector2.zero;
		}
		else
		{
			if(curArrowStatus == Config.Direction.Left)
			{
				if(tf.lossyScale.x == initScaleX)
					tf.localScale = new Vector3(-tf.localScale.x, tf.localScale.y, tf.localScale.z);
			}
			else
			{
				if(tf.lossyScale.x != initScaleX)
					tf.localScale = new Vector3(-tf.localScale.x, tf.localScale.y, tf.localScale.z);
			}
			ani.play(Config.CharcterAction.Walk);
		}
	}

	private void autoJumpHandler()
	{
		if(isBottom > 0)
		{
			allowToPressSpace = false;
			isBottom = 0;
			rb.AddForce(Vector2.up * jumpForce);
			ani.play(Config.CharcterAction.Jump);
		}

		if(Input.GetKeyUp(KeyCode.Space))
			allowToPressSpace = true;
	}

	private void spaceJumpHandler()
	{
		if(allowToPressSpace && Input.GetKeyDown(KeyCode.Space) && isBottom > 0)
		{
			isBottom = 0;
			allowToPressSpace = false;
			rb.AddForce(Vector2.up * jumpForce);
			ani.play(Config.CharcterAction.Jump);
		}

		if(Input.GetKeyUp(KeyCode.Space))
			allowToPressSpace = true;


	}

	public void OnCollisionEnter2D(Collision2D coll)
	{
//		for(int i = 0; i < coll.contacts.Length; i++)
//			print(coll.contacts[i].point.y - tf.position.y);
		GameObject obj = coll.gameObject;
		switch(coll.gameObject.tag)
		{
		case Config.TAG_CHAR:
			float delta = coll.contacts[0].point.y - tf.position.y;
			CharacterCell cell = obj.GetComponent<CharacterCell>();
			if(delta < cell.fontData.height)
			{
				if(isBottom <= 0)
					onBottom();
				cell.onPlayerLand(this);
			}
			else if(delta > height)
			{
				rb.AddForce(Vector2.down * hitForce);
				coll.transform.GetComponent<CharacterCell>().pushUp();
			}
			break;
		}
	}

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

	public void OnTriggerExit2D(Collider2D coll)
	{
		if(coll.transform == tf.parent)
			tf.SetParent(null, true);
	}

	public void Rebound(Vector2 param)
	{
		isBottom = 0;
		allowToPressSpace = false;
		curArrowStatus = Config.Direction.None;
		rb.velocity = Vector2.zero;
		rb.AddForce(param);
		ani.play(Config.CharcterAction.Jump);
	}
}
