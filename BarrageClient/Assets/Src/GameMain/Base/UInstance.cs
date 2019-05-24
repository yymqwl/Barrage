using UnityEngine;
using GameFramework;

namespace GameMain
{
    public class UInstance<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_Instance;
        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {

                    m_Instance = (T)FindObjectOfType(typeof(T));
                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        throw new GameFrameworkException("UInstance there should never be more than 1 singleton!");
                    }
                    if (m_Instance == null)
                    {
                        GameObject singleton = new GameObject();
                        m_Instance = singleton.AddComponent<T>();
                        singleton.name = typeof(T).Name;
                        DontDestroyOnLoad(singleton);
                        GameObject inspar = GameObject.Find(GameConstant.Str_UInstanceName);//所有父节点
                        if (inspar == null)
                        {
                            inspar = new GameObject();
                            DontDestroyOnLoad(inspar);
                            inspar.name = GameConstant.Str_UInstanceName;
                        }
                        singleton.transform.parent = inspar.transform;
                        Log.Debug("[UInstance] An UInstance of " + typeof(T) +
                              " is needed in the scene, so '" + singleton +
                              "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Log.Debug("[Singleton] Using instance already created: " +
                              m_Instance.gameObject.name);
                    }

                }

                return m_Instance;
            }
        }
    }
}