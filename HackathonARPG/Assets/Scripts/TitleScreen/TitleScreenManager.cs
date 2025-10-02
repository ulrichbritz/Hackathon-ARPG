using UnityEngine;
using Unity.Netcode;
using AsyncRoutines;

namespace UB
{
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldRoutineManager.Instance.Run(WorldSaveGameManager.Instance.LoadNewGame());
        }

        public void JoinGame()
        {
            WorldRoutineManager.Instance.Run(JoinGameAsync());
        }

        private async Routine JoinGameAsync()
        {
            NetworkManager.Singleton.Shutdown();
            
            // Wait until NetworkManager is actually shut down
            while (NetworkManager.Singleton.IsListening) {
                await RoutineBase.WaitForNextFrame();
            }
            
            NetworkManager.Singleton.StartClient();
        }
    }
}


