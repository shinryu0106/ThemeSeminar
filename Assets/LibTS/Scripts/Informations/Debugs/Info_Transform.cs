using UnityEngine;

namespace LibTS
{
    public readonly struct Info_Transform
    {
        public Info_Transform(Vector3 r_position = default, Quaternion r_rotation = default, Vector3 r_scale = default)
        {
            this.r_position = r_position;
            this.r_rotation = r_rotation;
            this.r_scale = r_scale;
        }

        private readonly Vector3 r_position;
        public readonly Vector3 Position { get { return r_position; } }
        private readonly Quaternion r_rotation;
        public readonly Quaternion Rotation { get { return r_rotation; } }
        private readonly Vector3 r_scale;
        public readonly Vector3 Scale { get { return r_scale; } }
    }
}