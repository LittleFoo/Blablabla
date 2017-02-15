using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using common;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectMask2D))]
public class UIScrollView : MonoBehaviour, ITimerEvent
{
	[HideInInspector][SerializeField] RectTransform _viewRect;
	public RectTransform viewRect{
		get{return _viewRect;}
		set{ 
			_viewRect = value;
			if(_width == 0)
				_width = _viewRect.GetComponent<RectTransform>().sizeDelta.x;

			if(_height == 0)
				_width = _viewRect.GetComponent<RectTransform>().sizeDelta.y;
		}

	}

	[HideInInspector][SerializeField] RectTransform _content;
	public RectTransform content
	{
		get{return _content;}
		set{
			_content = value;
						if(content == null)
							return;
						content.anchorMax = new Vector2 (0, 1);
						content.anchorMin = content.anchorMax;
						content.pivot = content.anchorMax;
		}
	}

	[HideInInspector][SerializeField] bool _isVertical = true;
	public bool isVertical {
		get{return _isVertical;}
		set{ _isVertical = value;}
	}

	[HideInInspector][SerializeField] bool _isHorizon = false;
	public bool isHorizon {
		get{return _isHorizon;}
		set{ _isHorizon = value;}
	}

	[HideInInspector][SerializeField] bool _lockDirection = false;
	public bool lockDirection {
		get{return _lockDirection;}
		set{ _lockDirection = value;}
	}


	[HideInInspector][SerializeField] float _height;
	public float height {
		get{return _height;}
		set {
			_height = value;
			if(_viewRect != null)
				_viewRect.sizeDelta = new Vector2 (_width, _height);

		}
	}

	[HideInInspector][SerializeField] float _width;
	public float width {
		get{return _width;}
		set {
			_width = value;
			if(_viewRect != null)
				_viewRect.sizeDelta = new Vector2 (_width, _height);
		}
	}

	[HideInInspector][SerializeField] float _contentHeight;
	public float contentHeight {
		get{return _contentHeight;}
		set{ _contentHeight = value;
			if(_content != null)
				_content.sizeDelta = new Vector2(content.sizeDelta.x, _contentHeight);}
	}

	[HideInInspector][SerializeField] float _contentWidth;
	public float contentWidth {
		get{return _contentWidth;}
		set{ 
			_contentWidth = value;
			if(_content != null)
				_content.sizeDelta = new Vector2(_contentWidth, content.sizeDelta.y);
		}
	}

	[HideInInspector][SerializeField] float _disFromTop;
	public float disFromTop {
		get{return _disFromTop;}
		set {
			_disFromTop = value;
			if(_disFromTop < 0)
				_disFromTop = 0;
			if (_disFromTop + _height > _contentHeight)
			{
				_disFromTop = _contentHeight - _height;
				if (_disFromTop < 0)
					_disFromTop = 0;
			}
			if(content == null)
				return;
				content.anchoredPosition = new Vector2 (content.anchoredPosition.x, _disFromTop);
		}
	}

	[HideInInspector][SerializeField] float _disFromLeft;
	public float disFromLeft {
		get{return _disFromLeft;}
		set {
			_disFromLeft = value;
			if (_disFromLeft + _width > _contentWidth)
			{
				_disFromLeft = _contentWidth - _width;
				if (_disFromLeft < 0)
					_disFromLeft = 0;
			}
			if(content == null)
				return;
			content.anchoredPosition = new Vector2 (_disFromLeft, -content.anchoredPosition.y);
		}
	}


//		[HideInInspector][SerializeField] bool _fromTop = true;
//		public bool fromTop {
//			get{return _fromTop;}
//			set {
//				_fromTop = value;
//			}
//		}
//	
//		[HideInInspector][SerializeField] bool _fromLeft = true;
//		public bool fromLeft {
//			get{return _fromLeft;}
//			set {
//				_fromLeft = value;
//			}
//		}

	private EventTriggerListener _listener;
	private int _lastDragId;
	private Vector2 _lastSpeed;
	private Vector2 _target;
	public float inerTime = 0.5f; 
	void Awake()
	{
		_listener= EventTriggerListener.Get( viewRect.gameObject);
		_listener.onDown = onDown;
		_listener.onUp = onUp;
		_listener.onExit = onUp;
	}

	void Start()
	{
		gameObject.GetComponent<RectMask2D>().enabled = true;
	}

	public void onDown(GameObject obj, PointerEventData d)
	{
		TimerManager.instance.removeEventListeners(this);
		_listener.onDrag = onDrag;
	}

	public void onDrag(GameObject obj, PointerEventData d)
	{
		float angle = Vector2.Angle(Vector2.right, d.delta);
		if(_lockDirection )
		{
			if((angle < 40 || angle > 140))
			{
				if(_isHorizon)
					scrollX(d.delta.x);
			}
			else
			{
				if(_isVertical)
					scrollY(d.delta.y);
			}
		}
		else
		{
			if(_isHorizon)
				scrollX(d.delta.x);
			if(_isVertical)
				scrollY(d.delta.y);
		}
		_lastDragId = d.pointerId;
		_lastSpeed= d.delta/Time.deltaTime;


	}

	public void onUp(GameObject obj, PointerEventData d)
	{
		if(d.pointerId != _lastDragId || _listener.onDrag == null)
			return;

		_listener.onDrag = null;
		_target = _lastSpeed*inerTime + content.anchoredPosition;

		TimerManager.instance.addEventListeners(this);

	}

	public void onUpdate()
	{
		float newPosition;
		if(_isHorizon)
		{
		 	newPosition = Mathf.SmoothDamp(content.anchoredPosition.x, _target.x, ref _lastSpeed.x, inerTime);
			scrollX(newPosition- content.anchoredPosition.x);
			if(Mathf.Abs(_target.x- content.anchoredPosition.x) < Mathf.Abs(_lastSpeed.x*Time.deltaTime))
			{
				TimerManager.instance.removeEventListeners(this);
			}
		}
		if(_isVertical)
		{
			newPosition = Mathf.SmoothDamp(content.anchoredPosition.y, _target.y, ref _lastSpeed.y, inerTime);
			scrollY(newPosition- content.anchoredPosition.y);
			if(Mathf.Abs(_target.y- content.anchoredPosition.y) < Mathf.Abs(_lastSpeed.y*Time.deltaTime))
			{
				TimerManager.instance.removeEventListeners(this);
			}
		}
	}

	public void scrollX(float deltaX)
	{
		disFromLeft = disFromLeft-deltaX;
	}

	public void scrollY(float deltaY)
	{
		disFromTop = disFromTop+deltaY;
	}

	public void scrollToTop()
	{
		TimerManager.instance.removeEventListeners(this);
			disFromTop = 0;
	}

	public void scrollToBottle()
	{
		TimerManager.instance.removeEventListeners(this);
			disFromTop = _contentHeight - _height;
	}

	void OnDestroy()
	{
		TimerManager.instance.removeEventListeners(this);
	}
}
