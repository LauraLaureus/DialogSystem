using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogControl : MonoBehaviour {

    public Button up;
    public Button down;
    public Button middle;

    public Text text;
    public Image image;

    private List<Button> buttons;
    private List<string> dialogQueue;


    public enum FlipSide {
        Right,
        Left
    }

    private void Start()
    {
        buttons = new List<Button> { up, middle, down };
    }

    public void ShowButtons(int numButtons) {

        hideAllButtons();
        for (int b = 0; b < numButtons; b++) {
            buttons[b].gameObject.SetActive(true);
        }
    }

    public void hideAllButtons()
    {
        for (int b = 0; b < 3; b++)
        {
            buttons[b].gameObject.SetActive(false);
        }
    }

    public void SetPosition(Vector2 pos) {
        this.transform.position = pos;
    }

    public void AddText(string text) {
        dialogQueue.Add(text);
    }

    public void SetText(string text) {
        this.transform.GetChild(1).GetComponent<Text>().text = text;
    }

    public void SetButtonText(int number, string text) {
        this.transform.GetChild(2).transform.GetChild(number).transform.GetChild(0).GetComponent<Text>().text = text;
    }

    public void Clear() {
        this.dialogQueue.Clear();
    }

    public void flipTo(FlipSide side) {
        switch (side) {
            case FlipSide.Right:
                this.image.transform.localScale = new Vector3(-1, 1, 1);
                break;
            case FlipSide.Left:
                this.image.transform.localScale = new Vector3(1, 1, 1);
                break;
        }
    }
}
