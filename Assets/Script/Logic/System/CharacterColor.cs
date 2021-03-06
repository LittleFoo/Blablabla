﻿using UnityEngine;
using System.Collections.Generic;

public class CharacterColor : MonoBehaviour {
	public List<CharacterColorData> characterList = new List<CharacterColorData>();
	public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();
	public Dictionary<int, CharacterColorData> colorDic = new Dictionary<int, CharacterColorData>();
	private static CharacterColor _instance;
	public static CharacterColor instance
	{
		get{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<CharacterColor>();
				if(_instance == null)
					return null;
				for(int i = 0; i < _instance.characterList.Count; i++)
				{
					for(int j = 0; j < _instance.characterList[i].assicIds.Count; j++)
					{
						_instance.colorDic.Add(_instance.characterList[i].assicIds[j], _instance.characterList[i]);
					}
				}
			}
			return _instance;
		}
	}

	void Awake()
	{
		if(_instance == null)
		{
			_instance = this;

			for(int i = 0; i < characterList.Count; i++)
			{
				for(int j = 0; j < characterList[i].assicIds.Count; j++)
				{
					colorDic.Add(characterList[i].assicIds[j], characterList[i]);
				}
			}
		}
	}
}

[System.Serializable]
public class CharacterColorData
{
	public List<int> assicIds = new List<int>();
	public string character;
	public Color color = Color.white;
}
