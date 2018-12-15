using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementManager : MonoBehaviour
{
	public LayerMask groundLayer;

	public SelectionManager _selectionManager;

	public List<GameObject> selectedUnits;

	public List<NavMeshAgent> playerAgents;

	#region UnityEvents

	private void Start()
	{
		selectedUnits = new List<GameObject>();
		playerAgents = new List<NavMeshAgent>();
	}

	private void Update()
	{
		if (_selectionManager.selectedObjects.Count != 0)
		{
			if (_selectionManager.selectedObjects.Count != selectedUnits.Count && _selectionManager.UnitSelected == true)
			{
				var selection = _selectionManager.selectedObjects[0].gameObject;
				var agent = selection.GetComponent<NavMeshAgent>();
				AddToSelection(selection, agent);
			}

			if (Input.GetMouseButton(1))
			{
				foreach (NavMeshAgent agent in playerAgents)
				{
					agent.SetDestination(GetPointUnderCursor());
				}
			}
		}

		else
		{
			selectedUnits.Clear();
			playerAgents.Clear();
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

	private void AddToSelection(GameObject selection, NavMeshAgent agent)
	{
		selectedUnits.Add(selection);
		playerAgents.Add(agent);
	}
}
