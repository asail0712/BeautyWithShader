using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Granden.Core
{
    public class Heartbeat : MonoBehaviour
    {
        // Start is called before the first frame update
        private static List<IHeart> s_HeartList = new List<IHeart>();
        public static bool RegisterHaert(IHeart Heart)
        {
            if (s_HeartList.Contains(Heart))
            {
                return false;
            }

            s_HeartList.Add(Heart);

            return true;
        }

        public static void UnregisterHaert(IHeart Heart)
        {
            s_HeartList.Remove(Heart);
        }

        // Update is called once per frame
        void Update()
        {
            float DeltaTime = Time.deltaTime;

            foreach (IHeart Heart in s_HeartList)
            {
                Heart.UpdateHeart(DeltaTime);
            }
        }
    }
}