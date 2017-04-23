using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectWaterfall : MonoBehaviour {

	public GameObject _prefab;

	public bool isSpawning = false;

	private List<GameObject> _spawnedObjects = new List<GameObject>();

	[System.Serializable]
	public struct SpawningParameters 
	{
		public float delayBetweenSpawns;
	}

	public SpawningParameters m_Parameters;

	void Start()
	{
	}

	void SpawningCorotuine()
	{
		SpawnNew();
	}

	public void SpawnNew() 
	{
		GameObject newObject = Instantiate(_prefab, transform.position, Quaternion.identity) as GameObject;

		newObject.transform.parent = transform;
	}
}
