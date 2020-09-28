using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tron
{
    [RequireComponent(typeof(Rigidbody))]
    public class LightCycleController : MonoBehaviour
    {
        public InputController controls;


        private Rigidbody rb;

        [SerializeField] private float _maxVelocity = 100;
        [SerializeField] private float _acceleration = 100;

        private Vector3 _currentDirection = new Vector3();
        private bool _engineStarted = false;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();

            controls = new InputController();
            controls.Enable();
            controls.Player.StartEngine.performed += StartEngine;
            controls.Player.Turn.performed += TurnCycle;

        }

        // Update is called once per frame
        void Update()
        {
            if (_engineStarted)
            {
                if (rb.velocity.sqrMagnitude < _maxVelocity*_maxVelocity)
                {
                    rb.velocity +=  _currentDirection * _acceleration * Time.deltaTime;
                }

            }
        }

        public void StartEngine(InputAction.CallbackContext ctx)
        {
            if (!_engineStarted)
            {
                Debug.Log("Engine Starting!");

                _currentDirection = transform.forward;
                transform.forward = _currentDirection;

                _engineStarted = true;
            }
        }

        public void TurnCycle(InputAction.CallbackContext ctx)
        {
            if (!_engineStarted) return;
            var value = ctx.action.ReadValue<float>();
            transform.RotateAround(transform.localToWorldMatrix*rb.centerOfMass, transform.up, 90);
            rb.velocity = new Vector3();
            _currentDirection = transform.forward;
        }

    }
}