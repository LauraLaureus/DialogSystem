using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTextBehaviour : MonoBehaviour {


    private int currentPosition = 0;
    private string text;
    private Text display;


    public void SetNewText(string text,  bool byChar=false)
    {
        currentPosition = 0;
        this.text = "";
        if (byChar) {
            StartCoroutine("CharByChar");
        }
    }

	// Use this for initialization
	void Start () {
        text = "";
        display = this.GetComponent<Text>();

	}
	
	
    IEnumerator CharByChar() {

        for (int c = 0; c < this.text.Length; c++) {
            this.display.text = this.text.Substring(0, c + 1);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
