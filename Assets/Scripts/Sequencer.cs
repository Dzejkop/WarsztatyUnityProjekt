using UnityEngine;
using UnityEngine.Events;

public class Sequencer : MonoBehaviour {
	public float delay;
	public UnityEvent onTimeout;
	public bool startEnabled;

	private float _t = 0;
	private bool _working;

	void Start()
	{
		_working = startEnabled;
	}

	void Update () 
	{
		_t += Time.deltaTime;

		if (_working && _t >= delay)
		{
			_t = 0;
			onTimeout.Invoke();
		}
	}

	public void SetSequencing(bool val)
	{
		_working = val;
	}

	public void Reset()
	{
		_t = 0;
	}
}
