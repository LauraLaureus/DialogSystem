﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

public class Prompter : MonoBehaviour {


    Dialog current;

    private GameObject dialogControl;
    private DialogData script;

	// Use this for initialization
	void Start () {
        dialogControl = GameObject.Find("DialogControl");
        script = AssetDatabase.LoadAssetAtPath("Assets/DialogData.asset", typeof(DialogData)) as DialogData;
        current = script.GetDialogByKey("start");
        prompt();
	}

    void prompt() {

        ParserInfo parsed = Parser.parse(current.dialog);

        updateControl(parsed);

        //current = script.GetDialogByKey(current.details_nextKey);
    }

    private void updateControl(ParserInfo parsed)
    {
        DialogControl control = dialogControl.GetComponent<DialogControl>();

        control.hideAllButtons();
        control.ShowButtons(parsed.buttonsText.Count);

        for (int i = 0; i < parsed.buttonsText.Count; i++)
        {
            control.SetButtonText(i, parsed.buttonsText[i]);
        }

        control.SetText(parsed.dialog);
    }

    public class Parser
    {
        public static ParserInfo parse(string stream) {

            ParserInfo result = new ParserInfo();

            List<string> toParse = extractParseable(stream);

            foreach (string statement in toParse)
            {
                string content = statement.Substring(1);
                content = content.Substring(0,content.Length - 2);
                string[] contentInfo = content.Split(' ');

                if (contentInfo[0].ToLower().StartsWith("b"))
                { //is a button
                    result.buttonsText.Add(contentInfo[2]); // Button text
                    //TODO save the key to button dialog. Maybe an update to dictionary
                }
                

            }

            result.dialog = extractDialog(stream);


            return result;
        }

        private static string extractDialog(string stream)
        {
            return stream.Substring(0, stream.IndexOf('<'));
        }

        private static List<string> extractParseable(string stream)
        {
            List<string> result = new List<string>();
            int currentPosition = 0, endOfsintaxis = 0;

            while (currentPosition < stream.Length)
            {
                currentPosition = stream.IndexOf("<");
                if (currentPosition == -1) break;
                endOfsintaxis = stream.IndexOf(">");
                if (endOfsintaxis == -1) throw new BadSintaxInDialogException("Non closed sintax phrase");

                result.Add(stream.Substring(currentPosition, endOfsintaxis - currentPosition + 1));
                stream = stream.Substring(endOfsintaxis + 1);
                currentPosition = 0;
                endOfsintaxis = 0;
            }

            return result;
        }
    }

    public class ParserInfo
    {
        public List<string> buttonsText;
        public string dialog;
        public int flipOption;

        public ParserInfo() {
            buttonsText = new List<string>();
            flipOption = 1;
        }
    }
}

[Serializable]
internal class BadSintaxInDialogException : Exception
{
    public BadSintaxInDialogException()
    {
    }

    public BadSintaxInDialogException(string message) : base(message)
    {
    }

    public BadSintaxInDialogException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected BadSintaxInDialogException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}