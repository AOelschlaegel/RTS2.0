using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
public class DataContainer : MonoBehaviour
{
	[HideInInspector] public List<NeutralDataContainer.GatherPoint> GatherPoints;
	private float offset = 1.5f;

    void Start()
    {
		GatherPoints = new List<NeutralDataContainer.GatherPoint>();

		switch (GetComponent<NeutralDataContainer>().ObjectSize)
		{
			case "1x1": //Trees etc.

				AddToContainer(new Vector3(0, 0, offset));
				AddToContainer(new Vector3(offset, 0, offset));
				AddToContainer(new Vector3(offset, 0, -offset));
				AddToContainer(new Vector3(-offset, 0, -offset));
				AddToContainer(new Vector3(-offset, 0, 0));
				AddToContainer(new Vector3(-offset, 0, offset));
				AddToContainer(new Vector3(0, 0, -offset));
				AddToContainer(new Vector3(offset, 0, offset));

				break;

			case "2x2": //Pumpkins etc.
				AddToContainer(new Vector3(-offset, 0, 0.5f));
				AddToContainer(new Vector3(-offset, 0, 0));
				AddToContainer(new Vector3(-offset, 0, -0.5f));
				AddToContainer(new Vector3(-0.5f, 0, -offset));
				AddToContainer(new Vector3(0, 0, -offset));
				AddToContainer(new Vector3(0.5f, 0, -offset));
				AddToContainer(new Vector3(offset, 0, -0.5f));
				AddToContainer(new Vector3(offset, 0, 0));
				AddToContainer(new Vector3(offset, 0, 0.5f));
				AddToContainer(new Vector3(0.5f, 0, offset));
				AddToContainer(new Vector3(0, 0, offset));
				AddToContainer(new Vector3(-0.5f, 0, offset));
				break;
		}
	}

	void AddToContainer(Vector3 pos)

	{
		var GatherPoint = new NeutralDataContainer.GatherPoint();

		GatherPoint.Object = null;
		GatherPoint.Position = pos;
		GatherPoints.Add(GatherPoint);
	}
}
