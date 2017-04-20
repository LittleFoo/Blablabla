using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class DollMachine : MonoBehaviour {
	private Config.Direction curArrow;
	private float curSpeed;
	private Vector3 initPos;
	private bool isDown = false;
	public float downDis;
	public float pauseTime;
	public float minX;
	public float maxX;
	public float moveSpeed;
	public Tweener _curTween;
	public Animator ani;
	public DollMachineDetect detect;
	public Transform tf;
//	public GameObject left;
//	public GameObject right;
//	public GameObject down;


//	public void onClickLeft(GameObject obj, UnityEngine.EventSystems.PointerEventData d)
//	{
//	}
//
//	public void onClickLeft(GameObject obj, UnityEngine.EventSystems.PointerEventData d)
//	{
//	}
//
//	public void onClickLeft(GameObject obj, UnityEngine.EventSystems.PointerEventData d)
//	{
//	}

	void Start()
	{
		tf = transform;
		initPos = tf.position;
	}

	void Update()
	{
		if(!isDown)
		{
			if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				curArrow = Config.Direction.Left;
				curSpeed = -moveSpeed;
			}

			if(Input.GetKeyUp(KeyCode.LeftArrow))
			{
				if(curArrow == Config.Direction.Left)
				{
					curArrow = Config.Direction.None;
					curSpeed = 0;
				}
			}

			if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				curArrow = Config.Direction.Right;
				curSpeed = moveSpeed;
			}
			
			if(Input.GetKeyUp(KeyCode.RightArrow))
			{
				if(curArrow == Config.Direction.Right)
				{
					curArrow = Config.Direction.None;
					curSpeed = 0;
				}
			}

			if((curSpeed > 0 && tf.position.x < maxX) || (curSpeed < 0 && tf.position.x > minX))
			tf.position += new Vector3(curSpeed, 0, 0);

			if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				curArrow = Config.Direction.Right;
				
			}
			
			if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				isDown = true;
				_curTween = tf.DOMoveY(tf.position.y - downDis, 1).OnComplete(catchDoll);
			}
		}

		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			ani.Play("init");
			tf.position = initPos;
			curSpeed = 0;
			curArrow = Config.Direction.None;
			isDown = false;
			if(_curTween != null)
				_curTween.Kill();
			Collider2D[] cols = tf.GetComponentsInChildren<Collider2D>();
			for(int i =0 ; i < cols.Length; i++)
				cols[i].enabled = false;
		}
	}


	private void catchDoll()
	{
		ani.Play("catch");
		_curTween = tf.DOMoveY(initPos.y , 0.5f).SetDelay(pauseTime);

		_curTween = tf.DOShakePosition(0.3f, 3, 20, 5).SetDelay(0.4f+pauseTime).OnStart(()=>{
			Collider2D[] cols = tf.GetComponentsInChildren<Collider2D>();
			for(int i =0 ; i < cols.Length; i++)
				cols[i].enabled = false;
			for(int i = 0; i < detect.catchList.Count; i++)
			{
				if( detect.catchList[i].name == "king")
				{
					detect.catchList[i].parent = null;
					detect.catchList[i].GetComponent<Rigidbody2D>().gravityScale = 10;
					detect.catchList[i].GetComponent<Collider2D>().enabled = true;
				}
			}
		});
	}
}
