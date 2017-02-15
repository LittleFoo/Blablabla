using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

	// Use this for initialization
	public string contentStr;
	public FontAnalyse analyse;
	public BoxCollider2D col;
	private List<SpriteRenderer> character;
	private bool hasEntered = false;
	private int crossIdx;
	void Start () {
		GameObject obj;
		SpriteRenderer spr;
		FontData d = new FontData();
		int x = 0;
		char[] chars = contentStr.ToCharArray();
		for(int i = 0; i < chars.Length; i++)
		{
			obj = new GameObject();
			spr = obj.AddComponent<SpriteRenderer>();
			spr.color = Color.blue;
			spr.sortingOrder = 1;
			obj.transform.SetParent(transform, false);
			obj.name = chars[i].ToString();

			if(analyse.fontDatas.TryGetValue( System.Convert.ToInt32(chars[i]), out d))
			{
				obj.transform.localPosition = new Vector2(x, -d._actualOffsetY);
				x += d.xadvance;
			}
			spr.sprite = d.spr;
		}

		col.size = new Vector2(x + d.width, analyse.lineHeight);
		col.offset = new Vector2(col.size.x*0.5f, - col.size.y*0.5f);
	}

	public void check(PlayerController t)
	{
		
	}

	public void enter()
	{
		hasEntered = true;
	}

	public void leave()
	{
		hasEntered = false;
	}
}
