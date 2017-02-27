using UnityEngine;
using System;
using System.IO;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;

public class FontAnalyse : MonoBehaviour {
	public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();
	private Dictionary<int, FontData> _fontDatas = new Dictionary<int, FontData>();
	public Dictionary<int, FontData> fontDatas
	{
		get
		{
			if(_fontDatas.Count == 0)
			{
				FontData d;
				for(int i = 0; i < fontDataList.Count; i++)
				{
					d = fontDataList[i];
					_fontDatas.Add(d.id, d);
					d.colList = d.prefab.GetComponents<BoxCollider2D>();
				}
			}

			return _fontDatas;
		}
	}
	public List<FontData> fontDataList = new List<FontData>();
	public string ImgPath;
	public string DataPath;
	public Texture2D texture;
	private int _textHeight;
	private int _minOffsety = 999;
	public int lineHeight;
	private string[] stringSeparators = new string[] { "\n" };

	void Awake () {
		createSprite();
	}

	public void getFontData(string fileName)
	{
		string[] stringSeparatorsSpace = new string[] { " ", "=" };
	

		TextAsset s = Resources.Load<TextAsset>(fileName);
		string[] lineArray = s.text.Split(stringSeparators, StringSplitOptions.None);

		string[] strs = lineArray[1].Split(stringSeparatorsSpace, StringSplitOptions.None);
		for(int k = 0; k < strs.Length; k++)
		{
			if(strs[k].Equals("scaleH"))
			{
				_textHeight = int.Parse(strs[k+1]);
				break;
			}

//			if(strs[k].Equals("base"))
//			{
//				lineHeight = int.Parse(strs[k+1]);
//				k++;
//			}
		}

		int i = 0;
		string[] words;
		FontData data;
		PropertyInfo pi;
		Type t = typeof(FontData);
		for (; i < lineArray.Length; i++)
		{
			if (!Regex.IsMatch(lineArray[i], "char "))
				continue;
			words = lineArray[i].Split(stringSeparatorsSpace, StringSplitOptions.None);
			data = new FontData();
			for(int j = 1; j < words.Length; j++)
			{
				pi = t.GetProperty(words[j]);
				j++;
				if (pi == null)
					continue;

				pi.SetValue(data, Convert.ChangeType( words[j],pi.PropertyType), null);
			}
			if(data.yoffset < _minOffsety)
				_minOffsety = data.yoffset;
			data.Name = ((char)data.id).ToString();
//			fontDatas.Add(int.Parse(words[2]), data);
			fontDataList.Add(data);
		}
	}
		
	public void slice(string imgPath, string docPath)
	{
		string spriteDir = Application.dataPath + "/Resources/" + docPath;
		spriteDir = spriteDir.Substring(0, spriteDir.LastIndexOf("/"));
		string subName;
		GameObject go;
		clear();
		texture = Resources.Load<Texture2D>(imgPath);
		getFontData(docPath);
		FontData d;
		for(int i = 0; i < fontDataList.Count; i++)
		{
			d = fontDataList[i];

			d._actualOffsetY = d.yoffset-_minOffsety;
			if(lineHeight < d._actualOffsetY+ d.height)
				lineHeight = d._actualOffsetY+ d.height;
			
			d.spr = Sprite.Create(texture, new Rect(d.x, (_textHeight - d.y - d.height), d.width, d.height), new Vector2(0, 1), 1);

			go = new GameObject(d.id.ToString());
			go.AddComponent<SpriteRenderer>().sprite = d.spr;
			if(d.id != 32)
			go.AddComponent<BoxCollider2D>();
			subName = spriteDir + "/" + d.id.ToString() + ".prefab";
			subName = subName.Substring(subName.IndexOf("Assets"));
			d.prefab = go;//PrefabUtility.CreatePrefab(subName, go);
			go.transform.SetParent(transform);
			go.transform.localPosition = new Vector3(37*(i%20), 37*(int)(i/20), 0);
			_fontDatas.Add(d.id, d);
//			print(d.prefab.GetComponent<SpriteRenderer>().sprite);
//			GameObject.DestroyImmediate(go);
		}

	}

	public void createSprite()
	{
		FontData d;
//		int width, height;
//		Color c;
//		int count;
//		Rect rect;
		for(int i = 0; i < fontDataList.Count; i++)
		{
			d = fontDataList[i];
			_fontDatas.Add(d.id, d);
			d.colList = d.prefab.GetComponents<BoxCollider2D>();
//			rect = d.spr.rect;
//			d.pixelArray = new List<List<int>>(d.width);
//			for(int j = 0; j < d.width; j++)
//			{
//				d.pixelArray.Add(new List<int>(lineHeight));
//				count = lineHeight - d.height - d._actualOffsetY;
//				for(int k = 0; k < count; k++)
//				{
//					d.pixelArray[j].Add(0);
//				}
//				for(int k = 0; k < rect.height; k++)
//				{
//					width = (int)(j + rect.x);
//					height = (int)(k+ rect.y);
//					c = texture.GetPixel(width, height);
//					if(c.a == 0)
//						d.pixelArray[j].Add(0);
//					else
//						d.pixelArray[j].Add(1);
//				}
//				for(int k = lineHeight - d._actualOffsetY; k < lineHeight; k++)
//				{
//					d.pixelArray[j].Add(0);
//				}
//			}
		}
	}

	public void clear()
	{
		for(int i = 0; i < fontDataList.Count; i++)
		{
			GameObject.DestroyImmediate(fontDataList[i].prefab);
			GameObject.DestroyImmediate(fontDataList[i].spr, true);
		}
		fontDataList.Clear();
		_fontDatas.Clear();
	}
}

[System.Serializable]
public class FontData
{
	public int _id;
	public int id
	{
		get{return _id;}
		set{_id = value;}
	}
	public int _x;
	public int x
	{
		get{return _x;}
		set{_x = value;}
	}
	public int _y;
	public int y
	{
		get{return _y;}
		set{_y = value;}
	}
	public int _width;
	public int width
	{
		get{return _width;}
		set{_width = value;}
	}
	public int _height;
	public int height
	{
		get{return _height;}
		set{_height = value;}
	}
	public int _xoffset;
	public int xoffset
	{
		get{return _xoffset;}
		set{_xoffset = value;}
	}
	public int _yoffset;
	public int yoffset
	{
		get{return _yoffset;}
		set{_yoffset = value;}
	}
	public int _xadvance;
	public int xadvance
	{
		get{return _xadvance;}
		set{_xadvance = value;}
	}
	public Sprite spr;
	public string Name;
	public int _actualOffsetY;
	public GameObject prefab;
	public BoxCollider2D[] colList;
}	
