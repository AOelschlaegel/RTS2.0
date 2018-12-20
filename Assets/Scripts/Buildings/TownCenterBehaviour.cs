using UnityEngine;
using UnityEngine.AI;

public class TownCenterBehaviour : MonoBehaviour
{
	#region Initialization
	private SelectionManager _selectionManager;
	[SerializeField] private GameObject _wayPointPrefab;
	private ResourceManager _resourceManager;
	[SerializeField] private GameObject _citizen;

	private GameObject _wayPoint;
	private GameObject _hitGameobject;
	public GameObject SelectedObject;

	public bool Selected;
	public bool HasWaypoint;

    private Vector3 _hitPos;

	#endregion

	#region UnityEvents

	private void Start()
	{
		HasWaypoint = false;
		Selected = false;

		_selectionManager = GameObject.Find("GameManager").GetComponent<SelectionManager>();
		_resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
	}

	private void Update()
	{
		Checks();
		Inputs();

	}

	#endregion

	#region CustomEvents

	private void Checks()
	{
		// Check if List is empty
		if (_selectionManager.SelectedObjects.Count != 0)
		{
			SelectedObject = _selectionManager.SelectedObjects[0].gameObject;

			//Check if this gameobject is selected
			if (SelectedObject == gameObject)
			{
				Selected = true;
			}
			else Selected = false;
		}
		else
		{
			SelectedObject = null;
			Selected = false;
		}

		// Check if waypoint exists
		if (this.transform.childCount > 0)
		{
			HasWaypoint = true;
		}

		//Check if Selected and already Has Waypoint
		if (HasWaypoint)
		{
			if(Selected == true)
			{
				_wayPoint.GetComponent<Renderer>().enabled = true;
			}
			else _wayPoint.GetComponent<Renderer>().enabled = false;
		}
	}

	private void Inputs()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Selected == true)
		{
			if (_resourceManager.food >= 30)
			{
				if(Input.GetKeyDown(KeyCode.F))
				{
					CreateCitizen();
					_resourceManager.food -= 30;
				}
			}
            //Start Raycast
            if (Physics.Raycast(ray, out hit))
            {
                //Reference hit object

                _hitGameobject = hit.transform.gameObject;

                if (Input.GetMouseButtonDown(1))
                {
                        // Check 

                        switch (_hitGameobject.layer)
                        {
                            // Buildings
                            case 10:
                                _hitPos = _hitGameobject.transform.position;
                                CreateWaypoint(_hitPos);
                                break;

                            // Ground
                            case 11:
                                _hitPos = hit.point;
                                CreateWaypoint(_hitPos);
                                break;

                            //Units
                            case 12:

                                break;

                        case 13:
                            _hitPos = _hitGameobject.transform.position;
                            CreateWaypoint(_hitPos);

                            break;
                                }
                                
                                }

			}
		}
	}

	private void CreateWaypoint(Vector3 position)
	{
        if(HasWaypoint)
        {
            var waypoint = this.transform.GetChild(0).gameObject;
            Destroy(waypoint);
        }

        _wayPoint = Instantiate(_wayPointPrefab, position, Quaternion.identity);
		_wayPoint.transform.parent = this.transform;
	}

	private void CreateCitizen()
	{
        if (HasWaypoint)
        {
            var waypoint = this.transform.GetChild(0).gameObject;
            var citizen = Instantiate(_citizen, transform.position, Quaternion.identity);
            var agent = citizen.GetComponent<NavMeshAgent>();
            agent.SetDestination(waypoint.transform.position);
        }
        else
        {
            Instantiate(_citizen, transform.position, Quaternion.identity);
        }
	}

	#endregion
}
