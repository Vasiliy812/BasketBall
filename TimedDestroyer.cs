using UnityEngine;
using System.Collections;

public class TimedDestroyer : MonoBehaviour {

	public float TimeToDestroy = 1f;

	void Start () {
		Destroy(gameObject, TimeToDestroy);
	}
}
