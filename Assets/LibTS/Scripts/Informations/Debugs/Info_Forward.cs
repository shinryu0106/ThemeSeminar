using UnityEngine;

namespace LibTS
{
    public readonly struct Info_Forward
    {
        public Info_Forward(float r_dist = 0f, float r_angle = 0f, bool r_isGoal = false, Vector3 r_dict = default)
        {
            this.r_dist = r_dist;
            this.r_angle = r_angle;
            this.r_isGoal = r_isGoal;
            this.r_dict = r_dict;
        }

        private readonly float r_dist;
        public readonly float Distance { get { return r_dist; } }
        private readonly float r_angle;
        public readonly float Angle { get { return r_angle; } }
        private readonly bool r_isGoal;
        public readonly bool IsGoal { get { return r_isGoal; } }
        private readonly Vector3 r_dict;
        public readonly Vector3 Direction { get { return r_dict; } }
    }
}