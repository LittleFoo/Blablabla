using UnityEngine;
using System.Collections;

public class UnderLineMove : MonoBehaviour, common.ITimerEvent, IAwake {

	// Use this for initialization
	public Transform tf;
	private Vector3 _speed;
	private float nextSpeed;
	private float centerX;
	void Awake()
	{
		tf = transform;
	}

	void Start () {
		_speed = new Vector3( GlobalController.instance.setting.underLineSpeed, 0, 0);

		centerX = GetComponent<BoxCollider2D>().offset.x;
		AwakeManager.instance.addEventListener(this);
	}

	public void onAwake()
	{
		common.TimerManager.instance.addEventListeners(this);
	}
	
	// Update is called once per frame
	public void onUpdate () {
		tf.position += _speed*Time.deltaTime;
		if(_speed.x == 0)
			_speed.x = nextSpeed;

	}

	public void OnCollisionEnter2D(Collision2D coll)
	{
		GameObject obj = coll.gameObject;
		switch(coll.gameObject.tag)
		{
			case Config.TAG_CHAR:
				_speed.x = 0;
				if(coll.contacts[0].point.x < tf.position.x + centerX)
				{
					nextSpeed = GlobalController.instance.setting.underLineSpeed;
				}
				else
					nextSpeed = -GlobalController.instance.setting.underLineSpeed;
				break;
		}
	}
}
