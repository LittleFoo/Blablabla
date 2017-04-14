using UnityEngine;
using System.Collections.Generic;

public class DistanceTriggerManager : MonoBehaviour {

	public List<IDistanceTrigger> triggers = new List<IDistanceTrigger>();
	private static DistanceTriggerManager _instance;
	public static DistanceTriggerManager instance
	{
		get{
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<DistanceTriggerManager>();

			return _instance;
		}
	}

	private float xOffset;
	private float yOffset;

	public void Start()
	{
		xOffset = GlobalController.instance.setting.screenWidth*0.5f;
		yOffset = GlobalController.instance.setting.screenHeight*0.5f;
		sort();
	}

	public void addEventListener(IDistanceTrigger item)
	{
		int idx = triggers.IndexOf(item);
		if(idx == -1)
		{
			triggers.Add(item);
		}
	}

	public void removeEventListener(IDistanceTrigger item)
	{
		int idx = triggers.IndexOf(item);
		if(idx != -1)
			triggers.RemoveAt(idx);
	}

	public void notice(Vector3 playerPos)
	{
		Vector3 pos;
		float minX = playerPos.x - xOffset, maxX = playerPos.x + xOffset,minY = playerPos.y - yOffset, maxY = playerPos.y + yOffset;

		for(int i = triggers.Count-1; i > -1; i--)
		{
			pos = triggers[i].getPosition();
			if(pos.x > minX &&  pos.x < maxX && pos.y < maxY + yOffset && pos.y >minY)
			{
				triggers[i].trigger();
				triggers.RemoveAt(i);
			}
		}
	}

	private void sort()
	{
		triggers.Sort(compare);
	}

	private static int compare(IDistanceTrigger obj1, IDistanceTrigger obj2)
	{
		if(obj1.getPosition().x < obj2.getPosition().x)
			return -1;
		else 
			return 1;
	}


}
	
public interface IDistanceTrigger
{
	void trigger();
	Vector3 getPosition();
}
