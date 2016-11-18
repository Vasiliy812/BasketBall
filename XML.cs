using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class XML : MonoBehaviour {

	public string GetParam(string ParamName, string AttributeName)
	{	
		string s = "";
		string Path = Application.dataPath + "/Data.xml";
     
		if (File.Exists(Path))
		{ 
			XmlDocument XmlFile = new XmlDocument();
			XmlFile.Load(Path);
			XmlNodeList nodeList = XmlFile.GetElementsByTagName(ParamName);
			foreach (XmlNode node in nodeList)
				s = node.Attributes[AttributeName].Value;			
		} 
		return(s);
	}

	public void SetParam(string ParamName, string AttributeName, string sValue)
	{			
		string Path = Application.dataPath + "/Data.xml";

		if (File.Exists(Path))
		{ 
			XmlDocument XmlFile = new XmlDocument();
			XmlFile.Load(Path);
			XmlNodeList nodeList = XmlFile.GetElementsByTagName(ParamName);
			foreach (XmlNode node in nodeList)
			{
				if (node.Name == ParamName)
					node.Attributes[AttributeName].Value = sValue;
			}
			XmlFile.Save(Path);
		} 
	}

}

