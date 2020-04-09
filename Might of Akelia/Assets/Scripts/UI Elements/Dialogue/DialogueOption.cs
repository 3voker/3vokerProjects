using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

public class DialogueOption
{
    [XmlElement("text")]
    public string text;
    [XmlElement("destinationNodeID")]
    public int destinationNodeID;
    [XmlElement("bark")]
    public int bark;

    public DialogueOption() { }
    public DialogueOption(string _text, int _destinationID, int _bark)
    {
        text = _text;
        destinationNodeID = _destinationID;
        bark = _bark;
    }
}