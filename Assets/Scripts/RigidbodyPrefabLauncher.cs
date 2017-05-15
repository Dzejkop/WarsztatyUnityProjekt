using UnityEngine;
using UnityEngine.Events;

public class RigidbodyPrefabLauncher : MonoBehaviour
{
	[System.SerializableAttribute]
	public struct Parameters
	{
		public float recoil;
		public GameObject rocketPrefab;

		public float launchForce;
		public ForceMode forceMode;

		public Transform launchMuzzle;
		public Transform aimingReference;

		public LayerMask castLayerMask;
		public float castRange;
	}

	public Parameters parameters;

	public void Fire()
	{
		Vector3 direction = parameters.launchMuzzle.forward;

		RaycastHit hit;

		if (Physics.Raycast(
			parameters.aimingReference.position, 
			parameters.aimingReference.forward,
			out hit,
			parameters.castRange, parameters.castLayerMask))
		{
			direction = (hit.point - parameters.launchMuzzle.position).normalized;
		}

		GameObject go = Instantiate(parameters.rocketPrefab, parameters.launchMuzzle.position, parameters.launchMuzzle.rotation);

		Rigidbody goBody = go.GetComponent<Rigidbody>();

		if (goBody != null)
		{
			goBody.AddForce(direction * parameters.launchForce, parameters.forceMode);
		}
	}
}