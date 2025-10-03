using UnityEngine;
using Unity.Netcode;

namespace UB
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        private float snappedVerticalAnimatorParameter;
        private float snappedHorizontalAnimatorParameter;

        private CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, float smoothTime = 0.1f)
        {
            // TODO Create and Use SnapValues (clamped values) if animations dont look good blended
            character.animator.SetFloat("Horizontal", horizontalValue, smoothTime, Time.deltaTime);
            character.animator.SetFloat("Vertical", verticalValue, smoothTime, Time.deltaTime);
        }

        public virtual void PlayTargetAnimation(
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false,
            float crossfadeDuration = 0.2f
            )
        {
            character.animator.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, crossfadeDuration);
            // Can stop character from attempting actions while busy with another action
            character.IsPerformingAction = isPerformingAction;
            character.CanRotate = canRotate;
            character.CanMove = canMove;

            // Tell the SERVER/HOST to play the animation for all clients
            character.characterNetworkManager.NotifyServerOfActionAnimationServerRPC(
                NetworkManager.Singleton.LocalClientId,
                targetAnimation,
                applyRootMotion
                );
        }

        protected virtual void OnDestroy()
        {
            character = null;
        }
    }
}
