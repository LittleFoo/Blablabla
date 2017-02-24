using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour {
	public Sprite[] walk;
	public Sprite[] idle;
	public Sprite[] jump;
	public SpriteAnimation ani;
	// Use this for initialization
	void Awake () {
		if(ani == null)
			ani = gameObject.AddComponent<SpriteAnimation>();
	}
	
	public void play(Config.CharcterAction act)
	{
		switch(act)
		{
		case Config.CharcterAction.Idle:
			ani._spriteList = idle;
			break;

		case Config.CharcterAction.Jump:
			ani._spriteList = jump;
			break;

		case Config.CharcterAction.Walk:
			ani._spriteList = walk;
			break;
		}

		ani.play(-1);
	}
}

