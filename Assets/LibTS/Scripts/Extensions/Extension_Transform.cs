using System;
using System.Collections;
using UnityEngine;

namespace LibTS
{
    public static class Extension_Transform
    {
        #region Forward
        /// <summary>
        /// 指定した方向に、自分の向きを変えつつ移動する（1フレームしか実行されない）
        /// </summary>
        public static Tuple<float, float> Forward(
            this Transform transform, Vector3 direction,
            float moveSpeed = 1f, float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default
        )
        {
            float time = 
                timeType == TimeType.FixedTime ? Time.fixedDeltaTime :
                timeType == TimeType.RealTime ? Time.unscaledDeltaTime :
                Time.deltaTime;
            
            Vector3 prePosition = transform.position;
            transform.position += moveSpeed * time * direction.normalized;
            Quaternion preRotation = transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * time);
            
            return new Tuple<float, float>(
                Vector3.Distance(transform.position, prePosition),
                Quaternion.Angle(transform.rotation, preRotation)
            );
        }
        
        /// <summary>
        /// 指定したターゲットの位置に向けて、自分の向きを変えつつ移動する（1フレームしか実行されない）
        /// </summary>
        public static Tuple<float, float> Forward(
            this Transform transform, Transform target,
            float moveSpeed = 1f, float rotateSpeed = 1f, float error = 0.1f,
            TimeType timeType = TimeType.Default
        )
        {
            var direction = target.position - transform.position;
            if (direction.magnitude < error)
                return new Tuple<float, float>(0f, 0f);
            return transform.Forward(direction, moveSpeed, rotateSpeed, timeType);
        }

        /// <summary>
        /// 指定した方向に、自分の向きを変えつつ移動する（1フレームしか実行されない）
        /// directionは、移動方向を示すベクトル
        /// </summary>
        public static Tuple<float, float> Forward(
            this Transform transform, Transform target, out Vector3 direction,
            float moveSpeed = 1f, float rotateSpeed = 1f, float error = 0.1f,
            TimeType timeType = TimeType.Default
        )
        {
            direction = target.position - transform.position;
            return transform.Forward(target, moveSpeed, rotateSpeed, error, timeType);
        }
        #endregion

        #region ForwardConstantly
        public static void ForwardConstantly(
            this MonoBehaviour monoBehaviour, Vector3 direction, float time,
            float moveSpeed = 1f, float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default
        )
        {
            monoBehaviour.StartCoroutine(monoBehaviour.transform.ForwardConstantlyCoroutine(direction, time, moveSpeed, rotateSpeed, timeType));
        }

        public static IEnumerator ForwardConstantlyCoroutine(
            this Transform transform, Vector3 direction, float time,
            float moveSpeed = 1f, float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default
        )
        {
            float preTime = Time.time;
            while (true)
            {
                transform.Forward(direction, moveSpeed, rotateSpeed, timeType);
                if (Time.time - preTime >= time)
                    break;
                yield return null;
                
                // timeTypeは使わないで、timeを最大時間とする
                // switch (timeType)
                // {
                //     case TimeType.FixedTime:
                //         yield return new WaitForFixedUpdate();
                //         break;
                //     case TimeType.RealTime:
                //         yield return new WaitForSecondsRealtime(time);
                //         break;
                //     default:
                //         yield return null;
                //         break;
                // }
            }
        }
        #endregion
    }
}