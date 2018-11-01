using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityBase<T> : MonoBehaviour
{
    public T data { get; private set; }

    public virtual void SetData(T _data)
    {
        data = _data;
    }

    public void SetPosition(float x,float y)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(new Vector3(x,0,y), out hit, 5, NavMesh.AllAreas))
        {
           transform.position = hit.position;
        }
    }
    public void SetForword(float x, float z)
    {
        transform.forward = new Vector3(x, 0, z);
    }
}

