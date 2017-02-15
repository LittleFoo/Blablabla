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
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class Bar : MonoBehaviour
{
    public GameObject content;
    public GameObject tf;
  
    public float maxVal;
    public bool isShowTxt;
    public bool isHorizon;
    public bool isShowMax;
    public float tweenTime = 0.5f;

    public delegate void OnSetPercent(float percent);
    public OnSetPercent onSetPercent;

	public System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();

	protected float initWidth;
	protected Text text;
	protected float _val;
	public virtual float val
    {
        get { return _val; }
        set
        {
            if (value > this.maxVal)
                value = this.maxVal;
            if (value < 0)
                value = 0;
            this._val = value;
            RectTransform img = content.GetComponent< RectTransform>();
            if (isHorizon)
                img.sizeDelta = new Vector2(this.initWidth * this._val / this.maxVal, img.sizeDelta.y);
            else
                img.sizeDelta = new Vector2(img.sizeDelta.x, this.initWidth * this._val / this.maxVal);
			
			if(isShowTxt)
			{
				if(text == null)
					text = tf.GetComponent<Text>();
	            if (isShowMax)
					text.text = _val.ToString() + "/" + this.maxVal;
	            else
					text.text = _val.ToString() ;
			}
        }
    }

    void Awake()
    {
        if (isHorizon)
            this.initWidth = (content.GetComponent("RectTransform") as RectTransform).sizeDelta.x;
        else
            this.initWidth = (content.GetComponent("RectTransform") as RectTransform).sizeDelta.y;
		if (this.tf != null && !this.isShowTxt)
            this.tf.SetActive(false);

		nfi.NumberDecimalDigits = 2;
    }

    public void tweenVal(float curVal)
    {
        if (curVal == _val)
            return;
        Vector2 rect = content.GetComponent<RectTransform>().sizeDelta;
        if (isHorizon)
            DOTween.To(() => content.GetComponent<RectTransform>().sizeDelta, x => content.GetComponent<RectTransform>().sizeDelta = x,
                       new Vector2(this.initWidth *curVal / this.maxVal, rect.y), tweenTime).OnComplete(() => { val = curVal; }).SetUpdate(true);
        else
            DOTween.To(() => content.GetComponent<RectTransform>().sizeDelta, x => content.GetComponent<RectTransform>().sizeDelta = x,
			           new Vector2(rect.x, this.initWidth *curVal / this.maxVal), tweenTime).OnComplete(() => { val = curVal; }).SetUpdate(true); 

        if(onSetPercent != null)
            onSetPercent(curVal / this.maxVal);
    }


	public void setText(string s)
	{
		if(text == null)
			text = tf.GetComponent<Text>();
		text.text = s;
	}
}
