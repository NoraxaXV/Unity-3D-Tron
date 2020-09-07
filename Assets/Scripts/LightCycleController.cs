using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LightCycleController : MonoBehaviour
{
    [SerializeField] private float speed = 100;
    private bool engineStarted = false;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!engineStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Engine Starting!");
                rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);
                engineStarted = true;
            }
        }
        else {
            rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Force);
        }
    }
}
