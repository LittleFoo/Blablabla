using UnityEngine;
using System.Collections.Generic;

public class DeadCharacterGroup : MonoBehaviour {
	public FontAnalyse analyse;
	public Color color = Color.black;
	public BoxCollider2D col;
	public List<GameObject> _character = new List<GameObject>();
	public float MaxTextWidth;
	[SerializeField]
	private string _contentStr;
	public string contentStr
	{
		set{_contentStr = value; setTextInEditor();}
		get{return _contentStr;}
	}
	[SerializeField]
	private Transform _fontObj;
	public Transform fontObj
	{
		set{
			_fontObj = value; 
			if(_fontObj == null)
				return;
			analyse = _fontObj.GetComponent<FontAnalyse>();}
		get{return _fontObj;}
	}
	[SerializeField]
	private Vector2 _pivot = new Vector2(0,1);
	public Vector2 pivot
	{
		set{
			_pivot = value; 
			setPivot();
		}
		get{return _pivot;}
	}
	[SerializeField]
	private char[] chars ;
	[SerializeField]
	private string _lastStr;
	[SerializeField]
	private float _textWidth;
	public float textWidth{get{return _textWidth;}}
	[SerializeField]

	public void setPivot()
	{
		if(_character.Count == 0 || isCreating)
			return;
		
		Transform obj;
		FontData d;
		float x = 0;
		
		float xOffset = -pivot.x*_textWidth, yOffset = (1-pivot.y)*analyse.lineHeight;
		if(analyse.fontDatas.TryGetValue( System.Convert.ToInt32(chars[0]), out d))
		{
			obj = _character[0].transform;
			float firstX = x + xOffset - obj.localPosition.x, firstY =  -d._actualOffsetY+yOffset - obj.localPosition.y;
			if(firstX != 0 || firstY != 0)
				transform.position -= new Vector3(firstX, firstY, 0);
		}
		for(int i = 0; i < chars.Length; i++)
		{
			obj = _character[i].transform;
			
			if(analyse.fontDatas.TryGetValue( System.Convert.ToInt32(chars[i]), out d))
			{
				obj.transform.localPosition = new Vector2(x + xOffset, -d._actualOffsetY+yOffset);
				x += d.actualAdvance;
			}
		}
	}

	private bool isCreating = false;
	public void setTextInEditor(bool ignoreSame = true)
	{
		if(ignoreSame && _lastStr == contentStr)
			return;
		Vector2 lastPivot = pivot;
		pivot = new Vector2(0, 1);
		isCreating = true;
		_lastStr = contentStr;
		PrefabSetting settings = GlobalController.instance.prefabSetting;
		GameObject obj;
		SpriteRenderer spr;
		FontData d;
		float x = 0;

		
		chars = contentStr.ToCharArray();
		for(int i = 0; i < chars.Length; i++)
		{
			if(analyse.fontDatas.TryGetValue( System.Convert.ToInt32(chars[i]), out d))
			{
				if(i >= _character.Count)
				{
					obj = new GameObject();
					spr = obj.AddComponent<SpriteRenderer>();
					_character.Add(obj);
				}
				else
				{
					obj = _character[i];
					obj.SetActive(true);
					spr = obj.GetComponent<SpriteRenderer>();
				}

				obj.tag = Config.TAG_CHAR;
				obj.gameObject.layer = transform.gameObject.layer;
				spr = obj.GetComponent<SpriteRenderer>();
				spr.color = color;
				spr.sortingOrder = 1;
				obj.transform.SetParent(transform, false);
				obj.name = chars[i].ToString();
				obj.transform.localPosition = new Vector2(x, -d._actualOffsetY);
				x += d.actualAdvance;
				spr.sprite = d.spr;
				
			}
			
		}
		
		for(int i = chars.Length; i < _character.Count; i++)
		{
			_character[i].SetActive(false);
		}
		_textWidth = x;
		pivot = lastPivot;
		isCreating = false;
	}

}
