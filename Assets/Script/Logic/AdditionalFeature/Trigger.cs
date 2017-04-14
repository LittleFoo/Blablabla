using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trigger : MonoBehaviour {
	public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();
	public List<TriggerData> triggerDataList = new List<TriggerData>();
	public Transform tf;
	private float centerY;
	private List<TriggerData> topTriggers = new List<TriggerData>();
	private List<TriggerData> bottomTriggers = new List<TriggerData>();
	// Use this for initialization
	void Awake () {
		tf = transform;
		Sprite spr = tf.GetComponent<SpriteRenderer>().sprite;
		centerY = spr.rect.height*(0.5f- spr.pivot.y);

		TriggerData d;
		for(int i = 0; i < triggerDataList.Count; i++)
		{
			d = triggerDataList[i];
			switch( d.triggerCondition)
			{
				case Config.TriggerCondition.OnCollider:
					topTriggers.Add(d);
					bottomTriggers.Add(d);
					break;
				case Config.TriggerCondition.OnTop:
					topTriggers.Add(d);
					break;
				case Config.TriggerCondition.OnBottom:	
					bottomTriggers.Add(d);
					break;
			}
		}
	}

	public void OnCollisionEnter2D(Collision2D coll)
	{
		float delta = coll.contacts[0].point.y-(tf.position.y+centerY);
		if(delta < 0)
		{
			if(topTriggers.Count > 0)
			{
				for(int i = 0; i < topTriggers.Count; i++)
				{
					trigger(topTriggers[i]);
				}
				topTriggers.Clear();
			}
		}
		else
		{
			if(bottomTriggers.Count > 0)
			{
				for(int i = 0; i < bottomTriggers.Count; i++)
				{
					trigger(bottomTriggers[i]);
				}
				bottomTriggers.Clear();
			}
		}
	}


	public void trigger(TriggerData d)
	{
		switch(d.triggerType)
		{
			case Config.TriggerType.Action:
				CharactersAction action = d.triggerObj.GetComponent<CharactersAction>();
				action.trigger(Config.ActionTriggerCondition.TriggerByOthers);
				break;
		}
	}
}


[System.Serializable]
public class TriggerData
{
	public Transform triggerObj;
	public Config.TriggerType triggerType;
	public Config.TriggerCondition triggerCondition;
	public bool isTriggered = false;
}