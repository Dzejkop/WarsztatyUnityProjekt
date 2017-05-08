using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Enemy : MonoBehaviour {

	public Common.TransformEvent movementOutput;

	private GameObject _player;

	public float turningAngleThreshold;

	void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Player");

		Assert.IsNotNull(_player, gameObject.name + " could no find the player :(");
	}

	void Update()
	{
		Vector3 directionToPlayer = _player.transform.position - transform.position;
		Vector3 forward = transform.forward;

		Vector3 cross = Vector3.Cross(directionToPlayer, forward);

		float angle = Vector3.Angle(directionToPlayer, forward);

		movementOutput.Invoke(_player.transform);
	}

	public void Die()
	{
		Destroy(gameObject);
	}

}
