using UnityEngine;
using System.Collections;

public class screenDistort : MonoBehaviour {
	Vector3 startPos;
	Vector3 startScale;
	public float amplitude;
	public float period;

    float intensityTarget = 10	;
	float intensity = 10;

    float fadeTime = 1f;
    float fadeRate;

	public Material material;
	bool flag = true;
	float i = 0;

	private GrabPass grabPass;

	//public Camera grabPassCam;
	private Vector4 grabTextureTexelSize;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		startScale = transform.localScale;

		// this does not work with varied viewports
		//Rect viewport = Camera.main.pixelRect;
		//grabTextureTexelSize = new Vector4 (1 / viewport.width, 1 / viewport.height, 0, 0);

		// magic amount of distortion
		grabTextureTexelSize = new Vector4 (0.000390625f, 0.0008665511f, 0, 0);

		// alternate RT to replace the need for GrabPass
		// GrabPass has issues with GLES and Graphics.Blit
		grabPass = GetComponent<GrabPass> ();
	}

    // Update is called once per frame
    void Update() {

        if (flag == true) {
            //StartCoroutine (lineChange (0.03F));
            flag = false;
        }
        i += 0.05F;

        if (intensityTarget < intensity)
        {
            //intensity -= 10;
            intensity += fadeRate * Time.deltaTime;
        }

	}

    public void SetIntensity(float frac)
    {
        intensity = ComputeIntensity(frac);
        intensityTarget = intensity;
    }

    public void ResetIntensity()
    {
        intensityTarget = ComputeIntensity(0);
        fadeRate = (intensityTarget - intensity) / fadeTime;
    }

    float ComputeIntensity(float frac)
    {
        return 10 * (1 + frac * 8); 
    }

	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		float theta = Time.timeSinceLevelLoad / period;
		float distance = amplitude * Mathf.Sin(theta);
		float scaleX = Mathf.Cos(Time.time) * 0.09F + 1; //CHANGES WAVE SPEED
		float scaleY = Mathf.Sin(Time.time) * 0.09F + 1;
	
		if (intensity == 0)
		{
			Graphics.Blit (source, destination);
			return;
		}

		material.SetTexture ("_GrabTexture", grabPass.GetGrabTex());
		material.SetVector ("_GrabTexture_TexelSize", grabTextureTexelSize);
	
		material.SetTextureScale ("_BumpMap", new Vector2(scaleX,scaleY)); 
		material.SetFloat("_BumpAmt", intensity);

		//mat is the material containing your shader
		Graphics.Blit(source,destination,material);
	}
		IEnumerator lineChange(float waitTime) {
			flag = false;
			yield return new WaitForSeconds(waitTime);
			intensity = Random.Range (-20f, 20f);
			flag = true;
		}
}