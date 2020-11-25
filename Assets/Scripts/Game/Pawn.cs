using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Assets.Scripts.Game
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PawnControl))]
    public class Pawn : MonoBehaviour
    {
        // public serialized:
        [SerializeField] public Team m_Team;
        [SerializeField] public PawnData m_PawnData;
        [SerializeField] public PawnSight m_PawnSight;

        // public
        public PawnState m_PawnState => m_PawnControl.m_PawnState;
        public float m_ElapsedTimeInState => m_PawnControl.m_ElapsedTimeInState;
        public Pawn m_FollowTarget => m_PawnControl.m_FollowTarget;
        public NavMeshAgent m_Agent => m_NavMeshAgent;

        public bool CheckVisible(Vector3 pos) => m_PawnSight.CheckVisible(pos);
        public bool IsReachedDestination() => m_PawnControl.IsReachedDestination(m_NavMeshAgent);
        public bool IsTargetDead() => m_PawnControl.IsTargetDead();

        // private:
        private PawnControl m_PawnControl;

        private NavMeshAgent m_NavMeshAgent;

        private void Awake()
        {
            m_PawnSight.SetOwner(this);
            if (!PawnManager.Instance.m_Pawns.ContainsKey(m_Team))
            {
                PawnManager.Instance.m_Pawns[m_Team] = new List<Pawn>();
            }

            if (!PawnManager.Instance.m_Pawns[m_Team].Contains(this))
            {
                PawnManager.Instance.m_Pawns[m_Team].Add(this);
            }

            m_PawnControl = GetComponent<PawnControl>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void OnDestroy()
        {
            if (PawnManager.Instance.m_Pawns[m_Team].Contains(this))
            {
                PawnManager.Instance.m_Pawns[m_Team].Remove(this);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) { return; }

            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            Handles.Label(
                transform.position,
                string.Format("state: {0} / walking[{1}]",
                    m_PawnState,
                    !IsReachedDestination()
            ));

            switch (m_PawnState)
            {
                case PawnState.WALK:
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(transform.position, m_Agent.destination);
                    break;

                case PawnState.GATHER:
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(transform.position, m_Agent.destination);
                    break;
            }
        }
#endif

        private void Update()
        {
            Debug.DrawRay(transform.position, transform.forward, Color.red);
            switch (m_PawnState)
            {
                case PawnState.WALK:
                    Debug.DrawLine(transform.position, m_Agent.destination, Color.green);
                    break;

                case PawnState.GATHER:
                    Debug.DrawLine(transform.position, m_Agent.destination, Color.blue);
                    break;
            }
        }

        public void SetState(PawnState nextState)
        {
            m_PawnControl.SetState(nextState);
        }

        public void SetTarget(Pawn target)
        {
            m_PawnControl.m_FollowTarget = target;
        }
    }
}