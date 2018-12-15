using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementManager : MonoBehaviour
{
	public LayerMask groundLayer;

	public NavMeshAgent playerAgent;

	public SelectionManager _selectionManager;

	public List<GameObject> selectedUnits;

	#region UnityEvents

	private void Start()
	{
		selectedUnits = new List<GameObject>();
	}

	private void Update()
	{
		if (_selectionManager.selectedObjects.Count != 0)
		{
			selectedUnits.Add(_selectionManager.selectedObjects[0].gameObject);
			playerAgent = selectedUnits[0].GetComponent<NavMeshAgent>();

			if (Input.GetMouseButton(1))
			{
				playerAgent.SetDestination(GetPointUnderCursor());
			}
		}
		else playerAgent = null;
		selectedUnits.Clear();
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
