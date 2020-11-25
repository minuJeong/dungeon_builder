using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Game
{
    public class PawnsMover
    {
        const float RANDOM_STRAY_RANGE = 5.0f;
        const float LOOK_AROUND_TIME = 0.5f;
        const float GATHER_DISTANCE = 5.0f;
        const float MELEE_DISTANCE = 3.0f;

        private PawnManager m_PawnManager = PawnManager.Instance;

        private void OnIdle(Pawn pawn)
        {
            Vector3 pos = pawn.transform.position;
            pawn.m_Agent.SetDestination(pos + (Random.insideUnitSphere * RANDOM_STRAY_RANGE));
            pawn.SetState(PawnState.WALK);
        }

        private void OnWalk(Pawn pawn)
        {
            if (!pawn.IsReachedDestination()) { return; }

            switch (Random.Range(0, 2))
            {
                case 0:
                    pawn.SetState(PawnState.IDLE);
                    break;

                case 1:
                    pawn.SetState(PawnState.LOOK_AROUND);
                    break;
            }
        }

        private void OnLookAround(Pawn pawn)
        {
            if (pawn.m_ElapsedTimeInState < LOOK_AROUND_TIME) { return; }

            pawn.SetState(PawnState.IDLE);
        }

        private void OnAttack(Pawn pawn)
        {
            if (pawn.m_FollowTarget == null || pawn.m_FollowTarget.m_PawnState == PawnState.DEAD)
            {
                pawn.SetState(PawnState.IDLE);
                return;
            }

            Vector3 delta = pawn.transform.position - pawn.m_FollowTarget.transform.position;
            if (delta.sqrMagnitude < MELEE_DISTANCE * MELEE_DISTANCE + 0.1f)
            {
                pawn.transform.LookAt(pawn.m_FollowTarget.transform.position);
            }
            else
            {
                pawn.m_Agent.SetDestination(pawn.m_FollowTarget.transform.position + (delta).normalized * MELEE_DISTANCE);
            }
        }

        private void ReactWhenAlone(Pawn pawn)
        {
            switch (pawn.m_PawnState)
            {
                case PawnState.IDLE:
                    OnIdle(pawn);
                    break;

                case PawnState.WALK:
                    OnWalk(pawn);
                    break;

                case PawnState.LOOK_AROUND:
                    OnLookAround(pawn);
                    break;
            }
        }

        private void ReactToVisibleFriends(Pawn pawn, List<Pawn> visibleFriends)
        {
            switch (pawn.m_PawnState)
            {
                default:
                    break;

            }
        }

        private void ReactToVisibleEnemies(Pawn pawn, List<Pawn> visibleEnemies)
        {
            switch (pawn.m_PawnState)
            {
                case PawnState.ATTACK:
                    OnAttack(pawn);
                    break;

                default:
                    Pawn closestVisibleEnemy = visibleEnemies.OrderBy((p) => (pawn.transform.position - p.transform.position).sqrMagnitude).FirstOrDefault();
                    pawn.SetTarget(closestVisibleEnemy);
                    pawn.m_Agent.SetDestination(closestVisibleEnemy.transform.position);
                    pawn.SetState(PawnState.ATTACK);
                    break;
            }
        }

        private void PawnThink(Pawn pawn, List<Pawn> friends, List<Pawn> enemies)
        {
            List<Pawn> visibleEnemies = enemies.FindAll((e) => pawn.CheckVisible(e.transform.position));
            if (visibleEnemies.Count > 0)
            {
                ReactToVisibleEnemies(pawn, visibleEnemies);
            }
            else
            {
                ReactWhenAlone(pawn);
            }
        }

        public IEnumerator ThinkCoroutine()
        {
            yield return null;

            bool escapeEnumerator = true;
            while (escapeEnumerator)
            {
                Dictionary<Team, List<Pawn>>.Enumerator en = m_PawnManager.m_Pawns.GetEnumerator();
                while (en.MoveNext())
                {
                    en.Current.Value.ForEach((pawn) => PawnThink(pawn, en.Current.Value, m_PawnManager.GetEnemies(en.Current.Key)));
                }

                yield return null;
            }
        }
    }
}
