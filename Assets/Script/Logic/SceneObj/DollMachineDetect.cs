using UnityEngine;
using System.Collections.Generic;

public class DollMachineDetect : MonoBehaviour {

	public  List<Transform> catchList = new List<Transform>();

	void OnTriggerEnter2D(Collider2D other)
	{
		Transform obj = other.transform;
		catchList.Add(obj);

		obj.SetParent(transform);
		obj.GetComponent<Rigidbody2D>().gravityScale = 0;
	}


}
