using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	[System.Serializable]
	public struct Parameters
	{
		public GameObject muzzle;
		public GameObject aimingReference;

		public float range;
		public AnimationCurve recoil;
		public float recoilGainRate;
		public float recoilReductionRate;

		public float damage;

		public LayerMask castLayerMask;

		public bool orientSelfAtTarget;
	}

	public Parameters parameters;

	private List<Vector3> _hitPointList = new List<Vector3>();

	public bool debug;

	private float _currentRecoil = 0;

	void Update()
	{
		RaycastHit hit;
		if (Physics.Raycast(
			parameters.aimingReference.transform.position, 
			parameters.aimingReference.transform.forward,
			out hit,
			parameters.range, parameters.castLayerMask)
			)
		{
			if (parameters.orientSelfAtTarget)
			{
				transform.LookAt(hit.point);
			}
		}

		if (_currentRecoil > 0)
		{
			_currentRecoil -= parameters.recoilReductionRate * Time.deltaTime;
			if (_currentRecoil < 0)
				_currentRecoil = 0;
		}
	}

	void OnDrawGizmosSelected()
	{
		if (debug)
		{
			foreach (Vector3 pos in _hitPointList)
			{
				Gizmos.DrawSphere(pos, 0.2f);
			}
		}
	}

	public void Fire()
	{
		float effectiveRecoil = parameters.recoil.Evaluate(_currentRecoil);
		Vector3 recoilVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		recoilVector = recoilVector.normalized * effectiveRecoil;

		RaycastHit hit;
		if (Physics.Raycast(
			parameters.aimingReference.transform.position, 
			parameters.aimingReference.transform.forward + recoilVector,
			out hit,
			parameters.range, parameters.castLayerMask)
			)
		{
			_hitPointList.Add(hit.point);

			if (hit.rigidbody != null)
			{
				HealthModule healthModule = hit.rigidbody.GetComponent<HealthModule>();

				if (healthModule != null)
				{
					healthModule.TakeDamage(parameters.damage);
				}
			}
		}

		_currentRecoil += parameters.recoilGainRate;
		if (_currentRecoil > 1)
			_currentRecoil = 1;
	}
}
