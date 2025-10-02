using UnityEngine;
using AsyncRoutines;

namespace UB
{
    public class WorldRoutineManager : MonoBehaviour
    {
        public static WorldRoutineManager Instance { get; private set; }

        private RoutineManager routineManager;

        private void Awake()
        {
            // Singleton pattern
            if (Instance == null) {
                Instance = this;
                routineManager = new RoutineManager();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            routineManager?.Update();
        }

        private void LateUpdate()
        {
            routineManager?.Flush();
        }

        private void OnDestroy()
        {
            if (Instance == this) {
                routineManager?.StopAll();
                Instance = null;
            }
        }

        /// <summary>
        /// Run a routine with the global routine manager
        /// </summary>
        public RoutineHandle Run(Routine routine, System.Action<System.Exception> onStop = null)
        {
            return routineManager.Run(routine, onStop);
        }

        /// <summary>
        /// Stop all managed routines
        /// </summary>
        public void StopAll()
        {
            routineManager?.StopAll();
        }

        /// <summary>
        /// Throw an exception in all managed routines
        /// </summary>
        public void ThrowAll(System.Exception exception)
        {
            routineManager?.ThrowAll(exception);
        }
    }
}