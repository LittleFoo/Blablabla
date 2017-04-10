using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class FeatureScroll : MonoBehaviour , common.ITimerEvent
{
	[SerializeField]
//	private bool _isClockWise = false;
//	public bool isClockWise
//	{
//		get{return _isClockWise;}
//		set{
//			if(value == _isClockWise)
//			return;
//			_isClockWise = value;
//			create();
//		}
//	}
	public Transform tf;
	[SerializeField]
	private float _speed = 3;
	public float speed
	{
		get{return  _speed;}
		set{
			if(_speed * value > 0)
			{
				_speed = value;
				return;
			}
			_speed = value;
			create();
		}
	}

	public Transform root;
	public BoxCollider2D col;
	[SerializeField]
	private float height;
	[SerializeField]
	private float width;
	[SerializeField]
	private int _xNum;
	[SerializeField]
	private int _yNum;
	[SerializeField]
	private List<GameObject> units = new List<GameObject>();
	[SerializeField]
	private int startIdx = 0;
	[SerializeField]
	private float flagY = 0;
	private Sprite _spr;
	private float _rotationTime;
	private float _dis;
	private bool isRotating = false;
	private System.Action curUpdate;
	public static int unitWidth = 7;
	public static int unitHeight = 4;
	public static int gap = 2;
	public static int edge = 1;
	// Use this for initialization
	void Awake()
	{
		_dis = unitWidth + gap;
		_rotationTime = _dis/Mathf.Abs(_speed);
		if(_speed < 0)
			curUpdate = antiClockUpdate;
		else
			curUpdate = clockWiseUpdate;

		common.TimerManager.instance.addEventListeners(this);
	}

	public void onUpdate()
	{
		curUpdate();
	}

	void antiClockUpdate()
	{
		int count, idx = 0;

		count = startIdx + _xNum;
		for(int i = startIdx; i < count; i++)
		{
			idx = i;
			if(idx >= units.Count)
				idx = idx%units.Count;
			units[idx].transform.localPosition += new Vector3(_speed * Time.deltaTime, 0, 0);


			idx = idx + _xNum + _yNum;
			if(idx >= units.Count)
				idx = idx%units.Count;
			units[idx].transform.localPosition -= new Vector3(_speed * Time.deltaTime, 0);
		}

		count = startIdx + _xNum + _yNum;
		for(int i = startIdx + _xNum ; i < count; i++)
		{
			idx = i;
			if(idx >= units.Count)
				idx = idx%units.Count;
			units[idx].transform.localPosition -= new Vector3(0, _speed * Time.deltaTime);

			idx = idx + _xNum + _yNum;
			if(idx >= units.Count)
				idx = idx%units.Count;
			units[idx].transform.localPosition += new Vector3(0, _speed * Time.deltaTime);
		}

		idx = startIdx;
		if(!isRotating && units[idx].transform.localPosition.y >= flagY)
		{
			int addition = units.Count -1;
			int idx1 = (idx + _xNum)%units.Count;
			int idx2 = (idx1 + _yNum)%units.Count;
			int idx3 = (idx2 + _xNum)%units.Count;
			isRotating = true;
			units[idx].transform.DOLocalRotate(new Vector3(0, 0, units[idx].transform.localRotation.eulerAngles.z + 90), _rotationTime).OnComplete(()=>{
				startIdx = (startIdx+units.Count+1)%units.Count;
				isRotating = false;
				units[idx].transform.localPosition = units[(idx + addition)%units.Count].transform.localPosition + new Vector3(0, _dis, 0);
				units[idx1].transform.localPosition = units[(idx1 + addition)%units.Count].transform.localPosition + new Vector3(_dis, 0, 0);
				units[idx2].transform.localPosition = units[(idx2 + addition)%units.Count].transform.localPosition - new Vector3(0, _dis, 0);
				units[idx3].transform.localPosition = units[(idx3 + addition)%units.Count].transform.localPosition - new Vector3(_dis, 0, 0);
			});

			units[idx].transform.DOLocalMoveY(units[idx].transform.localPosition.y - gap, _rotationTime).SetEase(Ease.OutQuad);

			units[idx1].transform.DOLocalRotate(new Vector3(0, 0, units[idx1].transform.localRotation.eulerAngles.z + 90), _rotationTime);
			units[idx1].transform.DOLocalMoveX(units[idx1].transform.localPosition.x - gap, _rotationTime).SetEase(Ease.OutQuad);

			units[idx2].transform.DOLocalRotate(new Vector3(0, 0, units[idx2].transform.localRotation.eulerAngles.z + 90), _rotationTime);
			units[idx2].transform.DOLocalMoveY(units[idx2].transform.localPosition.y + gap, _rotationTime).SetEase(Ease.OutQuad);

			units[idx3].transform.DOLocalRotate(new Vector3(0, 0, units[idx3].transform.localRotation.eulerAngles.z + 90), _rotationTime);
			units[idx3].transform.DOLocalMoveX(units[idx3].transform.localPosition.x +gap, _rotationTime).SetEase(Ease.OutQuad);
		}
	}

	void clockWiseUpdate()
	{
		int count, idx = 0;

		count = startIdx + _xNum;
		for(int i = startIdx; i < count; i++)
		{
			idx = i;
			if(idx >= units.Count)
				idx = idx%units.Count;
			units[idx].transform.localPosition += new Vector3(_speed * Time.deltaTime, 0, 0);


			idx = idx + _xNum + _yNum;
			if(idx >= units.Count)
				idx = idx%units.Count;
			units[idx].transform.localPosition -= new Vector3(_speed * Time.deltaTime, 0);
		}

		count = startIdx + _xNum + _yNum;
		for(int i = startIdx + _xNum ; i < count; i++)
		{
			idx = i;
			if(idx >= units.Count)
				idx = idx%units.Count;
			units[idx].transform.localPosition -= new Vector3(0, _speed * Time.deltaTime);

			idx = idx + _xNum + _yNum;
			if(idx >= units.Count)
				idx = idx%units.Count;
			units[idx].transform.localPosition += new Vector3(0, _speed * Time.deltaTime);
		}

		if(!isRotating && units[idx].transform.localPosition.y >= flagY)
		{
			int idx1 = (idx + _xNum)%units.Count;
			int idx2 = (idx1 + _yNum)%units.Count;
			int idx3 = (idx2 + _xNum)%units.Count;
			isRotating = true;
			units[idx].transform.DOLocalRotate(new Vector3(0, 0, units[idx].transform.localRotation.eulerAngles.z - 90), _rotationTime).OnComplete(()=>{
				startIdx = (startIdx+units.Count-1)%units.Count;
				isRotating = false;
				units[idx].transform.localPosition = units[(idx + 1)%units.Count].transform.localPosition - new Vector3(_dis, 0, 0);
				units[idx1].transform.localPosition = units[(idx1 + 1)%units.Count].transform.localPosition + new Vector3(0, _dis, 0);
				units[idx2].transform.localPosition = units[(idx2 + 1)%units.Count].transform.localPosition + new Vector3(_dis, 0, 0);
				units[idx3].transform.localPosition = units[(idx3 + 1)%units.Count].transform.localPosition - new Vector3(0, _dis, 0);
			});

			units[idx].transform.DOLocalMoveX(units[idx].transform.localPosition.x + gap, _rotationTime).SetEase(Ease.OutQuad);

			units[idx1].transform.DOLocalRotate(new Vector3(0, 0, units[idx1].transform.localRotation.eulerAngles.z - 90), _rotationTime);
			units[idx1].transform.DOLocalMoveY(units[idx1].transform.localPosition.y - gap, _rotationTime).SetEase(Ease.OutQuad);

			units[idx2].transform.DOLocalRotate(new Vector3(0, 0, units[idx2].transform.localRotation.eulerAngles.z - 90), _rotationTime);
			units[idx2].transform.DOLocalMoveX(units[idx2].transform.localPosition.x -gap, _rotationTime).SetEase(Ease.OutQuad);

			units[idx3].transform.DOLocalRotate(new Vector3(0, 0, units[idx3].transform.localRotation.eulerAngles.z - 90), _rotationTime);
			units[idx3].transform.DOLocalMoveY(units[idx3].transform.localPosition.y + gap, _rotationTime).SetEase(Ease.OutQuad);
		}
	}

	public void create()
	{
		startIdx = 0;
		tf = transform;
		CharacterGroup cg = tf.GetComponent<CharacterGroup>();
		cg.createColliderForChar = false;
		if(root == null)
		{
			root = new GameObject().transform;
			root.tag = Config.TAG_SCROLL;
			root.name = "scroll";
			root.SetParent(tf);

		}

			col = tf.GetComponent<BoxCollider2D>();
		if(col == null);
			col = tf.gameObject.AddComponent<BoxCollider2D>();
		col.isTrigger = false;
		if(_spr)
			DestroyImmediate(_spr);

		for(int i = 0; i < units.Count; i++)
		{
			DestroyImmediate(units[i]);
		}
		units.Clear();

		float pivotX; 
		if(_speed < 0)
			pivotX = 1;
		else
			pivotX = 0;

		_spr = Sprite.Create(GlobalController.instance.prefabSetting.scrollUnitTexture, new Rect(0, 0, unitWidth, unitHeight), new Vector2(pivotX, 0), 1);
		
		_xNum = (int)Mathf.Ceil((cg.textWidth+edge*2-gap)/(unitWidth+gap));
		_yNum = (int)Mathf.Ceil((cg.analyse.lineHeight+edge*2 - gap)/(unitWidth+gap));


		width = _xNum*(unitWidth+gap) + unitHeight*2 + gap;
		height = _yNum*(unitWidth+gap)+ unitHeight*2 + gap;

		SpriteRenderer spr;
		GameObject unit;
		SpriteRenderer parentSpr = cg._character[0].GetComponent<SpriteRenderer>();
		float startX = unitHeight + gap + unitWidth*pivotX;
		float startY = unitHeight + (unitWidth+gap)*_yNum + gap;
		for(int i = 0; i < _xNum; i++)
		{
			unit = new GameObject();
			spr = unit.AddComponent<SpriteRenderer>();
			spr.sprite = _spr;
			unit.transform.SetParent(root);
			spr.color = Config.scrollColors[i%3];
			unit.transform.localPosition = new Vector2( i*(unitWidth+gap) + startX, startY);
			spr.sortingLayerName = parentSpr.sortingLayerName;
			spr.sortingOrder = parentSpr.sortingOrder;
			unit.name = "row"+i.ToString();
			units.Add(unit);
		}

		startX = startX + (unitWidth + gap)*_xNum- unitWidth*pivotX;
		startY = startY - gap -  unitWidth*pivotX;
		for(int i = 0; i < _yNum; i++)
		{
			unit = new GameObject();
			spr = unit.AddComponent<SpriteRenderer>();
			spr.sprite = _spr;
			spr.color = Config.scrollColors[(i+_xNum)%3];
			unit.transform.SetParent(root);
			unit.transform.localPosition = new Vector2(startX, startY - i*(unitWidth+gap));
			unit.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
			spr.sortingLayerName = parentSpr.sortingLayerName;
			spr.sortingOrder = parentSpr.sortingOrder;
			unit.name = "col"+i.ToString();
			units.Add(unit);
		}

		startX = startX + unitHeight*0.5f - unitWidth*0.5f -  unitWidth*pivotX;
		startY= unitHeight ;
		for(int i = 0; i < _xNum; i++)
		{
			unit = new GameObject();
			spr = unit.AddComponent<SpriteRenderer>();
			spr.sprite = _spr;
			spr.color = Config.scrollColors[(i+_xNum + _yNum)%3];
			unit.transform.SetParent(root);
			unit.transform.localPosition = new Vector2( startX -(unitWidth+gap)*i, startY);
			unit.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -180));
			spr.sortingLayerName = parentSpr.sortingLayerName;
			spr.sortingOrder = parentSpr.sortingOrder;
			unit.name = "row"+i.ToString();
			units.Add(unit);
		}

		startX = unitHeight;
		startY =unitHeight + gap + unitWidth*pivotX;
		for(int i = 0; i < _yNum; i++)
		{
			unit = new GameObject();
			spr = unit.AddComponent<SpriteRenderer>();
			spr.sprite = _spr;
			spr.color = Config.scrollColors[(i+_xNum*2 + _yNum)%3];
			unit.transform.SetParent(root);
			unit.transform.localPosition =  new Vector2(startX, startY + i*(unitWidth+gap));
			unit.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -270));
			spr.sortingLayerName = parentSpr.sortingLayerName;
			spr.sortingOrder = parentSpr.sortingOrder;
			unit.name = "col"+i.ToString();
			units.Add(unit);
			flagY = unit.transform.localPosition.y;
		}

		col.size = new Vector2(width - (unitHeight+gap)*2, height);
		col.offset =  new Vector2((0.5f - cg.pivot.x)*cg.textWidth, (0.5f-cg.pivot.y)*cg.analyse.lineHeight);
		root.localPosition = new Vector3(-width*0.5f +(0.5f-cg.pivot.x)*cg.textWidth, -height*0.5f +(0.5f-cg.pivot.y)*cg.analyse.lineHeight, 0);
	}

	public void onPlayerLand(PhysicalPlayerController player)
	{
		
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		FeatureReactionBase item = coll.gameObject.GetComponent<FeatureReactionBase>();
		if(item != null)
			item.onScroll(coll, this);
	}

	public void OnCollisionExit2D(Collision2D coll)
	{
		FeatureReactionBase item = coll.gameObject.GetComponent<FeatureReactionBase>();
		if(item != null)
			item.leaveScroll(coll, this);
	}

	public void OnDestroy()
	{
		common.TimerManager.instance.removeEventListeners(this);
	}
}
