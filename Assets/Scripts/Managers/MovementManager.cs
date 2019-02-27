using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering.HDPipeline;
using RTS;

public class MovementManager : MonoBehaviour
{
	public LayerMask groundLayer;

	private SelectionManager _selectionManager;

	public List<GameObject> selectedUnits;

	public List<NavMeshAgent> playerAgents;

	[SerializeField] private DecalProjectorComponent _projector;
	[SerializeField] private Transform _uiRoot;

	[SerializeField] private bool _isMovingToObject;
	[SerializeField] private int _destinationDecalSize;

	[SerializeField] private List<GameObject> _destinationDecals;

	#region UnityEvents

	private void Start()
	{
		selectedUnits = new List<GameObject>();
		playerAgents = new List<NavMeshAgent>();
		_selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
		_destinationDecals = new List<GameObject>();
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
				if (_destinationDecals.Count > 0)
				{
					foreach (GameObject decal in _destinationDecals)
					{
						Destroy(decal);

					}
					_destinationDecals.Clear();
				}

				var hitObject = GetObjectUnderCursor();

				if (hitObject.gameObject.GetComponent<SelectableUnitComponent>())
				{
					var selectableUnitComponent = hitObject.gameObject.GetComponent<SelectableUnitComponent>();

					_isMovingToObject = true;

					switch (selectableUnitComponent.SelectionDecalType)

					{
						case "neutral":

							var size = selectableUnitComponent.SelectionDecalSize;
							var position = GetObjectUnderCursor().transform.position;
							DrawDecal(size, position);
							break;

						//TODO
						case "enemy":

							break;
					}
				}
				else
				{
					_isMovingToObject = false;
				}

				for (int x = 0; x < playerAgents.Count; x++)
				{
					// Get selected Unit
					var _unit = playerAgents[x].gameObject;

					// Get the DataContainer of Selected Unit
					var _unitDataContainer = _unit.GetComponent<UnitDataContainer>();

					// If Destination is an Object
					if (_isMovingToObject)
					{
						// Get DestinationObject and it's Pos
						var _destinationObject = GetObjectUnderCursor().gameObject;
						var _destinationObjectPos = _destinationObject.transform.position;

						// Get Available GatherPoints of DestinationObject
						var _availableGatherPoints = _destinationObject.GetComponent<NeutralDataContainer>().AvailableGatherPoints;

						// Get Occupied GatherPoints of DestinationObject
						var _occupiedGatherPoints = _destinationObject.GetComponent<NeutralDataContainer>().OccupiedGatherPoints;

						// Get Random GatherPointPosition from Available GatherPoints
						var _randomPos = Random.Range(0, _availableGatherPoints.Count);

						// Create new GatherPoint
						var _gatherPoint = new NeutralDataContainer.GatherPoint();

						_gatherPoint.Position = _availableGatherPoints[_randomPos].Position;

						foreach (NeutralDataContainer.GatherPoint gatherPoint in _availableGatherPoints)
						{
							if (gatherPoint.Position == _availableGatherPoints[x].Position)
							{
								// Add variables to GatherPoint
								_gatherPoint.Position = _availableGatherPoints[_randomPos].Position;
								_gatherPoint.Object = _unit;
							}
						}

						// If Unit is already gathering
						if (_unitDataContainer.IsGathering == true)
						{
							// Stay at the same GatherPoint
							MoveUnit(_unit, _destinationObjectPos + _unitDataContainer.GatherPoint.Position);
						}

						// If Unit is not gathering
						else
						{
							// Add new GatherPoint to Occupied GatherPoints
							_occupiedGatherPoints.Add(_gatherPoint);

							// Add GatherPoint to Selected Unit
							var _unitGatherPoint = new NeutralDataContainer.GatherPoint();
							_unitGatherPoint.Object = _destinationObject;
							_unitGatherPoint.Position = _gatherPoint.Position;
							_unitDataContainer.GatherPoint = _unitGatherPoint;

							// Move Unit to GatherPoint
							MoveUnit(_unit, _destinationObjectPos + _unitDataContainer.GatherPoint.Position);

							_unitDataContainer.IsGathering = true;
						}
					}

					// If Destination is not an Object
					else
					{
						// Create Distance between Units in Formation
						var offset = 1.5f;
						var offsetPos = x * offset - playerAgents.Count / offset;
						var position = GetPointUnderCursor() + new Vector3(offsetPos, 0, 0);

						// Draw DestinationDecal on Map
						DrawDecal(_destinationDecalSize, position);

						// Move Unit to Position
						MoveUnit(_unit, position);

						// Add Position to UnitContainer
						_unitDataContainer.DestinationPosition = position;

						// Initialize GatherPoint of Unit
						_unitDataContainer.GatherPoint = new NeutralDataContainer.GatherPoint();
						_unitDataContainer.IsGathering = false;
					}
				}
			}
		}
	}

	private void MoveUnit(GameObject unit, Vector3 destination)
	{
		unit.GetComponent<NavMeshAgent>().SetDestination(destination);
	}

	private void DrawDecal(int size, Vector3 position)
	{
		var destinationDecal = Instantiate(_projector.gameObject, position, Quaternion.identity);
		_destinationDecals.Add(destinationDecal);
		destinationDecal.transform.localScale += new Vector3(size, 0.01f, size);
		destinationDecal.transform.eulerAngles = new Vector3(0, 0, 0);
		destinationDecal.name = "destination";
		destinationDecal.transform.SetParent(_uiRoot);
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
