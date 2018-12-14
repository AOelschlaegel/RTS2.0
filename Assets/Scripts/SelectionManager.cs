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


	public void Start()
	{
		_selectionText.text = null;
	}

	public void Update()
	{
		if(GameObject.FindGameObjectWithTag("unitSelectionOutline") != null)
		{
			var unitSelection = GameObject.FindGameObjectWithTag("unitSelectionOutline");
			unitSelection.transform.position = selectedObject.transform.position;
		}

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
				else if (hit.transform.CompareTag(_unitColliderTagName))
				{
					Destroy(GameObject.FindGameObjectWithTag(_unitOutlineTagName));
					Instantiate(_unitSelectionOutline, hit.transform.position, Quaternion.identity);
					selectedObject = hit.transform.gameObject;
					_selectionText.text = selectedObject.name;
				}
				else
				{
					Destroy(GameObject.FindGameObjectWithTag(_unitOutlineTagName));
					Destroy(GameObject.FindGameObjectWithTag(_outlineTagName));
					selectedObject = null;
					_selectionText.text = null;
				}
			}
		}
	}
}
