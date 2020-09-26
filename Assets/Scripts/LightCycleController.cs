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
        [SerializeField] private float speed = 100;
        public InputController controls;

        private bool engineStarted = false;
        private Rigidbody rb;



        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();

            controls = new InputController();
            controls.Enable();
            controls.Player.StartEngine.performed += StartEngine;
        }

        // Update is called once per frame
        void Update()
        {
            if (!engineStarted)
            {
            }
            else
            {
                rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Force);
            }
        }

        public void StartEngine(InputAction.CallbackContext ctx)
        {
            if (!engineStarted)
            {
                Debug.Log("Engine Starting!");

                rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);
                engineStarted = true;
            }
        }
    }
}