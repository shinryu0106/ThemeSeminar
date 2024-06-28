using LibTS;
using UnityEngine;

public class Sample_CollisionCheck : CollisionChecker
{
    [Header("Trigger設定")]
    [SerializeField] private bool _isTriggerEnter = true;
    [SerializeField] private bool _isTriggerStay = false;
    [SerializeField] private bool _isTriggerExit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggerEnter)
            if (CollisionCheck(other))
                Debug.Log("OnTriggerEnter");
            else
                Debug.Log("OnTriggerEnter Failed");
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isTriggerStay)
            if (CollisionCheck(other))
                Debug.Log("OnTriggerStay");
            else
                Debug.Log("OnTriggerStay Failed");
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isTriggerExit)
            if (CollisionCheck(other))
                Debug.Log("OnTriggerExit");
            else
                Debug.Log("OnTriggerExit Failed");
    }
}
