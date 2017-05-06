using UnityEngine;
using System.Collections;

public class SceneSetting : MonoBehaviour {
	public bool underWater;
	public Vector2 boundsX;
	public Vector2 boundsY;
	public bool isRunAtAwake = false;
	void Awake()
	{
		GlobalController.instance.curScene = this;

	}

	void Start()
	{
		if(isRunAtAwake)
			StartCoroutine(notice(0.1f));
	}


	IEnumerator notice(float delay)
	{
		yield return new WaitForSeconds(delay);
		AwakeManager.instance.notice();
	}

	public void trigger()
	{
		AwakeManager.instance.notice();
	}
}
