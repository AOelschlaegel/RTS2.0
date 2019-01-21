using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class DestinationDecal : MonoBehaviour
{
	private DecalProjectorComponent _projector;
	[SerializeField] private Material _destinationDecalMaterial;
	[SerializeField] private Color _destinationDecalColor;
	private float t = 0;
	public float FadeSpeed;

    // Start is called before the first frame update
    void Start()
    {
		_projector = GetComponent<DecalProjectorComponent>();
    }

    // Update is called once per frame
    void Update()
    {
		
		_destinationDecalMaterial.shader = Shader.Find("HDRP/Decal");
		_destinationDecalMaterial.SetColor("_BaseColor", _destinationDecalColor);

		_destinationDecalColor.a = Mathf.Lerp(1, 0, t);
		t += FadeSpeed * Time.deltaTime;
	}
}
