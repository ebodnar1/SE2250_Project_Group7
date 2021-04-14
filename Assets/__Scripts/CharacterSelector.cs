using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    private GameObject[] characters;
    private int index;
    public GameObject gameObj;

    private void Start()
    {
        index = PlayerPrefs.GetInt("CharacterSelect");

        characters = new GameObject[transform.childCount];

        //Fill the characters array with our prefabs
        for(int i = 0; i < transform.childCount; i++)
        {
            characters[i] = transform.GetChild(i).gameObject;
        }

        //Toggle the characters off
        foreach(GameObject obj in characters)
        {
            obj.SetActive(false);
        }

        //Toggle on the selected character as active
        if (characters[index])
        {
            characters[index].SetActive(true);
        }
    }

    private void Update()
    {
        //Slowly rotate character while the selection scene is active
        if (SceneManager.GetActiveScene().name.Equals("CharacterSelect"))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * -5);
        }
    }

    public void ToggleLeft()
    {
        //Toggle the current character off
        characters[index].SetActive(false);

        //Decrement the index
        index--;
        if(index < 0)
        {
            index = characters.Length - 1;
        }

        //Target the next character on
        characters[index].SetActive(true);

        //Face character forward
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void ToggleRight()
    {
        //Toggle the current character off
        characters[index].SetActive(false);

        //Increment the index
        index++;
        if (index >= characters.Length)
        {
            index = 0;
        }

        //Target the next character on
        characters[index].SetActive(true);

        //Face character forward
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void ChangeScene()
    {
        PlayerPrefs.SetInt("CharacterSelect", index);
        SceneManager.LoadScene("Scene1");
    }

    public void GoBackScene()
    {
        SceneManager.LoadScene("Menu");
    }

}
