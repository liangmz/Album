using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

/// <summary>
/// XML 的摘要说明
/// </summary>
public class ToXML
{
	public ToXML()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    static public XmlDocument GetXml()
    {
        XmlDocument xml = new XmlDocument();
        XmlDeclaration xd = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
        xml.AppendChild(xd);
        XmlNode root = xml.CreateElement("AblumData");
        xml.AppendChild(root);

        return xml;
    }
}