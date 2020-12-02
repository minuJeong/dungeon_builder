using Assets.Scripts.Game.Init.Map;
using UnityEngine;

namespace Assets.Scripts.Game.Init
{
    public class InitMap : MonoBehaviour
    {
        [SerializeField] public ImportanceMap m_ImportMap;

        private void Awake()
        {
            Debug.Assert(null != m_ImportMap);
        }
    }
}