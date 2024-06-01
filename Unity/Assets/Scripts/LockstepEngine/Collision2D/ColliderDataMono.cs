using System;
using Lockstep.Math;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif

namespace Lockstep.Collision2D {
#if UNITY_5_3_OR_NEWER
    public class ColliderDataMono : UnityEngine.MonoBehaviour {
        public ColliderData colliderData;
    }
#endif
    [Serializable]
    public partial class ColliderData :IComponent{
#if UNITY_5_3_OR_NEWER
        [Header("Offset")]
#endif
        public LFloat y;
        public LVector2 pos;
#if UNITY_5_3_OR_NEWER
        [Header("Collider data")]
#endif
        public LFloat high = 1;
        public LFloat radius = 1;
        public LVector2 size = new LVector2(1,1);
        public LVector2 up;
        public LFloat deg;
        
    }
}