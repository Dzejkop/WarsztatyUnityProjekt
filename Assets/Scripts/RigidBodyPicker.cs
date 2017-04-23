using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyPicker : MonoBehaviour {
	public float m_MaxDistance;
	public float m_PickingForce;

	private Rigidbody _pickedBody = null;
	private float _pickDistance;

	public LayerMask m_LayerMask;
	public QueryTriggerInteraction m_TriggerInteraction;

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
			Enable();
		
		if (Input.GetMouseButtonUp(0))
			Disable();
	}

	void FixedUpdate() 
	{
		if (_pickedBody != null)
		{
			Vector3 targetPoint = transform.position + (_pickDistance*transform.forward.normalized);
			_pickedBody.AddForce((targetPoint - _pickedBody.position) * m_PickingForce);
		}
	}

	public void Enable()
	{
		Debug.Log("Enable");
		if (_pickedBody != null)
			return;

		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position, transform.forward, out hitInfo, m_MaxDistance, m_LayerMask, m_TriggerInteraction))
		{
			_pickedBody = hitInfo.rigidbody;
			_pickDistance = hitInfo.distance;
		}
	}

	public void Disable()
	{
		Debug.Log("Disable");
		_pickedBody = null;
	}
}
