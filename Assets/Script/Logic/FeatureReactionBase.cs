using UnityEngine;
using System.Collections;

public class FeatureReactionBase : MonoBehaviour {
	protected Config.ReactionType lastTriggerType = Config.ReactionType.Null;
	public virtual void onIce()
	{
		
	}

	public virtual void onScroll(Collision2D coll, FeatureScroll scroll)
	{
		
	}

	public virtual void leaveScroll(Collision2D coll, FeatureScroll scroll)
	{

	}

	public virtual void leaveIce()
	{
		
	}
}
