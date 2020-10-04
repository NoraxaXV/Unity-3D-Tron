using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Tron
{
    public class PlayerManager : MonoBehaviour
    {
        public int id;
        public Color color;

        public CycleTrailGenerator trail;
        public LightCycleController mover;
        public Camera cameraBrain;
        public CinemachineVirtualCamera virtualCamera;

        // Start is called before the first frame update
        void Start()
        {


        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}