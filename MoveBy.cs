using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBy : MonoBehaviour {

	public GameObject Target;
	public Vector3 Shift;

	void Update () {
		transform.position = new Vector3(Target.transform.position.x + Shift.x, Target.transform.position.y + Shift.y, Target.transform.position.z + Shift.z);
	}
}
