using UnityEngine;
using System.Collections;

public class Config {
	public enum Direction{None, Top, Bottom, Left, Right}
	public enum CharcterAction{Walk, Idle, IdleRandom, Jump, JumpLoop, Crash, squat}
	public enum ColliderAction{Movement, Rotation, Scale, Alpha}
	public enum Pivot{TopLeft, Top, TopRight, Left, Center, Right, BottomLeft, Bottom, BottomRight}
	public enum ActionTriggerCondition{Awake, OnCollider, OnBottom, OnTop, TriggerByOthers = 98}
	public enum ReactionType{Null, Normal, Ice, Scroll}
	public enum TriggerType{Action}
	public const string TAG_PLAYER = "Player";
	public const string TAG_GROUP = "CharacterGroup";
	public const string TAG_CHAR = "Character";
	public const string TAG_DANGER = "Danger";
	public const string TAG_SCROLL = "Scroll";
	public const string TAG_COIN = "Coin";
	public const string TAG_MST = "Monster";

	public static Color BLUE = new Color(0, 239/255.0f, 221/255.0f);
	public static Color ORANGE = new Color(248/255.0f, 183/255.0f, 3/255.0f);
	public static Color RED = new Color(242/255.0f, 60/255.0f, 0);

	public static Color[] scrollColors = new Color[]{BLUE, ORANGE, RED};
}
