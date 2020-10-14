/*
 * Name: Menu (Menu.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-28
 * Last Modified: 2020-09-06
 * Used in: PHASED OUT
 * Description: Previously used to manage the launch screen and the scene management for that.
 * Status: UNUSED
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject mainMenu, controls, credits, countdown;
    public int latestClick = -1, curBackground;
    public float timePer = 10f, prevChange = 0f, timeLeft;
    public Sprite[] backgrounds;
    // Start is called before the first frame update
    void Start()
    {
        SetActive(true, false, false);
        countdown.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch(latestClick)
        {
            case 0:
                countdown.SetActive(true);
                timeLeft -= Time.deltaTime;
                countdown.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (timeLeft).ToString("0");
                if (timeLeft < 0)
                    SceneManager.LoadScene("Main");
                break;

            case 1:
                SetActive(false, true, false);
                break;

            case 2:
                SetActive(false, false, true);
                break;

            case 3:
                SetActive(true, false, false);
                break;
        }
        CycleImages();
    }

    public void SetActive(bool mainOn, bool controlOn, bool creditsOn)
    {
        mainMenu.SetActive(mainOn);
        controls.SetActive(controlOn);
        credits.SetActive(creditsOn);

    }

    public void CycleImages()
    {
        if((Time.time - prevChange) >= timePer)
        {
            prevChange = Time.time;
            curBackground++;
            if (curBackground >= backgrounds.Length)
                curBackground = 0;
            ChangeImage();
        }
    }

    public void ChangeImage()
    {
        mainMenu.transform.GetChild(0).GetComponent<Image>().sprite = backgrounds[curBackground];
        controls.transform.GetChild(0).GetComponent<Image>().sprite = backgrounds[curBackground];
        credits.transform.GetChild(0).GetComponent<Image>().sprite = backgrounds[curBackground];
    }

    public void SetScene(int i) { latestClick = i; }
    public int GetScene(int i) { return latestClick; }
}
