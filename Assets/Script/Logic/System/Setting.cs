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

	public float waterUpSpeed = 50;
	public float waterMoveSpeed = 50;

	//for test
	public float bulletSpeed = 400;
	public float bulletCD = 0.5f;

	//角括号反弹速度
	public Vector2 angleBlanketReboundParam;
	//反弹保护时间
	public float reboundProtectTime = 0.3f;
	//数字消失时间
	public float numberDisappearTime = 1.0f;
	//下划线移动速度
	public float underLineSpeed = 100;
	//大括号间隔
	public float braceTimeMoveGap = 2;
	//子弹弹跳高度
	public float bulletBounceGrid = 2;
	//子弹弹跳到最高点时间
	public float bulletBounceTime = 1.5f;
	//子弹弹跳高度
	public float bulletBounceXspeed = -20;
	//上发子弹间隔
	public float atShootGap = 3;

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
	private float _screenWidth;
	public float screenWidth
	{
		get{return _screenWidth;}
	}
	private float _screenHeight;
	public float screenHeight
	{
		get{return _screenHeight;}
	}

	public void init()
	{
		_playerG = jumpGridNum*gridSize*2/jumpTime/jumpTime;
		_smallUpSpeed = _playerG*jumpTime;
		_bigJumpTime = Mathf.Sqrt(jumpGridNum*gridSize*2/_playerG);
		_bigUpSpeed =_playerG*bigJumpTime;


		_screenWidth= Mathf.Ceil( gridSize*gridNum/9.0f*16);
		_screenWidth = Mathf.Ceil(screenWidth*1.0f/16)*16;
		_screenHeight = screenWidth/16*9;

		float rate = UIModule.instance.actualWidth/UIModule.instance.actualHeight;
		_screenHeight = _screenWidth/rate;

		Camera.main.orthographicSize = _screenHeight*0.5f;
		print("screenW:"+screenWidth.ToString() +",screenHeight:"+screenHeight.ToString());
	}
}
