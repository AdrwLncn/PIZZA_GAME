using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    [SerializeField] private IngredientArray ingredientArray;
    [SerializeField] private TMP_Text textMenu;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text attempCountText;
    [SerializeField] private GameTimer gameTimer;

    [SerializeField] private Button addButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private Button finishButton;

    [SerializeField] private RectTransform[] positions;

    public List<Ingredient> CurrentIngredients => currentIngredients;
    // Для проверки соблюдения иутрукций
    private List<Ingredient> currentIngredients = new ();
    // Чтобы было удобно удалять объекты (ингредиенты на пицце)
    private List<GameObject> currentGameObjects;
    private Ingredient currentIngredient;
    private Instruction currentInstruction;
    private int money;
    public static int AttempCount;


    public void ChangeInstruction()
    {
        currentInstruction = InstructionArray.Instructions[Random.Range(0, InstructionArray.Instructions.Length - 1)];
        textMenu.text = currentInstruction.Task;
    }

    public void BlockButton()
    {
        removeButton.interactable = false;
        finishButton.interactable = false;
    }

    private void Start()
    {
        attempCountText.text = AttempCount.ToString();
        ChangeInstruction();
        BlockButton();
    }

    private void Update()
    {
        if (gameTimer.CurrentSec == 0)
            MakePizza();
    }

    public void ChangeIngredient(Ingredient ingredient)
    {
        currentIngredient = ingredient;
    }
    // И такой метод для каждого ингредиента 
    // Называь методы можно по названию ингредиента
    public void ChooseIngredient0()
    {
        ChangeIngredient(ingredientArray.IngredientsList[0]);
    }
    // Прикрепляется к кнопке +
    public void AddIngredient()
    {
        currentIngredients.Add(currentIngredient);
        GameObject obj = Instantiate(currentIngredient.Object, positions[currentGameObjects.Count]);
        currentGameObjects.Add(obj);

        removeButton.interactable = true;
        if (currentIngredients.Count > 0)
            removeButton.interactable = true;
        if (currentIngredients.Count > 9)
            finishButton.interactable = true;
        if (currentIngredients.Count == 20)
            addButton.interactable = false;
    }
    // Прикрепляется к кнопке -
    public void RemoveIngredient()
    {
        currentIngredients.RemoveAt(currentIngredients.Count - 1);
        var obj = currentGameObjects[currentGameObjects.Count - 1];
        currentGameObjects.Remove(obj);
        Destroy(obj);

        if (currentIngredients.Count == 0)
            removeButton.interactable = false;
        if (currentIngredients.Count < 10)
            finishButton.interactable = false;
        if (currentIngredients.Count < 20)
            addButton.interactable = true;
    }
    // Прикрепляется к кнопке сбросить пицццу
    public void ResesPizza()
    {
        currentIngredients = new List<Ingredient>();
        foreach (var obj in currentGameObjects)
            Destroy(obj);
        currentGameObjects = new List<GameObject>();
    }

    public void CheckPizza()
    {
        if (currentInstruction.Check(currentIngredients) && AttempCount > 0)
            moneyText.text = (money + 100).ToString();
            if (gameTimer.CurrentSec > 20)
                moneyText.text = (money + 50).ToString();
            else if (gameTimer.CurrentSec > 10)
                moneyText.text = (money + 30).ToString();
        if (AttempCount > 0)
            attempCountText.text = (AttempCount - 1).ToString();
    }
    // Прикрепляется к кнопке завершить
    public void MakePizza() 
    { 
        ResesPizza();
        CheckPizza();
        ChangeInstruction();
        BlockButton();
    }
}
