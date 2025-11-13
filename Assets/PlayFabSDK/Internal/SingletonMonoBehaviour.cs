/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;

namespace PlayFab.Internal
{
    //public to be accessible by Unity engine
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T m_instance;

        public static T instance
        {
            get
            {
                if (m_instance == null)
                {
                    //find existing instance
                    m_instance = GameObject.FindObjectOfType<T> ();
                    if (m_instance == null)
                    {
                        //create new instance
                        GameObject go = new GameObject (typeof (T).Name);
                        m_instance = go.AddComponent<T> ();
                        DontDestroyOnLoad (go);
                    }
                    //initialize instance if necessary
                    if (!m_instance.initialized)
                    {
                        m_instance.Initialize ();
                        m_instance.initialized = true;
                    }
                }
                return m_instance;
            }
        }

        private void Awake ()
        {
            //check if instance already exists when reloading original scene
            if (m_instance != null)
            {
                DestroyImmediate (gameObject);
            }
        }

        protected bool initialized { get; set; }

        protected virtual void Initialize ()
        {}
    }
}
