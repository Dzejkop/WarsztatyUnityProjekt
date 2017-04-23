using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyPrefabLauncher : MonoBehaviour {

	[System.Serializable]
	public struct Parameters
	{
		public GameObject prefab;
		public float delay;
		public float force;
		public Transform spawnPosition;
		public Vector3 direction;
		public float randomRecoilMagnitude;
		public bool startEnabled;
	}

	public Parameters m_Parameters;

	public struct State
	{
		public bool isSpawning;
	}

	private State m_State;

	void Start()
	{
		m_State.isSpawning = m_Parameters.startEnabled;

		StartCoroutine(SpawningCoroutine());
	}

	IEnumerator SpawningCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(m_Parameters.delay);
			if (m_State.isSpawning)
				Go();
		}
	}

	public void Go()
	{
		GameObject obj = Instantiate(m_Parameters.prefab, m_Parameters.spawnPosition.position, Quaternion.identity);

		Rigidbody body = obj.GetComponent<Rigidbody>();

		Vector3 recoil = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		recoil = recoil.normalized * m_Parameters.randomRecoilMagnitude;

		body.velocity = recoil + (transform.TransformVector(m_Parameters.direction.normalized) * m_Parameters.force);
	}

	public void Begin() 
	{
		m_State.isSpawning = true;
	}

	public void Stop()
	{
		m_State.isSpawning = false;
	}
}
