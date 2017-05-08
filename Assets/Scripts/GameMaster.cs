using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {
	public List<PrefabSpawner> enemySpawners;

	private List<Enemy> _enemies = new List<Enemy>();

	public void SpawnNewEnemy()
	{
		Enemy e = enemySpawners[Random.Range(0, enemySpawners.Count - 1)].Spawn().GetComponent<Enemy>();

		_enemies.Add(e);
	}

	public void PlayerDied()
	{
		foreach (Enemy e in _enemies)
		{
			e.Die();
		}
	}
}
