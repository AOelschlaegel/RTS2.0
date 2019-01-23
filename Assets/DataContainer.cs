using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
public class DataContainer : MonoBehaviour
{
	[HideInInspector] public List<Vector3> GatherPoints;
	private float offset = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
		GatherPoints = new List<Vector3>();

		switch (GetComponent<NeutralDataContainer>().ObjectSize)
		{
			case "1x1": //Trees etc.

				GatherPoints.Add(new Vector3(0, 0, offset));
				GatherPoints.Add(new Vector3(offset, 0, offset));
				GatherPoints.Add(new Vector3(offset, 0, -offset));
				GatherPoints.Add(new Vector3(-offset, 0, -offset));
				GatherPoints.Add(new Vector3(-offset, 0, 0));
				GatherPoints.Add(new Vector3(-offset, 0, offset));
				GatherPoints.Add(new Vector3(0, 0, -offset));
				GatherPoints.Add(new Vector3(offset, 0, offset));

				break;

			case "2x2": //Pumpkins etc.
				GatherPoints.Add(new Vector3(-offset, 0, 0.5f));
				GatherPoints.Add(new Vector3(-offset, 0, 0));
				GatherPoints.Add(new Vector3(-offset, 0, -0.5f));
				GatherPoints.Add(new Vector3(-0.5f, 0, -offset));
				GatherPoints.Add(new Vector3(0, 0, -offset));
				GatherPoints.Add(new Vector3(0.5f, 0, -offset));
				GatherPoints.Add(new Vector3(offset, 0, -0.5f));
				GatherPoints.Add(new Vector3(offset, 0, 0));
				GatherPoints.Add(new Vector3(offset, 0, 0.5f));
				GatherPoints.Add(new Vector3(0.5f, 0, offset));
				GatherPoints.Add(new Vector3(0, 0, offset));
				GatherPoints.Add(new Vector3(-0.5f, 0, offset));
				break;
		}


	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
