using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
	#region Serialization
	[Header("Setup")]
	[SerializeField] private GameObject _neutralSelectionOutline;
	[SerializeField] private GameObject _resourceDestinationOutline;
	[SerializeField] private GameObject _buildingSelectionOutline;
	[SerializeField] private GameObject _unitSelectionOutline;

	[Header("Info")]
	public GameObject Selector;
	public List<GameObject> SelectedObjects;
	public Text SelectionText;
    public Text BuildQueue;
	public string SelectedType;
	public string SelectedObject;



	private ResourceCount _resourceCount;
	#endregion

	#region UnityEvents
	public void Start()
	{
		SelectionText.text = null;
        BuildQueue.text = null;
		SelectedObjects = new List<GameObject>();
	}

	public void Update()
	{
		Checks();
		Inputs();
	}

	#endregion

	#region CustomEvents

	void Inputs()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		//Start Raycast
		if (Physics.Raycast(ray, out hit))
		{
			//Reference hit object
			var hitGameobject = hit.transform.gameObject;

			//Check if LeftMouseButton was pressed
			if (Input.GetMouseButtonDown(0))
			{
				//Check if list already contains hit Object
				if (!SelectedObjects.Contains(hitGameobject))
				{
					//Clear selection
					SelectedObjects.Clear();

					//Add hitObject to selectionList
					SelectedObjects.Add(hitGameobject);

					//Cycle through possible Layers
					switch (hitGameobject.layer)
					{
						case 10: //Buildings
							SelectedType = "Building";
							DrawSelectionOutline(_buildingSelectionOutline, hit.transform);
							break;

						case 11: //Ground
							SelectedType = null;
							SelectedObject = null;
							SelectedObjects.Clear();
							DestroyAllSelectors();
							break;

						case 12: //Units
							SelectedType = "Unit";
							DrawSelectionOutline(_unitSelectionOutline, hit.transform);
							break;

						case 13: //Neutral (resources etc.)
							SelectedType = "Neutral";
							DrawSelectionOutline(_neutralSelectionOutline, hit.transform);
							break;
					}
				}
			}
		}
	}

	void Checks()
	{
		//Check if object is selected
		if (SelectedObject != null)
		{
			SelectionText.text = SelectedObject;

		}
		else
		{
            // Reset SelectionText and Queue if nothing is selected
			SelectionText.text = null;
            BuildQueue.text = null;
        }

		//Check if anything is in List
		if (SelectedObjects.Count != 0)
		{
			SelectedObject = SelectedObjects[0].name;

            if (SelectedObjects[0].tag == "building")
            {
                // Get Queue if a building is selected
                var queue = SelectedObjects[0].GetComponent<QueueBehaviour>();
                if (queue.IsCreating == true)
                {
                    BuildQueue.text = queue.QueueTime.ToString();
                }
            }

            //Check if selectionOutline exists
            if (Selector != null)
			{
				//Make outline follow selection
				Selector.transform.position = SelectedObjects[0].transform.position;
			}
		}
	}

	void DrawSelectionOutline(GameObject outline, Transform hit)
	{
		DestroyAllSelectors();
		var ui = GameObject.Find("UI");
		Selector = Instantiate(outline, hit.transform.position, Quaternion.identity);
		Selector.transform.parent = ui.transform;
	}
	
	void DestroyAllSelectors()
	{
		var selectors = GameObject.FindGameObjectsWithTag("selector");
		foreach (GameObject selector in selectors)
		{
			Destroy(selector);
		}
	}

	#endregion

	IEnumerator SelectionBlinking(Transform resource)
	{
		var outline = Instantiate(_resourceDestinationOutline, resource.transform.position, Quaternion.identity);
		var ui = GameObject.Find("UI");
		outline.transform.parent = ui.transform;

		if (outline != null)
		{
			outline.transform.gameObject.GetComponent<Renderer>().enabled = true;
			yield return new WaitForSeconds(0.2f);
			outline.transform.gameObject.GetComponent<Renderer>().enabled = false;
			yield return new WaitForSeconds(0.2f);
			Destroy(outline);
		}
	}
}
