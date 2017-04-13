using UnityEngine;
using System.Collections;
using PathologicalGames;

public class GlobalController : MonoBehaviour {

	public PrefabSetting prefabSetting;
	public Setting setting;
	public SceneSetting curScene;
	private SpawnPool _pool;

	private static GlobalController _instance;
	public static GlobalController instance
	{
		get{
			if(_instance == null)
			{
				_instance = FindObjectOfType<GlobalController>();
				_instance.init(); 
			}
			return _instance;
		}
	}


	void Awake () {
		if(_instance != null) 
			return;
		_instance = this;
		init();
	}

	void init()
	{
		Application.targetFrameRate = 60;
		setting.init();
	}
	
	// Update is called once per frame
	void Update () {
		common.TimerManager.instance.Update();
	}

	public PathologicalGames.SpawnPool getCurPool()
	{
		if(_pool != null)
			return _pool;

		_pool = gameObject.AddComponent<SpawnPool>();
		PrefabPool pp = new PrefabPool(GlobalController.instance.prefabSetting.bullet);
		_pool.CreatePrefabPool(pp);
		pp.preloadAmount = 10;
		pp.limitInstances = false;
		return _pool;
	}

	void OnDestroy()
	{
		if(_instance == this)
			_instance = null;
	}
}
