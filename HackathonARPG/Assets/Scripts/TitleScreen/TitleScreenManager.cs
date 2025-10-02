using UnityEngine;
using Unity.Netcode;

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
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.StartClient();
        }
    }
}


