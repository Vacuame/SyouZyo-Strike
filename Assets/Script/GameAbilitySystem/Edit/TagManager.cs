using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : Singleton<TagManager>
{
    private Dictionary<string,TagNode>TagDict = new Dictionary<string,TagNode>();

    public List<string> GetTagGeneration(string name)
    {
        List<string> result = new List<string>();
        if (name == "") return result;
        result.Add(name);

        if(TagDict.ContainsKey(name))
        {
            TagNode tagNode = TagDict[name];
            while(tagNode.parent!=null)
            {
                result.Add(tagNode.parent.name);
                tagNode = tagNode.parent;
            }
        }
        result.Reverse();
        return result;
    }

    public TagManager() 
    {
        List<string> lines = FileReader.ReadFile("Resources/TestFile.txt");
        GetTagRelationship(lines);
    }


    private void GetTagRelationship(List<string> lines)
    {
        Stack<TagNode> stack = new Stack<TagNode>();
        for(int i=0;i< lines.Count;i++)
        {
            //将Line处理成name和level
            int level = lines[i].LastIndexOf('\t')+1;
            string tagName = lines[i].Substring(level);

            if (tagName == "") continue;
            if (tagName.Contains("//")) continue;

            //找parent
            while(level < stack.Count)
                stack.Pop();
            TagNode parent = stack.Count == 0 ? null : stack.Peek();

            //更新结构
            TagNode newNode = new TagNode(tagName, parent);
            TagDict.Add(tagName, newNode);
            stack.Push(newNode);
        }
    }

    public class TagNode
    {
        public string name;
        public TagNode parent;
        public TagNode(string name, TagNode parent=null) 
        {
            this.name = name;
            this.parent = parent;
        }
    }

}

