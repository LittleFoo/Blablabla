using UnityEngine;
using System.Collections;

public class AppLoaderModule  {

	private static AppLoaderModule _instance;
	public static AppLoaderModule instance
	{
		get{
			if(_instance == null)
				_instance = new AppLoaderModule();
			return _instance;
		}
	}

	public const string loadingScene = "Loading";

	private string _sceneToLoad;
	public string sceneToLoad{get{return _sceneToLoad;}}
	private bool _isLoading = false;

	public AppLoaderModule()
	{
		if(_instance != null)
			throw new System.Exception("apploadermodule is a singleton");
	}

	public void loadScene(string name)
	{
		if(_isLoading)
			return;

		_sceneToLoad = name;
		_isLoading = true;
		Application.LoadLevel(loadingScene);
	}

	public void onSceneLoaded()
	{
		_isLoading = false;
	}

	public string getCurScene()
	{
		return _sceneToLoad;
	}
}
