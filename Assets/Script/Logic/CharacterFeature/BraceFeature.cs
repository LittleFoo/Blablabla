using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BraceFeature : MonoBehaviour {
	public Transform tf;
	private Sequence _seq;
	public Config.Direction direction;
	private Vector3 _shootStartOffset;
	private float initY;
	private float topY;
	private BulletData bulletData;

	void Start () {
		tf = transform;

		initY = tf.localPosition.y;
		topY = initY + GlobalController.instance.setting.gridSize;

		_seq = DOTween.Sequence();
		_seq.AppendInterval(GlobalController.instance.setting.braceTimeMoveGap);
		_seq.Append(tf.DOLocalMoveY(topY, 1).OnComplete(shoot));
		_seq.AppendInterval(0.5f);
		_seq.Append(tf.DOLocalMoveY(initY, 1));
		_seq.SetLoops(int.MaxValue);

		bulletData = new BulletData();
		BoxCollider2D col = tf.GetComponent<BoxCollider2D>();
		bulletData.speed = GlobalController.instance.setting.bulletSpeed;
		switch(direction)
		{
			case Config.Direction.Left:
				_shootStartOffset = new Vector3(col.offset.x - col.size.x*0.5f - 3, col.offset.y);
				break;

			case Config.Direction.Right:
				_shootStartOffset = new Vector3(col.offset.x + col.size.x*0.5f + 3, col.offset.y);
				break;
		}
	}
	
	void shoot()
	{
		Bullet bullet = GlobalController.instance.getCurPool().Spawn(GlobalController.instance.prefabSetting.bullet).GetComponent<Bullet>();
		bullet.init(tf.position + _shootStartOffset, 
			direction,
			bulletData
		);
	}
}
