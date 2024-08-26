using System.Collections;
using UnityEngine;

public class SkillInstance : MonoBehaviour
{
    public GameObject target;
    public float Cooltime;
    public SkillInfo info;

    public bool IsCooltiming()
    {
        return Cooltime > 0.0f;
    }

    public void StartCooltime()
    {
        StartCoroutine(StartCooltime_Internal());
    }

    IEnumerator StartCooltime_Internal()
    {
        Cooltime = info.Cooltime;
        while (Cooltime > 0.0f)
        {
            Cooltime -= Time.deltaTime;
            yield return null;
        }
    }
}