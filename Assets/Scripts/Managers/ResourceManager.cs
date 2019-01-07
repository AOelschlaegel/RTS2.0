using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{ 
	public int food;
	public int wood;

	public Text foodText;
	public Text woodText;

	private void Update()
	{
		if (food != 0)
		{
			foodText.text = "Food: " + food.ToString();
		}
		else foodText.text = null;

		if (wood != 0)
		{
			woodText.text = "Wood: " + wood.ToString();
		}
		else woodText.text = null;
	}
}
