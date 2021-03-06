﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using DG.Tweening;

public class InputCorrect : MonoBehaviour, IDistanceTrigger, common.ITimerEvent {
	public CharacterGroup cg;
	public InputField txt;
	public float maxWidth;
	public SpriteRenderer shiny;
	public GameObject bg;
	private string lastContent;
	private Tween _curTween;
	private RectTransform txtRect;

	void Start()
	{
		FontData d;
		if(cg.analyse.fontDatas.TryGetValue(124, out d))
		{
			shiny.sprite = d.spr;
		}

		shiny.gameObject.SetActive(false);
		common.EventTriggerListener.Get(txt.gameObject).onDown = onFucos;
		txtRect= txt.GetComponent<RectTransform>();

		float width = maxWidth/GlobalController.instance.setting.screenWidth*UIModule.width;
		float height = GlobalController.instance.setting.gridSize/(GlobalController.instance.setting.screenHeight/UIModule.instance.actualHeight);
		txtRect.sizeDelta = new Vector2(width, height);
		Vector3 vPoint = Camera.main.WorldToViewportPoint(cg.transform.position);
		txtRect.anchoredPosition = new Vector2((vPoint.x - 0.5f)*UIModule.instance.actualWidth,(vPoint.y - 0.5f)*UIModule.instance.actualHeight);


		SpriteRenderer spr = bg.GetComponent<SpriteRenderer>();
		 width = maxWidth/spr.sprite.textureRect.width;
		 height = GlobalController.instance.setting.gridSize/spr.sprite.textureRect.width;

		bg.transform.localScale = new Vector2(width, height);
		bg.transform.localPosition = new Vector3(maxWidth*0.5f, -GlobalController.instance.setting.gridSize*0.5f,0);

		txt.onValueChanged.AddListener( onValChange);
		txt.onEndEdit.AddListener(onInputEnd);

		DistanceTriggerManager.instance.addEventListener(this);
	}

	public void trigger()
	{
		common.TimerManager.instance.addEventListeners(this);
	}

	public Vector3 getPosition()
	{
		return transform.position;
	}

	public void onUpdate()
	{
		Vector3 vPoint = Camera.main.WorldToViewportPoint(cg.transform.position);
		txtRect.anchoredPosition = new Vector2((vPoint.x - 0.5f)*UIModule.instance.actualWidth,(vPoint.y - 0.5f)*UIModule.instance.actualHeight);
	}

	public void onValChange(string str)
	{
		str = txt.text;
		if(lastContent == str)
			return;

		lastContent = str;
		StringBuilder sb = new StringBuilder();
		int assic;
		FontData d;
		float x = 0;
		for(int i = 0; i < str.Length; i++)
		{
			assic = (int)(str[i]);
			if(cg.analyse.fontDatas.TryGetValue(assic, out d))
			{
				x += d.actualAdvance;
				if(x > maxWidth)
					break;
				sb.Append(str[i]);
			}

		}
		cg.contentStr = sb.ToString();
		txt.text = cg.contentStr;
		shiny.transform.localPosition = new Vector3(cg.textWidth - cg.pivot.x*cg.textWidth, cg.analyse.lineHeight-cg.pivot.y*cg.analyse.lineHeight, 0);

	}

	public void onInputEnd(string str)
	{
		str = txt.text;
		cg.Start();
		bg.SetActive(false);
		onExit();
		txt.gameObject.SetActive(false);
		txt.GetComponent<Image>().raycastTarget = false;
		GlobalController.instance.resume();

	}



	public void onFucos(GameObject obj, UnityEngine.EventSystems.PointerEventData data)
	{
		shiny.gameObject.SetActive(true);
		ColorUtil.toAlpha(shiny, 1);
		_curTween = ColorUtil.doFade(shiny, 0, 0.5f).SetLoops(int.MaxValue, LoopType.Yoyo).SetUpdate(true);
		GlobalController.instance.pause();
	}

	public void onExit()
	{
		shiny.gameObject.SetActive(false);
		_curTween.Kill();
		common.TimerManager.instance.removeEventListeners(this);
	}
}
