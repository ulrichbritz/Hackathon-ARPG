using UnityEngine;
using UnityEngine.UI;

namespace UB
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;

        // TODO make a secondary bar for polish flash (to show how much we used with an action)

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }
    }
}

