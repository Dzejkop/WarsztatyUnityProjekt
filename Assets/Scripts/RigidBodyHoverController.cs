using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidBodyHoverController : MonoBehaviour {

	private Rigidbody m_Body;

	public float floatingDistance;

	void Start()
	{
		m_Body = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{

	}
}
