using UnityEngine;

namespace UB
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] private UI_StatBar healthBar;
        [SerializeField] private UI_StatBar manaBar;

        public void SetNewManaValue(int oldValue, int newValue)
        {
            manaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxManaValue(int maxMana)
        {
            manaBar.SetMaxStat(Mathf.RoundToInt(maxMana));
        }
    }
}
