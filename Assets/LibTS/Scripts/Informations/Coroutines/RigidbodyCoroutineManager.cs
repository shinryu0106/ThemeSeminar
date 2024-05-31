using System.Collections;
using UnityEngine;

namespace LibTS
{
    [AddComponentMenu("")]
    public class RigidbodyCoroutineManager : BaseCoroutine
    {
        #region ForwardConstantly
        public void InvokeForwardConstantly(
            Rigidbody rigidbody, Vector3 direction, float time = 1f,
            float moveSpeed = 1f, float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default, ForceMode forceMode = ForceMode.Force
        )
        {
            CoroutineInstance = StartCoroutine(ForwardConstantlyCoroutine(rigidbody, direction, time, moveSpeed, rotateSpeed, timeType, forceMode));
        }

        private IEnumerator ForwardConstantlyCoroutine(
            Rigidbody rigidbody, Vector3 direction, float time = 1f,
            float moveSpeed = 1f, float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default, ForceMode forceMode = ForceMode.Force
        )
        {
            float preTime = Time.time;
            while (true)
            {
                rigidbody.Forward(direction, moveSpeed, rotateSpeed, timeType, forceMode);

                if (Time.time - preTime >= time)
                    Stop();
                yield return null;
            }
        }

        public void InvokeForwardConstantly(
            Rigidbody rigidbody, Transform target, float time = 1f,
            float moveSpeed = 1f, float rotateSpeed = 1f, float error = 0.1f,
            TimeType timeType = TimeType.Default, ForceMode forceMode = ForceMode.Force
        )
        {
            CoroutineInstance = StartCoroutine(ForwardConstantlyCoroutine(rigidbody, target, time, moveSpeed, rotateSpeed, error, timeType, forceMode));
        }

        private IEnumerator ForwardConstantlyCoroutine(
            Rigidbody rigidbody, Transform target, float time = 1f,
            float moveSpeed = 1f, float rotateSpeed = 1f, float error = 0.1f,
            TimeType timeType = TimeType.Default, ForceMode forceMode = ForceMode.Force
        )
        {
            float preTime = Time.time;
            while (true)
            {
                rigidbody.Forward(target, moveSpeed, rotateSpeed, error, timeType, forceMode);

                if (Time.time - preTime >= time)
                    Stop();
                yield return null;
            }
        }
        #endregion
    }
}