using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInstantiateRpc : NetworkBehaviour
{
    bool FunctionCalled;
    public MovementScript movement;
    private void OnDisconnectedFromServer()
    {
        FunctionCalled = false;
    }
    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("PlayerPrefab").Length < 2) return;
        if (!isServer) return;
        if (FunctionCalled) return;
        movement.InstantiateCommand();
        FunctionCalled = true;
    }
}
