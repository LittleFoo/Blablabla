using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using common;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIButton : MonoBehaviour ,IPointerDownHandler, IPointerUpHandler{

	public Texture2D normal;
    [SerializeField] public Color normalColor;
	public Texture2D down;
    [SerializeField] public Color downColor;

	private RawImage _buttonImg;
    public RawImage buttonImg
    {
        get{return _buttonImg;}
    }
	public GameObject go;

	void Awake()
	{
		go = gameObject;
		_buttonImg = go.GetComponent<RawImage>();
		if(_buttonImg == null)
			_buttonImg = go.AddComponent<RawImage>();
		OnPointerUp(null);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_buttonImg.texture = (down == null)?normal:down;
        _buttonImg.color = downColor;
		go.GetComponent<RectTransform>().sizeDelta = new Vector2(_buttonImg.texture.width, _buttonImg.texture.height);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_buttonImg.texture = normal;
        _buttonImg.color = normalColor;
		go.GetComponent<RectTransform>().sizeDelta = new Vector2(_buttonImg.texture.width, _buttonImg.texture.height);
	}

    public bool btnEnable
    {
        set{_buttonImg.raycastTarget = value;}
    }
}
