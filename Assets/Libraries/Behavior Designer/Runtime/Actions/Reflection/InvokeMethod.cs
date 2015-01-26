using UnityEngine;
using System;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Invokes the specified method with the specified parameters. Can optionally store the return value. Returns success if the method was invoked.")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=145")]
    [TaskCategory("Reflection")]
    [TaskIcon("{SkinColor}ReflectionIcon.png")]
    public class InvokeMethod : Action
    {
        [Tooltip("The GameObject to invoke the method on")]
        public SharedGameObject targetGameObject;
        [Tooltip("The component to invoke the method on")]
        public SharedString componentName;
        [Tooltip("The name of the method")]
        public SharedString methodName;
        [Tooltip("The first parameter of the method")]
        public SharedVariable parameter1;
        [Tooltip("The second parameter of the method")]
        public SharedVariable parameter2;
        [Tooltip("The third parameter of the method")]
        public SharedVariable parameter3;
        [Tooltip("The fourth parameter of the method")]
        public SharedVariable parameter4;
        [Tooltip("Store the result of the invoke call")]
        public SharedVariable storeResult;

        public override TaskStatus OnUpdate()
        {
            if (targetGameObject == null || targetGameObject.Value == null) {
                Debug.LogWarning("Unable to invoke method");
                return TaskStatus.Failure;
            }

            var component = targetGameObject.Value.GetComponent(TaskUtility.GetTypeWithinAssembly(componentName.Value));
            if (component == null) {
                Debug.LogWarning("Unable to invoke method with component " + componentName.Value);
                return TaskStatus.Failure;
            }

            var parameterList = new List<object>();
            var parameterTypeList = new List<Type>();
            SharedVariable sharedVariable = null;
            for (int i = 0; i < 4; ++i) {
                var parameterField = GetType().GetField("parameter" + (i + 1));
                if ((sharedVariable = parameterField.GetValue(this) as SharedVariable) != null) {
                    parameterList.Add(sharedVariable.GetValue());
                    parameterTypeList.Add(sharedVariable.GetType().GetProperty("Value").PropertyType);
                } else {
                    break;
                }
            }
            var methodInfo = component.GetType().GetMethod(methodName.Value, parameterTypeList.ToArray());

            if (methodInfo == null) {
                Debug.LogWarning("Unable to invoke method " + methodName.Value + " on component " + componentName.Value);
                return TaskStatus.Failure;
            }

            var result = methodInfo.Invoke(component, parameterList.ToArray());
            if (storeResult != null) {
                storeResult.SetValue(result);
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            componentName = null;
            methodName = null; 
            parameter1 = null;
            parameter2 = null;
            parameter3 = null;
            parameter4 = null;
            storeResult = null;
        }
    }
}