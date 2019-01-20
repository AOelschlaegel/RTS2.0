using UnityEngine;

public class SelectableUnitComponent : MonoBehaviour
{
    public GameObject selectionCircle;
	public int SelectionDecalSize;
	public string SelectionDecalType;
	public bool Selected;

	private void Update()
	{
		if (selectionCircle != null)
		{
			Selected = true;
		}
		else Selected = false;
	}
}
