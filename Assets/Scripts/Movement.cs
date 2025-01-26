using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float maxSpeed = 50f;
    [SerializeField] float acceleration = 20f;
    [SerializeField] float deceleration = 10f;
    [SerializeField] float rotationSpeed = 129f;
    [SerializeField] float gravityScale = 0.02f; // Serialized variable to control gravity

    private float currentSpeed = 0f;
    private Rigidbody rb;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // Disable default gravity
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thruster();
        Rotation();
        ApplyCustomGravity();
    }

    void Thruster()
    {
        // Move object in the object's y axis
        if (Input.GetKey(KeyCode.Space))
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            currentSpeed -= deceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
            audioSource.Stop();
        }

        transform.Translate(0f, currentSpeed * Time.deltaTime, 0f);
    }


    void Rotation()
    {
        rb.freezeRotation = true;
        // Rotate object in the object's Z axis
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
        {
            transform.Rotate(Vector3.back * Time.deltaTime * rotationSpeed);
        }
        rb.freezeRotation = false;
    }

    void ApplyCustomGravity()
    {
        if (rb != null)
        {
            Vector3 gravity = Physics.gravity * gravityScale;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }
}
