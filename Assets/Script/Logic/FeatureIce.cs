using UnityEngine;
using System.Collections;

public class FeatureIce : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other) 
	{
		GameObject obj = other.gameObject;
		switch(obj.tag)
		{
		case Config.TAG_Player:
			PhysicalPlayerController p =obj.GetComponent<PhysicalPlayerController>();
			p.onIce();
			break;
		}
	}

	public void OnTriggerExit2D(Collider2D coll)
	{
		GameObject obj = coll.gameObject;
		switch(obj.tag)
		{
		case Config.TAG_Player:
			PhysicalPlayerController p =obj.GetComponent<PhysicalPlayerController>();
			p.leaveIce();
			break;
		}
	}
}
