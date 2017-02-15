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
using UnityEngine.UI;
using System.Collections;
using common;
using System.Collections.Generic;
using LitJson;

public class ImageAni : MonoBehaviour, ITimerEvent
{
    [HideInInspector][SerializeField] TextAsset _aniJson;
	public TextAsset aniJson
    {
        get{return _aniJson;}
        set{_aniJson = value;}
    }
    [HideInInspector][SerializeField] Texture _aniTexture;
	public Texture aniTexture
        {
        get{return _aniTexture;}
        set{
			_aniTexture = value;}
        }
    [HideInInspector][SerializeField] string _aniName;
	public string aniName
    {
        get{return _aniName;}
        set{_aniName = value;} 
    }
    [HideInInspector][SerializeField] bool _hideOnComplete;
    public bool hideOnComplete
    {
        get{return _hideOnComplete;}
        set{_hideOnComplete = value;}
    }
	[HideInInspector][SerializeField] bool _playOnStart = false;
	public bool playOnStart
	{
		get{return _playOnStart;}
		set{_playOnStart = value;}
	}
	[HideInInspector][SerializeField] bool _positionInfluence = true;
	public bool positionInfluence
	{
		get{return _positionInfluence;}
		set{_positionInfluence = value;}
	}
    [HideInInspector][SerializeField] Vector2 _anchoredPosition;
    public Vector2 anchoredPosition
    {
        get{return _anchoredPosition;}
        set{_anchoredPosition = value;}
    }

    private int _deltaFrame = 4;
    public delegate void OnPlayComplete(ImageAni mc);
    public OnPlayComplete onPlayComplete;
    public RawImage rawImg
    {
        get
        {
            RawImage img = root.GetComponent<RawImage>();
            if(img == null) 
                return root.AddComponent<RawImage>();
            return img;
        }
    }

	private bool _isPlaying;
	public bool isPlaying
	{
		get{return _isPlaying;}
	}
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
    private int _curFrameCount ;
    public int curFrameCount
    {
        set
        {
            if(value >= _tilingAndOffset.Count)
            {
                return;
            }

            _curFrameCount = value;
            RawImage raw = root.GetComponent<RawImage>();
            raw.uvRect = new Rect(_tilingAndOffset[_curFrameCount].offset.x,
                                  _tilingAndOffset[_curFrameCount].offset.y,
                                  _tilingAndOffset[_curFrameCount].tiling.x,
                                  _tilingAndOffset[_curFrameCount].tiling.y);
            root.GetComponent<RectTransform>().sizeDelta = new Vector2(_tilingAndOffset[_curFrameCount].size.x, _tilingAndOffset[_curFrameCount].size.y);
			if(_positionInfluence)
            	root.GetComponent<RectTransform>().anchoredPosition = _tilingAndOffset[_curFrameCount].pos + _anchoredPosition;
        }
        get{return _curFrameCount;}
    }
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
    private int _curPlayTimes;
    private List<frameData> _tilingAndOffset = new List<frameData>();
    private GameObject _root;
    public GameObject root
    {
        get
        {
            if(_root == null)
                _root = gameObject;
            return _root;
        }
    }

	void Awake()
	{
        root.GetComponent<RectTransform>().pivot = Vector2.up;
		gererateAni();
        if(hideOnComplete)
            ColorUtil.toAlpha(rawImg, 0);
	}

	void Start()
	{
		if(_playOnStart)
			play(-1);
	}

	void OnDestroy()
	{
		TimerManager.instance.removeEventListeners(this);
	}

    public ImageAni play(int loop = 1)
    {
        if (loop == -1)
            loop = int.MaxValue;
        _playTimes = loop;
        _curPlayTimes = 0;
        _startCount = Time.frameCount;
		if(!_isPlaying)
        {
            TimerManager.instance.addEventListeners(this);
            curFrameCount = 0;
        }
		else
		    _isPlaying = true;

        ColorUtil.toAlpha(rawImg, 1);
		return this;
    }

	public void gererateAni()
	{
		if(aniJson != null && aniTexture != null && _aniName != null)
			generateAni(JsonMapper.ToObject( aniJson.text), aniTexture, aniName);
	}

    public void generateAni(JsonData aniData, Texture texture, string aniName)
    {
        _tilingAndOffset.Clear();

        //if (root.GetComponent<MeshRenderer>().material == null)
        //    root.GetComponent<MeshRenderer>().material = new Material();

        JsonData frames = aniData["mc"][aniName]["frames"];
        JsonData res = aniData["res"];
        Vector2 vec;
        string name;
        frameData d;
        _totalFrame = frames.Count;
		frameRate = (int)aniData["mc"][aniName]["frameRate"];
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
            vec.y = -(int)frames[i]["y"];
            d.pos = vec;

        }

        curFrameCount = 0;
//        RawImage raw = root.GetComponent<RawImage>();
//        raw.uvRect = new Rect(_tilingAndOffset[_curFrameCount].offset.x,
//            _tilingAndOffset[_curFrameCount].offset.y,
//            _tilingAndOffset[_curFrameCount].tiling.x,
//            _tilingAndOffset[_curFrameCount].tiling.y);
//        root.GetComponent<RectTransform>().sizeDelta = new Vector2(_tilingAndOffset[_curFrameCount].size.x, _tilingAndOffset[_curFrameCount].size.y);
//        root.GetComponent<RectTransform>().anchoredPosition = _tilingAndOffset[_curFrameCount].pos + anchoredPos;
        root.GetComponent<RawImage>().texture = texture;
    }

    public void stop()
    {
		_isPlaying = false;
        TimerManager.instance.removeEventListeners(this);
        if(hideOnComplete)
            ColorUtil.toAlpha(rawImg, 0);
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
                    stop();
                    if (onPlayComplete != null)
                        onPlayComplete(this);
                    return;
                }
            }

            curFrameCount = _curFrameCount;
        }
    }
}
