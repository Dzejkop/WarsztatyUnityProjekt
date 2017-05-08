using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleRigidbodyWalker : MonoBehaviour {

	[System.Serializable]
	public struct Parameters
	{
		public float speed;
	}

	public Parameters parameters;

	private Rigidbody _body;
	private Transform _target;

	void Start()
	{
		_body = GetComponent<Rigidbody>();
		_body.velocity = transform.forward * parameters.speed;
	}

	public void MovementInput(Transform target)
	{
		_target = target;
	}

	void FixedUpdate()
	{
		_body.velocity = transform.forward * parameters.speed;

		if (_target != null)
		{
			Vector3 directionToTarget = _target.position - transform.position;
			Vector3 directinoWithoutY = directionToTarget - (Vector3.up * directionToTarget.y);

			transform.LookAt(transform.position + directinoWithoutY, Vector3.up);
		}
	}
}
