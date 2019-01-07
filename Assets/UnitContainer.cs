using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitContainer : MonoBehaviour
{
	[SerializeField] private Transform _pop1Units;
	[SerializeField] private Text _popCount;
	public int Pop;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		Pop = _pop1Units.childCount;
		_popCount.text = "Population:" + Pop.ToString();
    }
}
