using BotsMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LiveRecolor
{
    class UIBodyPartPicker : UITextMesh
    {
        PlayerController player;
        public UIBodyPartPicker(tk2dTextMesh textMesh, Color selectedColor, Color defaultColor, PlayerController playerController) : base(textMesh, selectedColor, defaultColor)
        {
            this.textMesh = textMesh;
            this.selectedColor = selectedColor;
            this.defaultColor = defaultColor;
            textMesh.color = defaultColor;
            this._transform = textMesh.transform;
            baseText = "pew pew someone forgot to initialize eh?";
            player = playerController;
            index = 0;
        }
        string baseText = null;

        public int index;
        public event Action<int> OnIndexChanged;

        public int GetIndex()
        {
            return index;
        }
        public void SetIndex(int idx)
        {
            if(Wand.spells == null) { return; }
            if (player.CurrentGun.gameObject.GetComponent<Wand>() == null)
            {
                ETGModConsole.Log("go fuck yourself");
                return;
            }
            if (idx >= Wand.spells.Count)
            {
                index = Wand.spells.Count - 1; 
                UpdateText();
                return;
            }
            if(idx <= 0) 
            {
                index = 0; 
                UpdateText();
                return;
            }

            

            index = idx;
            UpdateText();

        }

      
        override public void SetText(string text)
        {
            baseText = text;
            if (player.CurrentGun.gameObject.GetComponent<Wand>() == null)
            {
                ETGModConsole.Log("fuck!!!");
                textMesh.text = baseText;
                return;
            }
            if (Wand.spells == null || Wand.spells.Count <= 0)
            {
                textMesh.text = baseText;
                return;
            }
            textMesh.text = baseText + " <" + index + ">";
        }


        override public string GetText()
        {
            return baseText;
        }
        void UpdateText()
        {
            SetText(baseText);
        }
        public override void PressLeft()
        {
            index--;
            SetIndex(index);
            if (OnIndexChanged!= null)
            {
                OnIndexChanged(index);
            }
            UpdateText();
            base.PressLeft();
        }
        public override void PressRight()
        {
            index++ ;
            SetIndex(index);
            if (OnIndexChanged != null)
            {
                OnIndexChanged(index);
            }
            UpdateText();
            base.PressRight();
        }
    }
}
