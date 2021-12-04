using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyBook
{

    static Dictionary<Effect, string> ColorBook = new Dictionary<Effect, string>();
    static Dictionary<Effect, ActionOnTestSubject> ActionOnTestSubjectBook = new Dictionary<Effect, ActionOnTestSubject>();

    static List<KeyValuePair<List<Ingredient>, Effect>> RecipeBook = new List<KeyValuePair<List<Ingredient>, Effect>>();

    public delegate void ActionOnTestSubject(TestSubject testSubject);

    static AlchemyBook()
    {
        registerColors();
        registerActionOnTestSubject();
        registerRecipes();
    }

    private static void registerColors()
    {
        //The string is the key in the bottle color dictionnary. Must be synchronized
        ColorBook.Add(Effect.NO_EFFECT, "blue? I dont know change the color maybe");
    }

    private static void registerActionOnTestSubject()
    {
        ActionOnTestSubject action = DelDoNothing;
        ActionOnTestSubjectBook.Add(Effect.NO_EFFECT, action);

        // action change color to red
        action = ChangeColorToRed;
        ActionOnTestSubjectBook.Add(Effect.CHANGE_COLOR_TO_RED, action);

        ActionOnTestSubject action2 = delegate (TestSubject testSubject)
        {
            Debug.Log("2nd way to do delegates, to test");
        };
    }

    private static void registerRecipes()
    {
        RecipeBook.Add(new KeyValuePair<List<Ingredient>, Effect>(new List<Ingredient>(), Effect.NO_EFFECT));
    }


    /*_____________________________________________________________________________________________________________________________*/


    public static string SearchColor(Effect effect)
    {
        string color;
        if(ColorBook.TryGetValue(effect, out color)){
            return color;
        }
        return null;
    }

    public static ActionOnTestSubject SearchActionOnTestSubject(Effect effect)
    {
        ActionOnTestSubject action;
        if (ActionOnTestSubjectBook.TryGetValue(effect, out action))
        {
            return action;
        }
        return null;
    }

    public static Effect SearchRecipe(List<Ingredient> ingredientsList)
    {
        Effect result = Effect.NO_EFFECT;
        foreach (KeyValuePair<List<Ingredient>, Effect> recipe in RecipeBook)
        {
            if(isIngredientListIdentic(ingredientsList, recipe.Key))
            {
                return recipe.Value;
            }
        }

        return result;
    }

    private static bool isIngredientListIdentic(List<Ingredient> list1, List<Ingredient> list2)
    {
        if(list1.Capacity != list2.Capacity)
        {
            return false;
        }
        foreach(Ingredient ingredient1 in list1)
        {
            if(! IngredientContains(ingredient1, list2))
            {
                return false;
            }
        }
        return true;
    }

    private static bool IngredientContains(Ingredient ingredient, List<Ingredient> list)
    {
        foreach(Ingredient ing in list)
        {
            if (ing.Equals(ingredient))
            {
                return true;
            }
        }
        return false;
    }


    /*________________________Actions on testSubject page_______________________________________________________*/


    public static void DelDoNothing(TestSubject testSubject){}
    public static void ChangeColorToRed(TestSubject testSubject) {
        //need to get the child of the TestSubject object (=Capsule at the moment)
        var testSubjectRenderer = testSubject.transform.GetChild(0).GetComponent<Renderer>();
        testSubjectRenderer.material.color= Color.red;
        Debug.Log("test subject color changed to red");
    }
}
