using JetBrains.Annotations;
using UnityEngine;

namespace UB
{
    public class CharacterStatsManager : MonoBehaviour
    {
        private CharacterManager character;

        [Header("Mana Regeneration")]
        private float manaRegenerationTimer = 0f;
        [SerializeField]
        private float manaRegenerationDelay = 1f; // Time in seconds before mana starts regenerating
        [SerializeField]
        private float manaTickTimer = 0;
        private float manaTickTimeInSeconds = 1f;
        [SerializeField]
        private float manaRegenerationRate = 1; // TODO Mana points regenerated per tick, this will change later and will need a new way of doing it

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public int CalculateManaBasedOnWisdom(int wisdom)
        {
            float mana = 0;

            mana = wisdom * 2;

            return Mathf.RoundToInt(mana);
        }

        public virtual void RegenerateMana()
        {
            if (!character.IsOwner) {
                return;
            }

            if (character.IsPerformingAction) {
                return;
            }

            manaRegenerationTimer += Time.deltaTime;

            if (manaRegenerationTimer >= manaRegenerationDelay) {
                // Regenerate mana over time
                if (character.characterNetworkManager.CurrentMana.Value < character.characterNetworkManager.MaxMana.Value) {
                    manaTickTimer += Time.deltaTime;

                    if (manaTickTimer >= manaTickTimeInSeconds) {
                        manaTickTimer = 0;
                        character.characterNetworkManager.CurrentMana.Value += Mathf.RoundToInt(manaRegenerationRate);
                    }
                }
            }
        }

        public virtual void ResetManaRegenerationTimer(int previousManaAmount, int currentManaAmount)
        {
            // Only reset regeneration timer if mana was used
            if (currentManaAmount < previousManaAmount) {
                manaRegenerationTimer = 0f;
            }
        }
    }
}
