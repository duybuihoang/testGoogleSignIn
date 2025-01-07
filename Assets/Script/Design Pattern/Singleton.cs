using UnityEngine;

public abstract class Singleton<T> : BaseMonobehavior where T : BaseMonobehavior
{
	private static T _instance = null;

	public static bool IsAwake { get { return (_instance != null); } }
	public static T Instance
	{
		get
		{
			
			return _instance;
		}
	}
    protected override void Awake()
    {
		if (_instance == null)
		{
			_instance = (T)FindFirstObjectByType(typeof(T));
			if (_instance == null)
			{

				string goName = typeof(T).ToString();

				GameObject go = GameObject.Find(goName);
				if (go == null)
				{
					go = new GameObject();
					go.name = goName;
				}

				_instance = go.AddComponent<T>();
			}
		}
	}

    public virtual void OnApplicationQuit()
	{
		_instance = null;
	}

	protected void SetParent(string parentGOName)
	{
		if (parentGOName != null)
		{
			GameObject parentGO = GameObject.Find(parentGOName);
			if (parentGO == null)
			{
				parentGO = new GameObject();
				parentGO.name = parentGOName;
			}
			this.transform.parent = parentGO.transform;
		}
	}

}