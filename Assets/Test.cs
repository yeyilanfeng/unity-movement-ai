using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool IsAdjacent(Transform targetTrans)
    {
        using (var p = new ScopedProfiler("Adjacent"))
        {
            if (targetTrans == null)
            {
                return false;
            }

            for (int i = 0; i < targetTrans.childCount; i++)
            {
                if (targetTrans.GetChild(i).gameObject.name == "Text")
                {
                    return true;
                }
            }

            return false;
        }

    }

    //public bool IsAdjacent(Transform targetTrans)
    //{
    //    Profiler.BeginSample("Adjacent");
    //    {
    //        if (targetTrans == null)
    //        {
    //            Profiler.EndSample();
    //            return false;
    //        }

    //        for (int i = 0; i < targetTrans.childCount; i++)
    //        {
    //            if (targetTrans.GetChild(i).gameObject.name == "Text")
    //            {
    //                Profiler.EndSample();
    //                return true;
    //            }
    //        }
    //        Profiler.EndSample();
    //        return false;
    //    }
    //}
}


public struct ScopedProfiler : IDisposable
{
    public ScopedProfiler(string name)
    {
        Profiler.BeginSample(name);
    }

    public ScopedProfiler(string name, UnityEngine.Object targetObject)
    {
        Profiler.BeginSample(name, targetObject);
    }

    void IDisposable.Dispose()
    {
        Profiler.EndSample();
    }
}
