using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class BlobDetectionGateway : MonoBehaviour
{
    TcpConnection _Connection;

    private void Start()
    {
        ConnectToDSoft();
    }

    private void ConnectToDSoft()
    {
        TcpClient temp = new TcpClient();
        temp.BeginConnect(IPAddress.Loopback.ToString(), 3000, ConnectCallback, temp);
    }
    private void ConnectCallback(IAsyncResult ar)
    {
        TcpClient temp = ar.AsyncState as TcpClient;
        temp.EndConnect(ar);
        lock (_Connection)
        {
            _Connection = new TcpConnection(temp);
        }
        _Connection.MessageRecieved += MessageFromDSoft;
        _Connection.Disconnected += ConnectionLost;
    }

    private void MessageFromDSoft(int dataLength, byte[] data)
    {

    }

    private void ConnectionLost(string message)
    {

    }
}
