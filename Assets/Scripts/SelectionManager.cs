using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
	[SerializeField] private string _resourceColliderTagName;
	[SerializeField] private string _buildingColliderTagName;
	[SerializeField] private string _unitColliderTagName;
	[SerializeField] private string _outlineTagName;
	[SerializeField] private string _unitOutlineTagName;
	[SerializeField] private GameObject _resourceSelectionOutline;
	[SerializeField] private GameObject _resourceDestinationOutline;
	[SerializeField] private GameObject _buildingSelectionOutline;
	[SerializeField] private GameObject _unitSelectionOutline;

	[SerializeField] private GameObject _wayPoint;

	[SerializeField] private Text _selectionText;

	public List<GameObject> selectedObjects;
	public ResourceCount resourceCount;

	public bool UnitSelected;

	bool isSelecting = false;
	Vector3 mousePosition1;

	public void Start()
	{
		_selectionText.text = null;
		selectedObjects = new List<GameObject>();
		UnitSelected = false;
	}

	public void Update()
	{ 
		if (selectedObjects.Count != 0)
		{
			if (selectedObjects.Count == 1)
			{
				_selectionText.text = selectedObjects[0].name;

				if (resourceCount != null && selectedObjects[0].tag != _unitColliderTagName && selectedObjects[0].tag != _buildingColliderTagName)
				{
					_selectionText.text = selectedObjects[0].name + ": " + resourceCount.Resources;
				}
			}

			if (selectedObjects.Count > 1)
			{
				_selectionText.text = "GroupSelection";
			}

			if (GameObject.FindGameObjectWithTag("unitSelectionOutline") != null)
			{
				var unitSelection = GameObject.FindGameObjectWithTag("unitSelectionOutline");

				if (selectedObjects[0].tag == _unitColliderTagName)
					unitSelection.transform.position = selectedObjects[0].transform.position;
			}
		} 
		else _selectionText.text = null;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			if (Input.GetMouseButtonDown(0))
			{
				Destroy(GameObject.FindGameObjectWithTag(_outlineTagName));
				Destroy(GameObject.FindGameObjectWithTag(_unitOutlineTagName));
				selectedObjects.Clear();
				selectedObjects.Add(hit.transform.gameObject);

				if (hit.transform.CompareTag(_resourceColliderTagName))
				{
					SelectionOutline(_resourceSelectionOutline, hit.transform);
					resourceCount = hit.transform.gameObject.GetComponent<ResourceCount>();
					UnitSelected = false;
				}

				else if (hit.transform.CompareTag(_buildingColliderTagName))
				{
					SelectionOutline(_buildingSelectionOutline, hit.transform);
					UnitSelected = false;
					if (hit.transform.childCount > 0)
					{
						var waypoint = hit.transform.GetChild(0);
						waypoint.GetComponent<Renderer>().enabled = true;
					}
				}

				else if (hit.transform.CompareTag(_unitColliderTagName))
				{
					SelectionOutline(_unitSelectionOutline, hit.transform);
					UnitSelected = true;
				}
				else
				{
					Destroy(GameObject.FindGameObjectWithTag(_unitOutlineTagName));
					Destroy(GameObject.FindGameObjectWithTag(_outlineTagName));
					_selectionText.text = null;
					UnitSelected = false;
					selectedObjects.Clear();
					var waypoints = GameObject.FindGameObjectsWithTag("waypoint");

					foreach (GameObject waypoint in waypoints)
					{
						waypoint.GetComponent<Renderer>().enabled = false;
					}
				}
			}

			if (Input.GetMouseButtonDown(1))
			{
				if (hit.transform.CompareTag(_resourceColliderTagName) && selectedObjects[0].tag == _unitColliderTagName)
				{
					StartCoroutine(SelectionBlinking(hit.transform));
				}

				if (hit.transform.CompareTag(_resourceColliderTagName) && selectedObjects[0].tag == _buildingColliderTagName)
				{
					if (selectedObjects[0].transform.childCount > 0)
					{
						var oldPoint = selectedObjects[0].transform.GetChild(0).gameObject;
						Debug.Log("destroy waypoint");
						Destroy(oldPoint);
					}

					var point = Instantiate(_wayPoint, hit.transform.position, Quaternion.identity);
					point.transform.parent = selectedObjects[0].transform;
					StartCoroutine(SelectionBlinking(hit.transform));
				}
			}
		}
		// If we press the left mouse button, save mouse location and begin selection
		if (Input.GetMouseButtonDown(0))
		{
			isSelecting = true;
			mousePosition1 = Input.mousePosition;
		}
		// If we let go of the left mouse button, end selection
		if (Input.GetMouseButtonUp(0))
			isSelecting = false;
	}

	void SelectionOutline(GameObject outline, Transform position)
	{
		var instance = Instantiate(outline, position.position, Quaternion.identity);
		var ui = GameObject.Find("UI");
		instance.transform.parent = ui.transform;
	}

	IEnumerator SelectionBlinking(Transform resource)
	{
		var outline = Instantiate(_resourceDestinationOutline, resource.transform.position, Quaternion.identity);
		var ui = GameObject.Find("UI");
		outline.transform.parent = ui.transform;

		if (outline != null)
		{
			outline.transform.gameObject.GetComponent<Renderer>().enabled = true;
			yield return new WaitForSeconds(0.2f);
			outline.transform.gameObject.GetComponent<Renderer>().enabled = false;
			yield return new WaitForSeconds(0.2f);
			Destroy(outline);
		}
	}
}
