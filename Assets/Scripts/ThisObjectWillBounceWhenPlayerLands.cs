using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisObjectWillBounceWhenPlayerLands : MonoBehaviour {

	Rigidbody _body;
	void Start() 
	{
		_body = GetComponent<Rigidbody>();
	}

	void OnEnable()
	{
		PlayerController.onPlayerLanded += Bounce;
	}

	void OnDisable() 
	{
		PlayerController.onPlayerLanded -= Bounce;
	}

	public void Bounce() 
	{
		_body.AddForce(Vector3.up * 10, ForceMode.VelocityChange);
	}

	public void OtherBounce(string msg)
	{
		Debug.Log("Halo: " + msg);
		_body.AddForce(Vector3.up * 10, ForceMode.VelocityChange);
	}

}
