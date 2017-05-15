using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rocket : MonoBehaviour {

	[System.Serializable]
	public struct Parameters
	{
		public GameObject explosionPrefab;

		public bool unconditionalExplosion;

		public float damage;

		public float explosionRange;
		public float explosionForce;
		public ForceMode explosionForceMode;
		public LayerMask explosionLayerMask;
		public bool gizmosEnabled;

		public AnimationCurve distanceToForceMultiplier;
	}

	private Rigidbody m_Body;

	private List<ParticleSystem> m_ParticleSystems;

	private bool isExploded = false;

	private Vector3 m_CurrentCollisionNormal = Vector3.zero;

	void Start()
	{
		m_Body = GetComponent<Rigidbody>();

		m_ParticleSystems = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());
	}

	public Parameters m_Parameters;

	void OnCollisionEnter(Collision col)
	{
		m_CurrentCollisionNormal = col.contacts[0].normal;
		if (m_Parameters.unconditionalExplosion)
		{
			Explode();
		}
	}

	void OnDrawGizmosSelected()
	{
		if (m_Parameters.gizmosEnabled)
		{
			Gizmos.DrawWireSphere(transform.position, m_Parameters.explosionRange);
		}
	}

	public void Launch(Vector3 direction)
	{
		m_Body.velocity = direction;
	}

	public void LaunchAt(GameObject obj)
	{
		Launch(obj.transform.position - transform.position);
	}

	public void Explode()
	{
		if (isExploded)
			return;

		if (m_Parameters.explosionPrefab != null)
			Instantiate(m_Parameters.explosionPrefab, transform.position, transform.rotation);

		RaycastHit[] hits = Physics.SphereCastAll(transform.position, m_Parameters.explosionRange, Vector3.up, 1, m_Parameters.explosionLayerMask, QueryTriggerInteraction.Ignore);

		Vector3 explosionPoint = transform.position;

		foreach (RaycastHit h in hits)
		{
			if (h.rigidbody != null)
			{
				Vector3 line = h.rigidbody.position - explosionPoint;
				float distance = line.magnitude;
				float distanceRatio = distance / m_Parameters.explosionRange;
				Vector3 direction = line.normalized;
				h.rigidbody.AddForceAtPosition(
					direction * m_Parameters.explosionForce * m_Parameters.distanceToForceMultiplier.Evaluate(distanceRatio),
					h.point,
					m_Parameters.explosionForceMode
					);

				HealthModule healthModule = h.rigidbody.GetComponent<HealthModule>();
				if (healthModule != null)
				{
					healthModule.TakeDamage(m_Parameters.damage);
				}
			}
		}

		isExploded = true;
		StartCoroutine(DestroyAfterParticleSystems());
	}

	IEnumerator DestroyAfterParticleSystems()
	{
		foreach (ParticleSystem ps in m_ParticleSystems)
		{
			ps.Stop();
		}

		bool stillWaiting = true;
		while (stillWaiting)
		{
			yield return null;
			stillWaiting = false;
			foreach (ParticleSystem ps in m_ParticleSystems)
			{
				if (ps.particleCount != 0)
				{
					stillWaiting = true;
					break;	
				}
			}
		}

		Destroy(gameObject);
	}
}
