using System;
using UnityEngine;

namespace LibTS
{
    [Serializable]
    public struct Info_Transform
    {
        public Info_Transform(Vector3 position = default, Quaternion rotation = default, Vector3 scale = default)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public Info_Transform(Vector3 position = default, Vector3 eulerAngles = default, Vector3 scale = default)
        {
            this.position = position;
            this.rotation = Quaternion.Euler(eulerAngles);
            this.scale = scale;
        }

        public Info_Transform(Transform transform)
        {
            this.position = transform.position;
            this.rotation = transform.rotation;
            this.scale = transform.localScale;
        }

        [SerializeField] private Vector3 position;
        public readonly Vector3 Position { get { return position; } }
        [SerializeField] private Quaternion rotation;
        public readonly Quaternion Rotation { get { return rotation; } }
        [SerializeField] private Vector3 scale;
        public readonly Vector3 Scale { get { return scale; } }
    }
}