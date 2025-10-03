using UnityEngine;

namespace UB
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        private CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
        {
            // TODO Create and Use SnapValues (clamped values) if animations dont look good blended
            character.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
            character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
        }

        protected virtual void OnDestroy()
        {
            character = null;
        }
    }
}
