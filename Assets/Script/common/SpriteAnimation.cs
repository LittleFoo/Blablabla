using UnityEngine;
using System.Collections;
using common;

[RequireComponent (typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour, ITimerEvent {
	public bool playOnStart = false;
	public Sprite[] _spriteList;
	private UnityEngine.Events.UnityAction<GameObject> _onComplete;
	private UnityEngine.Events.UnityAction _onEvent;
	private int _eventIdx;
	private SpriteRenderer _spr;
	public SpriteRenderer spr
	{
		get{return _spr;}
	}
	private int _targetLoop;
	private int _loopCount;
	private bool _backWards;
	private int _deltaFrame;
	public int frameRate = 8;
	private UnityEngine.Events.UnityAction _curUpdateCount;

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
	private bool _isScaled = true;
	public bool isScale
	{
		set{_isScaled = value;}
	}

	public void init(Sprite[] spriteList)
	{
		if(_spr == null)
			_spr = gameObject.GetComponent<SpriteRenderer>();
		_spriteList = spriteList;
	}

	public void play(int loop, 
	                 bool backwards = false,
						UnityEngine.Events.UnityAction<GameObject> onComplete = null, 
	                 int eventIdx = -1, 
					 UnityEngine.Events.UnityAction onEvent = null)
	{
		play(new Vector2(0, _spriteList.Length-1), loop, backwards, onComplete, eventIdx, onEvent);
	}
	
	public void play(Vector2 range, 
	                 int loop, 
	                 bool backwards = false,
		UnityEngine.Events.UnityAction<GameObject> onComplete = null, 
	                 int eventIdx = -1, 
					 UnityEngine.Events.UnityAction onEvent = null)
	{
		_curRange = range;
		_targetLoop = loop;
		_onComplete = onComplete;
		_eventIdx = eventIdx;
		_onEvent = onEvent;
		if(_spr == null)
			_spr = gameObject.GetComponent<SpriteRenderer>();

		if(backwards)
			_curUpdateCount = playBackwardsUpdate;
		else
			_curUpdateCount = playForwardUpdate;

		if (loop == -1)
			loop = int.MaxValue;
		_targetLoop = loop;
		_loopCount = 0;
		_startFrameCount = 0;
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
			_onComplete(gameObject);
	}

	public void Start()
	{
		if(playOnStart)
			play(new Vector2(0, _spriteList.Length-1), -1);
	}
	
	public void onUpdate()
	{
		if(_isScaled)
			_deltaFrame = (int)(Application.targetFrameRate/Time.timeScale/frameRate);
		else
			_deltaFrame = (int)(Application.targetFrameRate/frameRate);
		
		if(_deltaFrame <= 0)
			_deltaFrame = 1;

		_curUpdateCount();
	}

	private void playForwardUpdate()
	{
		_startFrameCount++;
		if( _startFrameCount%_deltaFrame == 0)
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
				_startFrameCount = 0;
			}
			curFrameIdx = _curFrameIdx;
			
			if(_eventIdx == _curFrameIdx && _onEvent != null)
				_onEvent();

		}
	}

	private void playBackwardsUpdate()
	{
		_startFrameCount++;
		if(_startFrameCount%_deltaFrame == 0)
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
				_startFrameCount = 0;
			}
			curFrameIdx = _curFrameIdx;
			
			if(_eventIdx == _curFrameIdx && _onEvent != null)
				_onEvent();
		}
	}

	void OnDestroy()
	{
		TimerManager.instance.removeEventListeners(this);
	}
}
