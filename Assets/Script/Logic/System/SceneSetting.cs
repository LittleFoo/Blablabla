using UnityEngine;
using System.Collections;

public class SceneSetting : MonoBehaviour {
	public bool underWater;
	public Vector2 boundsX;
	public Vector2 boundsY;
	void Awake()
	{
		GlobalController.instance.curScene = this;
	}


}
