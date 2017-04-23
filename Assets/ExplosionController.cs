using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplosionController : MonoBehaviour {

	private List<ParticleSystem> m_Systems;

	public UnityEvent onStart;
	public UnityEvent onEnd;

	public bool startImmediately = false;

	private bool hasBeenStarted = false;

	void Start () 
	{
		m_Systems = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());

		if (startImmediately)
			Go();
	}

	void Update()
	{	
		if (!hasBeenStarted)
			return;

		foreach (ParticleSystem ps in m_Systems)
		{
			if (ps.particleCount != 0)
				return;
		}

		Destroy(gameObject);
		onEnd.Invoke();
	}

	public void Go()
	{
		if (hasBeenStarted)
			return;
		onStart.Invoke();
		hasBeenStarted = true;
	}
}
