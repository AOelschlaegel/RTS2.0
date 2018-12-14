using System.Collections;
using UnityEngine;


public class MovementController : MonoBehaviour
{
	[SerializeField] private float _movementSpeed;
	[SerializeField] private GameObject _pivot;
	[SerializeField] private Camera _mainCamera;
	[SerializeField] private GameObject[] _colliderObjects;

	private Animator _animator;

	public string _colliderTagName;
	public string _resourceTag;
	public string _gatherZoneTag;

	public GameObject[] DropZones;
	public GameObject CurrentDropZone;
	public float minDropZoneDist;

	public int maxCarryWeight;

	public string CurrentResourceObject;
	public string CurrentResource;
	public bool PivotSet;

	public bool BringToNextCityCentre;

	public float ResourceTime;
	private float _timePerResource = 2;

	public int food;

	private Vector3 _lookAtPos;

	private bool _isGathering;

	private void Start()
	{
		_animator = GetComponent<Animator>();
		ResourceTime = _timePerResource;
	}

	private void Update()
	{
		DropZones = GameObject.FindGameObjectsWithTag("DropZone");

		foreach (GameObject dropzone in DropZones)
		{
			float dist = Vector3.Distance(transform.position, dropzone.transform.position);
			if (dist < minDropZoneDist)
			{
				CurrentDropZone = dropzone.gameObject;
			}
		}

		if (CurrentResourceObject != null)
		{
			if (CurrentResourceObject == "Pumpkins")
			{
				CurrentResource = "food";
			}
		}
		else CurrentResource = null;

		if (CurrentResource == "food")
		{
			ResourceTime -= Time.deltaTime;

			if (ResourceTime <= 0)
			{
				food++;
				ResourceTime = _timePerResource;
			}
		}

		if (food >= maxCarryWeight && PivotSet == false && BringToNextCityCentre)
		{
			Destroy(GameObject.FindGameObjectWithTag("pivot"));
			Instantiate(_pivot, CurrentDropZone.transform.GetChild(0).transform.position, Quaternion.identity);
			PivotSet = true;
		}

			var oldPos = transform.position;
		if (GameObject.FindGameObjectWithTag("pivot") != null)
		{
			var pivot = GameObject.FindGameObjectWithTag("pivot");

			if (_isGathering == false)
			{
				transform.LookAt(pivot.transform.position);
			}

			transform.position = Vector3.MoveTowards(transform.position, pivot.transform.position, 0.01f * _movementSpeed);
		}

		if (_isGathering == true)
		{
			_animator.SetBool("isGathering", true);
			Debug.Log("is Gathering");
		}
		else _animator.SetBool("isGathering", false);

		var newPos = transform.position;

		if (oldPos != newPos)
		{
			_animator.SetBool("isWalking", true);
		}
		else _animator.SetBool("isWalking", false);

		if (Input.GetMouseButtonDown(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform.CompareTag(_colliderTagName))
				{
					Destroy(GameObject.FindGameObjectWithTag("pivot"));
					Instantiate(_pivot, hit.point, Quaternion.identity);
				}

				if (hit.transform.CompareTag(_resourceTag))
				{
					Destroy(GameObject.FindGameObjectWithTag("pivot"));
					Instantiate(_pivot, hit.transform.GetChild(1).position, Quaternion.identity);
					transform.LookAt(hit.transform.position);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == _gatherZoneTag)
		{
			_isGathering = true;
			CurrentResourceObject = other.gameObject.transform.parent.gameObject.name;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == _gatherZoneTag)
		{
			_isGathering = false;
			CurrentResourceObject = null;
		}
	}
}
