using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
	private Animator _animator;

	private Vector3 _oldPos;
	private Vector3 _newPos;

	// Start is called before the first frame update
	void Start()
    {
		_animator = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
		_newPos = transform.position;

		if (_oldPos != _newPos)
		{
			_animator.SetBool("isWalking", true);
		}
		else _animator.SetBool("isWalking", false);

		_oldPos = transform.position;
	}
}
