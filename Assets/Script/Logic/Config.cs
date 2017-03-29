﻿using UnityEngine;
using System.Collections;

public class Config {
	public enum Direction{None, Top, Bottom, Left, Right}
	public enum CharcterAction{Walk, Idle, Jump}
	public enum ColliderAction{Movement, Rotation, Scale, Alpha}
	public enum Pivot{TopLeft, Top, TopRight, Left, Center, Right, BottomLeft, Bottom, BottomRight}
	public enum ActionTriggerType{Awake, onCollider}

	public const string TAG_GROUP = "CharacterGroup";
	public const string TAG_CHAR = "Character";
	public const string TAG_DANGER = "Danger";
}