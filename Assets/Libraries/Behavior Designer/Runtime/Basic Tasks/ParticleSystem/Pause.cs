using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityParticleSystem
{
    [TaskCategory("Basic/ParticleSystem")]
    [TaskDescription("Pause the Particle System.")]
    public class Pause : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;

        private ParticleSystem targetParticleSystem;

        public override void OnStart()
        {
            targetParticleSystem = GetDefaultGameObject(targetGameObject.Value).GetComponent<ParticleSystem>();
        }

        public override TaskStatus OnUpdate()
        {
            if (targetParticleSystem == null) {
                Debug.LogWarning("ParticleSystem is null");
                return TaskStatus.Failure;
            }

            targetParticleSystem.Pause();

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
        }
    }
}