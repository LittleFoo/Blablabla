using UnityEngine;
using System.Collections;
using DG.Tweening;
public class AtShootFeature : MonoBehaviour {

	// Use this for initialization
	private Vector3 topOffset;
	private float gScale;
	private Vector3 initVelocity;
	private BulletData bulletData;
	private Coroutine _curRoutine;
	private float bottomOffset;
	private Sequence _curSeq;
	private float endY;
	private float initY;
	public Transform tf;
	void Start () {
		tf = transform;
		BoxCollider2D col = tf.GetComponent<BoxCollider2D>();
		topOffset = col.offset + new Vector2(0, col.size.y*0.5f + 3);
		bottomOffset = col.offset.y - col.size.y*0.5f;
		Setting s = GlobalController.instance.setting;
		endY = tf.localPosition.y + s.gridSize*0.5f;
		initY = tf.localPosition.y;
		float realG =(s.gridSize*s.bulletBounceGrid)*2/(s.bulletBounceTime*2);
		gScale = realG/s.g;
		initVelocity = new Vector2(s.bulletBounceXspeed, realG*s.bulletBounceTime);
		bulletData = new BulletData();

		_curRoutine = StartCoroutine(move());
	}
	
	IEnumerator move()
	{
		WaitForSeconds delay = new WaitForSeconds(GlobalController.instance.setting.atShootGap);
		while(true)
		{
			yield return delay;
			_curSeq = DOTween.Sequence();
			_curSeq.Append(tf.DOLocalMoveY(endY, 0.5f));
			_curSeq.AppendInterval(0.3f);
			_curSeq.AppendCallback(shoot);
			_curSeq.AppendInterval(0.5f);
			_curSeq.Append(tf.DOLocalMoveY(initY, 0.5f));

		}
	}

	void shoot()
	{
		BounceBullet bullet = GlobalController.instance.getCurPool().Spawn(GlobalController.instance.prefabSetting.bounceBullet).GetComponent<BounceBullet>();
		bullet.init(tf.position + topOffset, 
			gScale,
			initVelocity,
			bulletData,
			tf.position.y + bottomOffset
		);
	}
}
