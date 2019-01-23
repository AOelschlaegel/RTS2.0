using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class SelectionManager : MonoBehaviour
{
	bool isSelecting = false;
	Vector3 mousePosition1;

	[SerializeField] private DecalProjectorComponent _projector;

	private Transform _mouseDownHit;
	private Transform _mouseUpHit;

	public Color EnemySelectionDecalColor;
	public Color NeutralSelectionDecalColor;
	public Color FriendlySelectionDecalColor;

	[SerializeField] private Material _decalMaterial;

	public List<GameObject> CurrentSelection = new List<GameObject>();
	public bool UnitSelection;

	void Update()
	{
		if (CurrentSelection.Count > 0)
		{
			if (CurrentSelection.Count > 1 || CurrentSelection[0].tag == "unit")
			{
				UnitSelection = true;
			}
			else UnitSelection = false;
		}
		// If we press the left mouse button, begin selection and remember the location of the mouse
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray, out hit))
			{
				_mouseDownHit = hit.transform;
			}


			isSelecting = true;
			mousePosition1 = Input.mousePosition;

			// Destroy former SelectionCircle
			foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
			{
				if (selectableObject.selectionCircle != null)
				{
					Destroy(selectableObject.selectionCircle.gameObject);
					selectableObject.selectionCircle = null;
					CurrentSelection.Clear();
				}
			}
		}

		// If we let go of the left mouse button, end selection
		if (Input.GetMouseButtonUp(0))
		{
			var selectedObjects = new List<SelectableUnitComponent>();
			foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
			{
				if (IsWithinSelectionBounds(selectableObject.gameObject))
				{
					if (selectableObject.gameObject.tag == "unit")
					{
						selectedObjects.Add(selectableObject);
						CurrentSelection.Add(selectableObject.gameObject);
					}
				}
			}

			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				_mouseUpHit = hit.transform;
			}


			if (_mouseUpHit.tag != "ground")
			{
				if (_mouseDownHit == _mouseUpHit)
				{
					DrawSelectionDecal(_mouseDownHit.GetComponent<SelectableUnitComponent>());
					CurrentSelection.Add(_mouseDownHit.gameObject);
				}
			}

			// Debug
			var sb = new StringBuilder();
			sb.AppendLine(string.Format("Selecting [{0}] Units", selectedObjects.Count));
			foreach (var selectedObject in selectedObjects)
				sb.AppendLine("-> " + selectedObject.gameObject.name);
			Debug.Log(sb.ToString());

			isSelecting = false;
		}

		// Highlight all objects within the selection box
		if (isSelecting)
		{
			foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
			{
				if (IsWithinSelectionBounds(selectableObject.gameObject))
				{
					if (selectableObject.selectionCircle == null)
					{
						if(selectableObject.tag == "unit")
						DrawSelectionDecal(selectableObject);
					}
				}
				else
				{
					if (selectableObject.selectionCircle != null)
					{
						Destroy(selectableObject.selectionCircle.gameObject);
						selectableObject.selectionCircle = null;
					}
				}
			}
		}
	}

	public bool IsWithinSelectionBounds(GameObject gameObject)
	{
		if (!isSelecting)
			return false;

		var camera = Camera.main;
		var viewportBounds = Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);
		return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
	}

	void OnGUI()
	{
		if (isSelecting)
		{
			// Create a rect from both mouse positions
			var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
			Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.1f));
			Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
		}
	}

	void DrawSelectionDecal(SelectableUnitComponent selection)
	{
		switch (selection.SelectionDecalType)
		{
			case "friendly":
				_decalMaterial.shader = Shader.Find("HDRP/Decal");
				_decalMaterial.SetColor("_BaseColor", FriendlySelectionDecalColor);
				break;

			case "neutral":
				_decalMaterial.shader = Shader.Find("HDRP/Decal");
				_decalMaterial.SetColor("_BaseColor", NeutralSelectionDecalColor);
				break;

			case "enemy":
				_decalMaterial.shader = Shader.Find("HDRP/Decal");
				_decalMaterial.SetColor("_BaseColor", EnemySelectionDecalColor);
				break;
		}

		selection.selectionCircle = Instantiate(_projector.gameObject);
		selection.selectionCircle.name = _projector.name;
		selection.selectionCircle.transform.SetParent(selection.transform, false);
		selection.selectionCircle.transform.eulerAngles = new Vector3(0, 0, 0);
		var size = selection.SelectionDecalSize;
		selection.selectionCircle.transform.localScale += new Vector3(size, 0.01f, size);
	}
}
