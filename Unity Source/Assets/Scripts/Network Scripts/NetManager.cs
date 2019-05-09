using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

/* *
   CLASS NAME

          NetManager : NetworkManager
                       
    DESCRIPTION

         This is a custom network manager that handles launching and handling LAN servers for the players in game to be able to connect to and create new servers.
                   
    AUTHOR

            Aayush B Shrestha

    DATE

            2:37pm 4/12/2019  
                       
 * */

public class NetManager : NetworkManager
{

    private bool firstPlayerJoined;


    /* *

    NAME

      public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)

    SYNOPSIS

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
            NetworkConnection conn      - incoming connection 
            short playerControllerId    - id of the incoming player

    DESCRIPTION

        This function overrides the OnServerAddPlayer method to spawn players in a list of specified spawn locations instead of dropping them
        in random parts of the map which could potentially lead to unexpected behavior.

    RETURNS

      Nothing     

    AUTHOR

         Aayush B Shrestha

    DATE

         9:37pm 2/23/2019  

    * */

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject playerObj = Instantiate(playerPrefab);
        List<Transform> spawnPositions = singleton.startPositions; //list of transforms 

        if (!firstPlayerJoined)
        {
            firstPlayerJoined = true;
            playerObj.transform.position = spawnPositions[0].position;
        }
        else
        {
            playerObj.transform.position = spawnPositions[1].position;
        }

        NetworkServer.AddPlayerForConnection(conn, playerObj, playerControllerId);
    }

/* *

    NAME

      void SetPortAndAddress()

    SYNOPSIS

        void SetPortAndAddress()

    DESCRIPTION

        This function sets the port and address for servers.

    RETURNS

      Nothing     

    AUTHOR

         Aayush B Shrestha

    DATE

         9:37pm 2/23/2019  

* */

    void SetPortAndAddress()
    {
        //setting default port
        singleton.networkPort = 7777;
        singleton.networkAddress = "localhost";
    }

/* *

    NAME

      void HostGame()

    SYNOPSIS

        void HostGame()

    DESCRIPTION

        This function is attached to Host game button on UI 
        and will start a new server instance

    RETURNS

      Nothing     

    AUTHOR

         Aayush B Shrestha

    DATE

         9:37pm 2/23/2019  

* */
    public void HostGame()
    {
        SetPortAndAddress();
        singleton.StartHost();
    }

/* *

        NAME

          void JoinGame()

        SYNOPSIS

            void JoinGame()

        DESCRIPTION

            This function is Attached to join game button on UI 
            will connect to hosted server

        RETURNS

          Nothing     

        AUTHOR

             Aayush B Shrestha

        DATE

             9:37pm 2/23/2019  

    * */

    public void JoinGame()
    {
        SetPortAndAddress();
        singleton.StartClient();
    }

}
