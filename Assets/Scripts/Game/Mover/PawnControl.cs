using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Game
{
    public enum PawnState
    {
        IDLE = 0,
        WALK,
        LOOK_AROUND,
        GATHER,
        ATTACK,

        DEAD,
    }

    public class PawnControl : MonoBehaviour
    {
        public PawnState m_PawnState = PawnState.IDLE;
        public float m_StateChangedTime = 0.0f;
        public float m_ElapsedTimeInState => Time.time - m_StateChangedTime;
        public Pawn m_FollowTarget = null;
        public bool IsTargetDead() => m_FollowTarget == null || m_FollowTarget.m_PawnState == PawnState.DEAD;

        private void OnStateChange(PawnState prev, PawnState next)
        {
            m_StateChangedTime = Time.time;
        }

        public void SetState(PawnState nextState)
        {
            PawnState prevState = m_PawnState;
            m_PawnState = nextState;
            OnStateChange(prevState, nextState);
        }

        public bool IsReachedDestination(NavMeshAgent agent)
        {
            if (agent.pathPending) { return false; }
            if (agent.remainingDistance > agent.stoppingDistance) { return false; }
            return !agent.hasPath || agent.velocity.sqrMagnitude < 0.01f;
        }
    }
}
