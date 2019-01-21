using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class MovementManager : MonoBehaviour
{
	public LayerMask groundLayer;

	private Selection _selectionManager;

	public List<GameObject> selectedUnits;

	public List<NavMeshAgent> playerAgents;

	[SerializeField] private DecalProjectorComponent _projector;
	[SerializeField] private Transform _uiRoot;

	[SerializeField] private bool _isMovingToObject;
	[SerializeField] private int _destinationDecalSize;

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

			if (Input.GetMouseButtonDown(1))
			{
				if(_uiRoot.Find("destination"))
				{
					Destroy(_uiRoot.Find("destination").gameObject);
				}

				var hitObject = GetObjectUnderCursor();

				if (hitObject.gameObject.GetComponent<SelectableUnitComponent>())
				{
					var selectableUnitComponent = hitObject.gameObject.GetComponent<SelectableUnitComponent>();

					_isMovingToObject = true;

					switch (selectableUnitComponent.SelectionDecalType)

					{
						case "neutral":
							var destinationDecal = Instantiate(_projector.gameObject, hitObject.transform.position, Quaternion.identity);
							var size = selectableUnitComponent.SelectionDecalSize;
							destinationDecal.transform.localScale += new Vector3(size, 0.01f, size);
							destinationDecal.transform.eulerAngles = new Vector3(0, 0, 0);
							destinationDecal.name = "destination";
							destinationDecal.transform.SetParent(_uiRoot);

							break;

							//TODO
						case "enemy":

							break;
					}
				}
				else
				{
					_isMovingToObject = false;
					var destination = Instantiate(_projector.gameObject, GetPointUnderCursor(), Quaternion.identity);
					destination.name = "destination";
					destination.transform.localScale += new Vector3(_destinationDecalSize, 0.01f, _destinationDecalSize);
					destination.transform.SetParent(_uiRoot);
				}

				foreach (NavMeshAgent agent in playerAgents)
				{
					if(_isMovingToObject == true)
					{
						agent.SetDestination(GetObjectUnderCursor().transform.position);
					} else
					agent.SetDestination(GetPointUnderCursor());
				}
			}
		}
	}


	#endregion

	private GameObject GetObjectUnderCursor()
	{
		RaycastHit hitPosition;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out hitPosition, 100, groundLayer);
		return hitPosition.transform.gameObject;
	}

	private Vector3 GetPointUnderCursor()
	{
		RaycastHit hitPosition;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out hitPosition, 100, groundLayer);
		return hitPosition.point;
	}
}
