using System.Collections;
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
	[SerializeField] private GameObject _buildingSelectionOutline;
	[SerializeField] private GameObject _unitSelectionOutline;

	[SerializeField] private Text _selectionText;

	public GameObject selectedObject;
	public ResourceCount resourceCount;

	bool isSelecting = false;
	Vector3 mousePosition1;

	public void Start()
	{
		_selectionText.text = null;
	}

	public void Update()
	{
		if (selectedObject != null)
		{
			_selectionText.text = selectedObject.name;

			if (resourceCount != null && selectedObject.tag != _unitColliderTagName && selectedObject.tag != _buildingColliderTagName)
			{
				_selectionText.text = selectedObject.name + ": " + resourceCount.Resources;
			}
		}
		else _selectionText.text = null;

		if (GameObject.FindGameObjectWithTag("unitSelectionOutline") != null)
		{
			var unitSelection = GameObject.FindGameObjectWithTag("unitSelectionOutline");

			if (selectedObject.tag == _unitColliderTagName)
				unitSelection.transform.position = selectedObject.transform.position;
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			if (Input.GetMouseButtonDown(0))
			{
				Destroy(GameObject.FindGameObjectWithTag(_outlineTagName));
				Destroy(GameObject.FindGameObjectWithTag(_unitOutlineTagName));

				if (hit.transform.CompareTag(_resourceColliderTagName))
				{
					Instantiate(_resourceSelectionOutline, hit.transform.position, Quaternion.identity);
					selectedObject = hit.transform.gameObject;
					resourceCount = hit.transform.gameObject.GetComponent<ResourceCount>();
				}

				else if (hit.transform.CompareTag(_buildingColliderTagName))
				{
					Instantiate(_buildingSelectionOutline, hit.transform.position, Quaternion.identity);
					selectedObject = hit.transform.gameObject;
				}

				else if (hit.transform.CompareTag(_unitColliderTagName))
				{
					Instantiate(_unitSelectionOutline, hit.transform.position, Quaternion.identity);
					selectedObject = hit.transform.gameObject;
				}

				else
				{
					Destroy(GameObject.FindGameObjectWithTag(_unitOutlineTagName));
					Destroy(GameObject.FindGameObjectWithTag(_outlineTagName));
					selectedObject = null;
					_selectionText.text = null;
				}
			}

			if (Input.GetMouseButtonDown(1))
			{
				if (hit.transform.CompareTag(_resourceColliderTagName) && selectedObject.tag == _unitColliderTagName)
				{
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

	IEnumerator SelectionBlinking(Transform resource)
	{
		var outline = Instantiate(_resourceSelectionOutline, resource.transform.position, Quaternion.identity);

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
