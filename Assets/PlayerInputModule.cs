using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class PlayerInputModule : MonoBehaviour {
	
	[Serializable]
	public struct Names 
	{
		public string xAxisName;
		public string yAxisName;

		public string mouseXAxisName;
		
		public string mouseYAxisName;

		public string jumpAxisName;
	}

	[Serializable]
	public struct Parameters 
	{
		public bool flipMouseYAxis;

		public bool hideCursor;

		public KeyCode showCursorToggleKey;
	}
	
	public Names m_Names;

	public Parameters m_Parameters;

	public GameObject m_TargetObject;
	private PlayerInputModuleReceiver m_Receiver;

	void Start() 
	{
		m_Receiver = m_TargetObject.GetComponent<PlayerInputModuleReceiver>();

		Assert.IsNotNull(m_Receiver, "Receiver can not be null.");

		if (m_Parameters.hideCursor)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	void Update()
	{
		float jumpAxis = Input.GetAxis(m_Names.jumpAxisName);
		Vector2 mouseInput = new Vector2(
			Input.GetAxis(m_Names.mouseXAxisName), 
			m_Parameters.flipMouseYAxis ? -Input.GetAxis(m_Names.mouseYAxisName) : Input.GetAxis(m_Names.mouseYAxisName)
			);
		Vector2 movementInput = new Vector2(Input.GetAxis(m_Names.xAxisName), Input.GetAxis(m_Names.yAxisName));

		m_Receiver.onJump(jumpAxis);
		m_Receiver.onMouseInput(mouseInput);
		m_Receiver.onMovement(movementInput);

		if (Input.GetKeyDown(m_Parameters.showCursorToggleKey))
		{
			m_Parameters.hideCursor = !m_Parameters.hideCursor;
			Cursor.lockState = m_Parameters.hideCursor ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = !m_Parameters.hideCursor;
		}
	}
}
