using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipesManager : MonoBehaviour {
    List<Yaku> yakus = new List<Yaku>();
    [SerializeField] RecipeWindow recipeWindow;

	// Use this for initialization
	void Start () {

        yakus = PresentUtility.GetAllYaku();
        recipeWindow.SetElements(yakus);
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown(KeyConfig.Cancel))
        {
            GameManager.LoadScene(GameScenes.Title);
        }
	}
}
