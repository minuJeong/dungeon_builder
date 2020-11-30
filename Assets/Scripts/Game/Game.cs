using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class PawnManager
    {
        private static PawnManager _Instance = null;
        public static PawnManager Instance
        {
            get
            {
                if (null == _Instance) { _Instance = new PawnManager(); }
                return _Instance;
            }
        }
        readonly public Dictionary<Team, List<Pawn>> m_Pawns = new Dictionary<Team, List<Pawn>>();

        private PawnManager() { }

        public List<Pawn> GetFriends(Team myTeam) => m_Pawns[myTeam];
        public List<Pawn> GetEnemies(Team myTeam)
        {
            List<Pawn> enemies = new List<Pawn>();
            foreach (KeyValuePair<Team, List<Pawn>> teamPawns in m_Pawns)
            {
                if (teamPawns.Key == myTeam) { continue; }
                enemies.AddRange(teamPawns.Value);
            }
            return enemies;
        }
    }

    public class Game : MonoBehaviour
    {
        private PawnsMover m_PawnsMover;

        private void Start()
        {
            m_PawnsMover = new PawnsMover();
            StartCoroutine(m_PawnsMover.ThinkCoroutine());
        }
    }
}
