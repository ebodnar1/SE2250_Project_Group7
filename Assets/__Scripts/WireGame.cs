using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireGame : MonoBehaviour
{
    //Lists of wires and colours
    public List<Color> wireColors = new List<Color>();
    public List<Wire> leftWires = new List<Wire>();
    public List<Wire> rightWires = new List<Wire>();

    //Track the wire being dragged and hovered over
    public Wire CurrentDraggedWire;
    public Wire CurrentHoveredWire;

    public bool isTaskCompleted = false;

    private List<Color> availableColors;
    private List<int> availableLeftWireIndex;
    private List<int> availableRightWireIndex;

    private Player player;
    private bool inFunc;

    private void Start()
    {
        //Initialize the colour and wire lists
        availableColors = new List<Color>(wireColors);
        availableLeftWireIndex = new List<int>();
        availableRightWireIndex = new List<int>();

        //Add the left wires to the list
        for (int i = 0; i < leftWires.Count; i++)
        {
            availableLeftWireIndex.Add(i);
        }

        //Add the right wires to the list
        for (int i = 0; i < rightWires.Count; i++)
        {
            availableRightWireIndex.Add(i);
        }

        //This randomizes the order of colours on the left and right sides
        //While there are wires to be initialized
        while (availableColors.Count > 0 &&
               availableLeftWireIndex.Count > 0 &&
               availableRightWireIndex.Count > 0)
        {
            //Pick a random colour
            Color pickedColor =
             availableColors[Random.Range(0, availableColors.Count)];

            //Pick random right and left indices
            int pickedLeftWireIndex = Random.Range(0,
                                      availableLeftWireIndex.Count);
            int pickedRightWireIndex = Random.Range(0,
                                      availableRightWireIndex.Count);

            //Set the colour of those indeces
            leftWires[availableLeftWireIndex[pickedLeftWireIndex]]
                                              .SetColor(pickedColor);
            rightWires[availableRightWireIndex[pickedRightWireIndex]]
                                              .SetColor(pickedColor);

            //Remove the previously set wires and colour
            availableColors.Remove(pickedColor);
            availableLeftWireIndex.RemoveAt(pickedLeftWireIndex);
            availableRightWireIndex.RemoveAt(pickedRightWireIndex);
        }

        //Get the player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        inFunc = false;
    }

    //Check for completion constantly
    private void Update()
    {
        StartCoroutine(CheckTaskCompletion());

        //Allow the player to exit
        if (gameObject.activeInHierarchy && Input.GetKeyDown("escape"))
        {
            transform.parent.gameObject.SetActive(false);
            player.ReturnToScene();
        }
    }

    private IEnumerator CheckTaskCompletion()
    {
        //If the coroutine is already running, end it
        if (inFunc) yield break;

        inFunc = true;

        //While the task is not completed
        while (!isTaskCompleted)
        {
            int successfulWires = 0;

            //Check how many wires have been successfully connected
            for (int i = 0; i < rightWires.Count; i++)
            {
                if (rightWires[i].isSuccess)
                {
                    successfulWires++;
                }
            }

            //If all the wires have been connected, set the minigame to inactive, increase the player's special XP, and return to the level
            if (successfulWires >= rightWires.Count)
            {
                transform.parent.gameObject.SetActive(false);
                Player.IncrementSpecialXP();
                player.ReturnToScene();

                GameObject monolith = GameObject.Find("GreenMonolith");
                Destroy(monolith);
            }

            //Check every 1 seconds
            yield return new WaitForSeconds(1.0f);
            inFunc = false;
        }
    }
}
