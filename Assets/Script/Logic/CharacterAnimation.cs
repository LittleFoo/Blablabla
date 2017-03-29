using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour {
	public Sprite[] walk;
	public Sprite[] idle;
	public Sprite[] jump;
	public Sprite[] jumpLoop;
	public Sprite[] idleRandom;
	public Sprite[] crash;
	public SpriteAnimation ani;
	public Config.CharcterAction lastAct;
	// Use this for initialization
	void Awake () {
		ani = gameObject.GetComponent<SpriteAnimation>();
		if(ani == null)
			ani = gameObject.AddComponent<SpriteAnimation>();
	}
	
	public void play(Config.CharcterAction act)
	{
		if(act == lastAct)
			return;
		lastAct = act;
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

		case Config.CharcterAction.JumpLoop:
			ani._spriteList = jumpLoop;
			break;

		case Config.CharcterAction.Crash:
			ani._spriteList = crash;
			break;

		}

		ani.play(-1);
	}

	public void doJump()
	{
		lastAct = Config.CharcterAction.Jump;
		ani._spriteList = jump;
		ani.play(1, false, (GameObject arg0) => {
			if(lastAct == Config.CharcterAction.Jump) 
				play(Config.CharcterAction.JumpLoop);
		});
	}

	public void doRandomIdle()
	{
		lastAct = Config.CharcterAction.IdleRandom;
		ani._spriteList = idleRandom;
		ani.play(1, false, (GameObject arg0) => {
			if(lastAct == Config.CharcterAction.IdleRandom) 
				play(Config.CharcterAction.Idle);
		});
	}

	public void copy(CharacterAnimation ani)
	{
		walk = (Sprite[])ani.walk.Clone();
		idle = (Sprite[])ani.idle.Clone();
		jump = (Sprite[])ani.jump.Clone();
		jumpLoop = (Sprite[])ani.jumpLoop.Clone();
		idleRandom = (Sprite[])ani.idleRandom.Clone();
		crash = (Sprite[])ani.crash.Clone();

	}
}

