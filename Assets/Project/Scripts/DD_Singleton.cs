using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 싱글톤 패턴을 구현하는 기본 클래스
/// </summary>
/// <typeparam name="T">싱글톤으로 만들 클래스 타입</typeparam>
public class DD_Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    
    /// <summary>
    /// 싱글톤 인스턴스를 가져오는 프로퍼티
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject prefab = Resources.Load(typeof(T).Name) as GameObject;
                GameObject singleton = Instantiate(prefab);
                _instance = singleton.GetComponent<T>();
            }
            
            return _instance;
        }
    }

    /// <summary>
    /// 싱글톤 생성자
    /// </summary>
    public DD_Singleton()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
    }
    
    /// <summary>
    /// 싱글톤 초기화 함수
    /// </summary>
    protected virtual void Awake()
    {
        if (_instance == this)
        {
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
