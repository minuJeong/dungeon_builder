using Assets.Scripts.Data.Map;
using UnityEngine;

namespace Assets.Scripts.Game.Init.Map
{
    public class ImportanceMap : MonoBehaviour
    {
        [SerializeField] public MapGrid m_MapGrid;

        private void Start()
        {

        }

        private void OnDrawGizmosSelected()
        {
            m_MapGrid.OnDrawGizmosSelected();
        }
    }
}