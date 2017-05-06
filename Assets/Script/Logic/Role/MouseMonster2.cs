using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMonster2 : FeatureReactionBase, common.ITimerEvent
{

	PhysicalPlayerController _player;
	public float initSpeedX;
	public float initspeedY;
	public float maxDis;
	private float a;

	void Start()
	{
		tf = transform;
		tf.GetComponent<TriggerHandler>().handler += trigger;

	}

	public void onUpdate()
	{
		getCloser();
	}

	public void trigger()
	{
		a = 1.5f*initSpeedX;
		delta.x = -initSpeedX;
		delta.y = -initspeedY;
		CameraScroll cam = Camera.main.GetComponent<CameraScroll>();
		cam.lockCamera();
		_player = FindObjectOfType<PhysicalPlayerController>();
		cam.move(_player.tf, new Vector2(0.2f, 0.2f), 1.5f);
		common.TimerManager.instance.addEventListeners(this);
		closerTrigger();
	}

	private void closerTrigger()
	{
		tf.position = _player.tf.position + 0.8f * new Vector3(GlobalController.instance.setting.screenWidth, GlobalController.instance.setting.screenHeight, 0);

		common.TimerManager.instance.addEventListeners(this);
	}

	private Vector3 delta = new Vector3();
	private float ax;
	private float ay;

	private void getCloser()
	{
		if(ax != 0)
		{
			delta.x += ax * Time.deltaTime;
			if(Mathf.Abs(delta.x) >= initSpeedX)
			{
				ax = 0;
				maxDis *= 0.8f;
			}

		}

		if(ay != 0)
		{
			delta.y += ay * Time.deltaTime;
			if(Mathf.Abs(delta.y) >= initspeedY)
				ay = 0;
		}

		tf.position += delta * Time.deltaTime;
		if(delta.x < 0 && tf.position.x - _player.tf.position.x < -maxDis)
		{
			ax = a;
		}
		else if(delta.x > 0 && tf.position.x - _player.tf.position.x > maxDis)
		{
			ax = -a;
		}

		if(delta.y < 0 && tf.position.y - _player.tf.position.y < -maxDis)
		{
			ay = a;
		}
		else if(delta.y > 0 && tf.position.y - _player.tf.position.y > maxDis)
		{
			ay = -a;
		}
	}
}
