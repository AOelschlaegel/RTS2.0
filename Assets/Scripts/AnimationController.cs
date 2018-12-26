using UnityEngine;
public class AnimationController : MonoBehaviour
{
	private Animator _animator;

	private Vector3 _oldPos;
	private Vector3 _newPos;

	private ResourceManager _resourceManager;

	public string resource;
	public bool IsCollectingFood;

	public float ResourceTime;
	private float _timePerResource = 5f;

	public NeutralDataContainer neutralDataContainer;
	private UnitDataContainer _unitDataContainer;

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        ResourceTime = _timePerResource;
		_unitDataContainer = GetComponent<UnitDataContainer>();


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
		if(other.tag == "neutral")
		{
			if (other.name == "Pumpkins")
			{
				neutralDataContainer = other.gameObject.GetComponent<NeutralDataContainer>();

				if (neutralDataContainer.Resources > 0)
				{
					resource = "food";
					IsCollectingFood = true;
					_animator.SetBool("isGathering", true);
					transform.LookAt(other.transform);
				}
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "neutral")
		{
			_animator.SetBool("isGathering", false);
			resource = null;
			IsCollectingFood = false;
			neutralDataContainer = null;
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
				neutralDataContainer.Resources--;
				_unitDataContainer.Resources++;
			}
		}
	}
}
