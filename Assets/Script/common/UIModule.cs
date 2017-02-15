using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace common
{
    public class UIModule
    {
		public const int width = 1024;
        public const int height = 768;
		public static bool matchWidth = true;
        private float _actualWidth = 1024;
        public float actualWidth {
			get { return _actualWidth; }
        }

		private float _actualHeight = 768;
		public float actualHeight {
			get{ return _actualHeight;}
		}

		private float _scale;
		public float scale
		{
			get{return _scale;}
		}

        private static UIModule _instance;
        public static UIModule instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UIModule();
                return _instance;
            }
        }

        public void init()
        {
			if(matchWidth)
			{
				_scale = Screen.width* 1.0f/UIModule.width;
				_actualWidth = width;
				_actualHeight = UIModule.width * 1.0f / Screen.width * Screen.height;
			}
			else
			{
				_scale = Screen.height* 1.0f/UIModule.height;
				_actualWidth = UIModule.height * 1.0f / Screen.height * Screen.width;
				_actualHeight = height;
			}
           
        }



    }
}
