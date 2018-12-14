using UnityEngine;
public class AnimationController : MonoBehaviour
{
	private Animator _animator;

	private Vector3 _oldPos;
	private Vector3 _newPos;

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
}
