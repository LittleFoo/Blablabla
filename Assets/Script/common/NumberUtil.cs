//
// /********************************************************
// * 
// *　　　　　　Copyright (c) 2015  Feiyu
// *  
// * Author     : Binglei Gong</br>
// * Date       : 15-12-21 </br>
// * Declare    : NumberUtil.cs</br>
// * Version    : 1.0.0</br>
// * Summary    : create</br>
// *
// *
//  *******************************************************/
using System;
using System.Text;
namespace common
{
    public class NumberUtil
    {
        public static string numberToTime(float time, int digit)
        {
            string srt = "";
            int rnd = (int)time;
            int result = (int)time;
            int temp;
            for(int i = digit-1; i > 0; i--)
            {
                temp = (int)Math.Pow(60, i);
                rnd = result/(int)temp;
                if(rnd < 10)
                    srt += "0";
                srt += rnd.ToString() + ":";
                result -= rnd*temp;
            }
            if(result < 10)
                srt += "0";
            srt += result.ToString();
            return srt;
        }

    }
}

