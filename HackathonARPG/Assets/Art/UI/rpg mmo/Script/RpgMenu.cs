using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpgMenu : MonoBehaviour
{
    public GameObject screen;
    public GameObject screen1;
    public GameObject screen2;
    public GameObject screen3;
    public GameObject screen4;
    public GameObject screen5;
    public GameObject screen6;
    public GameObject screen7;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            displayscreenmain();
        }
    }
    public void displayscreenmain() {
        screen.SetActive(true);
        screen1.SetActive(false);
        screen2.SetActive(false);
        screen3.SetActive(false);
        screen4.SetActive(false);
        screen5.SetActive(false);
        screen6.SetActive(false);
        screen7.SetActive(false);

    }
    public void displayscreen1() {
        screen.SetActive(false);
        screen1.SetActive(true);
        screen2.SetActive(false);
        screen3.SetActive(false);
        screen4.SetActive(false);
        screen5.SetActive(false);
        screen6.SetActive(false);
        screen7.SetActive(false);
    }
    public void displayscreen2() {
        screen.SetActive(false);
        screen1.SetActive(false);
        screen2.SetActive(true);
        screen3.SetActive(false);
        screen4.SetActive(false);
        screen5.SetActive(false);
        screen6.SetActive(false);
        screen7.SetActive(false);
    }
    public void displayscreen3() {
        screen.SetActive(false);
        screen1.SetActive(false);
        screen2.SetActive(false);
        screen3.SetActive(true);
        screen4.SetActive(false);
        screen5.SetActive(false);
        screen6.SetActive(false);
        screen7.SetActive(false);
    }
    public void displayscreen4() {
        screen.SetActive(false);
        screen1.SetActive(false);
        screen2.SetActive(false);
        screen3.SetActive(false);
        screen4.SetActive(true);
        screen5.SetActive(false);
        screen6.SetActive(false);
        screen7.SetActive(false);
    }
    public void displayscreen5() {
        screen.SetActive(false);
        screen1.SetActive(false);
        screen2.SetActive(false);
        screen3.SetActive(false);
        screen4.SetActive(false);
        screen5.SetActive(true);
        screen6.SetActive(false);
        screen7.SetActive(false);
    }
    public void displayscreen6() {
        screen.SetActive(false);
        screen1.SetActive(false);
        screen2.SetActive(false);
        screen3.SetActive(false);
        screen4.SetActive(false);
        screen5.SetActive(false);
        screen6.SetActive(true);
        screen7.SetActive(false);
    }
    public void displayscreen7() {
        screen.SetActive(false);
        screen1.SetActive(false);
        screen2.SetActive(false);
        screen3.SetActive(false);
        screen4.SetActive(false);
        screen5.SetActive(false);
        screen6.SetActive(false);
        screen7.SetActive(true);
    }
    public void displayscreen8()
    {
        screen.SetActive(false);
        screen1.SetActive(false);
        screen2.SetActive(false);
        screen3.SetActive(false);
        screen4.SetActive(false);
        screen5.SetActive(false);
        screen6.SetActive(false);
        screen7.SetActive(false);
    }
}
