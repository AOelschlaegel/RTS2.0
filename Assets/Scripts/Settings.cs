using UnityEngine;

public class Settings : MonoBehaviour
{
	[SerializeField] private Transform _debugCanvas;
	public bool HideUI;

    // Start is called before the first frame update
    void Start()
    {
        if(HideUI)
		{
			_debugCanvas.gameObject.SetActive(false);
		}
    }
}
