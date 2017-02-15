using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

	// Use this for initialization
	public string contentStr;
	public FontAnalyse analyse;
	public BoxCollider2D col;
	private List<Transform> character = new List<Transform>();
	private bool hasEntered = false;
	private int crossIdx;
	private char[] chars ;
	void Start () {
		GameObject obj;
		SpriteRenderer spr;
		FontData d = new FontData();
		int x = 0;
		chars = contentStr.ToCharArray();
		for(int i = 0; i < chars.Length; i++)
		{
			obj = new GameObject();
			spr = obj.AddComponent<SpriteRenderer>();
			spr.color = Color.blue;
			spr.sortingOrder = 1;
			obj.transform.SetParent(transform, false);
			obj.name = chars[i].ToString();
			character.Add(obj.transform);
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
		Transform tf = t.transform;
		if(tf.position.x > character[crossIdx].transform.position.x)
		{
			if(crossIdx == character.Count-1 || tf.position.x < character[crossIdx+1].transform.position.x)
				return;

			crossIdx ++;
		}
		else
			crossIdx--;

		if(crossIdx < 0 || chars[crossIdx] == " "[0])
		{
			col.enabled = false;
			t.onDie();
		}
	}

	public void enter(PlayerController t)
	{
		if(hasEntered)
			return;
		
		hasEntered = true;
		crossIdx = -1;
		int lastIdx = character.Count-1;
		Transform tf = t.transform;
		for(int i = 0; i < lastIdx; i++)
		{
			if(tf.position.x > character[i].transform.position.x && tf.position.x < character[i+1].transform.position.x)
			{
				crossIdx = i;
				break;
			}

		}

		if(crossIdx == -1)
			crossIdx = lastIdx;



	}

	public void leave()
	{
		hasEntered = false;
	}
}
