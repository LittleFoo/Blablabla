using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {
	public Transform tf;
	public SpriteRenderer spr;
	public Collider2D col;


	public void Eat()
	{
		ColorUtil.doFade(spr, 0, 0.5f);
		col.enabled = false;
	}
}
