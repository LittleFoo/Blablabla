using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class FeatureScroll : MonoBehaviour
{
	public float scrollSpeed;
	public Transform tf;
	private float speed = 12;
		private int isClockWise = 1;
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

	private bool isRotating = false;
	public static int unitWidth = 7;
	public static int unitHeight = 4;
	public static int gap = 2;
	public static int edge = 1;
	// Use this for initialization
	void Awake()
	{
		_rotationTime = (unitWidth*0.5f+unitHeight*0.5f+gap)/speed;

//		if(isClockWise == 0)
//			_spr.pivot = new Vector2(1, 0);
//		else
//			_spr.pivot = new Vector2(0, 0);
	}


	void Update()
	{
		int count, idx = 0;

		count = startIdx + _xNum;
		for(int i = startIdx; i < count; i++)
		{
			idx = i;
			if(idx >= units.Count)
				idx = idx%units.Count;
			units[idx].transform.localPosition += new Vector3(speed * Time.deltaTime, 0, 0);


			idx = idx + _xNum + _yNum;
			if(idx >= units.Count)
				idx = idx%units.Count;
			units[idx].transform.localPosition -= new Vector3(speed * Time.deltaTime, 0);
		}

		count = startIdx + _xNum + _yNum;
		for(int i = startIdx + _xNum ; i < count; i++)
		{
			idx = i;
			if(idx >= units.Count)
				idx = idx%units.Count;
			units[idx].transform.localPosition -= new Vector3(0, speed * Time.deltaTime);

			idx = idx + _xNum + _yNum;
			if(idx >= units.Count)
				idx = idx%units.Count;
			units[idx].transform.localPosition += new Vector3(0, speed * Time.deltaTime);
		}

		if(!isRotating && units[idx].transform.localPosition.y >= flagY)
		{
			int idx1 = (idx + _xNum)%units.Count;
			int idx2 = (idx1 + _yNum)%units.Count;
			int idx3 = (idx2 + _xNum)%units.Count;
			isRotating = true;
			Transform rotateTf = units[idx].transform;
			rotateTf.DOLocalRotate(new Vector3(0, 0, units[idx].transform.localRotation.eulerAngles.z - 90), _rotationTime).OnComplete(()=>{
				startIdx = (startIdx+units.Count-1)%units.Count;
				isRotating = false;
				rotateTf.localPosition = units[(idx + 1)%units.Count].transform.localPosition - new Vector3(unitWidth+gap, 0, 0);
				units[idx1].transform.localPosition = units[(idx1 + 1)%units.Count].transform.localPosition + new Vector3(0, unitWidth+gap, 0);
				units[idx2].transform.localPosition = units[(idx2 + 1)%units.Count].transform.localPosition + new Vector3(unitWidth+gap, 0, 0);
				units[idx3].transform.localPosition = units[(idx3 + 1)%units.Count].transform.localPosition - new Vector3(0, unitWidth+gap, 0);
			});

			units[idx1].transform.DOLocalRotate(new Vector3(0, 0, units[idx1].transform.localRotation.eulerAngles.z - 90), _rotationTime);
			units[idx2].transform.DOLocalRotate(new Vector3(0, 0, units[idx2].transform.localRotation.eulerAngles.z - 90), _rotationTime);
			units[idx3].transform.DOLocalRotate(new Vector3(0, 0, units[idx3].transform.localRotation.eulerAngles.z - 90), _rotationTime);
		}
	}

	public void create()
	{
		startIdx = 0;
		tf = transform;
		CharacterGroup cg = tf.GetComponent<CharacterGroup>();
		if(root == null)
		{
			root = new GameObject().transform;
			root.SetParent(tf);
			col = root.gameObject.AddComponent<BoxCollider2D>();
		}

		if(_spr)
			DestroyImmediate(_spr);
//		if(isClockWise == 0)
//			_spr = Sprite.Create(GlobalController.instance.prefabSetting.scrollUnitTexture, new Rect(0, 0, unitWidth, unitHeight), new Vector2(1, 0), 1);
//		else
//			_spr = Sprite.Create(GlobalController.instance.prefabSetting.scrollUnitTexture, new Rect(0, 0, unitWidth, unitHeight), new Vector2(0, 0), 1);
		_spr = Sprite.Create(GlobalController.instance.prefabSetting.scrollUnitTexture, new Rect(0, 0, unitWidth, unitHeight), new Vector2(0.5f, 0.5f), 1);

		_yNum = (int)Mathf.Ceil((cg.analyse.lineHeight+edge*2 + unitHeight)/(unitWidth+gap));
		_xNum = (int)Mathf.Ceil((cg.textWidth+edge*2+unitHeight)/(unitWidth+gap));


		width = _xNum*(unitWidth+gap) + unitHeight;
		height = _yNum*(unitWidth+gap)+ unitHeight;

		for(int i = 0; i < units.Count; i++)
		{
			DestroyImmediate(units[i]);
		}
		units.Clear();


		SpriteRenderer spr;
		GameObject unit;
		SpriteRenderer parentSpr = cg._character[0].GetComponent<SpriteRenderer>();
		float startX = unitWidth*0.5f;
		float startY = unitHeight + (unitWidth+gap)*_yNum - unitHeight*0.5f;
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

		startX = _xNum*(unitWidth+gap) + unitHeight*0.5f;
		startY = startY + unitHeight*0.5f - unitWidth*0.5f;
		for(int i = 0; i < _yNum; i++)
		{
			unit = new GameObject();
			spr = unit.AddComponent<SpriteRenderer>();
			spr.sprite = _spr;
			spr.color = Config.scrollColors[(i+_xNum)%3];
			unit.transform.SetParent(root);
			unit.transform.localPosition = new Vector2(startX, startY - i*(unitWidth+gap));
			unit.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
			spr.sortingLayerName = parentSpr.sortingLayerName;
			spr.sortingOrder = parentSpr.sortingOrder;
			unit.name = "col"+i.ToString();
			units.Add(unit);
		}

		startX = startX + unitHeight*0.5f - unitWidth*0.5f;
		startY= unitHeight*0.5f;
		for(int i = 0; i < _xNum; i++)
		{
			unit = new GameObject();
			spr = unit.AddComponent<SpriteRenderer>();
			spr.sprite = _spr;
			spr.color = Config.scrollColors[(i+_xNum + _yNum)%3];
			unit.transform.SetParent(root);
			unit.transform.localPosition = new Vector2( startX -(unitWidth+gap)*i, startY);
			unit.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
			spr.sortingLayerName = parentSpr.sortingLayerName;
			spr.sortingOrder = parentSpr.sortingOrder;
			unit.name = "row"+i.ToString();
			units.Add(unit);
		}

		startX = unitHeight*0.5f;
		startY = unitWidth*0.5f;
		for(int i = 0; i < _yNum; i++)
		{
			unit = new GameObject();
			spr = unit.AddComponent<SpriteRenderer>();
			spr.sprite = _spr;
			spr.color = Config.scrollColors[(i+_xNum*2 + _yNum)%3];
			unit.transform.SetParent(root);
			unit.transform.localPosition =  new Vector2(startX, startY + i*(unitWidth+gap));
			unit.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
			spr.sortingLayerName = parentSpr.sortingLayerName;
			spr.sortingOrder = parentSpr.sortingOrder;
			unit.name = "col"+i.ToString();
			units.Add(unit);
			flagY = unit.transform.localPosition.y;
		}

		col.size = new Vector2(width, height);
		col.offset = new Vector2(width*0.5f, height*0.5f);
		root.localPosition = new Vector3(-width*0.5f +(0.5f-cg.pivot.x)*cg.textWidth, -height*0.5f +(0.5f-cg.pivot.y)*cg.analyse.lineHeight, 0);
	}
}
