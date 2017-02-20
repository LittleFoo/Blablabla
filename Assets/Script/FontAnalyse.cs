using UnityEngine;
using System;
using System.IO;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;

public class FontAnalyse : MonoBehaviour {

	public Dictionary<int, FontData> fontDatas = new Dictionary<int, FontData>();
	private int _textHeight;
	private int _minOffsety = 999;
	public int lineHeight;
	private string[] stringSeparators = new string[] { "\n" };

	void Awake () {
		slice("Assets/Font/testfont@2x.png", "Assets/Font/testfont@2x.txt");
	}

	public void getFontData(string fileName)
	{
		string[] stringSeparatorsSpace = new string[] { " ", "=" };
		AssetDatabase.LoadAssetAtPath<TextAsset>(fileName);


		TextAsset s = AssetDatabase.LoadAssetAtPath<TextAsset>(fileName);
		string[] lineArray = s.text.Split(stringSeparators, StringSplitOptions.None);

		string[] strs = lineArray[1].Split(stringSeparatorsSpace, StringSplitOptions.None);
		for(int k = 0; k < strs.Length; k++)
		{
			if(strs[k].Equals("scaleH"))
			{
				_textHeight = int.Parse(strs[k+1]);
				k++;
			}

			if(strs[k].Equals("base"))
			{
				lineHeight = int.Parse(strs[k+1]);
				k++;
			}
		}

		int i = 0;
		string line;
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
			
			fontDatas.Add(int.Parse(words[2]), data);
		}
	}
		
	private void slice(string imgPath, string docPath)
	{
		Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(imgPath);
		getFontData(docPath);
		Dictionary<int, FontData>.Enumerator e = fontDatas.GetEnumerator();
		FontData d;
		while(e.MoveNext())
		{
			d = e.Current.Value;
			d.spr = Sprite.Create(texture, new Rect(d.x, (_textHeight - d.y - d.height), d.width, d.height), new Vector2(0, 1), 1);
			d._actualOffsetY = d.yoffset-_minOffsety;
		}
	}
}

public class FontData
{
	public int _id;
	public int id
	{
		get{return id;}
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
	public int _actualOffsetY;
}
