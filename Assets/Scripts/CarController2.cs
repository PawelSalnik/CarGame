using UnityEngine;
using UnityEngine.UI;

public class CarController2 : MonoBehaviour
{
    [Header("Car Settings")]
    public float accelerationSpeed = 5f;
    public float turnSpeed = 5f;
    public int maxGears = 5;

    [Header("Physics Settings")]
    public float gravity = 9.81f;
    public float downForce = 10f;
    public float suspensionDistance = 0.2f;

    [Header("UI Elements")]
    public Text speedLabel;
    public Text gearLabel;

    [System.Serializable]
    public class GearSettings
    {
        public float maxSpeed = 50f;
    }

    public GearSettings[] gears;

    private int currentGear = 1;
    private float currentSpeed = 0f;
    private bool isReversing = false;

    public WheelCollider[] wheelColliders;
    public Transform[] wheelTransforms;

    public GameObject reverseTriggerObject;
    public GameObject groundObject;
    public GameObject secondGroundObject;

    private Rigidbody carRigidbody;

    private void Start()
    {
        InitializeComponents();
        AdjustWheelFrictions();
    }

    private void InitializeComponents()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        float horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }

        transform.Rotate(Vector3.up, horizontalInput * turnSpeed * Time.deltaTime);

        float steerAngle = horizontalInput * turnSpeed;
        wheelColliders[0].steerAngle = steerAngle;
        wheelColliders[1].steerAngle = steerAngle;

        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeGear(1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeGear(-1);
        }

        Accelerate();
        ApplyDownForce();
        UpdateLabels();
        UpdateWheels();
    }

    private void Accelerate()
    {
        float acceleration = accelerationSpeed * Time.deltaTime;
        acceleration *= isReversing ? -1f : 1f;

        float maxSpeedForGear = currentGear > 0 ? gears[currentGear - 1].maxSpeed : Mathf.Abs(10f);
        float targetSpeed = Mathf.Min(currentSpeed + acceleration, maxSpeedForGear);

        if (isReversing && targetSpeed < -Mathf.Abs(10f))
        {
            targetSpeed = -Mathf.Abs(10f);
        }

        float gravityForce = gravity * carRigidbody.mass;
        float additionalGravity = Mathf.Clamp(targetSpeed, 0f, maxSpeedForGear) * downForce;
        gravityForce += additionalGravity;

        float totalForce = gravityForce * Time.deltaTime;

        RaycastHit hit;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, suspensionDistance);

        if (isGrounded && (hit.collider.gameObject == groundObject || hit.collider.gameObject == secondGroundObject))
        {
            carRigidbody.AddForce(Vector3.down * totalForce, ForceMode.Acceleration);
        }

        transform.Translate(Vector3.forward * targetSpeed * Time.deltaTime);
        currentSpeed = targetSpeed;
    }

    private void ApplyDownForce()
    {
        float gravityForce = gravity * carRigidbody.mass;
        float downForceMagnitude = gravityForce * downForce;
        carRigidbody.AddForce(Vector3.down * downForceMagnitude);
    }

    private void ChangeGear(int direction)
    {
        int newGear = Mathf.Clamp(currentGear + direction, -1, maxGears);

        if (newGear != currentGear)
        {
            if (newGear == 0)
            {
                newGear = (direction > 0) ? 1 : -1;
            }

            currentGear = newGear;
            isReversing = (currentGear == -1);

            if (!isReversing)
            {
                currentSpeed = Mathf.Clamp(currentSpeed, 0.1f, Mathf.Infinity);
            }
        }
    }

    private void UpdateLabels()
    {
        speedLabel.text = Mathf.RoundToInt(Mathf.Abs(currentSpeed)) + " km/h";
        gearLabel.text = (isReversing ? "R" : currentGear.ToString());
    }

    private void UpdateWheels()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            UpdateWheelPos(wheelColliders[i], wheelTransforms[i]);
        }
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }

    private void AdjustWheelFriction(WheelCollider wheelCollider)
    {
        WheelFrictionCurve sidewaysFriction = wheelCollider.sidewaysFriction;
        sidewaysFriction.stiffness = 2f; // Increase stiffness for better lateral grip
        wheelCollider.sidewaysFriction = sidewaysFriction;

        WheelFrictionCurve forwardFriction = wheelCollider.forwardFriction;
        forwardFriction.stiffness = 2f; // Increase stiffness for better forward grip
        wheelCollider.forwardFriction = forwardFriction;

        JointSpring suspensionSpring = wheelCollider.suspensionSpring;
        suspensionSpring.spring = 15000f; // Increase suspension stiffness
        suspensionSpring.damper = 5000f;  // Increase suspension damping
        wheelCollider.suspensionSpring = suspensionSpring;
    }

    private void AdjustWheelFrictions()
    {
        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            AdjustWheelFriction(wheelCollider);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == groundObject || other.gameObject == secondGroundObject)
        {
            ChangeGear(-6); // Change to 1st gear
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == groundObject)
        {
            // Additional logic when the car leaves the ground trigger area
        }
        else if (other.gameObject == reverseTriggerObject)
        {
            // Additional logic when the car leaves the reverse trigger area
        }
    }
}
