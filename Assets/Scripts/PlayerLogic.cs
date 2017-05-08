using UnityEngine;
using System;

public class PlayerLogic : MonoBehaviour {
	
	[System.SerializableAttribute]
	public struct Parameters
	{
		public LayerMask deadlyObjectsLayerMask;
	}

	public Parameters parameters;

	void OnCollisionEnter(Collision col)
	{
		if (Common.Utils.CheckLayer(parameters.deadlyObjectsLayerMask, col.gameObject.layer))
		{
			if (OnPlayerDiedEvent != null)
				OnPlayerDiedEvent();
		}
	}

#region PublicInterface

	public static event Action OnPlayerDiedEvent;

#endregion
}
