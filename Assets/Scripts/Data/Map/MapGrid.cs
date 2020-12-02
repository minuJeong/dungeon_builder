using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data.Map
{
    [Serializable]
    public class Cell
    {
        [SerializeField] public float m_Value;
    }

    [CreateAssetMenu(order = 52)]
    public class MapGrid : ScriptableObject
    {
        [SerializeField] public int m_Width;
        [SerializeField] public int m_Height;


        [SerializeField]
        readonly public Dictionary<Vector2Int, Cell> m_Data = new Dictionary<Vector2Int, Cell>();
        private void Awake()
        {
            Reset();
        }

        public void Reset()
        {
            for (int x = 0; x < m_Width; x++)
            {
                for (int y = 0; y < m_Height; y++)
                {
                    Vector2Int coord = new Vector2Int(x, y);
                    m_Data[coord] = new Cell()
                    {
                        m_Value = UnityEngine.Random.value * 5.0f,
                    };
                }
            }
        }

        public void OnDrawGizmosSelected()
        {
            for (int x = 0; x < m_Width; x++)
            {
                for (int y = 0; y < m_Height; y++)
                {
                    Vector2Int coord = new Vector2Int(x, y);
                    if (!m_Data.ContainsKey(coord)) { continue; }

                    float height = m_Data[coord].m_Value + 0.01f;
                    Gizmos.DrawWireCube(
                        center: new Vector3(x, height * 0.5f, y),
                        size: new Vector3(0.05f, height, 0.05f)
                    );
                }
            }
        }
    }
}
