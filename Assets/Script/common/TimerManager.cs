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
using System.Collections.Generic;
using System;

namespace common
{
    public class TimerManager
    {
        private List<delayCallData> delayCalls;
        private static TimerManager _instance;
		private float _lastRealTime;
		private float _deltaRealTime;
        private List<ITimerEvent> _toRemove;
		private bool _paused = false;
		public bool paused
		{
			get{return _paused;}
			set{_paused = value;}
		}
        public static TimerManager instance
        {
            get
            {
                if (TimerManager._instance == null)
                    TimerManager._instance = new TimerManager();
                return _instance;
            }
        }

        private List<ITimerEvent> listeners;

        TimerManager()
        {
            if (TimerManager._instance != null)
                throw new Exception("TimerManager is a singleton");

            this.listeners = new List<ITimerEvent>();
            delayCalls = new List<delayCallData>();
            _toRemove = new List<ITimerEvent>();
        }

		public void reset()
		{
			_lastRealTime = Time.realtimeSinceStartup;
		}

        // Update is called once per frame
        public void Update()
        {
			if(_paused)
				return;
			_deltaRealTime = Time.realtimeSinceStartup - _lastRealTime;
			_lastRealTime = Time.realtimeSinceStartup;
            for (int i = this.listeners.Count - 1; i > -1; i--)
            {
                if(_toRemove.Contains( listeners[i]))
                    continue;
                this.listeners[i].onUpdate();
            }

            for (int i = delayCalls.Count - 1; i > -1; i--)
            {
				if(delayCalls[i].isIgnoreTimeScale)
					delayCalls[i].time -= _deltaRealTime;
				else
                	delayCalls[i].time -= Time.deltaTime;
                if (delayCalls[i].time <= 0)
                {
                    delayCalls[i].delayCallFunc();
                    delayCalls.RemoveAt(i);
                }
            }

            removeAfterUpdate();
        }

        public void addEventListeners(ITimerEvent item)
        {
			int idx = _toRemove.IndexOf(item);
			if(idx != -1)
				_toRemove.RemoveAt(idx);

            if(this.listeners.Contains(item))
                return;

            this.listeners.Add(item);
        }

        public void removeEventListeners(ITimerEvent item)
        {
            _toRemove.Add(item);
         }

        private void removeAfterUpdate()
        {
            for(int i = 0; i < _toRemove.Count; i++)
            {
                listeners.Remove(_toRemove[i]);
            }
            _toRemove.Clear();
        }

        public delayCallData delayCall(delayCallData.delayCall func, float time, bool isIgnoreTimeScale = false)
        {
			delayCallData e = new delayCallData(func, time, isIgnoreTimeScale);
            delayCalls.Add(e);
			return e;
        }

		public void cancelDelayCall(delayCallData d)
		{
			int idx = delayCalls.IndexOf(d);
			if(idx != -1)
				delayCalls.RemoveAt(idx);
		}
    }

    public class delayCallData
    {
        public delegate void delayCall();
        public delayCall delayCallFunc;
        public float time;
		public bool isIgnoreTimeScale;
        public delayCallData(delayCall func, float delay, bool ignoreTimeScale)
        {
            delayCallFunc = func;
            time = delay;
			isIgnoreTimeScale = ignoreTimeScale;
        }
    }
}