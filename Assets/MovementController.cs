using UnityEngine;
public class MovementController : MonoBehaviour
{
	[SerializeField] private float _movementSpeed;
	[SerializeField] private GameObject _pivot;
	[SerializeField] private Camera _mainCamera;
	[SerializeField] private GameObject[] _colliderObjects;

	private Animator _animator;
	private float _lookAtSpeed = 0.1f;

	public string _colliderTagName;
	public string _gatherZoneTagName;

	private bool _isGathering;

	private void Start()
	{
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		var oldPos = transform.position;
		if (GameObject.FindGameObjectWithTag("pivot") != null)
		{
			var pivot = GameObject.FindGameObjectWithTag("pivot");
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(pivot.transform.position), _lookAtSpeed);
			transform.position = Vector3.MoveTowards(transform.position, pivot.transform.position, 0.01f * _movementSpeed);
		}

		if (_isGathering == true)
		{
			_animator.SetBool("isGathering", true);
			Debug.Log("is Gathering");
		} else _animator.SetBool("isGathering", false);

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
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == _gatherZoneTagName)
		{
			_isGathering = true;
			transform.position = other.transform.position;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == _gatherZoneTagName)
		{
			_isGathering = false;
		}
	}

	private void GatherResources(Transform resource)
	{

	}
}
