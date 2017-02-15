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
using DG.Tweening;
using UnityEngine.UI;

public class ColorUtil
{
	public static Color ALPHA0 = new Color(1, 1, 1, 0);

    public static void toAlpha(RawImage img, float alpha)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
    }

    public static void toAlpha(Image img, float alpha)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
    }

    public static void toAlpha(Text img, float alpha)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
    }


	public static void toAlpha(SpriteRenderer img, float alpha)
	{
		img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
	}

    public static void toAlphaWithChildren(Image img, float alpha)
    {
        Image[] imgs = img.GetComponentsInChildren<Image>();
        for(int i = 0; i < imgs.Length; i++)
        {
            toAlpha(imgs[i], alpha);
        }
    }

    public static void toAlphaWithChildren(RawImage img, float alpha)
    {
        RawImage[] imgs = img.GetComponentsInChildren<RawImage>();
        for(int i = 0; i < imgs.Length; i++)
        {
            toAlpha(imgs[i], alpha);
        }
    }

    public static void toAlphaWithChildren(Text img, float alpha)
    {
        Text[] imgs = img.GetComponentsInChildren<Text>();
        for(int i = 0; i < imgs.Length; i++)
        {
            toAlpha(imgs[i], alpha);
        }
    }

    public static void doFadeWithChildren (Image img, float alpha, float time)
    {
        Image[] imgs = img.GetComponentsInChildren<Image>();
        for(int i = 0; i < imgs.Length; i++)
        {
            doFade(imgs[i], alpha, time);
        }
    }

    public static void doFadeWithChildren (Text img, float alpha, float time)
    {
        Text[] imgs = img.GetComponentsInChildren<Text>();
        for(int i = 0; i < imgs.Length; i++)
        {
            doFade(imgs[i], alpha, time);
        }
    }

	public static Tweener doFade (Image img, float alpha, float time)
	{
        return DOTween.To(() => img.color, x => img.color = x, new Color(img.color.r, img.color.g, img.color.b, alpha), time);
	}

	public static Tweener doFade (CanvasGroup img, float alpha, float time)
	{
		return DOTween.To(() => img.alpha, x => img.alpha = x, alpha, time);
	}

	public static Tweener doFade (TextMesh img, float alpha, float time)
	{
		return DOTween.To(() => img.color, x => img.color = x, new Color(img.color.r, img.color.g, img.color.b, alpha), time);
	}

    public static Tweener doFade (Text txt, float alpha, float time)
    {
        return DOTween.To(() => txt.color, x => txt.color = x, new Color(txt.color.r, txt.color.g, txt.color.b, alpha), time);
    }

	public static Tweener doFade (Image img, Color color, float time)
	{
		return DOTween.To(() => img.color, x => img.color = x, color, time);
	}

	public static Tweener doFade (RawImage img, float alpha, float time)
	{
        return DOTween.To(() => img.color, x => img.color = x, new Color(img.color.r, img.color.g, img.color.b, alpha), time);
	}

	public static Tweener doFade (SpriteRenderer sprite, float alpha, float time)
	{
        return DOTween.To(() => sprite.color, x => sprite.color = x, new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha), time);
	}

	public static Tweener doFade (SpriteRenderer sprite, Color color, float time)
	{
		return DOTween.To(() => sprite.color, x => sprite.color = x, color, time);
	}

	public static Tweener doFade (Material sprite, Color color, float time)
	{
		return DOTween.To(() => sprite.color, x => sprite.color = x, color, time);
	}

	public static Tweener doFade (Material sprite, float alpha, float time)
	{
		return DOTween.To(() => sprite.color, x => sprite.color = x, new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha), time);
	}
}
