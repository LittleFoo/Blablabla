using UnityEngine;
using System.Collections;

public class TriggerHandler : MonoBehaviour {
	public System.Action handler;
	public void Trigger()
	{
		if(handler != null)
			handler();
	}
}
