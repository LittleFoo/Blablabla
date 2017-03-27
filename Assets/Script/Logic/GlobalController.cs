using UnityEngine;
using System.Collections;
using PathologicalGames;

public class GlobalController : MonoBehaviour {

	public PrefabSetting prefabSetting;
	public Setting setting;
	private SpawnPool _pool;

	private static GlobalController _instance;
	public static GlobalController instance
	{
		get{
			if(_instance == null)
				_instance = FindObjectOfType<GlobalController>();
			return _instance;
		}
	}


	void Awake () {
		if(_instance != null) 
			throw new System.Exception("GlobalController is a singleton");
		
		_instance = this;

		Application.targetFrameRate = 60;

		setting.init();

		float screenWidth = Mathf.Ceil( setting.gridSize*setting.gridNum/9.0f*16);
		screenWidth = Mathf.Ceil(screenWidth*1.0f/16)*16;
		float screenHeight = screenWidth/16*9;
		Camera.main.orthographicSize = screenHeight*0.5f;
		print("screenW:"+screenWidth.ToString() +",screenHeight:"+screenHeight.ToString());
//		CharacterCell.initHandler();
	}
	
	// Update is called once per frame
	void Update () {
		common.TimerManager.instance.Update();
	}

	public PathologicalGames.SpawnPool getCurPool()
	{
		SpawnPool _pool;
		if(!PathologicalGames.PoolManager.Pools.TryGetValue("game", out _pool))
		{
			_pool = PathologicalGames.PoolManager.Pools.Create("game", gameObject);

			PrefabPool pPool = new PrefabPool(prefabSetting.charPrefab0.transform);
			pPool.preloadAmount = 5;
			_pool.CreatePrefabPool(pPool);

			pPool = new PrefabPool(prefabSetting.charPrefab1.transform);
			pPool.preloadAmount = 40;
			_pool.CreatePrefabPool(pPool);

			pPool = new PrefabPool(prefabSetting.charPrefab2.transform);
			pPool.preloadAmount = 20;
			_pool.CreatePrefabPool(pPool);
		}
		return _pool;
	}
}
