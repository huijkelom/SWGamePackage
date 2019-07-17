using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class CentralGateway : MonoBehaviour
{
    public static CentralGateway Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("CentralGateway | Awake | A second CentralGateway was loaded, maybe you should make a initialisation scene?");
            Destroy(this);
        }
    }

    private void Start()
    {
        ConnectToCentral();
    }

    private void ConnectToCentral()
    {
        TcpClient temp = new TcpClient();
        temp.BeginConnect(IPAddress.Loopback.ToString(), 2237, ConnectCallback, temp);
    }
    private void ConnectCallback(IAsyncResult ar)
    {

    }
}
