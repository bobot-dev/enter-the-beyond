using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using ItemAPI;
using System.Collections;
using BotsMod;

namespace LiveRecolor
{
    //thx kyle
    class GUIhandler : MonoBehaviour
    {
        static tk2dTextMesh textPrefab = Instantiate<tk2dTextMesh>(BraveResources.Load<GameObject>("textbox").GetComponentInChildren<tk2dTextMesh>());
        GameObject panel;
        UIBodyPartPicker bodyPartPicker;
        ColorPicker redPicker;
        int activePickerIndex = 0;
        UITextMesh[] pickers = new UITextMesh[12];





        void Start()
        {
            //makes it so out text meshes dont start a new line after a set amount of characters
            textPrefab.wordWrapWidth = int.MaxValue;
            m_player = gameObject.GetComponent<PlayerController>();

            if (textPrefab == null)
            {
                textPrefab = Instantiate<tk2dTextMesh>(BraveResources.Load<GameObject>("textbox").GetComponentInChildren<tk2dTextMesh>());
            }
            //wobbly text tend to break with quick starting and such, so bugs can arise
            textPrefab.supportsWooblyText = false;
            if (!FakePrefab.IsFakePrefab(textPrefab.gameObject))
            {
                FakePrefab.MarkAsFakePrefab(textPrefab.gameObject);
            }
            //basically our gui game object
            panel = new GameObject("panel");
            DontDestroyOnLoad(panel);
            panel.transform.SetParent(m_player.transform, false);
            panel.transform.localPosition = Vector3.zero;

            //a lot of label setup. most of these inteface directly with the material manger except the colors and the first body picker one because i was too lazy to figure it out 
            bodyPartPicker = new UIBodyPartPicker(Instantiate<tk2dTextMesh>(textPrefab, new Vector3(2, 5, 0), Quaternion.identity), Color.magenta, Color.white, m_player);
            bodyPartPicker.GetTransform().SetParent(panel.transform, false);
            bodyPartPicker.SetText("Spell:");
            
            bodyPartPicker.onPressedDirection += OnBodyPickerIndexChanged;
            pickers[0] = bodyPartPicker;

            redPicker = new ColorPicker(Instantiate<tk2dTextMesh>(textPrefab, new Vector3(2, 4, 0), Quaternion.identity), Color.red, Color.white, bodyPartPicker, m_player);
            redPicker.GetTransform().SetParent(panel.transform, false);
            redPicker.SetText("Spell Type:");
            redPicker.SetValue(Wand.spells[bodyPartPicker.GetIndex()].index);
            redPicker.onPressedDirection += OnColorPressed;
            pickers[1] = redPicker;

            MoveCursor(0);
            OnBodyPickerIndexChanged();
            panel.SetActive(false);
        }
        public PlayerController m_player;


        void OnDestroy()
        {
            Destroy(panel);
            for (int i = 0; i < pickers.Length; i++)
            {
                pickers[i].Destroy();
            }
        }

        int spellNum;
        bool locked = true;
        void Update()
        {
            //toggles the ui. taken directly from kyle. if player presses interact button for more than 5 seconds gui will pop up
            if (!BraveInput.HasInstanceForPlayer(m_player.PlayerIDX) && GameManager.Instance.PrimaryPlayer != null && GameManager.Instance.PrimaryPlayer.HasPickupID(BotsItemIds.Wand)) { return; }
            if (Key(GungeonActions.GungeonActionType.Interact) && KeyTime(GungeonActions.GungeonActionType.Interact) > .5f && !locked)
            {
                Toggle();
                locked = true;
            }

            if (!Key(GungeonActions.GungeonActionType.Interact))
                locked = false;

            if (shown)
            {
                //allows scrolling thorugh the labels and actiavting their press left and right methods.
                //if player holds down the left or right button it will auto scroll as fast as your frames go
                GungeonActions.GungeonActionType type = GungeonActions.GungeonActionType.SelectLeft;
                if (KeyDown(type) || (Key(type) && KeyTime(type) > .5f))
                {
                    pickers[activePickerIndex].PressLeft();
                }
                type = GungeonActions.GungeonActionType.SelectRight;
                if (KeyDown(type) || (Key(type) && KeyTime(type) > .5f))
                {
                    pickers[activePickerIndex].PressRight();
                }
                type = GungeonActions.GungeonActionType.SelectUp;
                if (KeyDown(type))
                {
                    MoveCursor(-1);
                }
                type = GungeonActions.GungeonActionType.SelectDown;
                if (KeyDown(type))
                {
                    MoveCursor(1);
                }
                type = GungeonActions.GungeonActionType.DropItem;
                if (KeyDown(type))
                {
                    DropSepll();
                }
            }
        }


        void DropSepll()
        {

            BotsModule.Log($"{Wand.avalableSpells[bodyPartPicker.GetIndex()].itemId}");
            BotsModule.Log($"{StaticSpellReferences.JUSTFUCKINGWORK[Wand.avalableSpells[redPicker.GetValue()].name]}");
            var gameObject2 = LootEngine.SpawnItem(PickupObjectDatabase.GetById(StaticSpellReferences.JUSTFUCKINGWORK[Wand.avalableSpells[redPicker.GetValue()].name]).gameObject, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Vector2.zero, 0);
            Wand.avalableSpells.RemoveAt(redPicker.GetValue());

            IPlayerInteractable[] interfacesInChildren = GameObjectExtensions.GetInterfacesInChildren<IPlayerInteractable>(gameObject2.gameObject);
            for (int i = 0; i < interfacesInChildren.Length; i++)
            {
                GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(interfacesInChildren[i]);
            }
        }

        void MoveCursor(int dir)
        {
            dir = activePickerIndex + dir;
            dir = Mathf.Clamp(dir, 0, pickers.Length - 1);
            pickers[activePickerIndex].ToggleColor(false);
            pickers[dir].ToggleColor(true);
            activePickerIndex = dir;
        }
        bool shown;
        void Toggle()
        {
            shown = !shown;
            if (shown)
            {
                m_player.SetInputOverride("color picker");
                panel.SetActive(true);
            }
            else
            {
                m_player.ClearInputOverride("color picker");
                panel.SetActive(false);
                if (m_player.CurrentGun.gameObject.GetComponent<Wand>() != null)
                {
                    m_player.CurrentGun.gameObject.GetComponent<Wand>().ChangeWandProperties(m_player.CurrentGun);
                }
            }
        }
        public float KeyTime(GungeonActions.GungeonActionType action)
        {
            return BraveInput.GetInstanceForPlayer(m_player.PlayerIDX).ActiveActions.GetActionFromType(action).PressedDuration;
        }

        public bool KeyDown(GungeonActions.GungeonActionType action)
        {
            return BraveInput.GetInstanceForPlayer(m_player.PlayerIDX).ActiveActions.GetActionFromType(action).WasPressed;
        }

        public bool Key(GungeonActions.GungeonActionType action)
        {
            return BraveInput.GetInstanceForPlayer(m_player.PlayerIDX).ActiveActions.GetActionFromType(action).IsPressed;
        }

        //pushes the colors inot mat manager
        void OnColorPressed()
        {
            BotsModule.Log("thing press yes");

        }
        //basically updates our color labels to the body part thats picked
        void OnBodyPickerIndexChanged()
        {
            BotsModule.Log("thing press yes yes");
            if (m_player.CurrentGun.gameObject.GetComponent<Wand>() != null)
            {
                BotsModule.Log("not null woooo");
                //redPicker.SetValue(bodyPartPicker.GetIndex());
                //BotsModule.Log(m_player.CurrentGun.gameObject.GetComponent<Wand>().spells[bodyPartPicker.GetIndex()].index.ToString());

                redPicker.SetValue(Wand.spells[bodyPartPicker.GetIndex()].index);
                redPicker.isForPageChange = true;
                ETGModConsole.Log((Wand.spells[bodyPartPicker.GetIndex()].index).ToString());
                //m_player.CurrentGun.gameObject.GetComponent<Wand>().spells[bodyPartPicker.GetIndex()]
            }
        }
        //gotta reset them labels

    }
}
