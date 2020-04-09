using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class XMLFileHandler
{
    private static XMLFileHandler instance;
    private XMLFileHandler() { }
    public static XMLFileHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new XMLFileHandler();
            }
            return instance;
        }
    }

    public const string FileExtension = ".xml";

    public void SaveCharacterDataToXML(CharacterDialogue dialogue, string fileLocation)
    {
        StreamWriter writer;
        FileInfo t = new FileInfo(fileLocation + "\\" + dialogue.Name + FileExtension);
        if (!t.Exists)
        {
            writer = t.CreateText();
        }
        else
        {
            t.Delete();
            writer = t.CreateText();
        }
        writer.Write(ObjectToXml(dialogue));
        writer.Close();
        Debug.Log("File written.");
    }

    public CharacterDialogue LoadCharacterData(string path)
    {
        CharacterDialogue character = null;

        TextAsset xml = Resources.Load(path) as TextAsset;

        if (xml != null)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CharacterDialogue));
            StringReader reader = new StringReader(xml.text);
            character = serializer.Deserialize(reader) as CharacterDialogue;
            reader.Close();
        }
        else
        {
            Debug.LogError("XML File Was Null At Path: " + path);
        }

        return character;
    }

    public string ObjectToXml<T>(T objectToSerialise)
    {
        StringWriter Output = new StringWriter(new StringBuilder());
        XmlSerializer xs = new XmlSerializer(objectToSerialise.GetType());
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        xs.Serialize(Output, objectToSerialise, ns);
        return Output.ToString();
    }
}
