using UnityEngine;
using UnityEngine.Events;

namespace Granden
{
    public class AnimationInOutEventTrigger : MonoBehaviour
    {
        public UnityEvent OnFadeInEnd;
        public UnityEvent OnFadeOutEnd;

        public void FadeInEnd()
        {
            OnFadeInEnd?.Invoke();
        }
        public void FadeOutEnd()
        {
            OnFadeOutEnd?.Invoke();
        }
    }
}