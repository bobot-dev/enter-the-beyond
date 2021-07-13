using BotsMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LiveRecolor
{
    class ColorPicker : UITextMesh
    {
        UIBodyPartPicker picker;
        PlayerController player;
        public bool isForPageChange;
        public ColorPicker(tk2dTextMesh textMesh, Color selectedColor, Color defaultColor, UIBodyPartPicker uIBodyPartPicker, PlayerController playerController) : base (textMesh, selectedColor,defaultColor)
        {
            this.textMesh = textMesh;
            this.selectedColor = selectedColor;
            this.defaultColor = defaultColor;
            textMesh.color = defaultColor;
            this._transform = textMesh.transform;
            baseText = "pew pew someone forgot to initialize eh?";
            picker = uIBodyPartPicker;
            player = playerController;
            this.num = 0;
        }
        int num;
       

        public void SetValue(int value)
        {
            num = value;
            UpdateText();
        }
        public int GetValue()
        {
            return num;
        }

        private string baseText = null;
       

        override public void SetText(string text)
        {
            baseText = text;
            /*if (player.CurrentGun.gameObject.GetComponent<Wand>() != null && isForPageChange)
            {
                textMesh.text = baseText + " <" + player.CurrentGun.gameObject.GetComponent<Wand>().spells[num].name + " (" + (num + 1) + "/" + Wand.avalableSpells.Count + ")" + ">";
                isForPageChange = false;
                return;
            }*/
            BotsModule.Log(num.ToString());
            if (player.CurrentGun.gameObject.GetComponent<Wand>() != null && num > 0 && num < Wand.avalableSpells.Count)
            {
                textMesh.text = baseText + " <" + Wand.avalableSpells[num].name + " (" + (num + 1) + "/" + Wand.avalableSpells.Count + ")" + ">";
                Wand.spells[picker.GetIndex()] = Wand.avalableSpells[num];
                player.CurrentGun.gameObject.GetComponent<Wand>().ChangeWandProperties(player.CurrentGun);
                return;
            }
            textMesh.text = baseText + " <" + "IT NULL :(" + ">";

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
            
            if (num > 0)
            {
                num--;
                BotsModule.Log($"{BotsItemIds.Wand}, {player.CurrentGun.PickupObjectId}");
                if (player.CurrentGun.gameObject.GetComponent<Wand>() != null)
                {
                    var wand = player.CurrentGun.gameObject.GetComponent<Wand>();
                    Wand.spells[picker.index] = Wand.avalableSpells[num];
                    BotsModule.Log($"{picker.index}, {num}");
                }

            }
            SetValue(num);
            base.PressLeft();
        }
        public override void PressRight()
        {
            if (player.CurrentGun.gameObject.GetComponent<Wand>() != null)
            {
                var wand = player.CurrentGun.gameObject.GetComponent<Wand>();
                if (num < Wand.avalableSpells.Count - 1)
                {
                    num++;

                    if (Wand.avalableSpells[num].spellProj != null)
                    {
                        Wand.spells[picker.index] = Wand.avalableSpells[num];
                    }
                   
                    BotsModule.Log($"{picker.index}, {num}");
                }
            }
            
            SetValue(num);
            base.PressRight();
        }

    }
}
