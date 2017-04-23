using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DelegatesTest2 : MonoBehaviour {
	


	public void Accept(Action acceptedDelegate)
	{
		Debug.Log("Accepting delegate.");
		acceptedDelegate();
	}
}
