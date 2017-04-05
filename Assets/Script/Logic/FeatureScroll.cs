using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class FeatureScroll : MonoBehaviour
{
	public float scrollSpeed;
	public Transform tf;
	private float speed = 1;
	//	public int isClockWise = 0;
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

			idx = idx + _xNum + _yNum+1;
			if(idx >= units.Count)
				idx = idx%units.Count;
			units[idx].transform.localPosition += new Vector3(0, speed * Time.deltaTime);
		}

		if(!isRotating && units[idx].transform.localPosition.y >= flagY)
		{
			startIdx = (startIdx+1)%units.Count;
			isRotating = true;
			Transform rotateTf = units[idx].transform;
			rotateTf.DOLocalRotate(new Vector3(0, 0, 360), _rotationTime).OnComplete(()=>{
				isRotating = false;});
//			idx = (idx + _xNum)%units.Count;
//			units[idx].transform.DOLocalRotate(new Vector3(0, 0, -90), _rotationTime);
//			idx = (idx + _yNum);
//			units[idx%units.Count].transform.DOLocalRotate(new Vector3(0, 0, -180), _rotationTime);
//			idx = idx + _xNum;
//			units[idx].transform.DOLocalRotate(new Vector3(0, 0, 90), _rotationTime);
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

		_spr = Sprite.Create(GlobalController.instance.prefabSetting.scrollUnitTexture, new Rect(0, 0, 7, 4), new Vector2(0.5f, 0.5f), 1);
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
