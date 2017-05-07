using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sequencer : MonoBehaviour {
	public float m_Delay;
	public UnityEvent m_OnTimeout;
	private float _t = 0;

	void Update () 
	{
		_t += Time.deltaTime;

		if (_t >= m_Delay)
		{
			_t = 0;
			m_OnTimeout.Invoke();
		}
	}
}
