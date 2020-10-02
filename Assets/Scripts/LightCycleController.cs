
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Tron
{
    [RequireComponent(typeof(Rigidbody))]
    public class LightCycleController : MonoBehaviour
    {
        public InputController controls;


        [SerializeField] private float _maxVelocity = 10;
        private bool _engineStarted = false;
        private Rigidbody _rb;

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.maxAngularVelocity = 0;

            controls = new InputController();
            controls.Enable();
            controls.Player.StartEngine.performed += OnInputStart;
            controls.Player.Turn.performed += OnInputTurn;

        }

        public void OnInputStart(InputAction.CallbackContext ctx)
        {
            if (!_engineStarted)
            {
                Debug.Log("Engine Starting!");
                _engineStarted = true;
                UpdateVelocity();
            }
        }

        public void OnInputTurn(InputAction.CallbackContext ctx)
        {
            if (!_engineStarted) return;
            var value = ctx.action.ReadValue<float>();
            Turn(value);
        }

        void Turn(float value)
        {
            transform.Rotate(0, 90 * value, 0);
            UpdateVelocity();
        }

        void UpdateVelocity()
        {
            _rb.velocity = transform.forward * _maxVelocity;
        }

    }
}