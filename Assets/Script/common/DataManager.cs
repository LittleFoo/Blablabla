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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Reflection;
using System.Text.RegularExpressions;

namespace common
{
    class DataManager
    {
        private static DataManager _instance;
        public static DataManager instance
        {
            get
            {
                if (DataManager._instance == null)
                    DataManager._instance = new DataManager();
                return _instance;
            }
        }

        public Dictionary<string, JsonData> jsons;
        public Dictionary<string, List<DramaData>> dramas;
		public JsonData doc;
        private Dictionary<string, ICsvBase> csvs;

        public DataManager()
        {
            if (DataManager._instance != null)
                throw new Exception("datamanager is a singleton");

            dramas = new Dictionary<string, List<DramaData>>();
            jsons = new Dictionary<string, JsonData>();
            csvs = new Dictionary<string, ICsvBase>();


        }

        public JsonData getData(string fileName)
        {
//            if (!jsons.ContainsKey(fileName))
//            {
//                UnityEngine.TextAsset s = Resources.Load(fileName) as TextAsset;
//                jsons.Add(fileName, JsonMapper.ToObject(s.text));
//            }
//            return jsons[fileName];
			UnityEngine.TextAsset s = Resources.Load(fileName) as TextAsset;
			return JsonMapper.ToObject(s.text);
        }

        private string[] stringSeparators = new string[] { "\r\n" };
        public List<DramaData> getDramaData(string fileName)
        {
            if (dramas.ContainsKey(fileName))
                return dramas[fileName];

            List<DramaData> result = new List<DramaData>();
            UnityEngine.TextAsset s = Resources.Load(fileName) as TextAsset;
            string[] lineArray = s.text.Split(stringSeparators, StringSplitOptions.None);

            int i = 0;
            for (; i < lineArray.Length; i++)
            {
                if (Regex.IsMatch(lineArray[i], "^//"))
                    continue;
                else
                    break;
            }

            string[] title = lineArray[i].Split(","[0]);
            i++;

            string[] unit;
            DramaData data;
            Type t = typeof(DramaData);
            PropertyInfo pi;
          
            for (; i < lineArray.Length; i++)
            {
                if (Regex.IsMatch(lineArray[i].ToLower(), "^end"))
                    break;
                if (Regex.IsMatch(lineArray[i], "^//"))
                    continue;

                unit = lineArray[i].Split(","[0]);
                data = new DramaData();
                for (int j = 0; j < title.Length; j++)
                {
                   pi = t.GetProperty(title[j]);
                   if (pi == null)
                       continue;
                   pi.SetValue(data, Convert.ChangeType( unit[j],pi.PropertyType), null);
                }
                result.Add(data);
            }
            return result;
        }

        public T getCsvData<T>(string idx)
        {
            return (T)csvs[idx];
        }

        public void loadCsvData(string fileName, Type t)
        {
            if (csvs.ContainsKey(fileName))
                return ;

            ICsvBase result = (ICsvBase)Activator.CreateInstance(t);
            UnityEngine.TextAsset s = Resources.Load(fileName) as TextAsset;
            string[] lineArray = s.text.Split(stringSeparators, StringSplitOptions.None);
            object data;

            int i = 0;
            for (; i < lineArray.Length; i++)
            {
                if (Regex.IsMatch(lineArray[i], "^//"))
                    continue;
                else
                    break;
            }

            string[] title = lineArray[i].Split(","[0]);
            i++;

            string[] unit;
            PropertyInfo pi;

            for (; i < lineArray.Length; i++)
            {
                if (Regex.IsMatch(lineArray[i].ToLower(), "^end"))
                    break;
                if (Regex.IsMatch(lineArray[i], "^//"))
                    continue;

                unit = lineArray[i].Split(","[0]);
                t = result.getUnitType();
                data = Activator.CreateInstance(t);
                for (int j = 0; j < title.Length; j++)
                {
                    pi = t.GetProperty(title[j]);
                    if (pi == null)
                        continue;
                    pi.SetValue(data, Convert.ChangeType(unit[j], pi.PropertyType), null);
                }
                result.addObj(unit[0], data);
            }

            csvs.Add(fileName, result);
        }


        private object analyseData(string row, Type t)
        {
            object obj = Activator.CreateInstance(t);
            PropertyInfo[] pis = t.GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                pi.SetValue(obj, 1, null);
            }

            return obj;
        }
    }


    public class DramaData
    {
        //SortId 说明	对象类型 0：现有角色 1：角色，2：物件 3：对话  4镜头	操作类型 1 添加 2删除 3移动	组件ID 如果现有角色则ID对应角色占位ID，如果是新加的对象，无ID， 如果是已经添加的对象，对应SORTID	参数 移动 x,y,time,type 0:直线 1：抛物线
        private int id;
        public int ID { get { return id; } set { id = value; } }
        private int objType;
        public int ObjType { get { return objType; } set { objType = value; } }
        private int actionType;
        public int ActionType { get { return actionType; } set { actionType = value; } }
        private int comID = -1;
        public int ComID { get { return comID; } set { comID = value; } }
        private string param1;
        public string Param1 { get { return param1; } set { param1 = value; } }
        private string param2;
        public string Param2 { get { return param2; } set { param2 = value; } }
        private float time;
        public float Time { get { return time; } set { time = value; } }
        private float frameTime;
        public float FrameTime { get { return frameTime; } set { frameTime = value; } }
        private int followComId;
        public int FollowComId { get { return followComId; } set { followComId = value; } }
        private int isLeft;
        public int IsLeft { get { return isLeft; } set { isLeft = value; } }
    }
}
