using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Behaviours
{
    class ButtonPrompterBehaviour: MonoBehaviour
    {

        public string nextKey;
        public bool doPrompt;

        private Prompter prompter; 

        private void Start()
        {
            prompter = GameObject.Find("DialogControl").GetComponent<Prompter>();
        }

        public void OnClick() {

            if (!String.IsNullOrEmpty(nextKey)) {
                prompter.SetNextDialog(nextKey, doPrompt);
            }
        }
    }
}
