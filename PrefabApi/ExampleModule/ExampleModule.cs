using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PrefabAPI;

namespace PrefabAPIExample
{
    public class ExampleModule : ETGModule
    {
        public override void Init()
        {
        }

        public override void Start()
        {
            ETGModConsole.Commands.AddUnit("spawnprefab", delegate(string[] args)
            {
                if(args.Length > 0)
                {
                    GameObject go = PrefabBuilder.builtObjects?.Find((GameObject g) => g.name == args[0]);
                    if(go == null)
                    {
                        go = PrefabBuilder.BuildObject(args[0]);
                        tk2dSprite.AddComponent(go, AmmonomiconController.ForceInstance.EncounterIconCollection, UnityEngine.Random.Range(0, AmmonomiconController.ForceInstance.EncounterIconCollection.spriteDefinitions.Length));
                    }
                    UnityEngine.Object.Instantiate(go, GameManager.Instance.PrimaryPlayer.sprite.WorldBottomLeft, Quaternion.identity);
                }
            });
        }

        public override void Exit()
        {
        }
    }
}
