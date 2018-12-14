using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
	[SerializeField] private string _resourceColliderTagName;
	[SerializeField] private string _buildingColliderTagName;
	[SerializeField] private string _outlineTagName;
	[SerializeField] private GameObject _resourceSelectionOutline;
	[SerializeField] private GameObject _buildingSelectionOutline;

	[SerializeField] private Text _selectionText;

	public GameObject selectedObject;

	private void Start()
	{
		_selectionText.text = null;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform.CompareTag(_resourceColliderTagName))
				{
					Destroy(GameObject.FindGameObjectWithTag(_outlineTagName));
					Instantiate(_resourceSelectionOutline, hit.transform.position, Quaternion.identity);
					selectedObject = hit.transform.gameObject;
					_selectionText.text = selectedObject.name;
				}
				else if (hit.transform.CompareTag(_buildingColliderTagName))
				{
					Destroy(GameObject.FindGameObjectWithTag(_outlineTagName));
					Instantiate(_buildingSelectionOutline, hit.transform.position, Quaternion.identity);
					selectedObject = hit.transform.gameObject;
					_selectionText.text = selectedObject.name;
				}
				else
				{
					Destroy(GameObject.FindGameObjectWithTag(_outlineTagName));
					selectedObject = null;
					_selectionText.text = null;
				}


			}
		}
	}

}
