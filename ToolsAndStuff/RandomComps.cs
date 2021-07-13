using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod.ToolsAndStuff
{
    class RandomComps
    {
        public class MakeObjSpin : MonoBehaviour
        {
            private void Start()
            {
                if (gameObject.layer != LayerMask.NameToLayer("Unpixelated"))
                {
                    base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
                }
                StartCoroutine(Spin());
            }
            
            private IEnumerator Spin()
            {

                
                while(gameObject != null)
                {
                    gameObject.transform.Rotate(Vector3.up * degreesPerSecond * Time.deltaTime, Space.Self);
                    gameObject.transform.Rotate(Vector3.left * degreesPerSecond * Time.deltaTime, Space.Self);
                    yield return new WaitForSeconds(delay);
                }
                yield break;
            }

            public float degreesPerSecond = 50;
            public float delay = 0f;

        }
    }
}
