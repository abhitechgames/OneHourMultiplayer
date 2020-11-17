using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject blueSlider;
    public GameObject redSlider;

    public TMP_InputField inputField;

    public GameObject MatchMaker;
    public GameObject[] joystick;

    public GameObject WinText;
    public GameObject LossText;

    private void Awake() {
        if(instance == null)
            instance = this;
        else   
            Destroy(this);
    }

    public void MatchButtonClicked(GameObject Button)
    {
        MatchMaker.SetActive(true);
        foreach(GameObject G in joystick)
            G.SetActive(true);
        Button.SetActive(false);
    }

    public void Win()
    {
        WinText.SetActive(true);
        LossText.SetActive(false);
        StartCoroutine(StartLevelAgain());
    }
    public void Lose()
    {   
        LossText.SetActive(true);
        StartCoroutine(StartLevelAgain());
    }   

    IEnumerator StartLevelAgain()
    {
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene("Game");
    }
    public void NamePlayer(GameObject p)
    {
        TMP_Text text =  p.GetComponentInChildren<TMP_Text>();
        if(inputField.text != "" && inputField.text != "Enter your Name")
            text.text = inputField.text;
        else{
            text.text = "Player " + Random.Range(0,10);
        }
    }
    public Slider SliderEnable(GameObject G)
    {
        if(G.tag == "Blue")
        {
            blueSlider.SetActive(true);
            return blueSlider.GetComponent<Slider>();;
        }    
            
        else if(G.tag == "Red")
        {
            redSlider.SetActive(true);
            return redSlider.GetComponent<Slider>();;
        }
        return blueSlider.GetComponent<Slider>();

    }

}
