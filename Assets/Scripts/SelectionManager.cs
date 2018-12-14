using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
	[SerializeField] private string _colliderTagName;
	[SerializeField] private string _outlineTagName;
	[SerializeField] private GameObject _selectionOutline;

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
				if (hit.transform.CompareTag(_colliderTagName))
				{
					Destroy(GameObject.FindGameObjectWithTag(_outlineTagName));
					Instantiate(_selectionOutline, hit.transform.position, Quaternion.identity);
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
