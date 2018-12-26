using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementManager : MonoBehaviour
{
	public LayerMask groundLayer;

	private SelectionManager _selectionManager;

	public List<GameObject> selectedUnits;

	public List<NavMeshAgent> playerAgents;

	#region UnityEvents

	private void Start()
	{
		selectedUnits = new List<GameObject>();
		playerAgents = new List<NavMeshAgent>();
		_selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
	}

	private void Update()
	{
		selectedUnits = _selectionManager.SelectedObjects;

		if (_selectionManager.SelectedType == "Unit")
		{
			if (playerAgents.Count != selectedUnits.Count)
				playerAgents.Add(selectedUnits[0].GetComponent<NavMeshAgent>());
		}
		if (playerAgents.Count != 0 && selectedUnits.Count != 0)
		{
			if (playerAgents[0].gameObject != selectedUnits[0].gameObject)
			{
				playerAgents.Clear();
			}
		} else playerAgents.Clear();

		if (Input.GetMouseButton(1))
		{
			foreach (NavMeshAgent agent in playerAgents)
			{
				agent.SetDestination(GetPointUnderCursor());
			}
		}
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
