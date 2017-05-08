using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    [System.Serializable]
    public class FloatEvent : UnityEvent<float> {}

    [System.Serializable]
    public class TransformEvent : UnityEvent<Transform> {}

    public class Utils
    {
        public static bool CheckLayer(LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}