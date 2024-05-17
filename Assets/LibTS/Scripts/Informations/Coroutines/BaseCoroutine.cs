using UnityEngine;

namespace LibTS
{
    [AddComponentMenu("")]
    public class BaseCoroutine : MonoBehaviour
    {
        private Coroutine _coroutine;
        protected Coroutine CoroutineInstance { set { _coroutine = value; } }

        public void Stop()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            Destroy(this);
        }
    }
}