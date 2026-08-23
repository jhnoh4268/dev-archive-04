using System.Collections;
using UnityEngine;

public class MANAGER : MonoBehaviour
{
    public static MANAGER instance;
    public static PoolManager poolManager;
    public static DBManager DB;
    public static SessionManager Session;
    public static SkillManager SkillManager;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        poolManager = GetComponentInChildren<PoolManager>();
        DB = GetComponentInChildren<DBManager>();
        Session = GetComponentInChildren<SessionManager>();
        SkillManager = GetComponentInChildren<SkillManager>();
    }

    public void Run(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
