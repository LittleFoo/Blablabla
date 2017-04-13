using UnityEngine;
using System.Collections;

public class SceneSetting : MonoBehaviour {
	public bool underWater;

	void Awake()
	{
		GlobalController.instance.curScene = this;
	}
}
