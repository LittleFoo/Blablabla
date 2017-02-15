//
// /********************************************************
// * 
// *　　　　　　Copyright (c) 2015  Feiyu
// *  
// * Author		: Binglei Gong</br>
// * Date		: 15-12-8下9:38</br>
// * Declare	: </br>
// * Version	: 1.0.0</br>
// * Summary	: create</br>
// *
// *
// *******************************************************/
using UnityEngine;
using System.Collections.Generic;
using LitJson;
using common;
public class Movieclip : MonoBehaviour, ITimerEvent
{
    // Update is called once per frame
    private int _deltaFrame = 4;
    public delegate void OnPlayComplete(Movieclip mc);
    public OnPlayComplete onPlayComplete;

    public int frameRate
    {
        set
        {
            _deltaFrame = (int)(60 / value);
        }

        get
        {
            return _deltaFrame;
        }
    }
    private int _curFrameCount = 0;
    private int _totalFrame = 0;
    public int totalFrame
    {
        get
        {
            return _totalFrame;
        }
    }
    private int _startCount = 0;
    private int _playTimes = 0;
    private int _curPlayTimes = 0;
    private Material _mainMaterial;
    private List<frameData> _tilingAndOffset;
    private GameObject _root;
    public GameObject root
    {
        get
        {
            return _root;
        }
    }
    void Awake()
    {
        _root = gameObject;
        _tilingAndOffset = new List<frameData>();

        //if (name == "mc")
        //{
        //    UnityEngine.TextAsset s = Resources.Load("skill/heal_json") as TextAsset;
        //    generateAni(JsonMapper.ToObject(s.text), Resources.Load<Texture2D>("skill/heal"), "heal");

        //    play(-1);
        //}
    }

    public void play(int loop = 1)
    {
        if (loop == -1)
            loop = int.MaxValue;

        _playTimes = loop;
        _curPlayTimes = 0;
        _startCount = Time.frameCount;
        TimerManager.instance.addEventListeners(this);
    }

    public void generateAni(JsonData aniData, Texture2D texture, string aniName)
    {
        _tilingAndOffset.Clear();

        //if (_root.GetComponent<MeshRenderer>().material == null)
        //    _root.GetComponent<MeshRenderer>().material = new Material();

        _mainMaterial = _root.GetComponent<MeshRenderer>().material;

        JsonData frames = aniData["mc"][aniName]["frames"];
        JsonData res = aniData["res"];
        Vector2 vec;
        string name;
        frameData d;
        _totalFrame = frames.Count;
        for (var i = 0; i < frames.Count; i++)
        {
            name = (string)frames[i]["res"];
            d = new frameData();
            _tilingAndOffset.Add(d);

            vec = new Vector2();
            vec.x = (int)res[name]["w"];
            vec.y = (int)res[name]["h"];
            d.size = vec;

            vec = new Vector2();
            vec.x = d.size.x / texture.width;
            vec.y = d.size.y / texture.height;
            d.tiling = vec;

            vec = new Vector2();
            vec.x = ((int)res[name]["x"]) * 1.0f / texture.width;
            vec.y = (texture.height - (int)res[name]["y"] - (int)res[name]["h"]) * 1.0f / texture.height;
            d.offset = vec;

            vec = new Vector2();
            vec.x = (int)frames[i]["x"];
            vec.y = (int)frames[i]["y"];
            d.pos = vec;

        }

        _mainMaterial.mainTexture = texture;
        _mainMaterial.mainTextureOffset = _tilingAndOffset[0].offset;
        _mainMaterial.mainTextureScale = _tilingAndOffset[0].tiling;
        _root.transform.localScale = new Vector3(_tilingAndOffset[0].size.x, _tilingAndOffset[0].size.y, 1);
        //print(_tilingAndOffset[1].tiling);
        //print(_tilingAndOffset[1].offset);
        //print(_tilingAndOffset[1].size);

    }

    public void stop()
    {
        TimerManager.instance.removeEventListeners(this);
    }

    public void onUpdate()
    {
        if ((Time.frameCount - _startCount) % _deltaFrame == 0)
        {
            _curFrameCount++;
            if (_curFrameCount >= _totalFrame)
            {
                _curFrameCount = _curFrameCount % _totalFrame;
                _curPlayTimes++;
                if (_curPlayTimes >= _playTimes)
                {
                    TimerManager.instance.removeEventListeners(this);
                    if (onPlayComplete != null)
                        onPlayComplete(this);
                    return;
                }
            }

            _mainMaterial.mainTextureOffset = _tilingAndOffset[_curFrameCount].offset;
            _mainMaterial.mainTextureScale = _tilingAndOffset[_curFrameCount].tiling;
            _root.transform.localScale = new Vector3(_tilingAndOffset[_curFrameCount].size.x, _tilingAndOffset[_curFrameCount].size.y, 1);
        }
    }
}

public class frameData
{
    public Vector2 tiling;
    public Vector2 offset;
    public Vector2 pos;
    public Vector2 size;

    public frameData()
    {
        tiling = new Vector2();
        pos = new Vector2();
        offset = new Vector2();
        size = new Vector2();
    }
}


