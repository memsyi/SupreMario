#if UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityCircleCollider2D
{
    [TaskCategory("Basic/CircleCollider2D")]
    [TaskDescription("Sets the center of the CircleCollider2D. Returns Success.")]
    public class SetCenter : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The center of the CircleCollider2D")]
        public SharedVector2 center;

        private CircleCollider2D circleCollider2D;

        public override void OnStart()
        {
            circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        }

        public override TaskStatus OnUpdate()
        {
            if (circleCollider2D == null) {
                Debug.LogWarning("CircleCollider2D is null");
                return TaskStatus.Failure;
            }

            circleCollider2D.center = center.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            center = Vector2.zero;
        }
    }
}
#endif