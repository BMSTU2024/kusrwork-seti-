using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;


//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;

public class ui_button : MonoBehaviour
{
    // Start is called before the first frame update
    public object type;
    public par_player pl;
    //public func fn;
    public void perehod_scene(string st)
    {
        func.get_data_sql().st_perehod_scene(st);
    }
    public void sql_login()
    {
        Debug.Log("logining");
        data_sql sql = func.get_data_sql();
        string lg = GameObject.Find("input tx login").GetComponent<InputField>().text;
        try
        {
            
            int ps = int.Parse(GameObject.Find("input tx password").GetComponent<InputField>().text);
            List<data_sql.player> lt_ = new List<data_sql.player>();
            lt_.Add(new data_sql.player("s", 1));
            Debug.Log(JsonConvert.SerializeObject(lt_));
            func.get_data_network().send_json<data_sql.player>(new data_sql.player(lg, ps),"find player login");
            string rez = func.get_data_network().get_rezult();
            if ( rez!= "none")
            {
                data_sql.player_now= JsonConvert.DeserializeObject<List<data_sql.player>>(rez)[0];
                perehod_scene("base menu");
            }
                
            //StartCoroutine(sql.Get_proverka_account("http://localhost/DBUnity/get_player.php", lg, ps));
        }
        catch (System.Exception e)
        {
            //GameObject.Find("Canvas").transform.Find("pn error").Find("tx error").gameObject.GetComponent<Text>().text = "� ������ ����� ���� ������ �����";
            GameObject.Find("Canvas").transform.Find("pn error").Find("tx error").gameObject.GetComponent<Text>().text = "��������� ������";
            GameObject.Find("Canvas").transform.Find("pn error").gameObject.SetActive(true);
            Debug.Log(e.Message);
            //throw;
        }
        
       

    }
    public void sql_registrated()
    {
        data_sql sql = GameObject.Find("data_sql").GetComponent<data_sql>();
        string lg = GameObject.Find("input tx login").GetComponent<InputField>().text;
        int ps = int.Parse(GameObject.Find("input tx password").GetComponent<InputField>().text);
        int ps1 = int.Parse(GameObject.Find("input tx password").GetComponent<InputField>().text);
        if (ps == ps1)
        {

        }
            //StartCoroutine(sql.registrated_account("http://localhost/DBUnity/registrated.php", lg, ps));

    }

    public void sql_entry_chat()
    {
        data_sql.chat_now = (data_sql.list_chats)type;
        GameObject.Find("data_sql").GetComponent<data_sql>().get_message_bl_now = true;
        GameObject.Find("data_sql").GetComponent<data_sql>().bl_entry_chat = true;
        StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().get_message("http://localhost/DBUnity/get_message.php", data_sql.chat_now.id));
        
    }
    public void sql_create_chat(InputField inp)
    {
        //data_sql.chat_now = (data_sql.lt_chat)type;
        //GameObject.Find("data_sql").GetComponent<data_sql>().get_message_bl_now = true;
        //GameObject.Find("data_sql").GetComponent<data_sql>().bl_entry_chat = true;
        string st = inp.text;
        if (st!="")
            StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().create_chat("http://localhost/DBUnity/create_chat.php", st));
        else
        {
            GameObject.Find("Canvas").transform.Find("pn error").Find("tx error").gameObject.GetComponent<Text>().text = "���� �� ����� ���� ������";
            GameObject.Find("Canvas").transform.Find("pn error").gameObject.SetActive(true);
        }
            
    }
    public void sql_post_message(InputField tx)
    {
        StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().post_message("http://localhost/DBUnity/post_message.php", data_sql.chat_now.id, tx.text));
        tx.text = "";
    }
    public void sql_get_players_chat(bool bl)
    {
        StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().get_players_chat("http://localhost/DBUnity/get_players_chat.php", data_sql.chat_now,bl));
    }
    public void sql_set_player_in_chat()
    {
        if(data_sql.type_chat_now==data_sql.type_chat_set.add)
            StartCoroutine(GameObject.Find("data_player").GetComponent<data_sql>().add_players_in_chat("http://localhost/DBUnity/chat_player_add.php",type.ToString() ,data_sql.chat_now.chat));
        else if (data_sql.type_chat_now == data_sql.type_chat_set.admin)
            StartCoroutine(GameObject.Find("data_player").GetComponent<data_sql>().admin_players_in_chat("http://localhost/DBUnity/chat_player_admin.php", type.ToString(), data_sql.player_now.login, data_sql.chat_now.chat));
        else if (data_sql.type_chat_now == data_sql.type_chat_set.del)
            StartCoroutine(func.get_data_sql().del_players_in_chat("http://localhost/DBUnity/chat_player_del.php", type.ToString(), data_sql.chat_now.chat));
    }
    
    public void connect_server(InputField tx)
    {
        
        IPEndPoint point = new IPEndPoint(IPAddress.Parse(tx.text), 2007);
        func.get_data_network().connect(point);
        if (!func.get_data_network().is_error())
        {
            perehod_scene("entry");
        }
        else
        {
            GameObject.Find("Canvas").transform.Find("pn error").gameObject.SetActive(true);
        }
    }
    public void yes_add_chat(ui_tx tx)
    {
        StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().yes_add_player_chat("http://localhost/DBUnity/yes_chat_player_add.php",data_sql.player_now.login ,((data_sql.list_chats)type).chat,tx));
        
    }
    public void not_add_chat(ui_tx tx)
    {
        StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().not_add_player_chat("http://localhost/DBUnity/not_chat_player_add.php", data_sql.player_now.login, ((data_sql.list_chats)type).chat, tx));
        //tx.sql_chats();
    }
    public void open_chat_set_add(string scene)
    {
        data_sql.type_chat_now = data_sql.type_chat_set.add;
        perehod_scene(scene);
    }
    public void open_chat_set_admin(string scene)
    {
        data_sql.type_chat_now = data_sql.type_chat_set.admin;
        perehod_scene(scene);
    }
    public void open_chat_set_del(string scene)
    {
        data_sql.type_chat_now = data_sql.type_chat_set.del;
        perehod_scene(scene);
    }
    public void exit_chat()
    {
        StartCoroutine(GameObject.Find("data_sql").GetComponent<data_sql>().exit_players_in_chat("http://localhost/DBUnity/chat_player_del.php", data_sql.player_now.login, data_sql.chat_now.chat));
    }
    public void exit_login()
    {
        data_sql sqll= GameObject.Find("data_sql").GetComponent<data_sql>();
        Debug.Log(sqll);
        string lg = data_sql.player_now.login;
        data_sql.player_now = null;
        StartCoroutine(sqll.status_account("http://localhost/DBUnity/logining.php", lg, "offline",false));
        SceneManager.LoadScene("entry");
    }



    public void create_mob(string tp)
    {
        Debug.Log("click");
        if (tp == "worker" &&pl.col_res["res rock"]>=5)
            pl.create_mob(new func.st_create_obj { name = tp, pl = this.pl, v2 = pl.gameObject.transform.position});
        if (tp == "warrior" && pl.col_res["res ruda"] >= 2 && pl.col_res["res rock"] >= 10)
            pl.create_mob(new func.st_create_obj { name = tp, pl = this.pl, v2 = pl.gameObject.transform.position });
    }
    /*
    public IEnumerator Get_proverka_account(string url, string lg, int ps)
    {
        using (UnityWebRequest web = UnityWebRequest.Get(url + "?php_login=" + lg + "&php_password=" + ps))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                string[] stt = web.downloadHandler.text.Split("_");
                Debug.Log(stt[0]);

                List<data_sql.player> lt_pl = new List<data_sql.player>();
                if (stt.Length > 0)
                {
                    for (int i = 0; i < stt.Length - 1; i += 2)
                    {
                        //int ind_now = i / 2;
                        Debug.Log(stt[i] + " " + stt[i + 1]);
                        data_sql.player pl = new data_sql.player(stt[i], int.Parse(stt[i + 1]));

                        lt_pl.Add(pl);
                    }
                }

                if (lt_pl.Count != 0)
                {
                    data_sql.player_now=lt_pl[0];
                    StartCoroutine(status_account("http://localhost/DBUnity/logining.php", lg,"online"));
                    
                }
                //



                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }


    }
    public IEnumerator status_account(string url,string login,string par)
    {
        WWWForm form = new WWWForm();
        Debug.Log("post "+login+" "+par);
        form.AddField("php_status", par);
        form.AddField("php_login", login);
        using (UnityWebRequest web = UnityWebRequest.Post(url,form))
        {
            yield return web.SendWebRequest();
            if (web.error != null)
            {
                Debug.Log("error web");
            }
            else
            {
                Debug.Log(web.downloadHandler.text);
                SceneManager.LoadScene("base menu");
                /*
                string[] stt = web.downloadHandler.text.Split("_");
                Debug.Log(stt[0]);

                List<data_sql.player> lt_pl = new List<data_sql.player>();
                if (stt.Length > 0)
                {
                    for (int i = 0; i < stt.Length - 1; i += 2)
                    {
                        //int ind_now = i / 2;
                        Debug.Log(stt[i] + " " + stt[i + 1]);
                        data_sql.player pl = new data_sql.player(stt[i], int.Parse(stt[i + 1]));

                        lt_pl.Add(pl);
                    }
                }

                if (lt_pl.Count != 0)
                {
                    data_sql.player_now = lt_pl[0];

                    SceneManager.LoadScene("base menu");
                }
                
                //



                //MatchCollection mc = Regex.Matches(web.downloadHandler.text, @"_");
                //string[] splitData = Regex.Split(dataText, @"_");
            }
            web.Dispose();
        }

    
    }
    */

}
