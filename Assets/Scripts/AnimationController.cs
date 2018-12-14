using UnityEngine;
public class AnimationController : MonoBehaviour
{
	private Animator _animator;

	private Vector3 _oldPos;
	private Vector3 _newPos;

	[SerializeField] private string _resourceTag;

	private void Start()
	{
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		_newPos = transform.position;

		if (_oldPos != _newPos)
		{
			_animator.SetBool("isWalking", true);
		} else _animator.SetBool("isWalking", false);

		_oldPos = transform.position;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == _resourceTag)
		{
			transform.LookAt(other.transform);
			_animator.SetBool("isGathering", true);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == _resourceTag)
		{
			_animator.SetBool("isGathering", false);
		}
	}
}
