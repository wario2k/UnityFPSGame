using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

//custom network manager
public class NetManager : NetworkManager
{

    private bool firstPlayerJoined;
    //when player is about to spawn the palyer
    //we will control where we spawn the player
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
    /// <summary>
    /// Sets the port and address for servers.
    /// </summary>
    void SetPortAndAddress()
    {
        //setting default port
        singleton.networkPort = 7777;
        singleton.networkAddress = "localhost";
    }

    /// <summary>
    /// Attached to Host game button on UI 
    /// will start a new server instance
    /// </summary>
    public void HostGame()
    {
        SetPortAndAddress();
        singleton.StartHost();
    }

    /// <summary>
    /// Attached to join game button on UI 
    /// will connect to hosted server
    /// </summary>
    public void JoinGame()
    {
        SetPortAndAddress();
        singleton.StartClient();
    }

}
