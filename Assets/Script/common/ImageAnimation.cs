using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using common;

[RequireComponent (typeof(Image))]
public class ImageAnimation : MonoBehaviour, ITimerEvent {
	public bool playOnStart = false;
	public Sprite[] _spriteList;
	private callback0 _onComplete;
	private callback1<int> _onEvent;
	private int _eventIdx;
	private Image _spr;
	public Image spr
	{
		get{
			if(_spr == null)
				_spr = gameObject.GetComponent<Image>();
			return _spr;}
	}
	private int _targetLoop;
	private int _loopCount;
	private bool _backWards;
	private int _deltaFrame;
	public int frameRate = 8;
	private callback0 _curUpdateCount;

	private int _curScale;
	
	private int _startFrameCount;
	private bool _isPlaying;
	private int _curFrameIdx;
	public int curFrameIdx
	{
		set
		{
			_curFrameIdx = value;
			_spr.sprite = _spriteList[_curFrameIdx];
		}
	}
	
	private Vector2 _curRange;

	public void init(Sprite[] spriteList)
	{
		if(_spr == null)
			_spr = gameObject.GetComponent<Image>();
		_spriteList = spriteList;
	}

	public void play(int loop, 
	                 bool backwards = false,
	                 callback0 onComplete = null, 
	                 int eventIdx = -1, 
	                 callback1<int> onEvent = null)
	{
		play(new Vector2(0, _spriteList.Length-1), loop, backwards, onComplete, eventIdx, onEvent);
	}
	
	public void play(Vector2 range, 
	                 int loop, 
	                 bool backwards = false,
	                 callback0 onComplete = null, 
	                 int eventIdx = -1, 
	                 callback1<int> onEvent = null)
	{
		_curRange = range;
		_targetLoop = loop;
		_onComplete = onComplete;
		_eventIdx = eventIdx;
		_onEvent = onEvent;
		if(_spr == null)
			_spr = gameObject.GetComponent<Image>();

		if(backwards)
			_curUpdateCount = playBackwardsUpdate;
		else
			_curUpdateCount = playForwardUpdate;

		if (loop == -1)
			loop = int.MaxValue;
		_targetLoop = loop;
		_loopCount = 0;
		_startFrameCount = Time.frameCount;
		curFrameIdx = (int)_curRange.x;
		
		if(!_isPlaying)
		{
			TimerManager.instance.addEventListeners(this);
		}
		else
			_isPlaying = true;
	}
	
	public void stop(bool callBack = false)
	{
		TimerManager.instance.removeEventListeners(this);
		_isPlaying = false;
		if(callBack && _onComplete != null)
			_onComplete();
	}

	public void Start()
	{
		if(playOnStart)
			play(new Vector2(0, _spriteList.Length-1), -1);
	}
	
	public void onUpdate()
	{
		_deltaFrame = (int)(Application.targetFrameRate/Time.timeScale/frameRate);
		if(_deltaFrame <= 0)
			_deltaFrame = 1;
		_curUpdateCount();
	}

	private void playForwardUpdate()
	{
		if((Time.frameCount - _startFrameCount)%_deltaFrame == 0)
		{
			_curFrameIdx++;
			if(_curFrameIdx > _curRange.y)
			{
				_loopCount++;
				if(_loopCount >= _targetLoop)
				{
					stop(true);
					return;
				}
				_curFrameIdx =  (int)_curRange.x;
				_startFrameCount = Time.frameCount;
			}
			curFrameIdx = _curFrameIdx;
			
			if(_eventIdx == _curFrameIdx && _onEvent != null)
				_onEvent(_eventIdx);
		}
			

	}

	private void playBackwardsUpdate()
	{
		if((Time.frameCount - _startFrameCount)%_deltaFrame == 0)
		{
			_curFrameIdx--;
			if(_curFrameIdx < _curRange.x)
			{
				_loopCount++;
				if(_loopCount >= _targetLoop)
				{
					stop(true);
					return;
				}
				_curFrameIdx =  (int)_curRange.y;
				_startFrameCount = Time.frameCount;
			}
			curFrameIdx = _curFrameIdx;
			
			if(_eventIdx == _curFrameIdx && _onEvent != null)
				_onEvent(_eventIdx);
		}

	}

	void OnDestroy()
	{
		TimerManager.instance.removeEventListeners(this);
	}
}
