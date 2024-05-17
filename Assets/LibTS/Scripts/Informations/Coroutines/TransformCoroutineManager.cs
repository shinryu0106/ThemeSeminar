using System.Collections;
using UnityEngine;

namespace LibTS
{
    [AddComponentMenu("")]
    public class TransformCoroutineManager : BaseCoroutine
    {
        #region ForwardConstantly
        public void InvokeForwardConstantly(
            Vector3 direction, float time = 1f,
            float moveSpeed = 1f, float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default
        )
        {
            CoroutineInstance = StartCoroutine(ForwardConstantlyCoroutine(direction, time, moveSpeed, rotateSpeed, timeType));
        }

        private IEnumerator ForwardConstantlyCoroutine(
            Vector3 direction, float time = 1f,
            float moveSpeed = 1f, float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default
        )
        {
            float preTime = Time.time;
            while (true)
            {
                transform.Forward(direction, moveSpeed, rotateSpeed, timeType);

                if (Time.time - preTime >= time)
                    Stop();
                yield return null;
            }
        }

        public void InvokeForwardConstantly(
            Transform target, float time = 1f,
            float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default
        )
        {
            CoroutineInstance = StartCoroutine(ForwardConstantlyCoroutine(target, time, rotateSpeed, timeType));
        }

        private IEnumerator ForwardConstantlyCoroutine(
            Transform target, float time = 1f,
            float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default
        )
        {
            float preTime = Time.time;
            float moveSpeed = Vector3.Distance(transform.position, target.position) / time;
            while (true)
            {
                transform.Forward(target, moveSpeed, rotateSpeed, timeType);

                if (Time.time - preTime >= time)
                    Stop();
                yield return null;
            }
        }
        #endregion
    }
}