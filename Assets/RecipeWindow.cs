using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeWindow : MonoBehaviour {
    ScrollRect scrollRect;
    [SerializeField]RectTransform contentRect;
    [SerializeField] GameObject recipeElement;
    [SerializeField] RectTransform canvasRect;
    // Use this for initialization
    void Start () {
        scrollRect = transform.Find("Scroll View").GetComponent<ScrollRect>();
       // contentRect = transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();



    }
	
    public void SetElements(List<Yaku> yakus)
    {
        foreach (Yaku yaku in yakus)
        {
            GameObject newObj = Instantiate(recipeElement, GameObject.Find("RecipeWindow/Scroll View/Viewport/Content").transform);
            RecipeElement element = newObj.GetComponent<RecipeElement>();
            element.SetUI(yaku);
        }

        contentRect.offsetMin = new Vector2(0, -yakus.Count * 105 + canvasRect.rect.height);
    }
	// Update is called once per frame
	void Update () {
        
    }
}
