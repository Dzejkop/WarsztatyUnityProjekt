using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePush : MonoBehaviour {

	[System.Serializable]
	public struct Parameters
	{
		public float force;
		public ForceMode forceMode;

		public float forwardCastOffset;

		public float rangeDistance;
		public float rangeWidth;

		public LayerMask layerMask;
	}

	[System.Serializable]
	public struct Settings
	{
		public bool drawGizmos;
	}

	public Settings m_Settings;

	void OnDrawGizmosSelected()
	{
		if (m_Settings.drawGizmos)
		{
			Vector3 origin = transform.position + (transform.forward * m_Parameters.forwardCastOffset);
			Vector3 end = origin + (transform.forward * m_Parameters.rangeDistance);
			Gizmos.DrawLine(origin, end);
			Gizmos.DrawWireSphere(origin, m_Parameters.rangeWidth);
			Gizmos.DrawWireSphere(end, m_Parameters.rangeWidth);
		}
	}

	public Parameters m_Parameters;

	public void Go()
	{
		Vector3 origin = transform.position + (transform.forward * m_Parameters.forwardCastOffset);

		RaycastHit[] hits = Physics.SphereCastAll(
			origin, 
			m_Parameters.rangeWidth,
			transform.forward, 
			m_Parameters.rangeDistance, 
			m_Parameters.layerMask
			);

		foreach (RaycastHit hit in hits)
		{
			if (hit.rigidbody)
				hit.rigidbody.AddForceAtPosition(transform.forward.normalized * m_Parameters.force, hit.point, m_Parameters.forceMode);
		}
	}
}
