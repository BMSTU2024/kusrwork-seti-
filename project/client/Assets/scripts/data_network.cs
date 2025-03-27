using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class data_network : MonoBehaviour
{
    // Start is called before the first frame update
    //public Socket host;
    public enum type_command
    {
        find,
        add,
        del
    }
    public Socket host;
    public IPAddress ip_client;
    public IPAddress ip_server;
    bool error {  get; set; }
    string result { get; set; }

    public bool is_error()
    {
        bool bl = error;
        error = false;
        return bl;
    }
    public string get_rezult()
    {
        string rez = result;
        result = null;
        return rez;
    }
    public async void send_json<T>(T obj,string com)
    {
        Debug.Log("send");
        //string st3 = JsonUtility.ToJson(obj);
        string st3 = JsonConvert.SerializeObject(obj);
        string st2 = ip_client.ToString();
        string st0 = com;

        int count = st3.Length;
        //host.

        //NetworkStream st = host.GetStream();
        byte[] ms = Encoding.UTF8.GetBytes(st0 + "|" +st2+"|"+count+"|"+ st3);
        host.Send(ms);
        Debug.Log(st3);
        byte[] buf=new byte[1024];
        //host = host.Accept();
        host.Receive(buf);
        //host.Receive(buf);
        string st = Encoding.UTF8.GetString(buf);
        result = st;
        if (st != "none")
        {
            Debug.Log(st);
        }
    }
    public void send(string st,type_command com)
    {

    }
    public async Task connect(IPEndPoint point)
    {
        //var delay = new CancellationTokenSource();
        //delay.CancelAfter(5000);
        //var tok = delay.Token;
        
        while (true)
        {
            try
            {
                
                if (!host.ConnectAsync(point.Address, point.Port).Wait(5000))
                {
                    Debug.Log("error connect");
                    error = true;
                    return;
                }
                break;
            }
            catch (Exception e)
            {

                Debug.Log("error connect");
                error = true;
                return;
                //throw;
            }
        }
        //host = host.Accept();
        //Task ts1 = Task.Delay(7000);
        //Task ts1 = host.ConnectAsync(point);
        //await ts1;
        //Debug.Log("start connect");
        /*
        try
        {
            if (!ts1.Wait(5000))
            {
                Debug.Log("too longet time connection");
                return;
            }
            else
            {
                Debug.Log(ts1.IsCompleted);
            }
        }
        catch (Exception)
        {

            throw;
        }
        */
        /*
        try
        {
            //SocketAsyncEventArgs arg = new SocketAsyncEventArgs();
            
            //arg.Ti
            //Task.D

        }
        catch (OperationCanceledException)
        {
            Debug.Log("too longet time connection");
            return;
        }*/
        

        IPAddress[] mas_adr = Dns.GetHostAddresses(Dns.GetHostName());
        IPAddress adress_client = null;
        foreach (IPAddress ip in mas_adr)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                adress_client = ip;
                Console.WriteLine(ip + " ");
                break;
            }
        }

        byte[] tx_b = new byte[1024];
        foreach (byte tx in tx_b)
        {
            Console.WriteLine(tx);
        }
        //NetworkStream n_str = host.GetStream();
        //n_str.Write(tx_b);
        //await host.SendAsync(tx_b, SocketFlags.None);
        //return;
        if (!host.ReceiveAsync(tx_b, SocketFlags.None).Wait(5000))
        {
            Debug.Log("error connect");
            error = true;
            return;
        }
        //return;
        //SocketAsyncEventArgs arg = new SocketAsyncEventArgs();
        //host.Receive(lt_b);
        //arg.SetBuffer(tx_b);
        
        string st = Encoding.UTF8.GetString(tx_b);
        //host.Accept();
        Debug.Log("connection ["+st+"]");
        ip_client = adress_client;
        ip_server = point.Address;

        //host.Send(tx_b);
    }
    void Start()
    {
        host = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //host = host.Accept();
        //host = new Socket();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
