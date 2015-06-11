﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Configuration;

/// <summary>
/// FileIO 的摘要说明
/// </summary>
public class FileIO
{
	public FileIO()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    static private String PicturePath = ConfigurationManager.AppSettings["uploadHost"];

    static public bool FileSave(byte[] FILEDATA, String FILENAME,ref String PATH)
    {
        try
        {
            //FILENAME = Regex.Replace(FILENAME, "/+", "-");
            FILENAME = FILENAME.Replace('/', '_').Replace('+','-');//URL编码和路径问题
            PATH = FILENAME;
            File.WriteAllBytes(ConfigurationManager.AppSettings["uploadSystem"] + PATH, FILEDATA);
            return true;
        }
        catch (Exception) { return false; }
    }

    static public String FileMD5OfBase64(byte[] FILE)
    {
        if (FILE == null) return "";
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        String MD5BASE64 = Convert.ToBase64String(md5.ComputeHash(FILE));
        md5.Dispose(); md5.Clear();
        return MD5BASE64;
    }
    static public String FileBase64(byte[] FILE)
    {
        if (FILE == null) return "";
        String BASE64 = Convert.ToBase64String(FILE);
        return BASE64;
    }

    static private string p = ConfigurationManager.AppSettings["uploadSystem"];
    static public byte[] FileRead(String PATH)
    {
        PATH = FileIO.p + PATH;
        try
        {
            FileStream stream = new FileInfo(PATH).OpenRead();
            Byte[] FileData = new Byte[stream.Length];
            //从流中读取字节块并将该数据写入给定缓冲区buffer中
            stream.Read(FileData, 0, Convert.ToInt32(stream.Length));
            stream.Close();
            return FileData;
        }
        catch (Exception)
        {
            return null;
        }
    }

    static public double GetPicSize(String PATH, ref int Height, ref int Width)
    {
        if (PATH.Substring(PATH.Length - 1, 1) == "\r")
        {
            PATH = PATH.Substring(0,PATH.Length-1);
        }
        PATH = FileIO.p + PATH;
        FileInfo File = null;
        try
        {
            File = new FileInfo(PATH);
        }
        catch (Exception)
        {
            return 0;
        }
        using (FileStream stream = File.OpenRead())
        {
            System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
            Height = image.Height;
            Width = image.Width;
            return (double)stream.Length;
        }
    }

    static public String GetPicUrl(String PATH)
    {
        if (PATH == null || PATH.Length <= 0) return "";
        return ConfigurationManager.AppSettings["uploadHost"] + PATH;
    }

}