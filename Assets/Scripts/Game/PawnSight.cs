using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game
{
    [Serializable]
    public class PawnSight
    {
        [HideInInspector] public Pawn m_Owner;
        [SerializeField] public float m_Distance;
        [SerializeField] public float m_HeightDistance;
        [SerializeField] public float m_Angle;

        public void SetOwner(Pawn owner) { m_Owner = owner; }

        public bool CheckVisible(Vector3 position)
        {
            if (null == m_Owner) { return false; }

            Vector3 delta = m_Owner.transform.position - position;
            if (delta.x * delta.x + delta.z * delta.z > m_Distance * m_Distance) { return false; }
            if (delta.y * delta.y > m_HeightDistance * m_HeightDistance) { return false; }

            float relAngle = Mathf.Atan2(delta.z, delta.x);
            float forwardAngle = Mathf.Atan2(-m_Owner.transform.forward.z, m_Owner.transform.forward.x);

            if (Mathf.Abs(relAngle - forwardAngle) > m_Angle) { return false; }
            return true;
        }
    }
}
