using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class RigidbodyCapsuleCharacterController : MonoBehaviour {

    public Camera m_Camera;

    [System.Serializable]
    public struct MovementParameters
    {
        public float forwardSpeed;
        public float backwardSpeed;
        public float strafeSpeed;
        public float jumpForce;

        public bool autoBreak;
        public float breakForce;
        public float breakForceThreshold;

        [Range(0, 1)]
        public float airControlModifier;

        public Curve angleToForceFeedback;
    }

    [System.Serializable]
    public struct Settings
    {
        public string horizontalInputName;
        public string verticalInputName;
        public string jumpAxisName;
        public bool airControl;

        public float groundcheckRaycastDistance;
        public LayerMask groundCheckLayerMask;
        public QueryTriggerInteraction groundCheckTriggerInteraction;
    }

    [System.Serializable]
    private struct State
    {
        public bool isGrounded;
        public bool wasGrounded;
        public bool isJumping;
        public bool isGoingToJump;
    }

    private State m_State;
    public MovementParameters m_MovementParameters;
    public Settings m_Settings;

    private Rigidbody m_Body;
    private CapsuleCollider m_CapsuleCollider;

    public MouseLook m_MouseLook;

    void Start()
    {
        m_Body = GetComponent<Rigidbody>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        UpdateLookRotation();
        m_State.isGoingToJump = Input.GetAxis(m_Settings.jumpAxisName) > 0 && !m_State.isJumping;
    }

    private void UpdateLookRotation()
    {
        Vector2 mouseInput = m_MouseLook.GetInput();
        transform.localRotation *= Quaternion.Euler(0, mouseInput.x, 0);
        m_Camera.transform.localRotation *= Quaternion.Euler(mouseInput.y, 0, 0);
        m_Camera.transform.localRotation = m_MouseLook.Clamp(m_Camera.transform.localRotation);
    }

    void CheckIfGrounded()
    {
        m_State.wasGrounded = m_State.isGrounded;

        RaycastHit hitInfo;

        if (Physics.SphereCast(
            transform.position,         // Początek cast'a
            m_CapsuleCollider.radius,   // Promień sfery castującej
            Vector3.down,               // Kierunek cast'a
            out hitInfo,                // Informacja zwrotna
            (m_CapsuleCollider.height / 2) - m_CapsuleCollider.radius + m_Settings.groundcheckRaycastDistance,
            m_Settings.groundCheckLayerMask,
            m_Settings.groundCheckTriggerInteraction))
        {
            m_State.isGrounded = true;
            m_State.isJumping = false;
        }
        else
        {
            m_State.isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        CheckIfGrounded();

        if (m_State.isGrounded || m_Settings.airControl)
        {
            Vector2 input = GetInput();

            Vector3 movementForce = transform.forward * input.y + transform.right * input.x;

            Vector3 currentVelocity = m_Body.velocity;

            float angle = Vector3.Angle(currentVelocity, movementForce);

            bool isAccelerating = angle < 90f;

            float currentTargetSpeed = CurrentTargetSpeed(input);

            float airControlMofidier = (m_Settings.airControl && !m_State.isGrounded) ? m_MovementParameters.airControlModifier : 1;
            
            float angleFeedback = m_MovementParameters.angleToForceFeedback.Value(angle);
            Vector3 angleFeedbackForce = angleFeedback * (currentVelocity.normalized * -1);

            Vector3 finalForce = (movementForce + angleFeedbackForce) * airControlMofidier;

            if (input.magnitude < float.Epsilon && m_MovementParameters.autoBreak)
            {
                finalForce = currentVelocity.normalized * -m_MovementParameters.breakForce;
                finalForce -= finalForce.y * Vector3.up;
            }

            if ((isAccelerating && m_Body.velocity.magnitude < CurrentTargetSpeed(input)) || !isAccelerating)
                m_Body.AddForce(finalForce, ForceMode.Impulse);
        }

        if (m_State.isGoingToJump && m_State.isGrounded)
        {
            m_Body.AddForce(new Vector3(0, m_MovementParameters.jumpForce, 0), ForceMode.Impulse);
            m_State.isGoingToJump = false;
            m_State.isJumping = true;
        }
    }

    private float CurrentTargetSpeed(Vector2 input)
    {
        if (input.y > 0)
        {
            return m_MovementParameters.forwardSpeed;
        }

        if (input.y < 0)
        {
            return m_MovementParameters.backwardSpeed;
        }

        if (input.x != 0)
        {
            return m_MovementParameters.strafeSpeed;
        }

        return 0;
    }

    private Vector2 GetInput()
    {
        return new Vector2(
            Input.GetAxis(m_Settings.horizontalInputName),
            Input.GetAxis(m_Settings.verticalInputName)
            );
    }

}
