﻿using UnityEngine;
using System.Collections;

public class PhysicalPlayerController : MonoBehaviour {
	public Rigidbody2D rb;
	public CharacterAnimation ani;
	public int isBottom;
	public float moveForce;
	public float jumpForce;
	public float hitForce;
	public Transform tf;
	private bool allowToPressSpace = true;
	private Config.Direction arrowStatus = Config.Direction.None;
	private Config.Direction curArrowStatus = Config.Direction.None;
	private float initScaleX;
	private float height;
	void Start()
	{
		ani.play(Config.CharcterAction.Jump);
		initScaleX = transform.localScale.x;
		tf = transform;
		height = GetComponent<BoxCollider2D>().size.y * transform.localScale.y;
	}

	void Update()
	{
		
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			arrowStatus =  Config.Direction.Right;
			if(isBottom > 0)
			{
				if(curArrowStatus != arrowStatus)
				{
					ani.play(Config.CharcterAction.Walk);
					curArrowStatus = arrowStatus;
				}
				tf.localScale = new Vector3(initScaleX, tf.localScale.y, tf.localScale.z);

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
					tf.localScale = new Vector3(-initScaleX, tf.localScale.y, tf.localScale.z);
				}
				ani.play(Config.CharcterAction.Walk);
			}
		}

		if(Input.GetKeyUp(KeyCode.RightArrow) && arrowStatus ==  Config.Direction.Right)
		{
			arrowStatus =  Config.Direction.None;
			curArrowStatus = arrowStatus;
			if(isBottom > 0)
			{
				ani.play(Config.CharcterAction.Idle);
			}
		}

		if(Input.GetKeyUp(KeyCode.LeftArrow) && arrowStatus ==  Config.Direction.Left)
		{
			arrowStatus =  Config.Direction.None;
			curArrowStatus = arrowStatus;
			if(isBottom > 0)
			{
				ani.play(Config.CharcterAction.Idle);
			}
		}


	}

	void onBottom()
	{
		isBottom = 1;
		curArrowStatus = arrowStatus;
		if(curArrowStatus == Config.Direction.None)
			ani.play(Config.CharcterAction.Idle);
		else
		{
			if(curArrowStatus == Config.Direction.Left)
				tf.localScale = new Vector3(-initScaleX, tf.localScale.y, tf.localScale.z);
			else
				tf.localScale = new Vector3(initScaleX, tf.localScale.y, tf.localScale.z);
			ani.play(Config.CharcterAction.Walk);
		}
//		rb.constraints = RigidbodyConstraints2D.FreezePosition;
//		StartCoroutine(unFreezePosition());
	}

	WaitForEndOfFrame nextFrame = new WaitForEndOfFrame();
	IEnumerator unFreezePosition()
	{
		yield return nextFrame;
		rb.constraints = RigidbodyConstraints2D.FreezePositionX;
	}

	void FixedUpdate()
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

		switch(curArrowStatus)
		{
		case Config.Direction.Left:
			rb.AddForce(Vector2.left * moveForce);
			break;

		case Config.Direction.Right:
			rb.AddForce(Vector2.right * moveForce);
			break;


		}
	}

	public void OnCollisionEnter2D(Collision2D coll)
	{
//		for(int i = 0; i < coll.contacts.Length; i++)
//			print(coll.contacts[i].point.y - tf.position.y);

		float delta = coll.contacts[0].point.y - tf.position.y;
		if(delta < 10)
			onBottom();
		else if(delta > height)
		{
			rb.AddForce(Vector2.down * hitForce);
			coll.transform.GetComponent<CharacterCell>().pushUp();
		}
		isBottom++;
	}

	void OnCollisionExit2D(Collision2D coll)
	{
		if(isBottom > 0)
			isBottom --;
		if(isBottom <= 0)
			ani.play(Config.CharcterAction.Jump);
	}

}
