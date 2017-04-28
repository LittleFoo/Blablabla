using UnityEngine;
using System.Collections;

public class HandMonster : FeatureReactionBase, common.ITimerEvent {
	private Vector3 center;
	private float _duration;
	PhysicalPlayerController _player;
	void Start () {
		tf = transform;
		tf.GetComponent<TriggerHandler>().handler += trigger;

	}
	
	public void onUpdate()
	{
		getCloser();
	}

	public void trigger()
	{
		CameraScroll cam = Camera.main.GetComponent<CameraScroll>();
		cam.lockCamera();
		center = cam.endPos;
		_player = FindObjectOfType<PhysicalPlayerController>();
		cam.move(_player.tf, new Vector2(0.2f, 0.2f), 1.5f);
		common.TimerManager.instance.addEventListeners(this);

		closerTrigger();




	}

	float basef;
	float angle;
	private void closerTrigger()
	{
		tf.position = _player.tf.position+ 0.8f*new Vector3(GlobalController.instance.setting.screenWidth, GlobalController.instance.setting.screenHeight, 0);

		center = _player.tf.position;
		float p = Vector2.Distance(tf.position, center)*0.5f;
	
		_curRate = 7;
		angle = Vector2.Angle(Vector2.right, tf.position - center)/180*3.14f+3.14f*8;
		basef = Mathf.Pow(p, 1/angle);
		common.TimerManager.instance.addEventListeners(this);
	}

	private float _timePassed;
	private float _curRate;
	private float _delta = 1;
	private void getCloser()
	{
		angle -= Time.deltaTime*_delta;
		float p = Mathf.Pow(basef, angle);
		tf.position = center + new Vector3((float)System.Math.Cos(angle), (float)System.Math.Sin(angle), 0)*p;

		if(angle <= 3.14*_curRate)
		{
			center = _player.tf.position;
			basef = Mathf.Pow(p, 1/angle);
			_delta*= 0.5f;
			tf.position = center + new Vector3((float)System.Math.Cos(angle), (float)System.Math.Sin(angle), 0)*p;
			common.TimerManager.instance.removeEventListeners(this);
		}

	}

}
