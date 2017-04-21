using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterAnimation : MonoBehaviour {
	public Sprite[] walk;
	public Sprite[] idle;
	public Sprite[] jump;
	public Sprite[] jumpLoop;
	public Sprite[] idleRandom;
	public Sprite[] crash;
	public Sprite[] rush;
	public Sprite[] attack;
	public Sprite[] attack1;
	public Sprite[] attack2;
	public Sprite[] attack3;
	public Sprite[] attack4;

	public SpriteAnimation ani;
	public Dictionary<Config.CharcterAction, Sprite[]> actions = new Dictionary<Config.CharcterAction, Sprite[]>();
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
		Sprite[] sprs;
		if(actions.TryGetValue(act, out sprs))
			ani._spriteList = sprs;
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

	public void addSprToDic()
	{
		actions.Add(Config.CharcterAction.Walk, walk);
		actions.Add(Config.CharcterAction.Idle, idle);
		actions.Add(Config.CharcterAction.Jump, jump);
		actions.Add(Config.CharcterAction.JumpLoop, jumpLoop);
		actions.Add(Config.CharcterAction.IdleRandom, idleRandom);
		actions.Add(Config.CharcterAction.Crash, crash);
		actions.Add(Config.CharcterAction.Rush, rush);
		actions.Add(Config.CharcterAction.Attack, attack);
		actions.Add(Config.CharcterAction.Attack1, attack1);
		actions.Add(Config.CharcterAction.Attack2, attack2);
		actions.Add(Config.CharcterAction.Attack3, attack3);
		actions.Add(Config.CharcterAction.Attack4, attack4);
	}
}

