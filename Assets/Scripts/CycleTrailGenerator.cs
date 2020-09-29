using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tron
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class CycleTrailGenerator : MonoBehaviour
    {
        public Material trailMat;
        public Transform emissionCenter;

        public float trailWidth = 2.0f;
        public float trailHeight = 5.0f;
        public float dissolveSpeed = 3.0f;
        public float detailConstant = 3.0f;

        private bool _updateTrail = true;

        private Mesh _trail;
        private MeshFilter _filter;
        private MeshRenderer _renderer;
        private MeshCollider _collider;

        private List<Vector3> _verts;
        private List<int> _tris;

        private float _trailCount = 0;

        private Vector3 TopVert => new Vector3(
                emissionCenter.position.x,
                emissionCenter.position.y + trailWidth / 2,
                emissionCenter.position.z
            );
        private Vector3 BottomVert => new Vector3(
                        emissionCenter.position.x,
                        emissionCenter.position.y - trailWidth / 2,
                        emissionCenter.position.z
                    );

        // private BoxCollider[] colliders;

        private void Start()
        {
            _filter = GetComponent<MeshFilter>();
            _renderer = GetComponent<MeshRenderer>();
            _collider = GetComponent<MeshCollider>();

            _trail = new Mesh();

            _verts = new List<Vector3>()
            {
                TopVert,
                BottomVert
            };
            _tris = new List<int>();


            _filter.sharedMesh = _trail;
            _renderer.sharedMaterial = trailMat;
        }

        private void Update()
        {
            if (_updateTrail) UpdateTrail();
        }



        public void UpdateTrail()
        {
            var bottom = BottomVert;
            var top = TopVert;
            if (Vector3.Distance(top, _verts[_verts.Count - 1]) <= 0.01) return;
            
            _trailCount += Time.deltaTime;
            float curveTime = GetCurveTime(bottom);

            if (_trailCount >= curveTime)
            {
                CutTrail(top, bottom);
                _trailCount = 0;
            }
            else
            {
                ExtendTrail(top, bottom);
            }

            _trail.vertices = _verts.ToArray();
            if (_tris.Count >= 3)
            {
                _trail.triangles = _tris.ToArray();
                _trail.RecalculateNormals();
            }


            }

        private void ExtendTrail(Vector3 top, Vector3 bottom)
        {
            _verts[_verts.Count - 1] = bottom;
            _verts[_verts.Count - 2] = top;
        }

        private void CutTrail(Vector3 top, Vector3 bottom)
        {
            _verts.Add(top);
            _verts.Add(bottom);
            if (_verts.Count < 3) return;

            // First Face
            var c = _verts.Count;
            _tris.Add(c);
            _tris.Add(c - 3);
            _tris.Add(c - 1);

            // Second Face
            _tris.Add(c);
            _tris.Add(c - 2);
            _tris.Add(c - 3);
        }

        private float GetCurveTime(Vector3 newBottomPoint)
        {
            if (_verts.Count < 3) return detailConstant;
            var lastPoint = _verts[_verts.Count - 1];

            // Vector from the point to the cycle
            var vectorToCycle = newBottomPoint - lastPoint;

            // Current direction beam is traveling
            var vectorOfBeam = lastPoint - _verts[_verts.Count - 3];

            var angle = Vector3.Angle(vectorToCycle, vectorOfBeam);
            if (angle == 0) angle = 0.001f; // Prevent DivideByZero ex

            return detailConstant / angle;
        }

    }
}
