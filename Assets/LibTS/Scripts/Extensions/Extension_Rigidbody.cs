using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace LibTS
{
    public static class Extension_Rigidbody
    {
        #region Forward
        /// <summary>
        /// 指定した方向に、自分の向きを変えつつ移動する（1フレームしか実行されない）
        /// </summary>
        public static Info_Forward Forward(
            this Rigidbody rigidbody, Vector3 direction,
            float moveSpeed = 1f, float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default, ForceMode forceMode = ForceMode.Force
        )
        {
            float time =
                timeType == TimeType.FixedTime ? Time.fixedDeltaTime :
                timeType == TimeType.RealTime ? Time.unscaledDeltaTime :
                Time.deltaTime;

            Vector3 prePosition = rigidbody.transform.position;
            Vector3 rD = moveSpeed * time * direction.normalized;
            rigidbody.AddForce(rD, forceMode);
            Quaternion preRotation = rigidbody.transform.rotation;
            rigidbody.transform.rotation = Quaternion.Slerp(rigidbody.transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * time);

            return new Info_Forward(
                Vector3.Distance(rigidbody.transform.position, prePosition),
                Quaternion.Angle(rigidbody.transform.rotation, preRotation),
                false,
                rD
            );
        }

        /// <summary>
        /// 指定したターゲットの位置に向けて、自分の向きを変えつつ移動する（1フレームしか実行されない）
        /// </summary>
        /// <remarks>
        /// errorは目標地点に到達したとみなす誤差
        /// </remarks>
        public static Info_Forward Forward(
            this Rigidbody rigidbody, Transform target,
            float moveSpeed = 1f, float rotateSpeed = 1f, float error = 0.1f,
            TimeType timeType = TimeType.Default, ForceMode forceMode = ForceMode.Force
        )
        {
            bool isGoal = Vector3.Distance(rigidbody.transform.position, target.position) <= error;
            if (isGoal)
                return new Info_Forward(0f, 0f, true, Vector3.zero);

            var direction = target.position - rigidbody.transform.position;
            var i = rigidbody.Forward(direction, moveSpeed, rotateSpeed, timeType, forceMode);

            if ((target.position - rigidbody.transform.position).normalized != direction.normalized)
            {
                rigidbody.transform.position = target.position;
                direction = Vector3.zero;
            }

            return new Info_Forward(i.Distance, i.Angle, isGoal, direction);
        }
        #endregion

        #region ForwardConstantly
        private static Dictionary<Rigidbody, CancellationTokenSource> _forwardConstantlyTasks = new();

        /// <summary>
        /// 指定した方向に、自分の向きを変えつつ指定時間だけ移動する
        /// </summary>
        public static void ForwardConstantly(
            this Rigidbody rigidbody, Vector3 direction, float time = 1f,
            float moveSpeed = 1f, float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default, ForceMode forceMode = ForceMode.Force
        )
        {
            CancellationTokenSource cTS = new();
            _forwardConstantlyTasks[rigidbody] = cTS;
            InvokeForwardConstantly(rigidbody, direction, time, moveSpeed, rotateSpeed, timeType, forceMode, cTS);
        }

        private static async void InvokeForwardConstantly(
            this Rigidbody rigidbody, Vector3 direction, float time = 1f,
            float moveSpeed = 1f, float rotateSpeed = 1f,
            TimeType timeType = TimeType.Default, ForceMode forceMode = ForceMode.Force,
            CancellationTokenSource cTS = null
        )
        {
            float preTime = Time.time;
            while (true)
            {
                rigidbody.Forward(direction, moveSpeed, rotateSpeed, timeType, forceMode);

                if (Time.time - preTime >= time || cTS.Token.IsCancellationRequested)
                    return;
                await Task.Yield();
            }
        }

        /// <summary>
        /// 指定したターゲットの位置に向けて、自分の向きを変えつつ指定時間内に到着するように移動する
        /// </summary>
        /// <remarks>
        /// errorは目標地点に到達したとみなす誤差
        /// </remarks>
        public static void ForwardConstantly(
            this Rigidbody rigidbody, Transform target, float time = 1f,
            float moveSpeed = 1f, float rotateSpeed = 1f, float error = 0.1f,
            TimeType timeType = TimeType.Default, ForceMode forceMode = ForceMode.Force
        )
        {
            CancellationTokenSource cTS = new();
            _forwardConstantlyTasks[rigidbody] = cTS;
            InvokeForwardConstantly(rigidbody, target, time, moveSpeed, rotateSpeed, error, timeType, forceMode, cTS);
        }

        private static async void InvokeForwardConstantly(
            this Rigidbody rigidbody, Transform target, float time = 1f,
            float moveSpeed = 1f, float rotateSpeed = 1f, float error = 0.1f,
            TimeType timeType = TimeType.Default, ForceMode forceMode = ForceMode.Force,
            CancellationTokenSource cTS = null
        )
        {
            float preTime = Time.time;
            while (true)
            {
                rigidbody.Forward(target, moveSpeed, rotateSpeed, error, timeType, forceMode);

                if (Time.time - preTime >= time || cTS.Token.IsCancellationRequested)
                    return;
                await Task.Yield();
            }
        }

        /// <summary>
        /// ForwardConstantlyの停止（種類不問）
        /// </summary>
        public static void StopForwardConstantly(this Rigidbody rigidbody)
        {
            if (_forwardConstantlyTasks.ContainsKey(rigidbody))
            {
                _forwardConstantlyTasks[rigidbody].Cancel();
                _forwardConstantlyTasks.Remove(rigidbody);
            }
            else
                Debug.LogWarning("開始されているForwardConstantlyが存在しません。");
        }
        #endregion
    }
}