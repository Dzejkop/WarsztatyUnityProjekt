using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSendMessageInput : MonoBehaviour {

	public UnityEvent onLeftMouseButtonDown;
	public UnityEvent onLeftMouseButtonUp;

	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			onLeftMouseButtonDown.Invoke();
		}

		if (Input.GetMouseButtonUp(0))
		{
			onLeftMouseButtonUp.Invoke();
		}
	}
}
