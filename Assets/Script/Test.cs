using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

	// Use this for initialization
	public string contentStr;
	public FontAnalyse analyse;
	public BoxCollider2D col;
	private List<CharacterCell> character = new List<CharacterCell>();
	private List<List<int>> blocks = new List<List<int>>(500);
	private bool hasEntered = false;
	private int crossIdx;
	private char[] chars ;
	void Start () {
		GameObject obj;
		SpriteRenderer spr;
		FontData d = new FontData();
		CharacterCell cell;
		int x = 0;
		int lastIdx = 0;
		chars = contentStr.ToCharArray();
		for(int i = 0; i < chars.Length; i++)
		{
			obj = new GameObject();
			cell = new CharacterCell();
			spr = obj.AddComponent<SpriteRenderer>();
			spr.color = Color.blue;
			spr.sortingOrder = 1;
			obj.transform.SetParent(transform, false);
			obj.name = chars[i].ToString();


			if(analyse.fontDatas.TryGetValue( System.Convert.ToInt32(chars[i]), out d))
			{
				obj.transform.localPosition = new Vector2(x, -d._actualOffsetY);
				//gap
//				for(int j = lastIdx; j < x; j++)
//				{
//					blocks.Add(new List<int>(analyse.lineHeight));
//					for(int k = 0; k < analyse.lineHeight; k++)
//					{
//						blocks[j].Add(0);
//					}
//				}
//
//				lastIdx = x+d.width;
//				for(int j = 0; j < d.width; j++)
//				{
//					blocks.Add(new List<int>(analyse.lineHeight));
//
//					for(int k = 0; k < d.pixelArray[j].Count; k++)
//					{
//						blocks[j].Add(d.pixelArray[j][k]);
//					}
//				}

				x += d.xadvance;
			}
			spr.sprite = d.spr;

			cell.tf = obj.transform;
			cell.fontData = d;
			character.Add(cell);
		}

//		System.Text.StringBuilder sb = new System.Text.StringBuilder();
//		for(int j = 35; j > 0; j--)
//		{
//			for(int i = 0; i < blocks.Count; i++)
//			{
//				if(j>=blocks[i].Count)
//					continue;
//				sb.Append(blocks[i][j].ToString());
//			}
//			sb.Append("\n");
//		}
//		print(sb.ToString());
		col.size = new Vector2(x + d.width, analyse.lineHeight);
		col.offset = new Vector2(col.size.x*0.5f, - col.size.y*0.5f);
	}

	public void check(PlayerController t)
	{
		if(crossIdx < 0 || crossIdx >= character.Count)
			return;
		Transform tf = t.transform;
		if(tf.position.x > character[crossIdx].tf.position.x + character[crossIdx].fontData.width)
		{
			if(crossIdx != character.Count-1 && tf.position.x >= character[crossIdx+1].tf.position.x)
			{
				character[crossIdx].enter = false;
				crossIdx ++;
			}
		}
		else if(tf.position.x < character[crossIdx].tf.position.x)
		{
			character[crossIdx].enter = false;
			crossIdx--;
		}
//		if(tf.position.y < character[crossIdx].position.y)
//		{
//			t.onTouch(col, false);
//		}
//		else
//		{
//			t.onTouch(col, true);
//		}

		if(crossIdx < 0 || (chars[crossIdx] == " "[0] && tf.position.y < character[crossIdx].tf.position.y))
		{
//			col.enabled = false;
			t.onDrop(col);
			return;
		}
		else if(t.speedy < 0)
		{
			if(tf.position.y <=  character[crossIdx].tf.position.y)
			{
				if(!character[crossIdx].enter)
				{
					character[crossIdx].enter = true;
					tf.position =  new Vector2(tf.position.x, character[crossIdx].tf.position.y);
					t.onTouch(col, Config.Direction.Bottom);
				}
			}
			else
				character[crossIdx].enter = false;
		}
		else if(t.speedy > 0)
		{
			if(tf.position.y + t.height >=  character[crossIdx].tf.position.y - character[crossIdx].fontData.height && tf.position.y + t.height < character[crossIdx].tf.position.y)
			{
				if(!character[crossIdx].enter)
				{
					t.onTouch(col, Config.Direction.Top);
					character[crossIdx].enter = true;
				}
			}
			else
				character[crossIdx].enter = false;
		}
		else
		{
			if(tf.position.y > character[crossIdx].tf.position.y)
			{
				character[crossIdx].enter = false;
 				t.onDrop(col);
			}
			else
				character[crossIdx].enter = true;
		}
	}

	public bool allowToMove(PlayerController t, float deltaX)
	{
		if(crossIdx < 0)
			return true;
		Transform tf = t.transform;
		bool isAllowToMove = true;
		int nextIdx;
		if(deltaX < 0)
		{
			if(tf.position.x + deltaX - t.halfWidth>= character[crossIdx].tf.position.x)
				return isAllowToMove;
			nextIdx = crossIdx -1;
			if(nextIdx < 0)
				return isAllowToMove;
			if(character[nextIdx].tf.position.y - tf.position.y > 0)
				isAllowToMove = false;
		}
		else if(deltaX > 0)
		{
			if(tf.position.x + deltaX + t.halfWidth<= character[crossIdx].tf.position.x + character[crossIdx].fontData.width)
				return isAllowToMove;
			nextIdx = crossIdx+1;
			if(nextIdx >= character.Count)
				return isAllowToMove;
			if(character[nextIdx].tf.position.y - tf.position.y > 0)
				isAllowToMove = false;
		}

		return isAllowToMove;
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
			if(tf.position.x > character[i].tf.position.x && tf.position.x < character[i+1].tf.position.x)
			{
				crossIdx = i;
				break;
			}

		}
		if(crossIdx == -1)
		{
			leave();
			return;
		}

		check(t);
	}

	public void leave()
	{
		hasEntered = false;
	}
}

public class CharacterCell
{
	public FontData fontData;
	public Transform tf;
	public bool enter;
}