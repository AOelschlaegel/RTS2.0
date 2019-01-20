using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementManager : MonoBehaviour
{
	public LayerMask groundLayer;

	private Selection _selectionManager;

	public List<GameObject> selectedUnits;

	public List<NavMeshAgent> playerAgents;

	#region UnityEvents

	private void Start()
	{
		selectedUnits = new List<GameObject>();
		playerAgents = new List<NavMeshAgent>();
		_selectionManager = GameObject.Find("Selection").GetComponent<Selection>();
	}

	private void Update()
	{
		if (_selectionManager.UnitSelection)
		{
			playerAgents.Clear();
			selectedUnits = _selectionManager.CurrentSelection;

			foreach (var unit in selectedUnits)
			{
				playerAgents.Add(unit.GetComponent<NavMeshAgent>());
			}

			if (Input.GetMouseButton(1))
			{
				foreach (NavMeshAgent agent in playerAgents)
				{
					agent.SetDestination(GetPointUnderCursor());
				}
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
