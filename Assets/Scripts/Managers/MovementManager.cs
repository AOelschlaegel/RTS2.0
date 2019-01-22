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

	[SerializeField] private List<GameObject> _destinationDecals;

	#region UnityEvents

	private void Start()
	{
		selectedUnits = new List<GameObject>();
		playerAgents = new List<NavMeshAgent>();
		_selectionManager = GameObject.Find("Selection").GetComponent<Selection>();
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
					if (_isMovingToObject)
					{
						playerAgents[x].SetDestination(GetObjectUnderCursor().transform.position);
					}
					else
					{
						var offset = 1.5f;
						var offsetPos = x * offset - playerAgents.Count / offset;

						var position = GetPointUnderCursor() + new Vector3(offsetPos, 0, 0);
						var size = _destinationDecalSize;
						DrawDecal(size, position);
						playerAgents[x].SetDestination(position);
					}
				}
			}
		}
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
