
using Neatly;
using UnityEngine;

namespace Battle.View
{
    public class EntityBase<T> : NeatlyBehaviour
    {
        public T mData;
        public virtual void Init(T _data)
        {
            mData = _data;
        }
        public virtual void CreateInit()
        {

        }
        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void SetLocalPosition(float x, float y)
        {
            transform.localPosition = new Vector3(x, 0, y);
        }

        public void SetLocalRotation(float y)
        {
            transform.localEulerAngles = new Vector3(0, y, 0);
        }
    }
}

