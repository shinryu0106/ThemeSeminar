using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LibTS
{
    public static class Extension_Transform
    {
        #region Forward
        /// <summary>
        /// 指定した方向に、自分の向きを変えつつ移動する（1フレームしか実行されない）
        /// </summary>
        public static Info_Forward Forward(
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
            Vector3 rD = moveSpeed * time * direction.normalized;
            transform.position += rD;
            Quaternion preRotation = transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * time);
            
            return new Info_Forward(
                Vector3.Distance(transform.position, prePosition),
                Quaternion.Angle(transform.rotation, preRotation),
                false,
                rD
            );
        }

        /// <summary>
        /// 指定したターゲットの位置に向けて、自分の向きを変えつつ移動する（1フレームしか実行されない）
        /// </summary>
        public static Info_Forward Forward(
            this Transform transform, Transform target,
            float moveSpeed = 1f, float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default
        )
        {
            bool isGoal = transform.position == target.position;
            if (isGoal)
                return new Info_Forward(0f, 0f, true, Vector3.zero);

            var direction = target.position - transform.position;
            var i = transform.Forward(direction, moveSpeed, rotateSpeed, timeType);

            if ((target.position - transform.position).normalized != direction.normalized)
            {
                transform.position = target.position;
                direction = Vector3.zero;
            }

            return new Info_Forward(i.Distance, i.Angle, isGoal, direction);
        }
        #endregion

        #region ForwardConstantly
        private static Dictionary<MonoBehaviour, Coroutine> _dicForwardConstantlyCoroutine = new();

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