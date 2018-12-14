using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MovementManager : MonoBehaviour
{

	Camera cam;

	public LayerMask groundLayer;

	public NavMeshAgent playerAgent;

	public SelectionManager _selectionManager;

	public GameObject SelectedUnit;

	#region UnityEvents
	private void Awake()
	{
		cam = Camera.main;
	}

	private void Update()
	{
		if (_selectionManager.selectedObject != null)
		{
			playerAgent = _selectionManager.selectedObject.GetComponent<NavMeshAgent>();
			SelectedUnit = _selectionManager.selectedObject;

			if (Input.GetMouseButton(1))
			{
				playerAgent.SetDestination(GetPointUnderCursor());
			}
		}
		else playerAgent = null;
	}


	#endregion

	private Vector3 GetPointUnderCursor()
	{
		RaycastHit hitPosition;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		Physics.Raycast(ray, out hitPosition, 100, groundLayer);

		return hitPosition.point;

	}
}
