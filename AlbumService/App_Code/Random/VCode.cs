using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using System.Web.Configuration;

/// <summary>
/// VCode 的摘要说明
/// </summary>
public class VCode
{
	public VCode()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    static public String getEmailVCode(string email)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        Random R = new Random();
        email = R.Next(1, 200) + email + R.Next(300,9000);
        string Ans = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(email)));
        Ans = Ans.Replace('=', '*').Replace('/', '-').Replace('+', '.');
        md5.Dispose();
        return Ans;
    }

    static public String getOPCode(String USERNAME)
    {
        return "";
    }

    static public String GetCodeImage(ref byte[] image, int t_width, int t_height)
    {
        String[] Code = new String[4]; ;
        Random rr = new Random();
        for (int i = 0; i < 4; i++)
        {
            int Cc = rr.Next(1,20);
            Code[i] = Cc.ToString();
        }//生成验证字符串

        MemoryStream ms = new MemoryStream();
        Bitmap bm = new Bitmap(t_width, t_height);//画布
        Graphics g = Graphics.FromImage(bm);//
        Color C = ToColor("#5d3f89");//Color.Purple;//背景色
        g.Clear(C);

        Font f = new Font("A", 22, FontStyle.Underline);
        Brush b = new SolidBrush(Color.White);

        //浮动范围
        int DeviationX = 3;
        int DeviationY = -3;
        
        for (int i = 0; i < 4; i++)
        {
            int index = (t_width / 4) * i;
            int index_x = 15 + rr.Next(DeviationY, DeviationX) + index;//设置位置浮动
            int index_y = 15 + rr.Next(DeviationY, DeviationX);
            g.DrawString(Code[i], f, b, new Point(index_x, index_y));
        }//画验证码字符串

        Random r = new Random();
        for (int i = 0; i < 1200; i++)
        {
            int x = r.Next(1, t_width);
            int y = r.Next(1, t_height);
            bm.SetPixel(x, y, Color.White);
        }//画点
        for (int j = 0; j < 10; j++)
        {
            Point x = new Point(r.Next(1, t_width), r.Next(1, t_height));
            Point y = new Point(r.Next(1, t_width), r.Next(1, t_height));
            g.DrawLine(Pens.Violet, x, y);
        }//画线

        bm.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        ms.Position = 0;
        image = new byte[ms.Length];
        ms.Read(image, 0, Convert.ToInt32(ms.Length));//写入字节数组

        return Code[0] + Code[1] + Code[2] + Code[3];
    }

    static public Color ToColor(string color)//颜色代码转为Color
    {

        int red, green, blue = 0;
        char[] rgb;
        color = color.TrimStart('#');
        color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
        switch (color.Length)
        {
            case 3:
                rgb = color.ToCharArray();
                red = Convert.ToInt32(rgb[0].ToString() + rgb[0].ToString(), 16);
                green = Convert.ToInt32(rgb[1].ToString() + rgb[1].ToString(), 16);
                blue = Convert.ToInt32(rgb[2].ToString() + rgb[2].ToString(), 16);
                return Color.FromArgb(red, green, blue);
            case 6:
                rgb = color.ToCharArray();
                red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                return Color.FromArgb(red, green, blue);
            default:
                return Color.FromName(color);

        }
    }

    /// <summary>
    /// 获取密码重设口令
    /// </summary>
    /// <returns></returns>
    static public String PSWresetCode(string USERNAME)
    {
        String KEY = USERNAME + "ALBUM_PASSWORD_RESET" + DateTime.Now.ToString("yyyyMMddHHmm");

        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        String CODE = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(KEY)));
        CODE = CODE.Replace('=', '*').Replace('/', '-').Replace('+', '.');
        md5.Dispose();
        return CODE;
    }

    /// <summary>
    /// 验证密码重设口令,20分钟有效
    /// </summary>
    /// <param name="USERNAME"></param>
    /// <param name="KEY"></param>
    /// <returns></returns>
    static public bool PSW_Code_Verification(string USERNAME, string KEY)
    {
        DateTime DT = DateTime.Now;
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        for (int i = 0; i < 20; i++)
        {
            DateTime _DT = DT.AddMinutes(-i);
            String key = USERNAME + "ALBUM_PASSWORD_RESET" + _DT.ToString("yyyyMMddHHmm");
            String CODE = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(key)));
            CODE = CODE.Replace('=', '*').Replace('/', '-').Replace('+', '.');
            if (CODE.Equals(KEY))
            {
                md5.Dispose();
                return true;
            }
        }
        md5.Dispose();
        return false;
    }
}