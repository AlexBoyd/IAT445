using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {

	public Transform targetTransform;
	private Vector3 originalPos;

    bool init = false;

	void Start() {
		if (!targetTransform)
			targetTransform = transform;
			
		originalPos = targetTransform.localPosition;
        init = true;

		if (targetTransform.tag=="MainCamera")
			originalPos = Vector3.zero;
	}

    public void OnEnable()
    {
        if (init)
            targetTransform.localPosition = originalPos;
    }

	public void shake(float shakeIntensity) {
		StopCoroutine("ShakeEffect");
		StartCoroutine("ShakeEffect",shakeIntensity);
	}
	
	IEnumerator ShakeEffect(float shakeIntensity) {
		float shake = shakeIntensity;
		float decreaseRate = 1 + shakeIntensity;
		
		while (shake > 0) {
			yield return null;
			targetTransform.localPosition = originalPos + Random.insideUnitSphere * shake;
			shake -= Time.deltaTime * decreaseRate;
			
			if(Time.timeScale==0)
				shake=0;
			
			if (shake <= 0) {
				targetTransform.localPosition = originalPos;
			}
		}
	}
}
