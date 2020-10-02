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
        [Range(0, 1)] public float dissolveSpeed = 3.0f;
        public float detailConstant = 3.0f;

        private bool _updateTrail = true;

        private Mesh _trail;
        private MeshFilter _filter;
        private MeshRenderer _renderer;
        private MeshCollider _collider;

        private List<Vector3> _verts;
        private List<int> _tris;

        private float _trailCount = 0;


        // private BoxCollider[] colliders;

        private void Start()
        {
            _filter = GetComponent<MeshFilter>();
            _renderer = GetComponent<MeshRenderer>();
            _collider = GetComponent<MeshCollider>();

            _trail = new Mesh();

            _verts = new List<Vector3>()
            {
                GetTopVertex(),
                GetBottomVertex()
            };
            _tris = new List<int>();

            

            _filter.sharedMesh = _trail;
            _renderer.sharedMaterial = trailMat;
            _collider.sharedMesh = _trail;

            _trail.vertices = _verts.ToArray();
            _trail.triangles = _tris.ToArray();

            CutTrail(_verts[0], _verts[1]);
        }

        private void Update()
        {
            if (_updateTrail) UpdateTrail();
        }

        Vector3 GetTopVertex() => new Vector3(
                emissionCenter.position.x,
                emissionCenter.position.y + trailHeight / 2,
                emissionCenter.position.z
            );

        Vector3 GetBottomVertex() => new Vector3(
                        emissionCenter.position.x,
                        emissionCenter.position.y - trailHeight / 2,
                        emissionCenter.position.z
                    );
        public void UpdateTrail()
        {
            var bottom = GetBottomVertex();
            var top = GetTopVertex();

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

            _collider.sharedMesh = _trail;
        }

        private void ExtendTrail(Vector3 top, Vector3 bottom)
        {
            _verts[_verts.Count - 1] = bottom;
            _verts[_verts.Count - 2] = top;
            _trail.vertices = _verts.ToArray();

            _trail.RecalculateNormals();
            _trail.RecalculateBounds();
            
        }

        private void CutTrail(Vector3 top, Vector3 bottom)
        {
            _verts.Add(top);
            _verts.Add(bottom);

            _trail.vertices = _verts.ToArray();

            // First Face
            var c = _verts.Count - 1;
            _tris.Add(c);
            _tris.Add(c - 3);
            _tris.Add(c - 1);

            // Second Face
            _tris.Add(c);
            _tris.Add(c - 2);
            _tris.Add(c - 3);

            _trail.triangles = _tris.ToArray();

            _trail.RecalculateNormals();
            _trail.RecalculateBounds();

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

        // WIP
        private void LerpTrail(int index)
        {
            _verts[index] = Vector3.Lerp(_verts[index], _verts[index + 2], dissolveSpeed * Time.deltaTime);
            _verts[index + 1] = Vector3.Lerp(_verts[index + 1], _verts[index + 3], dissolveSpeed * Time.deltaTime);

            // Merge the two points if they are too close
            if (Vector3.Distance(_verts[index], _verts[index + 2]) <= 0.01 || Vector3.Distance(_verts[index + 1], _verts[index + 3]) <= 0.01)
            {
                _verts.Remove(_verts[index]);
                _verts.Remove(_verts[index + 1]);

                foreach (var i in _tris.FindAll((int i) => i == index))
                {

                }
            }

        }

    }
}
