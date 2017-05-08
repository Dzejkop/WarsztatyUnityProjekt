using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour {

	public GameObject prefab;

	public GameObject Spawn()
	{
		return Instantiate(prefab, transform.position, transform.rotation);
	}
}
