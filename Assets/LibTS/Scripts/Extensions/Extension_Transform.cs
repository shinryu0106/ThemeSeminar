using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        private static Dictionary<Transform, CancellationTokenSource> _forwardConstantlyTasks = new();

        /// <summary>
        /// 指定した方向に、自分の向きを変えつつ指定時間だけ移動する
        /// </summary>
        public static void ForwardConstantly(
            this Transform transform, Vector3 direction, float time = 1f,
            float moveSpeed = 1f, float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default
        )
        {
            CancellationTokenSource cTS = new();
            _forwardConstantlyTasks[transform] = cTS;
            InvokeForwardConstantly(transform, direction, time, moveSpeed, rotateSpeed, timeType, cTS);
        }

        private static async void InvokeForwardConstantly(
            this Transform transform, Vector3 direction, float time = 1f,
            float moveSpeed = 1f, float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default,
            CancellationTokenSource cTS = null
        )
        {
            float preTime = Time.time;
            while (true)
            {
                transform.Forward(direction, moveSpeed, rotateSpeed, timeType);

                if (Time.time - preTime >= time || cTS.Token.IsCancellationRequested)
                    return;
                await Task.Yield();
            }

        }

        /// <summary>
        /// 指定したターゲットの位置に向けて、自分の向きを変えつつ指定時間内に到着するように移動する
        /// </summary>
        public static void ForwardConstantly(
            this Transform transform, Transform target, float time = 1f,
            float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default
        )
        {
            CancellationTokenSource cTS = new();
            _forwardConstantlyTasks[transform] = cTS;
            InvokeForwardConstantly(transform, target, time, rotateSpeed, timeType, cTS);
        }

        private static async void InvokeForwardConstantly(
            this Transform transform, Transform target, float time = 1f,
            float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default,
            CancellationTokenSource cTS = null
        )
        {
            float preTime = Time.time;
            float moveSpeed = Vector3.Distance(transform.position, target.position) / time;
            while (true)
            {
                transform.Forward(target, moveSpeed, rotateSpeed, timeType);

                if (Time.time - preTime >= time || cTS.Token.IsCancellationRequested)
                    return;
                await Task.Yield();
            }
        }

        /// <summary>
        /// ForwardConstantlyの停止（種類不問）
        /// </summary>
        public static void StopForwardConstantly(this Transform transform)
        {
            if (_forwardConstantlyTasks.ContainsKey(transform))
            {
                _forwardConstantlyTasks[transform].Cancel();
                _forwardConstantlyTasks.Remove(transform);
            }
            else
                Debug.LogWarning("開始されているForwardConstantlyが存在しません。");
        }
        #endregion
    }
}