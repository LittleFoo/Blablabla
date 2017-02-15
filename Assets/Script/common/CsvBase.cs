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

namespace common
{
    #region csvfilename
    public class CsvConfig
    {
        public static string CSV_LEVEL = "Data/Level";
        public static string CSV_PAGE = "Data/Page";
		public static string CSV_PREFAB = "Data/PrefabList";
        public List<int> stringToIntArray(string s)
        {
            if(s == "")
                return null;

            string[] ss = s.Split(","[0]);
            List<int> result = new List<int>(ss.Length);

            for(int i = 0; i < ss.Length; i++)
                result.Add(Convert.ToInt32( ss[i]));

            return result;
        }
    }
    #endregion
    #region CSVBASE
    public interface ICsvBase
    {
        Type getUnitType();
        void addObj(String idx, object obj);
    }
    #endregion

    #region CsvLevel
    public class CsvLevel : ICsvBase
    {
        private Dictionary<string, CsvLevelUnit> units;
        public CsvLevel()
        {
            units = new Dictionary<string, CsvLevelUnit>();
        }
        public Type getUnitType()
        {
            return typeof(CsvLevelUnit);
        }
        public void addObj(String idx, object obj)
        {
            units.Add(idx, (CsvLevelUnit)obj);
        }
        public CsvLevelUnit getObj(string idx)
        {
            CsvLevelUnit unit;
            units.TryGetValue(idx, out unit);
            if (unit == null)
                throw new Exception("CsvLevel 不存在 idx = " + idx.ToString());
            return unit;
        }
    }

    public class CsvLevelUnit
    {
//        private int id;
//        public int ID { get { return id; } set { id = value; } }
        private int id;
        public int Id{ get { return id; } set { id = value; } }

        private int bulletType;
        public int BulletType{ get { return bulletType; } set { bulletType = value; } }

        private string bulletGapRange;
        public string BulletGapRange{ get { return bulletGapRange; } set { bulletGapRange = value; } }

        private string bulletGapData;
        public string BulletGapData{ get { return bulletGapData; } set { bulletGapData = value; } }

        private string speedRange;
        public string SpeedRange{ get { return speedRange; } set { speedRange = value; } }
        
        private string speedData;
        public string SpeedData{ get { return speedData; } set { speedData = value; } }

        private int iceBulletRate;
        public int IceBulletRate{ get { return iceBulletRate; } set { iceBulletRate = value; } }

        private int limitTime;
        public int LimitTime{ get { return limitTime; } set { limitTime = value; } }

        private int itemType;
        public int ItemType{ get { return itemType; } set { itemType = value; } }

        private string itemGapRange;
        public string ItemGapRange{ get { return itemGapRange; } set { itemGapRange = value; } }

        private string itemGapData;
        public string ItemGapData{ get { return itemGapData; } set { itemGapData = value; } }

        private int iceStarRange;
        public int IceStarRange{ get { return iceStarRange; } set { iceStarRange = value; } }

        private float planetGap;
        public float PlanetGap{ get { return planetGap; } set { planetGap = value; } }

        private int planetDirection;
        public int PlanetDirection{ get { return planetDirection; } set { planetDirection = value; } }

        private int conditionType;
        public int ConditionType{ get { return conditionType; } set { conditionType = value; } }

        private string conditionData;
        public string ConditionData{ get { return conditionData; } set { conditionData = value; } }

		private int failData;
		public int FailData{ get { return failData; } set { failData = value; } }

        private string talks;
        public string Talks{ get { return talks; } set { talks = value; } }

        private string preCell;
        public string PreCell{ get { return preCell; } set { preCell = value; } }

        private int roleLoc;
        public int RoleLoc{ get { return roleLoc; } set { roleLoc = value; } }

		private string itemPrefab;
		public string ItemPrefab{ get { return itemPrefab; } set { itemPrefab = value; } }

		private string bulletPrefab;
		public string BulletPrefab{ get { return bulletPrefab; } set { bulletPrefab = value; } }
    }
    #endregion

	#region csvPrefabList

	public class CsvPrefabListUnit
	{
		//        private int id;
		//        public int ID { get { return id; } set { id = value; } }
		private int id;
		public int Id{ get { return id; } set { id = value; } }
		
		private string path;
		public string Path{ get { return path; } set { path = value; } }
	}

	public class CsvPrefabList : ICsvBase
	{
		private Dictionary<string, CsvPrefabListUnit> units;
		public CsvPrefabList()
		{
			units = new Dictionary<string, CsvPrefabListUnit>();
		}
		public Type getUnitType()
		{
			return typeof(CsvPrefabListUnit);
		}
		public void addObj(String idx, object obj)
		{
			units.Add(idx, (CsvPrefabListUnit)obj);
		}
		public CsvPrefabListUnit getObj(string idx)
		{
			CsvPrefabListUnit unit;
			units.TryGetValue(idx, out unit);
			if (unit == null)
				throw new Exception("CsvPrefabList 不存在 idx = " + idx.ToString());
			return unit;
		}
	}

	#endregion
}
