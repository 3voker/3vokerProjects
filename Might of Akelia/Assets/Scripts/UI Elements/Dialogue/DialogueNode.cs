using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

    public class DialogueNode
    {
        [XmlElement("ID")]
        public int nodeID = 0;
        [XmlElement("Title")]
        public string title;
        [XmlElement("Text")]
        public string text;
        [XmlArrayItem("Option")]
        public List<DialogueOption> Options;

        public DialogueNode() { title = "Node #" + nodeID;  text = "";  Options = new List<DialogueOption>(); }
    }
