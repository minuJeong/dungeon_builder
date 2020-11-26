using Assets.Scripts.Game;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Scripts.MeshGen
{
    public class GenPawnSightMesh : MonoBehaviour
    {
        private const int ARC_RESOLUTION = 5;

        private Pawn _m_pawn;
        private MeshFilter _m_MeshFilter;
        private MeshRenderer _m_MeshRenderer;

        private Pawn m_Pawn
        {
            get
            {
                if (_m_pawn == null)
                {
                    _m_pawn = GetComponentInParent<Pawn>();
                }
                return _m_pawn;
            }
        }
        private MeshFilter m_MeshFilter
        {
            get
            {
                if (_m_MeshFilter == null)
                {
                    _m_MeshFilter = GetComponent<MeshFilter>();
                }
                return _m_MeshFilter;
            }
        }
        private MeshRenderer m_MeshRenderer
        {
            get
            {
                if (_m_MeshRenderer == null)
                {
                    _m_MeshRenderer = GetComponent<MeshRenderer>();
                }
                return _m_MeshRenderer;
            }
        }

        private Mesh m_GeneratedMesh;

        private void Start()
        {
            Generate();
        }

        public void Generate()
        {
            if (null == m_Pawn) { return; }

            float angle = m_Pawn.m_PawnSight.m_Angle;
            float dist = m_Pawn.m_PawnSight.m_Distance;

            List<Vector3> vertices = new List<Vector3>();
            vertices.Add(Vector3.zero);
            float angleStep = 1.0f / ARC_RESOLUTION;
            for (int i = 0; i < ARC_RESOLUTION; i++)
            {
                float x = Mathf.Cos(-angle + (angle + i * angleStep)) * dist;
                float z = Mathf.Sin(-angle + (angle + i * angleStep)) * dist;
                vertices.Add(new Vector3(x, 0.02f, z));
            }

            List<int> triangles = new List<int>();
            for (int i = 0; i < ARC_RESOLUTION - 1; i++)
            {
                triangles.AddRange(new int[] { 0, i + 2, i + 1 });
            }

            m_GeneratedMesh = new Mesh();
            m_GeneratedMesh.vertices = vertices.ToArray();
            m_GeneratedMesh.triangles = triangles.ToArray();
            m_GeneratedMesh.RecalculateNormals();
            m_GeneratedMesh.RecalculateBounds();

            m_MeshFilter.sharedMesh = m_GeneratedMesh;
        }

        public void Release()
        {
            m_MeshFilter.sharedMesh = null;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GenPawnSightMesh))]
    public class GenPawnSightMeshEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Preview"))
            {
                GenPawnSightMesh genMesh = target as GenPawnSightMesh;
                genMesh.Generate();
            }

            if (GUILayout.Button("RemovePreview"))
            {
                GenPawnSightMesh genMesh = target as GenPawnSightMesh;
                genMesh.Release();
            }
        }
    }
#endif
}