using UnityEngine;
using System.Collections;

public class PinToCapsule : MonoBehaviour
{

	public Transform TargetTransform;

	public void Update ()
	{
		transform.localRotation = TargetTransform.rotation;
		transform.position = transform.localRotation * new Vector3 (TargetTransform.localScale.x, TargetTransform.localScale.y, TargetTransform.localScale.x) / 2;
	}
}
