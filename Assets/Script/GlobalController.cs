using UnityEngine;
using System.Collections;

public class GlobalController : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update () {
		common.TimerManager.instance.Update();
	}
}
