using System;
using UnityEngine;

namespace LibTS
{
    public abstract class CollisionChecker : MonoBehaviour
    {
        [Header("EventChecker設定")]
        [SerializeField] private EventCheckType _eventCheckType = EventCheckType.Layer;

        [SerializeField] private LayerMask _targetLayer = -1;
        private bool CheckLayer(int layer) => _targetLayer == (_targetLayer | (1 << layer));

        [SerializeField] private string[] _targetTags;
        private bool CheckTags(string tag) => Array.Exists(_targetTags, element => element == tag);

        [SerializeField] private string[] _targetNames;
        private bool CheckNames(string name) => Array.Exists(_targetNames, element => element == name);

        [SerializeField] private Transform _targetTransform;
        public Transform TargetTransform
        {
            set => _targetTransform = value;
        }
        private bool CheckDistance() => _targetTransform != null ? Vector3.Distance(_targetTransform.position, transform.position) <= _distance : false;

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

        public bool CollisionCheck(Collision other) => CollisionCheck(other.gameObject.layer, other.gameObject.tag, other.gameObject.name);

        public bool CollisionCheck(Collider other) => CollisionCheck(other.gameObject.layer, other.gameObject.tag, other.gameObject.name);

        private bool CollisionCheck(int layer, string tag = "", string name = "")
        {
            if (!_allowTrigger)
                return false;

            bool check = 
                (!_eventCheckType.HasFlag(EventCheckType.Layer) || CheckLayer(layer)) &&
                (!_eventCheckType.HasFlag(EventCheckType.Tag) || CheckTags(tag)) &&
                (!_eventCheckType.HasFlag(EventCheckType.Distance) || CheckDistance()) &&
                (!_eventCheckType.HasFlag(EventCheckType.Name) || CheckNames(name));
        
            if (Time.time < _preTime + _interval)
                return false;
            else
                _preTime = Time.time;
            
            return check;
        }
    }
}