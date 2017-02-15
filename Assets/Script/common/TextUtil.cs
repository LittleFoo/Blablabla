//
// /********************************************************
// * 
// *　　　　　　Copyright (c) 2015  Feiyu
// *  
// * Author		: Binglei Gong</br>
// * Date		: 15-12-14上11:19</br>
// * Declare	: </br>
// * Version	: 1.0.0</br>
// * Summary	: create</br>
// *
// *
// *******************************************************/

using System;
using System.Text.RegularExpressions;

namespace common
{
	public class TextUtil
	{
		private static string _insertRegStr = "/%[a-zA-Z]{1,4}/";
		public static string toRichText(string content, string[] insertArray)
		{
			Regex reg = new Regex(_insertRegStr);
			for(int i = 0; i < insertArray.Length; i++)
				content = reg.Replace(content, insertArray[i], 1);

			return content;
		}

        public static string toRichText(string content, string insertStr)
        {
            MatchCollection c = Regex.Matches(content, _insertRegStr);
            foreach(Match item in c)
            {
                content = content.Remove(item.Index, item.Length);
                content = content.Insert(item.Index, insertStr);
            }
            return content;
        }

		public static MatchCollection analyseColorText(string str)
		{
//			string content = "my name is <color = #FFFFF093 >SS\nSS </color>kkk";
			string pattern =  @"<\s*color\s*=\s*([A-Za-z0-9#]*)\s*>([\s\S]*?)</color>";

			Regex reg = new Regex(pattern);
			MatchCollection m = reg.Matches(str);
			
//			MatchCollection m2 = reg.Matches(content);
//			m2 = Regex.Matches(content, pattern, RegexOptions.Multiline);
//			foreach(Match mm in m2)
//			{
//				for(int i = 0; i < mm.Groups.Count; i++)
//				UnityEngine.Debug.Log(mm.Groups[i].Value);
//			}

			return m;

		}

		public static string cut(string content, string target, bool containTarget, bool containSuffix)
		{
			string[] a = Regex.Split(content, target);
			if(a.Length == 0)
				return content;
			
			if(containTarget)
				content = target + a[a.Length-1];
			else
			content = a[a.Length-1];

			if(containSuffix)
				return content;
			else
				return content.Remove(content.IndexOf("."));
		}


	}

}

