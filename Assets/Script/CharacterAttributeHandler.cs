using UnityEngine;
using System.Collections.Generic;

public class CharacterAttributeHandler  {
	public static Dictionary< int, System.Action<PhysicalPlayerController, CharacterCell> > handlers;
	public static void init()
	{
	}

	public static void angleBracketLeftTrigger(PhysicalPlayerController tf)
	{
		tf.Rebound(GlobalController.instance.setting.angleBlanketReboundParam);
	}
}
