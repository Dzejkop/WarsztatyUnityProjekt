using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class HealthModule : MonoBehaviour {

	[Serializable]
	public struct Parameters
	{
		public float startingHealth;

		public float healthRegenPerSecond;
	}

	public Parameters parameters;
	
	[System.Serializable]
	public struct State
	{
		public bool isAlive;
		public float health;
		public bool regenEnabled;
	}

	private State _state;

#region PublicInterface

	[Serializable]
	public struct EventInterface
	{
		public UnityEvent onDeath;
	}

	public EventInterface events;

	void Start()
	{
		_state.health = parameters.startingHealth;
		_state.isAlive = true;
		_state.regenEnabled = true;
	}

	public void TakeDamage(float dmg)
	{
		if (_state.isAlive && _state.health > 0)
		{
			Debug.Log("Ouch, " + gameObject.name + " is taking damage.");
			_state.health -= dmg;
		}

		if (_state.isAlive && _state.health <= 0f)
		{
			Debug.Log("Ouch, " + gameObject.name + " is dying :(");
			_state.isAlive = false;
			_state.health = 0f;
			events.onDeath.Invoke();
		}
	}

	public void EnableRegen()
	{
		_state.regenEnabled = true;
	}

	public void DisableRegen()
	{
		_state.regenEnabled = false;
	}

	public bool IsDead()
	{
		return _state.isAlive;
	}

#endregion
}
