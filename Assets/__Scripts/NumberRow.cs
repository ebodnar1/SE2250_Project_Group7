using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberRow : MonoBehaviour
{
    //Buttons and text for each button
    public List<Button> buttons = new List<Button>();
    private List<Text> circleTexts;
    private int currentIndex;
    private Player player;

    void Start()
    {
        //Create a list for the text for each button
        circleTexts = new List<Text>();
        for (int i = 0; i < buttons.Count; i++)
        {
            circleTexts.Add(buttons[i].gameObject.GetComponentInChildren<Text>());
        }

        //Create a list of numbers 1 to 8
        List<int> numbers = new List<int>();
        int max = buttons.Count;
        for (int i = 1; i <= max; i++)
        {
            numbers.Add(i);
        }

        //Randomize the order
        Randomize(numbers);

        //Set the text of each circle to one of the random numbers
        int counter = 0;
        foreach (Text ct in circleTexts)
        {
            ct.text = "" + numbers[counter];
            counter++;
        }

        //Set the current index to 1
        currentIndex = 1;

        //Ensure each button has functionality using an event listener
        for (int i = 0; i < max; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => CheckClick(index));
        }

        //Get the player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    //Allow the player to exit
    private void Update()
    {
        if (gameObject.activeInHierarchy && Input.GetKeyDown("escape"))
        {
            ResetColours();
            currentIndex = 1;
            gameObject.transform.parent.gameObject.SetActive(false);
            player.ReturnToScene();
        }
    }

    //Randomizes a list
    private void Randomize(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    //Checks to see if a button was clicked
    private void CheckClick(int index)
    {
        //Get the pressed button's game object, image, and text (number)
        GameObject buttonObj = buttons[index].gameObject;
        Image buttonImg = buttonObj.GetComponent<Image>();
        int curr = int.Parse(buttonObj.GetComponentInChildren<Text>().text);

        //If the number is not equal to the current index, reset the minigame
        if (curr != currentIndex)
        {
            currentIndex = 1;
            ResetColours();
        }
        //Otherwise, turn the button green and increasr the current index
        else
        {
            currentIndex++;
            buttonImg.color = new Color((31f / 255f), 1, 0);
        }

        //If all of the buttons have been pressed, hide the minigame, increase the player's special XP by 1, and return to the main game
        if(currentIndex > buttons.Count)
        {
            ResetColours();
            transform.parent.gameObject.SetActive(false);
            Player.IncrementSpecialXP();
            player.ReturnToScene();

            GameObject monolith = GameObject.Find("PurpleMonolith");
            Destroy(monolith);
        }
    }

    //Reset the colours of all the buttons to white
    private void ResetColours()
    {
        foreach(Button button in buttons)
        {
            GameObject buttonObj = button.gameObject;
            Image buttonImg = buttonObj.GetComponent<Image>();
            buttonImg.color = new Color(1, 1, 1);
        }
    }
}
