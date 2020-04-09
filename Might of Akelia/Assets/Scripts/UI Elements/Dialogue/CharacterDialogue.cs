using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

    [XmlRoot("CharacterDialogue")]
    public class CharacterDialogue
    {
        [XmlElement("Language")]
        public string Language;
        [XmlElement("Name")]
        public string Name;
        [XmlArrayItem("Node")]
        public List<DialogueNode> Nodes;

        //for serialization
       public CharacterDialogue() { }

        public CharacterDialogue(string characterName)
        {
            Name = characterName;
            Nodes = new List<DialogueNode>();
        }

        #region Manual Functions
        //Not in use due to Design Tool -- 
        //But keeping them just in case of manual necessity
        public DialogueNode CreateNewNode()
        {
            DialogueNode node = new DialogueNode();
            Nodes.Add(node);
            node.nodeID = Nodes.IndexOf(node);
            return node;
        }
        public void AddNode(DialogueNode node)
        {
            if (node == null) { return; } //we don't have to add it if it's null

            Nodes.Add(node);
            node.nodeID = Nodes.IndexOf(node);
        }
        public void AddOption(string optionText, DialogueNode node, DialogueNode destinationNode)
        {
            if (!Nodes.Contains(destinationNode))
            { AddNode(destinationNode); }

            if (!Nodes.Contains(node))
            { AddNode(node); }

            DialogueOption option;

            if (destinationNode == null)
            { option = new DialogueOption(optionText, 0, 1); }
            else { option = new DialogueOption(optionText, destinationNode.nodeID, 1); }

            node.Options.Add(option);
        }
        #endregion
}
