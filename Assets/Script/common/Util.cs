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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace common
{
	public delegate void callback0();
	public delegate void callback1<T>(T param1);
	public delegate void callback2<T1,T2>(T1 param1, T2 param2);
	public delegate void callback3<T1,T2,T3>(T1 param1, T2 param2, T3 param3);

    public class Util
    {
        public static void SetRecursively(Transform xform, int layer)
        {
            xform.gameObject.layer = layer;
            foreach (Transform child in xform)
                SetRecursively(child, layer);
        }

        public static void SetRecursivelyAlpha(Transform xform, Color color)
        {
            SpriteRenderer spr = xform.GetComponent<SpriteRenderer>();
            if(spr != null)
            spr.color = color;
            foreach (Transform child in xform)
            {
                SetRecursivelyAlpha(child, color);
            }
        }

		public static void SetImgRecursivelyAlpha(Transform xform, Color color)
		{
			Image spr = xform.GetComponent<Image>();
			if(spr != null)
				spr.color = color;
			foreach (Transform child in xform)
			{
				SetImgRecursivelyAlpha(child, color);
			}
		}
    }
}
