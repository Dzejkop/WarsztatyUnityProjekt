using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RigidBodyHolder : MonoBehaviour, IMouseResponder {

	public float m_MaxDistance;
	public LayerMask m_LayerMask;
	public float m_HoldForce;

	public QueryTriggerInteraction m_TriggerInteraction;

	private Rigidbody _pickedBody = null;
	private float _pickDistance;

	public void Enable() 
	{
		RaycastHit hitInfo;

		if (Physics.Raycast(transform.position, transform.forward, out hitInfo, m_MaxDistance, m_LayerMask, m_TriggerInteraction))
		{
			_pickedBody = hitInfo.rigidbody;
			_pickDistance = hitInfo.distance;
		}
	}

	public void Disable() 
	{
		_pickedBody = null;
	}

	void FixedUpdate()
	{
		if (_pickedBody == null)
			return;

		Action<Rigidbody, float, Transform, float> moveDelegate = (Rigidbody obj, float dist, Transform or, float force) => 
		{
			Vector3 pickPoint =  or.position + ( or.forward.normalized * dist);

			obj.AddForce((pickPoint - obj.position).normalized * force);
		};

		moveDelegate(_pickedBody, _pickDistance, transform, m_HoldForce);
	}
}
