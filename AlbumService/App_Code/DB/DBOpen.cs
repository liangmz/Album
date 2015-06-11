using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Xml;
using System.Configuration;

/// <summary>
/// DBOpen 的摘要说明
/// </summary>
public class DBOpen
{
	public DBOpen()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    static string linkString = ConfigurationManager.AppSettings["linkString"];// "User Id=root;pwd=root;Host=127.0.0.1;Port=3306;Database=Ablum;pooling=true;Min Pool Size=0;Max Pool Size=10000; Character Set=utf8";

    static public MySqlConnection GetCon()
    {
        MySqlConnection con = new MySqlConnection();
        con.ConnectionString = linkString;
        return con;
    }

}