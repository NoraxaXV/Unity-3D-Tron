
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Tron
{
    [RequireComponent(typeof(Rigidbody))]
    public class LightCycleController : MonoBehaviour
    {

        [SerializeField] private float _maxVelocity = 10;
        private bool _engineStarted = false;
        public Rigidbody Rigidbody { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Rigidbody.maxAngularVelocity = 0;

            // controls = new InputController();
            // controls.Enable();
            // controls.Player.StartEngine.performed += OnInputStart;
            // controls.Player.Turn.performed += OnInputTurn;

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
            if (!_engineStarted || ctx.phase != InputActionPhase.Performed) return;

            var value = ctx.action.ReadValue<float>();
            Debug.Log("Turning, value = " + value.ToString() + ", ctx = " + ctx.ToString());
            Turn(value);
        }

        void Turn(float value)
        {
            transform.Rotate(0, 90 * value, 0);
            UpdateVelocity();
        }

        void UpdateVelocity()
        {
            if (Rigidbody == null) throw new MissingReferenceException("Rigidbody is null!");
            Rigidbody.velocity = transform.forward * _maxVelocity;
        }

    }
}