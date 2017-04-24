using UnityEngine;
using System.Collections;

public class MouseMonster : FeatureReactionBase, common.ITimerEvent, IDistanceTrigger
{
	public BulletData bulletData;
	public float tracingTime;
	public float tracingAngle = 3;
	public float _minRadius = 10;
	private float _curRadius;
	private float _radiusDecrease;
	private float _angleDecrease;
	private float _tracedTime;
	private float _curAngle;

	private Vector3 _curCenter;
	private PhysicalPlayerController player;
	void Awake()
	{
		tf = transform;
		DistanceTriggerManager.instance.addEventListener(this);
	}


	public override void init()
	{
		isDead = false;
		ani.addSprToDic();
		ani.play(Config.CharcterAction.Idle);
	}

	public void OnDisable()
	{
		#if UNITY_EDITOR
		#else
		DistanceTriggerManager.instance.removeEventListener(this);
		common.TimerManager.instance.removeEventListeners(this);
		#endif
	}

	public void onUpdate()
	{
		getCloser();
	}

	public void trigger()
	{
		player = FindObjectOfType<PhysicalPlayerController>();
		_curRadius =  Vector2.Distance(player.tf.position, tf.position);
		_radiusDecrease = (_curRadius - _minRadius)/tracingTime;
		_curAngle = Vector2.Angle(Vector2.zero, tf.position - player.tf.position);
		_angleDecrease = tracingAngle*3.14f/tracingTime;
		_tracedTime = 0;
		_curCenter = player.tf.position;
		common.TimerManager.instance.addEventListeners(this);
	}

	public Vector3 getPosition()
	{
		return tf.position;
	}


	private void shoot()
	{
		Bullet bullet = GlobalController.instance.getCurPool().Spawn(GlobalController.instance.prefabSetting.bullet).GetComponent<Bullet>();

		Config.Direction dir = (tf.transform.lossyScale.x > 0)?Config.Direction.Right:Config.Direction.Left;
		Vector3 pos;
		if(dir == Config.Direction.Left)
		{
			pos = new Vector3(tf.position.x - GlobalController.instance.setting.gridSize, tf.position.y + col.offset.y, 0);
		}
		else
			pos = new Vector3(tf.position.x + GlobalController.instance.setting.gridSize, tf.position.y + col.offset.y, 0);
		bullet.init(pos, dir,bulletData);
	}

	private void getCloser()
	{
		_tracedTime += Time.deltaTime;
		_curRadius -= Time.deltaTime*_radiusDecrease;
		_curAngle -= Time.deltaTime*_angleDecrease;
		tf.position = _curCenter + new Vector3((float)System.Math.Cos(_curAngle)*_curRadius, (float)System.Math.Sin(_curAngle)*_curRadius, 0);
		if(_tracedTime > tracingTime)
			common.TimerManager.instance.removeEventListeners(this);
	}
}
