using System.Collections;
using UnityEngine;

namespace Granden
{
    public class SingleClickFilterStateResetter : MonoBehaviour
    {
        private WaitForEndOfFrame waitForEndOfFrame;

        private IStateValue<bool> isClicked;

        private void Awake()
        {
            waitForEndOfFrame = new WaitForEndOfFrame();
            isClicked = GetComponent<IStateValue<bool>>();
        }

        private void Start()
        {
            StartCoroutine(ResetSingleClickFilterState());
        }

        private IEnumerator ResetSingleClickFilterState()
        {
            while (true)
            {
                yield return waitForEndOfFrame;

                isClicked.State = false;
            }
        }
    }
}