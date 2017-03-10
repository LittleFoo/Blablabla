using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class CharacterGroup : MonoBehaviour {
	// Use this for initialization
//	public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();
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
	public FontAnalyse analyse;
	public Color color = Color.black;
	public List<CharacterCell> _character = new List<CharacterCell>();
	private char[] chars ;
	private string _lastStr;
	private float _textWidth;
	void Start () {
		gameObject.tag = Config.TAG_GROUP;
		for(int i = 0; i < _character.Count; i++)
		{
			_character[i].init();
		}
	}

	public void setPivot()
	{
		if(_character.Count == 0)
			return;

		Transform obj;
		FontData d;
		float x = 0;

		float xOffset = -pivot.x*_textWidth, yOffset = (1-pivot.y)*analyse.lineHeight;
		for(int i = 0; i < _character.Count; i++)
		{
			obj = _character[i].tf;
			if(analyse.fontDatas.TryGetValue( System.Convert.ToInt32(chars[i]), out d))
			{
				obj.transform.localPosition = new Vector2(x + xOffset, -d._actualOffsetY+yOffset);
				x += d.actualAdvance;
			}
		}

		BoxCollider2D col = GetComponent<BoxCollider2D>();
		if(col == null)
			col = gameObject.AddComponent<BoxCollider2D>();
		col.size = new Vector2(_textWidth, analyse.lineHeight);
		col.offset = new Vector2((0.5f - pivot.x)*_textWidth,  	(0.5f-pivot.y)*analyse.lineHeight);
		col.isTrigger = true;

	}

	public void setTextInEditor()
	{
		if(_lastStr == contentStr)
			return;

		_lastStr = contentStr;
		PrefabSetting settings = GlobalController.instance.prefabSetting;
		Transform obj;
		SpriteRenderer spr;
		FontData d;
		CharacterCell cell;
		BoxCollider2D col;
		int x = 0;
		BoxCollider2D prefabCol;

		for(int i = 0; i < _character.Count; i++)
		{
			DestroyImmediate(_character[i].gameObject);
		}
		_character.Clear();

		chars = contentStr.ToCharArray();
		for(int i = 0; i < chars.Length; i++)
		{
			if(analyse.fontDatas.TryGetValue( System.Convert.ToInt32(chars[i]), out d))
			{
				if(d.colList == null || d.colList.Length == 0)
				{
					obj = GameObject.Instantiate( settings.charPrefab0).transform;
					cell = obj.GetComponent<CharacterCell>();
				}
				else 
				{
					if(d.colList.Length == 1)
					{
						obj = GameObject.Instantiate( settings.charPrefab1).transform;
					}
					else
					{
						obj = GameObject.Instantiate( settings.charPrefab2).transform;
					}
					cell = obj.GetComponent<CharacterCell>();
					for(int j = 0; j < cell.colList.Length; j++)
					{
						col = cell.colList[j];
						prefabCol = d.colList[j];
						col.size = new Vector2(prefabCol.size.x, prefabCol.size.y);
						col.offset = new Vector2(prefabCol.offset.x, prefabCol.offset.y);
					}
				}

				obj.tag = Config.TAG_CHAR;
				spr = obj.GetComponent<SpriteRenderer>();
				spr.color = color;
				spr.sortingOrder = 1;
				obj.transform.SetParent(transform, false);
				obj.name = chars[i].ToString();
				obj.transform.localPosition = new Vector2(x, -d._actualOffsetY);

				x += d.actualAdvance;
				spr.sprite = d.spr;

				cell.tf = obj.transform;
				cell.fontData = d;
				_character.Add(cell);
			}

		}

		_textWidth = x;
		setPivot();
	}

	public void setText()
	{
		if(_lastStr == contentStr)
			return;
		
		_lastStr = contentStr;
		SpawnPool pool = GlobalController.instance.getCurPool();
		PrefabSetting settings = GlobalController.instance.prefabSetting;
		Transform obj;
		SpriteRenderer spr;
		FontData d;
		CharacterCell cell;
		BoxCollider2D col;
		int x = 0;
		BoxCollider2D prefabCol;

		for(int i = 0; i < _character.Count; i++)
		{
			pool.Despawn(_character[i].tf);
		}
		_character.Clear();

		chars = contentStr.ToCharArray();
		for(int i = 0; i < chars.Length; i++)
		{
			if(analyse.fontDatas.TryGetValue( System.Convert.ToInt32(chars[i]), out d))
			{
				if(d.colList == null || d.colList.Length == 0)
				{
					obj = pool.Spawn(settings.charPrefab0.transform);
					cell = obj.GetComponent<CharacterCell>();
				}
				else 
				{
					if(d.colList.Length == 1)
					{
						obj = pool.Spawn(settings.charPrefab1.transform);
					}
					else
					{
						obj = pool.Spawn(settings.charPrefab2.transform);
					}
					cell = obj.GetComponent<CharacterCell>();
					for(int j = 0; j < cell.colList.Length; j++)
					{
						col = cell.colList[j];
						prefabCol = d.colList[j];
						col.size = new Vector2(prefabCol.size.x, prefabCol.size.y);
						col.offset = new Vector2(prefabCol.offset.x, prefabCol.offset.y);
					}
				}

				spr = obj.GetComponent<SpriteRenderer>();
				spr.color = color;
				spr.sortingOrder = 1;
				obj.transform.SetParent(transform, false);
				obj.name = chars[i].ToString();
				obj.transform.localPosition = new Vector2(x, -d._actualOffsetY);

				x += d.actualAdvance;
				spr.sprite = d.spr;

				cell.tf = obj.transform;
				cell.fontData = d;
				_character.Add(cell);
			}

		}
	}
}

