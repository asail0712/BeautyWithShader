using System.Collections;
using UnityEngine;

namespace Granden.gwh
{
    public static class MonoBehaviourHelper
    {
        public static Coroutine StartCoroutine(IEnumerator routine, bool persistent = false)
        {
            MonoBehavourInstance MonoHelper = new GameObject("Coroutine").AddComponent<MonoBehavourInstance>();

            return MonoHelper.DestroyWhenComplete(routine, persistent);
        }

        public class MonoBehavourInstance : MonoBehaviour
        {
            public Coroutine DestroyWhenComplete(IEnumerator routine, bool persistent)
            {
                if (persistent)
                    DontDestroyOnLoad(this.gameObject);
                return StartCoroutine(DestroyObjHandler(routine));
            }

            private IEnumerator DestroyObjHandler(IEnumerator routine)
            {
                yield return StartCoroutine(routine);

                Destroy(this.gameObject);
            }
        }
    }
}