using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttonbtn : MonoBehaviour {

    public GameObject optionList;
    public Sprite neutral, highlight;

    private bool clicked = false;
    private Image sprite;

    void Awake() {
        sprite = GetComponent<Image>();
    }
    public void Click() {
        if (!clicked) {
            sprite.sprite = highlight;
            clicked = true;
        } else {
            sprite.sprite = neutral;
            clicked = false;
        }
        optionList.SetActive(clicked);
    }

    public bool ReClicked() {
        optionList.SetActive(clicked);
        return clicked;
    }

}
