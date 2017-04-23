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

	public Parameters m_Parameters;
	
	public struct State
	{
		public bool isAlive;
		public float health;
		public bool regenEnabled;
	}

	private State m_State;

#region PublicInterface

	[Serializable]
	public struct EventInterface
	{
		public UnityEvent onDeath;
	}

	public EventInterface m_Events;

	public void TakeDamage(float dmg)
	{
		if (m_State.health > 0)
			m_State.health -= dmg;

		if (m_State.isAlive && m_State.health < 0f)
		{
			m_State.isAlive = false;
			m_State.health = 0f;
			m_Events.onDeath.Invoke();
		}
	}

	public void EnableRegen()
	{
		m_State.regenEnabled = true;
	}

	public void DisableRegen()
	{
		m_State.regenEnabled = false;
	}

	public bool IsDead()
	{
		return m_State.isAlive;
	}

#endregion
}
