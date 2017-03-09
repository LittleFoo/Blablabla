using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ToHSL : MonoBehaviour{
	private  Material _m;
	public int minVal;
	public int maxVal;
	public float duration;
	public int loop = -1;
	private float _val;
	public float val
	{
		get{return _val;}
		set{_val = value;
			_m.SetInt( "_Hue", (int)_val );}
	}
	// Use this for initialization
	void Start () {
		_m = GetComponent<SpriteRenderer>().material;
		_val = minVal;
		if(loop <= 0)
			loop = int.MaxValue;
		DOTween.To(()=>val, x =>{_val = x; _m.SetInt( "_Hue", (int)_val );}, maxVal, duration).SetLoops(loop, LoopType.Yoyo);
	}
}
