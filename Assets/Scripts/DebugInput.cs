using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInput : MonoBehaviour {

	[System.Serializable]
	public struct InputEvent 
	{
		public UnityEngine.Events.UnityEvent onInputEvent;
		public KeyCode keyCode;
	}

	public List<InputEvent> m_InputEvents = new List<InputEvent>();

	void Update()
	{
		foreach (InputEvent ev in m_InputEvents)
		{
			if (Input.GetKeyDown(ev.keyCode))
				ev.onInputEvent.Invoke();
		}
	}

}
