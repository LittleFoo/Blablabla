using UnityEngine;
using System.Collections.Generic;

public class AwakeManager : MonoBehaviour {
	private List<IAwake> listener = new List<IAwake>();
	private static AwakeManager _instance;
	public static AwakeManager instance
	{
		get
		{
			return _instance;
		}
	}

	void Awake()
	{
		_instance = this;
	}

	public void addEventListener(IAwake item)
	{
		if(listener.Contains(item))
			return;
		listener.Add(item);
	}

	public void notice()
	{
		for(int i = 0; i < listener.Count; i++)
		{
			listener[i].onAwake();
		}
	}
}

public interface IAwake
{
	void onAwake();
}
