using UnityEngine;
using System.Collections;

public class HandMonster : FeatureReactionBase, common.ITimerEvent {
	public float timeEachRound = 5;
	private Vector3 differ = Vector3.zero;
	private PhysicalPlayerController player;
	private Vector3 initPos;
	private float arcSpeed;
	private float _curArc;
	private float _arcAddition;
	private Vector3 center;
	// Use this for initialization
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
		player = FindObjectOfType<PhysicalPlayerController>();
		cam.move(player.tf, new Vector2(0.2f, 0.2f), 1.5f);
		common.TimerManager.instance.addEventListeners(this);

		closerTrigger();
	}


	private void closerTrigger()
	{
		arcSpeed = 3.14f*2/5;
		_arcAddition = 0;


		center.y = player.tf.position.y;
	

		Vector3 centerPos = (tf.position + player.tf.position)*0.5f;

		center.x = tf.position.x +Vector3.Distance(center, tf.position)/(float)System.Math.Cos((Vector3.Angle(Vector3.right, tf.position- player.tf.position )/180.0*3.14));
		Transform obj = GameObject.Find("Coin").transform;
		print(Vector3.Angle(Vector3.right, tf.position- player.tf.position  ));
		obj.position = center;

	}

	private void getCloser()
	{
		
	}
}
