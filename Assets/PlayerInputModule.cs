using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputModule : MonoBehaviour {
	
	public string xAxisName;
	public string yAxisName;

	public string mouseXAxisName;
	
	public string mouseYAxisName;

	public string jumpAxisName;

	public GameObject m_TargetObject;
	private PlayerInputModuleReceiver m_Receiver;

}
