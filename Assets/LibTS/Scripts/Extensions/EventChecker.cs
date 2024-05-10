using System;
using UnityEngine;

namespace LibTS
{
    public abstract class EventChecker : MonoBehaviour
    {
        [Header("Event Checker Settings")]
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private string[] _targetTags;
        private bool CheckTags(string tag) => Array.Exists(_targetTags, element => element == tag);
        [SerializeField] private string[] _targetNames;
        private bool CheckNames(string name) => Array.Exists(_targetNames, element => element == name);
        [SerializeField] private Transform _targetTransform;
        public Transform TargetTransform
        {
            set => _targetTransform = value;
        }
        [SerializeField] private CheckType _checkType = CheckType.None;
        private enum CheckType
        {
            None,
            Layer,
            Tag,
            LayerAndTag,
            Distance,
            Name,
            
        }

        [SerializeField] private float _distance = 1f;

        [SerializeField] private float _interval = 1f;
        private float _preTime = 0f;

        [SerializeField] private bool _allowTrigger = true;
        public bool AllowTrigger
        {
            set => _allowTrigger = value;
        }

        protected virtual void Awake()
        {
            _preTime = -_interval;
        }

        protected virtual void OnCollisionEnter(Collision other){}
        protected virtual void OnCollisionStay(Collision other){}
        protected virtual void OnCollisionExit(Collision other){}
        protected virtual void OnTriggerEnter(Collider other){}
        protected virtual void OnTriggerStay(Collider other){}
        protected virtual void OnTriggerExit(Collider other){}

        public bool ColliderEventAllowCheck(Collider other)
        {
            return EventAllowCheck(other.gameObject.layer, other.gameObject.tag, other.gameObject.name);
        }

        public bool DistanceEventAllowCheck()
        {
            return EventAllowCheck(0, "", "");
        }

        private bool EventAllowCheck(LayerMask layerMask, string tag, string name)
        {
            if (!_allowTrigger)
                return false;

            switch (_checkType)
            {
                case CheckType.Layer:
                    if (layerMask != _targetLayer)
                        return false;
                    break;
                case CheckType.Tag:
                    if (!CheckTags(tag))
                        return false;
                    break;
                case CheckType.LayerAndTag:
                    if (layerMask != _targetLayer || !CheckTags(tag))
                        return false;
                    break;
                case CheckType.Distance:
                    if (_targetTransform != null)
                    {
                        if (Vector3.Distance(_targetTransform.position, transform.position) > _distance)
                            return false;
                    }
                    // else if (PlayerManager.Instance.GetMainPlayerTransform() != null)
                    // {
                    //     if (Vector3.Distance(PlayerManager.Instance.GetMainPlayerTransform().position, transform.position) > _distance)
                    //         return false;
                    // }
                    else
                    {
                        return false;
                    }
                    break;
                case CheckType.Name:
                    if (!CheckNames(name))
                        return false;
                    break;
            }
        
            if (Time.time < _preTime + _interval)
                return false;
            else
                _preTime = Time.time;
            
            return true;
        }
    }

    public enum ColliderEventType
    {
        Enter,
        Stay,
        Exit
    }
}