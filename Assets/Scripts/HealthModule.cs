using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class HealthModule : MonoBehaviour 
{
	[System.Serializable]
	public struct Parameters
	{
		public float startingHealth;
		public float maxHealth;
		public float healthRegenPerSecond;
	}

	public Parameters parameters;

	private float _currentHealth;
	private bool _isAlive = true;

	void Start()
	{
		_currentHealth = parameters.startingHealth;
	}

#region PublicInterface

	public UnityEvent onDeath;

	public void TakeDamage(float dmg)
	{
		if (_isAlive)
		{
			_currentHealth -= dmg;

			if (_currentHealth <= 0)
			{
				_currentHealth = 0;
				_isAlive = false;
				Debug.Log(gameObject.name + " is dead :(");
				onDeath.Invoke();
			}
		}
	}

#endregion
}
