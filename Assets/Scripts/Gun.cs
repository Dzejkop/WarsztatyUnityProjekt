using UnityEngine;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
	[System.Serializable]
	public struct Parameters
	{
		public Transform aimingReference;

		public AnimationCurve recoilCurve;
		public float recoilDeterioration;
		public float recoilGain;

		public float range;
		public LayerMask castLayerMask;

		public float damage;
		public bool orientSelfToTarget;
	}

	public Parameters parameters;

	public bool debug = false;

	private List<Vector3> _debugHitPoints = new List<Vector3>();

	private float _currentRecoil = 0;

	void Update()
	{
		RaycastHit hit;

		if (Physics.Raycast(
			parameters.aimingReference.position, 
			parameters.aimingReference.forward,
			out hit ,
			parameters.range, parameters.castLayerMask))
		{
			if (parameters.orientSelfToTarget)
			{
				transform.LookAt(hit.point);
			}
		}

		_currentRecoil -= parameters.recoilDeterioration * Time.deltaTime;
		if (_currentRecoil < 0)
			_currentRecoil = 0;
	}

	void OnDrawGizmosSelected()
	{
		if (debug)
		{
			foreach (Vector3 p in _debugHitPoints)
			{
				Gizmos.DrawSphere(p, 0.2f);
			}
		}
	}

#region PublicInterface
	public void Fire()
	{
		RaycastHit hit;
		float effectiveRecoil = parameters.recoilCurve.Evaluate(_currentRecoil);

		Vector3 recoilVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
		recoilVector = recoilVector* effectiveRecoil;

		if (Physics.Raycast(
			parameters.aimingReference.position, 
			parameters.aimingReference.forward + recoilVector,
			out hit ,
			parameters.range, parameters.castLayerMask))
		{
			_debugHitPoints.Add(hit.point);

			if (hit.rigidbody != null)
			{
				HealthModule healthModule = hit.rigidbody.GetComponent<HealthModule>();
				if (healthModule != null)
				{
					healthModule.TakeDamage(parameters.damage);
				}
			}
		}

		_currentRecoil += parameters.recoilGain;
		if (_currentRecoil > 1)
			_currentRecoil = 1;
	}
#endregion
}