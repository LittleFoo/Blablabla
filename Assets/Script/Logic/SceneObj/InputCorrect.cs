using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class InputCorrect : MonoBehaviour {
	public DeadCharacterGroup cg;
	public InputField txt;
	private string lastContent;
	public void onValChange(string str)
	{
		if(lastContent == str)
			return;

		lastContent = str;
		StringBuilder sb = new StringBuilder();
		int assic;
		for(int i = 0; i < str.Length; i++)
		{
			assic = (int)(str[i]);
			if(cg.analyse.fontDatas.ContainsKey(assic))
			{
				sb.Append(str[i]);
			}

		}
		cg.contentStr = sb.ToString();
		txt.text = cg.contentStr;
	}
}
