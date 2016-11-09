using UnityEngine;
using System.Collections;

public class GrabPass : MonoBehaviour {

	private RenderTexture grabTexture;

	// Use this for initialization
	void Start () {
		Rect viewport = Camera.main.pixelRect;

		// alternate camera to replace the need for GrabPass
		// GrabPass has issues with GLES and Graphics.Blit
		grabTexture = new RenderTexture ((int)viewport.width, (int)viewport.height, 24,
			RenderTextureFormat.ARGB32);
		grabTexture.Create();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination) {

		Graphics.Blit (source, grabTexture);
		Graphics.Blit (source, destination);
	}

	public RenderTexture GetGrabTex() {
		return grabTexture;
	}
}
