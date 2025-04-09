using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        print("PlayerNetwork: OnNetworkSpawn() called");
    }


    public override void OnNetworkDespawn()
    {
        print("PlayerNetwork: OnNetworkSpawn() called");
    }
}
