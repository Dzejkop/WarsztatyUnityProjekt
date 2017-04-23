using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DelegatesTest1 : MonoBehaviour {

	public DelegatesTest2 other;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			Send();
	}

	private void DisplaySomeMessage()
	{
		Debug.Log("fdsafdsafsdafsdafsdafsdafsdaf");
	}

	void Send()
	{
		Action testDelegate = DisplaySomeMessage;

		other.Accept(testDelegate);
	}
}
