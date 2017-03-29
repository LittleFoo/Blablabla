using UnityEngine;
using System.Collections;

public class Setting : MonoBehaviour {
	public int g = 10;
	public int gridSize = 10;
	public int gridNum = 11;
	public Vector2 roleColliderSize = new Vector2(10,10);
	public float jumpGridNum = 2;
	public float jumpTime = 0.5f;
	public float moveSpeed = 100;

	public Vector2 angleBlanketReboundParam;
	public float reboundProtectTime = 0.3f;
	private float _bigJumpTime;
	public float bigJumpTime
	{
		get{return _bigJumpTime;}
	}

	private float _smallUpSpeed;
	public float smallUpSpeed
	{
		get{return _smallUpSpeed;}
	}

	private float _bigUpSpeed;
	public float bigUpSpeed
	{
		get{return _bigUpSpeed;}
	}

	private float _playerG;
	public float playerG
	{
		get{return _playerG;}
	}

	public void init()
	{
		_playerG = jumpGridNum*gridSize*2/jumpTime/jumpTime;
		_smallUpSpeed = _playerG*jumpTime;
		_bigJumpTime = Mathf.Sqrt(jumpGridNum*gridSize*2/_playerG);
		_bigUpSpeed =_playerG*bigJumpTime;
	}
}
