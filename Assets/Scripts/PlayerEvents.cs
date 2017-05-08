using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEvents : MonoBehaviour {
	void OnEnable()
	{
		PlayerLogic.OnPlayerDiedEvent += onPlayerDied.Invoke;
	}

	void OnDisable()
	{
		PlayerLogic.OnPlayerDiedEvent -= onPlayerDied.Invoke;
	}

	public UnityEvent onPlayerDied;
}
