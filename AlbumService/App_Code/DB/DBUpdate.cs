﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;
using System.Xml;

/// <summary>
/// DBUpdate 的摘要说明
/// </summary>
public class DBUpdate : IDisposable
{
    private MySqlConnection CON = DBOpen.GetCon();
    private MySqlCommand COM = new MySqlCommand();

	public DBUpdate()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    public void Dispose()
    {
        COM.Dispose();
        if (CON.State == ConnectionState.Open) CON.Close();
    }

    /// <summary>
    /// 更新同一个图片集多张图片的权限
    /// </summary>
    /// <param name="PID"></param>
    /// <param name="AID"></param>
    /// <param name="power"></param>
    /// <returns></returns>
    public int[] UpdatePicPower(int[] PID, int AID, int power)
    {
        List<int> UpidList = new List<int>();
        for (int i = 0; i < PID.Length; i++)
        {
            string SQL = "update a_upicture set up_power = " + power + " where up_abid = " + AID + " and up_id = " + PID[i] + ";";
            if (_ExecuteSQL(SQL) > 0)
            { UpidList.Add(PID[i]); }
        }
        return UpidList.ToArray();
    }

    static public int[] UpdatePicAID(int[] PID, int AID)
    {
        List<int> UpidList = new List<int>();
        string SQL = "";
        for (int i = 0; i < PID.Length; i++)
        {
            SQL = "update a_upicture set up_abid = " + AID + " where up_id = " + PID[i] + ";";
            if (ExecuteSQL(SQL) > 0)
            {
                UpidList.Add(PID[i]);
            }
        }
        return UpidList.ToArray();
    }

    static public DataSet UpdateUserInfo(int UID, String PASSWORD, String PATH, String MD5CODE, String EMAIL)
    {
        if (PASSWORD == null && PATH == null && MD5CODE == null && EMAIL == null)
        { return null; }
        try
        {
            int PID = -1;
            if (MD5CODE != null)
            {
                String PICSSQL = "CALL UpdateUserPic('" + MD5CODE + "', '" + PATH + "');";
                DataSet ds = ExecuteSQL_2(PICSSQL);
                PID = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }
            bool flag = false;
            String SQL = "update a_user set";
            if (PASSWORD != null) { SQL += " u_password = '" + PASSWORD + "'"; flag = true; }
            if (EMAIL != null) { SQL += (flag ? "," : " ") + " u_Code = '' , u_Email = '" + EMAIL + "'"; flag = true; }
            if (PID != -1) { SQL += (flag ? "," : " ") + " u_pic = " + PID; }
            SQL += " where u_id = " + UID + ";";
            int Ans = ExecuteSQL(SQL);
            using (DBSelect DBS = new DBSelect())
            {
                return Ans == 0 ? null : DBS.SelectTheUser(UID);
            }
        }
        catch (Exception)
        {
            return null;
        }
    }

    static public int SuccessValiEmail(string USERNAME)
    {
        try
        {
            String SQL = "UPDATE a_user set u_Code = 'OK' where u_userName = '" + USERNAME+"';";
            return ExecuteSQL(SQL);
        }
        catch (Exception)
        { return -1; }
    }
    static public int UpdateUserEmailCode(int UID, string CODE)
    {
        try
        {
            String SQL = "UPDATE a_user set u_Code = '" + CODE + "' where u_id = " + UID;
            return ExecuteSQL(SQL);
        }
        catch (Exception)
        { return -1; }
    }
    static public int UpdateAblum(int UID,int AID, String ABLUMNAME)
    {
        try {
            String SQL = "UPDATE a_ablum SET a_name = '" + ABLUMNAME + "' WHERE a_uid = " + UID + " AND a_id = " + AID + ";";
            return ExecuteSQL(SQL);
        }
        catch (Exception) { return -1; }
    }
    static public int UpdateUPictureAblum(int UID,int PID,int AID)
    {
        try
        {
            String SQL = "UPDATE a_upicture SET up_abid = " + AID + " WHERE up_id = " + PID + ";";
            return ExecuteSQL(SQL);
        }
        catch (Exception) { return -1; }
    }
    /// <summary>
    /// 更新表A_UPICTURE数据
    /// </summary>
    /// <param name="PID">up_id</param>
    /// <param name="PIC_NAME">up_name</param>
    /// <param name="CONTENT">up_description</param>
    /// <returns></returns>
    static public int UpdateUPicture(int PID, String PIC_NAME, String CONTENT)
    {
        if (PIC_NAME == null && CONTENT == null) return -1;
        try
        {
            String SQL = "UPDATE a_upicture SET ";
            bool flag = false;
            if (PIC_NAME != null)
            {
                SQL += " up_name = '" + PIC_NAME + "'";
                flag = true;
            }
            if (CONTENT != null)
            {
                SQL += flag ? "," : "";
                SQL += "up_description = '" + CONTENT + "'";
            }
            SQL += " WHERE up_id=" + PID + ";";
            return ExecuteSQL(SQL);
        }
        catch (Exception)
        { return -1; }
    }

    static private int ExecuteSQL(String SQL)
    {
        try
        {
            MySqlConnection CON = DBOpen.GetCon();
            MySqlCommand COM = new MySqlCommand();
            CON.Open();
            COM.CommandText = SQL;
            COM.Connection = CON;
            int COUNT = COM.ExecuteNonQuery();
            COM.Dispose();
            CON.Close();
            return COUNT;
        }
        catch (Exception)
        {
            return -1;
        }
    }
    static private DataSet ExecuteSQL_2(String SQL)
    {
        try
        {
            DataSet DS = new DataSet();
            MySqlConnection CON = DBOpen.GetCon();
            MySqlCommand COM = new MySqlCommand();
            MySqlDataAdapter da = new MySqlDataAdapter(COM);

            CON.Open();
            COM.CommandText = SQL;
            COM.Connection = CON;

            da.Fill(DS);

            COM.Dispose();
            CON.Close();
            return DS;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private int _ExecuteSQL(String SQL)
    {
        try
        {
            if (CON.State != ConnectionState.Open)
                CON.Open();
            COM.CommandText = SQL;
            COM.Connection = CON;
            int COUNT = COM.ExecuteNonQuery();
            return COUNT;
        }
        catch (Exception)
        {
            return 0;
        }
    } 
}