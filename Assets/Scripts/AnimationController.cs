using UnityEngine;
public class AnimationController : MonoBehaviour
{
	private Animator _animator;

	private Vector3 _oldPos;
	private Vector3 _newPos;

	public ResourceManager _resourceManager;

	public string resource;
	public bool IsCollectingFood;

	[SerializeField] private string _resourceTag;

	public float ResourceTime;
	private float _timePerResource = 5f;

	private void Start()
	{
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		CollectingFood();

		_newPos = transform.position;

		if (_oldPos != _newPos)
		{
			_animator.SetBool("isWalking", true);
		} else _animator.SetBool("isWalking", false);

		_oldPos = transform.position;
	}

	public void OnTriggerEnter(Collider other)
	{
		if(other.tag == _resourceTag)
		{
			transform.LookAt(other.transform);
			_animator.SetBool("isGathering", true);

			if (other.name == "Pumpkins")
			{
				resource = "food";
				IsCollectingFood = true;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == _resourceTag)
		{
			_animator.SetBool("isGathering", false);
			resource = null;
			IsCollectingFood = false;
		}
	}

	void CollectingFood()
	{
		ResourceTime -= Time.deltaTime;

		if (IsCollectingFood)
		{
			if (ResourceTime <= 0)
			{
				_resourceManager.food++;
				ResourceTime = _timePerResource;
			}
		}
	}
}
