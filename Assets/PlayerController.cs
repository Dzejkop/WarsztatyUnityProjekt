﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour {

	[System.Serializable]
	public struct MovementParameters 
	{
		public float forwardSpeed;
		public float backwardSpeed;
		public float strafeSpeed;

		public float jumpForce;

		public float airControlMultiplier;

		public AnimationCurve angleToForceFeedback;
	}

	[System.Serializable]
	public struct Settings 
	{
		public string horizontalInputAxisName;
		public string verticalInputAxisName;
		public string jumpInputAxisName;

		public bool airControl;

		public float groundCheckDistance;
		public LayerMask groundCheckLayerMask;
		public QueryTriggerInteraction groundCheckTriggerInteraction;
	}

	[System.Serializable]
	public struct State
	{
		public bool isGrounded;
		public bool wasGrounded;
		public bool isJumping;
		public bool isGoingToJump;
	} 

	public Camera m_Camera;

	Rigidbody m_Body;
	CapsuleCollider m_Collider;

	public MouseLookHelper m_MouseLookHelper;

	public MovementParameters m_Parameters;
	public Settings m_Settings;

	private State m_State;

	void Start () {
		m_Camera = GetComponentInChildren<Camera>();
		Assert.IsNotNull(m_Camera);

		m_Body = GetComponent<Rigidbody>();
		m_Collider = GetComponent<CapsuleCollider>();
	}
	
	void Update ()
	{
		Vector2 mouseInput = m_MouseLookHelper.GetInput();
		m_Camera.transform.localRotation *= Quaternion.Euler(new Vector3(mouseInput.y, 0f, 0f));
		transform.localRotation *= Quaternion.Euler(new Vector3(0f, mouseInput.x, 0f));

		if (Input.GetAxis(m_Settings.jumpInputAxisName) > float.Epsilon && !m_State.isJumping)
		{
			m_State.isGoingToJump = true;
		}
	}

	float GetDesiredSpeed(Vector2 input)
	{
		if (input.y > 0)
			return m_Parameters.forwardSpeed;

		if (input.y < 0)
			return m_Parameters.backwardSpeed;

		if (Mathf.Abs(input.x) > float.Epsilon)
			return m_Parameters.strafeSpeed;

		return 0f;
	}

	void FixedUpdate()
	{
		CheckIfGrounded();

		if (m_State.isGoingToJump && m_State.isGrounded && !m_State.isJumping)
		{
			m_State.isJumping = true;
			m_State.isGoingToJump = false;

			m_Body.AddForce(Vector3.up * m_Parameters.jumpForce, ForceMode.Impulse);
		}

		Vector2 input = GetInput();

		Vector3 movementForce = transform.forward * input.y + m_Camera.transform.right * input.x;

		Vector3 currentVelocity = m_Body.velocity;

		float angle = Vector3.Angle(movementForce, currentVelocity);

		float angleFeedback = m_Parameters.angleToForceFeedback.Evaluate(angle);

		Vector3 forceFeedback = -currentVelocity.normalized * angleFeedback;

		forceFeedback -= forceFeedback.y * Vector3.up;

		float desiredSpeed = GetDesiredSpeed(input);

		bool isAccelerating = angle < 90f;

		if (m_State.isGrounded || m_Settings.airControl)
		{
			float airControl = m_State.isJumping ? m_Parameters.airControlMultiplier : 1;

			if (isAccelerating && currentVelocity.magnitude < desiredSpeed || !isAccelerating)
				m_Body.AddForce((movementForce.normalized + forceFeedback) * airControl, ForceMode.Impulse);
		}
	}

	private Vector2 GetInput()
	{
		return new Vector2(
			Input.GetAxis(m_Settings.horizontalInputAxisName),
			Input.GetAxis(m_Settings.verticalInputAxisName)
		);
	}

	void CheckIfGrounded()
	{
		RaycastHit hit;

		m_State.wasGrounded = m_State.isGrounded;

		if (Physics.SphereCast(
			transform.position, // Początek cast'a`
			m_Collider.radius,	// Promień sfery castującej
			Vector3.down,		// Kierunek
			out hit,			// Informacja zwrotna
			(m_Collider.height / 2) - m_Collider.radius + m_Settings.groundCheckDistance, // Odległość
			m_Settings.groundCheckLayerMask,// Maska warstw
			m_Settings.groundCheckTriggerInteraction // Czy kolidujemy z triggerami
		))
		{
			m_State.isGrounded = true;

			if (!m_State.wasGrounded && m_State.isJumping)
			{
				m_State.isJumping = false;
			}
		}
		else
		{
			m_State.isGrounded = false;
		}
	}
}