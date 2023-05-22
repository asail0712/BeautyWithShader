using UnityEngine;
using UnityEngine.Events;

namespace Granden.Common
{
    public class AnimationEventTrigger : MonoBehaviour
    {
        public UnityEvent OnAnimationEnd;

        public void AnimationEnd()
        {
            OnAnimationEnd?.Invoke();
        }
    }
}