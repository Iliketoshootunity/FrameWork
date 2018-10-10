using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

public class AssetBundleDAL
{
    /// <summary>
    /// xml 路径
    /// </summary>
    private string path;
    /// <summary>
    /// 实体集合
    /// </summary>
    private List<AssetBundleEntitly> list;

    public AssetBundleDAL(string path)
    {
        this.path = path;
        list = new List<AssetBundleEntitly>();
    }

    public List<AssetBundleEntitly> GetList()
    {
        list.Clear();

        XDocument xDocunment = XDocument.Load(this.path);
        XElement root = xDocunment.Root;
        XElement assetBundly = root.Element("AssetBundle");
        IEnumerable<XElement> Items = assetBundly.Elements("Item");

        int index = 0;
        foreach (var item in Items)
        {
            AssetBundleEntitly entitly = new AssetBundleEntitly();
            entitly.Key = "key" + ++index;
            entitly.Name = item.Attribute("Name").Value;
            entitly.Tag = item.Attribute("Tag").Value;
            entitly.Version = item.Attribute("Tag").Value.ToInt();
            entitly.Size = item.Attribute("Tag").Value.ToLong();
            entitly.ToPath = item.Attribute("ToPath").Value;

            IEnumerable<XElement> paths = item.Elements("Path");
            foreach (var path in paths)
            {
                entitly.Path.Add(string.Format("Assets/{0}", path.Attribute("Value").Value));
            }

            list.Add(entitly);
        }
        return list;
    }
}

