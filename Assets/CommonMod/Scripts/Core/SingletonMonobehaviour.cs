using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Monster.Core
{
    public class SingletonMonobehaviour<T> : MonoBehaviour where T : SingletonMonobehaviour<T>
    {
        private static T _instance = (T)((object)null);

        private static object _lock = new object();

        private static bool quitWarningShown = false;

        [SerializeField]
        [Tooltip("Use this instance for the entire game cycle, even when someone with the same flag wants to become the instance")]
        protected bool _overrideOldInstance;

        protected bool _isValidNewInstance = true;

        private static bool applicationIsQuitting = false;


		protected virtual void Awake()
		{
			applicationIsQuitting = false;
			quitWarningShown = false;
			if (_instance == this)
			{
				Debug.Log(string.Format("[Singleton] I'm {0}, already the instance. Not executing Awake again.", base.name));
				return;
			}
			if (_instance != null)
			{
				if (!this._overrideOldInstance)
				{
					this._isValidNewInstance = false;
					this.DestroyInstanceObject((T)((object)this));
					Debug.LogWarning(string.Format("[Singleton] There already exists a {0} singleton. Instance will be kept with old, be carefull that enherited classes might reset other values in Awake or it's constructor. Instance on object {1} is removed.", typeof(T), base.gameObject.name));
					return;
				}
				bool flag = this.DestroyInstanceObject(_instance);
				Debug.LogWarning(string.Format("[Singleton] There already exists a {0} singleton.  New instance is used because it has been flagged as ForceUse. {1}", typeof(T), (!flag) ? string.Empty : " Old gameobject has been destroyed."));
			}
			_instance = (T)((object)this);
			if (PersistentManager.HasAttribute(typeof(T)))
			{
				if (base.transform.parent != null)
				{
					Debug.LogWarning("Please don't parent any singletons with marked as DontDestroyOnLoad, as we can't promise the DontDestroyOnLoad functionality then.");
				}
				UnityEngine.Object.DontDestroyOnLoad(this);
			}
		}

        public static T InstanceIfAvailable
		{
			get
			{
				return (!IsAvailable) ? ((T)((object)null)) : Instance;
			}
		}

		public static T Instance
		{
			get
			{
				if (!object.ReferenceEquals(_instance, null))
				{
					return _instance;
				}
				if (applicationIsQuitting)
				{
					if (!quitWarningShown)
					{
						Debug.LogWarning(string.Format("[Singleton] Instance '{0}' already destroyed on application quit. Won't create again - returning null.", typeof(T)));
						quitWarningShown = true;
					}
					return (T)((object)null);
				}

				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = (T)FindObjectOfType(typeof(T));
						if (FindObjectsOfType(typeof(T)).Length > 1)
						{
							Debug.LogError("[Singleton] Something went really wrong - there should never be more than 1 singleton! Reopenning the scene might fix it.");
							return _instance;
						}
						if (PersistentManager.HasAttribute(typeof(T)))
						{
							if (PersistentManager.SceneWasLoaded)
							{
								Debug.LogWarning("[Singleton] PersistentManagers scene was already loaded.");
							}
							else
							{
								//bool flag = true;
								//if (SceneManager.GetSceneByName("PersistentManagers").IsValid())
								//{
								//	SceneManager.LoadScene("PersistentManagers", LoadSceneMode.Additive);
								//}
								//else
								//{
								//	Debug.LogWarning(typeof(T).ToString() + "'s attribute [PersistentManager] wants a PersistentManagers scene, but its not in the build settings! Will create a DontDestroyOnLoad object now. Please remove the attribute or implement it correctly (PersistentManager scene)");
								//	flag = false;
								//}
								PersistentManager.SceneWasLoaded = true;
								_instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
								if (_instance == null)//&& flag
								{
									Debug.LogWarning(string.Format("[Singleton] Loading of PersistentManagers scene did not create instance of {0}.", typeof(T)));
								}
							}
						}
						if (_instance == null)
						{
							GameObject gameObject = new GameObject();
							_instance = gameObject.AddComponent<T>();
							gameObject.name = string.Format("(singleton) {0}", typeof(T).ToString());
							if (PersistentManager.HasAttribute(typeof(T)))
							{
								DontDestroyOnLoad(gameObject);
							}
							if (!PersistentManager.IsLoadAtRuntime(typeof(T)))
							{
								Debug.LogError(string.Format("[Singleton] An instance of {0} is needed in the scene, so '{1}' was created{2}.", typeof(T), gameObject, (!PersistentManager.HasAttribute(typeof(T))) ? string.Empty : " with DontDestroyOnLoad"));
							}
						}
						else
						{
							Debug.Log(string.Format("[Singleton] Using instance already created: {0}", _instance.gameObject.name));
						}
					}

				}
				return _instance;
			}
		}

		public static S InstanceAs<S>() where S : T
		{
			T instance = Instance;
			if (instance is S)
			{
				return instance as S;
			}
			Debug.LogError(string.Format("[Singleton] Instance of type {0} is not assignable to type {1}", (!(instance == null)) ? instance.GetType().Name : "null", typeof(S)));
			return (S)((object)null);
		}

		public static bool IsAvailable
		{
			get
			{
				return _instance != null;
			}
		}

		protected virtual void OnApplicationQuit()
		{
			applicationIsQuitting = true;
			quitWarningShown = false;
		}

		protected virtual void OnDestroy()
		{
			if (_instance == null)
			{
				Debug.LogWarning(string.Format("[Singleton] instance of {0} singleton already is null in OnDestroy", typeof(T)));
			}
			_instance = (T)((object)null);
		}

		private bool DestroyInstanceObject(T instance)
		{
			GameObject gameObject = instance.gameObject;
			UnityEngine.Object.Destroy(instance);
			bool result = false;
			if (gameObject.transform.childCount == 0 && gameObject.GetComponents<Component>().Length <= 2)
			{
				UnityEngine.Object.Destroy(gameObject);
				result = true;
			}
			return result;
		}

		
	}

}
