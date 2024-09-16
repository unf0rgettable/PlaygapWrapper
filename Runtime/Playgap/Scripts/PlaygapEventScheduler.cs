using System;
using System.Collections.Generic;
using UnityEngine;

namespace Playgap
{
    internal class PlaygapEventScheduler : MonoBehaviour, IPlaygapEventScheduler
    {
        private static GameObject s_Instance;
        private static PlaygapEventScheduler s_Scheduler;
        public static IPlaygapEventScheduler Scheduler
        { 
            get
            {
                Create();
                return s_Scheduler;
            }
        }

        private object m_Lock = new object();
        private bool m_HasActionsToRun = false;
        private Queue<Action> m_Actions = new Queue<Action>();
        private List<Action> m_ActionsToRun = new List<Action>(16);

        public static void Create()
        {
            if (s_Instance != null)
            {
                return;
            }

            s_Instance = new GameObject("PlaygapEventScheduler", typeof(PlaygapEventScheduler));
            GameObject.DontDestroyOnLoad(s_Instance);

            s_Scheduler = s_Instance.GetComponent<PlaygapEventScheduler>();
        }

        public void ScheduleOnUpdate(Action action)
        {
            lock (m_Lock)
            {
                m_Actions.Enqueue(action);
                m_HasActionsToRun = true;
            }
        }

        void Update()
        {
            if (!m_HasActionsToRun)
            {
                return;
            }

            lock (m_Lock)
            {
                m_HasActionsToRun = false;
                m_ActionsToRun.AddRange(m_Actions);
                m_Actions.Clear();
            }

            for (int i = 0; i < m_ActionsToRun.Count; i++)
            {
                try
                {
                    m_ActionsToRun[i]();
                }
                catch (Exception e)
                {
                    Debug.LogError("[Playgap] Exception was thrown in user callback");
                    Debug.LogException(e);
                }
            }

            m_ActionsToRun.Clear();
        }
    }
}