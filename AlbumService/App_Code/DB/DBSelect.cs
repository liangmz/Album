using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;
using System.Xml;

/// <summary>
/// DBSelect 的摘要说明
/// </summary>
public class DBSelect : IDisposable
{
    //static MySqlConnection _CON = null;
    static private int SelectCount = 30;

    private MySqlConnection CON = null;
    private MySqlCommand COM = null;

    public DBSelect()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
        CON = DBOpen.GetCon();
        COM = new MySqlCommand();
    }

    public void Dispose()
    {
        COM.Dispose();
        if (CON.State == ConnectionState.Open) CON.Close();
    }

    public DataSet SelectPicKeyWord(String Keyword, int Mode, int PAGE, out int PAGECOUNT)
    {
        String pageCountSQL = "select count(*) from (" +
            "select * from A_Tag  where t_name like '%" + Keyword + "%' group by(t_upid) )PCOUNT;";

        String SQL = "select A_Upicture.* ,A_Picture.p_path from A_Upicture ,A_Tag ,A_Picture " +
            "where A_Tag.t_name like '%" + Keyword + "%' " +
            " and A_Upicture.up_id = A_Tag.t_upid " +
            " and A_Upicture.up_power < " + Mode +
            " and A_Upicture.up_pid = A_Picture.p_id " +
            " group by(A_Upicture.up_id) " +
            " limit " + ((PAGE - 1) * SelectCount) + "," + SelectCount + ";";

        return _ExecuteSQL_AndCount(SQL, pageCountSQL, out PAGECOUNT);
    }

    public DataSet SelectPicTag(int PID)
    {
        String SQL = "select * from a_tag where t_upid = " + PID + ";";
        return _ExecuteSQL(SQL);
    }

    /// <summary>
    /// 通过邮箱读取用户信息
    /// </summary>
    /// <param name="EMAIL"></param>
    /// <returns></returns>
    public DataSet SelectUserForEmail(string EMAIL)
    {
        String SQL = "select * from a_user where u_Email = '" + EMAIL + "';";
        return _ExecuteSQL(SQL);
    }

    public DataSet SelectAlbumList(int UID)
    {
        String SQL = "SELECT a_id,a_name FROM a_ablum WHERE a_uid = " + UID + ";"; ;
        return _ExecuteSQL(SQL);
    }
    //关注好友列表
    public DataSet FriendList(int UID, int PAGE, ref int PAGECOUNT)
    {
        String SQL = "SELECT a_relation.*,A.* ,B.p_path " +
            "FROM a_relation,a_user A " +
            "left join a_picture B on A.u_pic = B.p_id " +
            "WHERE r_my = " + UID + " AND r_friend = u_id LIMIT "
            + ((PAGE - 1) * SelectCount) + "," + SelectCount + ";";

        String PageCountSQL = "SELECT COUNT(*) FROM a_relation WHERE r_my = " + UID + ";";
        return _ExecuteSQL_AndCount(SQL, PageCountSQL, out PAGECOUNT);
    }
    //共享列表
    public DataSet SelectShare(int PAGE, ref int PAGECOUNT)
    {
        String SQL = "SELECT * FROM share_view order by up_uploadTime desc limit "
            + ((PAGE - 1) * SelectCount) + "," + SelectCount + ";";
        String PageCountSQL = "SELECT COUNT(*) FROM share_view";
        return _ExecuteSQL_AndCount(SQL, PageCountSQL, out PAGECOUNT);
    }
    //UID的好友共享图片-全部
    public DataSet SelectFriendShare(int UID, int PAGE, ref int PAGECOUNT)
    {
        String SQL = "select * from friend_view where u_id=" + UID + " order by up_uploadTime desc limit "
            + ((PAGE - 1) * SelectCount) + "," + SelectCount;
        String PageCountSQL = "select COUNT(*) from friend_view where u_id = " + UID;
        return _ExecuteSQL_AndCount(SQL, PageCountSQL, out PAGECOUNT);
    }
    //static public DataSet SelectFriendShare(int UID, int PAGE, ref int PAGECOUNT)
    //{
    //    String SQL = "SELECT * FROM relation_view WHERE u_id in (select r_friend from a_relation where r_my = " + UID + ")"
    //        + "order by up_uploadTime desc LIMIT "
    //        + ((PAGE - 1) * SelectCount) + "," + SelectCount + ";";
    //    String PageCountSQL = "SELECT COUNT(*) FROM relation_view WHERE u_id in (select r_friend from a_relation where r_my = " + UID + ");";
    //    return ExecuteSQL_AndCount(SQL, PageCountSQL, ref PAGECOUNT);
    //}
    //我的共享图片
    //static public DataSet SelectMyShare(int UID, int PAGE, ref int PAGECOUNT)
    //{
    //    String SQL = "SELECT * FROM relation_view WHERE u_id = " + UID + " LIMIT"
    //        + ((PAGE - 1) * SelectCount) + "," + SelectCount + ";";
    //    String PageCountSQL = "SELECT COUNT(*) FROM relation_view WHERE u_id = " + UID + ";";
    //    return ExecuteSQL_AndCount(SQL, PageCountSQL, ref PAGECOUNT);
    //}

    //专辑里面图片的列表
    public DataSet SelectAblumPics(int UID, int AID, int PAGE, ref int PAGECOUNT)
    {
        String SQL = "select * from my_view where u_id=" + UID + " and a_id=" + AID + " order by up_uploadTime desc limit "
            + ((PAGE - 1) * SelectCount) + "," + SelectCount + ";";
        //String SQL = "SELECT a_upicture.*,a_picture.p_path FROM a_upicture,a_picture WHERE up_abid = " + AID
        //    + " and a_upicture.up_pid = a_picture.p_id order by up_uploadTime DESC LIMIT "
        //    + ((PAGE - 1) * SelectCount) + "," + SelectCount + ";"; ;
        String PageCountSQL = "select COUNT(*) from my_view where u_id=" + UID + " and a_id=" + AID;
        return _ExecuteSQL_AndCount(SQL, PageCountSQL, out PAGECOUNT);
    }

    //Select我的专辑列表
    public DataSet SelectAblumList(int UID, int PAGE, ref int PAGECOUNT)
    {
        String SQL = "SELECT * FROM a_ablum WHERE a_uid = " + UID + " LIMIT "
            + ((PAGE - 1) * SelectCount) + "," + SelectCount + ";"; ;
        String PageCountSQL = "SELECT COUNT(*) FROM a_ablum WHERE a_uid = " + UID + ";";
        return _ExecuteSQL_AndCount(SQL, PageCountSQL, out PAGECOUNT);
    }

    //获得这张图片信息
    public DataSet SelectSharePic(int PID)//共享图片
    {
        String SQL = "SELECT * FROM Share_View WHERE up_id = " + PID + ";";
        return _ExecuteSQL(SQL);
    }
    public DataSet SelectMyPic(int UID, int PID)//这张是本人的图片
    {
        String SQL = "select * from My_View where up_id = " + PID + " and u_id = " + UID;
        return _ExecuteSQL(SQL);
    }
    //public DataSet SelectUserSharePic(int UID, int PID)//这张图片已经共享
    public DataSet SelectUserSharePic(int UID, int PID)
    {
        String SQL = "select * from friend_view where u_id=" + UID + " and up_id = " + PID;
        return _ExecuteSQL(SQL);
    }
    //{
    //    String SQL = "select * from friend_view where up_id = " + PID + " and u_id=" + UID;
    //    return _ExecuteSQL(SQL);
    //}


    //////////////////////


    //获得图片的回复列表
    public DataSet SelectEvaluation(int PID, int PAGE, ref int PAGECOUNT)
    {
        String SQLGETCOUNT = "SELECT COUNT(*) FROM a_evaluation WHERE e_upid = " + PID + ";";

        String SQL = "SELECT a_evaluation.*,A.u_userName,B.p_path " +
            "FROM a_evaluation,a_user A " +
            "left join a_picture B on B.p_id = A.u_pic " +
            "WHERE a_evaluation.e_upid = "
            + PID + " AND A.u_id = a_evaluation.e_uid order by e_time desc LIMIT "
            + ((PAGE - 1) * SelectCount) + "," + SelectCount + ";";
        return _ExecuteSQL_AndCount(SQL, SQLGETCOUNT, out PAGECOUNT);
    }
    public DataSet SelectTheUser(String USERNAME)
    {
        String SQL = "SELECT A.*,B.p_path " +
            "FROM a_user A " +
            "left join a_picture B on A.u_pic = B.p_id " +
            "WHERE u_userName = '" + USERNAME + "';";
        return _ExecuteSQL(SQL);
    }
    public DataSet SelectTheUser(int UID)
    {
        String SQL = "SELECT A.*,B.p_path " +
            "FROM a_user A " +
            "left join a_picture B on A.u_pic = B.p_id " +
            "WHERE u_id = " + UID + ";";
        return _ExecuteSQL(SQL);
    }
    //用户select
    public DataSet SelectUser(String USERNAME, int PAGE, ref int PAGECOUNT)
    {
        String SQLGETCOUNT = "SELECT COUNT(*) FROM a_user WHERE u_userName LIKE '%" + USERNAME + "%';";
        String SQL = "SELECT A.*,B.p_path " +
            "FROM a_user A " +
            "left join a_picture B on A.u_pic = B.p_id " +
            "WHERE u_userName LIKE '%" + USERNAME + "%' LIMIT " + ((PAGE - 1) * SelectCount) + "," + SelectCount + ";";
        return _ExecuteSQL_AndCount(SQL, SQLGETCOUNT, out PAGECOUNT);
    }

    public bool PicIsExist(int PID, int UID)
    {
        try
        {
            String SQL = "SELECT * FROM my_view WHERE up_id = " + PID + "AND u_id = " + UID + ";";
            return _ExecuteSQL(SQL).Tables[0].Rows.Count > 0 ? true : false;
        }
        catch (Exception)
        { return false; }
    }

    //static private DataSet ExecuteSQL(String SQL)
    //{
    //    try
    //    {
    //        DataSet ANS = new DataSet();
    //        MySqlConnection CON = DBOpen.GetCon();
    //        CON.Open();
    //        MySqlCommand COM = new MySqlCommand();
    //        COM.CommandText = SQL;
    //        COM.Connection = CON;
    //        DataSet ds = new DataSet();
    //        MySqlDataAdapter da = new MySqlDataAdapter(COM);
    //        da.Fill(ANS);
    //        COM.Dispose();
    //        CON.Close();
    //        return ANS;
    //    }
    //    catch (Exception)
    //    { throw new Exception(); }
    //}

    //static private DataSet ExecuteSQL_AndCount(String SelectSQL, String PageCountSQL, ref int PageCount)
    //{
    //    try
    //    {
    //        DataSet ANS = new DataSet();
    //        MySqlConnection CON = DBOpen.GetCon();
    //        CON.Open();
    //        MySqlCommand COM = new MySqlCommand();
    //        COM.CommandText = SelectSQL;
    //        COM.Connection = CON;
    //        DataSet ds = new DataSet();
    //        MySqlDataAdapter da = new MySqlDataAdapter(COM);
    //        da.Fill(ANS);
    //        ds.Dispose();//**********
    //        COM.Dispose();

    //        if (PageCount == -1 || PageCountSQL != null)
    //        {
    //            COM.CommandText = PageCountSQL;
    //            COM.Connection = CON;
    //            MySqlDataReader dr = COM.ExecuteReader();
    //            if (dr.Read())
    //            {
    //                int _COUNT = Convert.ToInt32(dr[0].ToString());
    //                PageCount = _COUNT / SelectCount + (_COUNT % SelectCount != 0 ? 1 : 0);
    //            }
    //            COM.Dispose();
    //        }
    //        CON.Close();
    //        return ANS;
    //    }
    //    catch (Exception)
    //    { return null; }

    //}

    private DataSet _ExecuteSQL(String SQL)
    {
        try
        {
            if (CON.State != ConnectionState.Open) CON.Open();

            DataSet ANS = new DataSet();
            COM.CommandText = SQL;
            COM.Connection = CON;
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter(COM);
            da.Fill(ANS);

            return ANS;
        }
        catch (Exception) { }
        return null;
    }

    private DataSet _ExecuteSQL_AndCount(String SelectSQL, String PageCountSQL, out int PageCount)
    {
        try
        {
            if (CON.State != ConnectionState.Open) CON.Open();

            DataSet ANS = new DataSet();
            COM.CommandText = SelectSQL;
            COM.Connection = CON;
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter(COM);
            da.Fill(ANS);

            PageCount = 0;
            if (PageCountSQL != null)
            {
                COM.CommandText = PageCountSQL;
                COM.Connection = CON;

                int _COUNT = Convert.ToInt32(COM.ExecuteScalar());
                PageCount = _COUNT / SelectCount + (_COUNT % SelectCount != 0 ? 1 : 0);
            }
            return ANS;
        }
        catch (Exception)
        {
            PageCount = 0;
            return null;
        }
    }

}