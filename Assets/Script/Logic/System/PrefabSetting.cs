using UnityEngine;
using System.Collections;

public class PrefabSetting : MonoBehaviour {
//	private static PrefabSetting _instance;
//	public static PrefabSetting instance
//	{
//		get{return _instance;}
//		set{if(_instance != null) throw new System.Exception("PrefabSetting is a singleton");}
//	}
	
	public GameObject charPrefab0;
	public GameObject charPrefab1;
	public GameObject charPrefab2;
	public Transform bullet;
	public Transform bounceBullet;
	public Texture2D scrollUnitTexture;
	public void Awake()
	{
		DontDestroyOnLoad(this);
	}


}
