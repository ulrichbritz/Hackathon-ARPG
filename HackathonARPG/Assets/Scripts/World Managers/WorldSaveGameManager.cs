using UnityEngine;
using UnityEngine.SceneManagement;
using AsyncRoutines;
using AsyncOperation = UnityEngine.AsyncOperation;

namespace UB
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance { get; private set; }

        [Header("Helpful Scene Indices")]
        public int WorldSceneIndex { get; private set; } = 1;

        private void Awake()
        {
            CreateInstance();
        }

        private void Start() 
        {
            
        }

        public async Routine LoadNewGame()
        {
            await RoutineBase.WaitForAsyncOperation(SceneManager.LoadSceneAsync(WorldSceneIndex));
        }

        private void CreateInstance()
        {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } 
            else {
                Destroy(gameObject);
            }
        }
    }
}
