using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

public class ItemsManager : Singleton<ItemsManager>
{
    [SerializeField]
    private List<ItemData> items = new List<ItemData>();

    private void Awake()
    {
        Instance = this;

        items = Resources.LoadAll<ItemData>("Data/Items").ToList();

        LoadData();
    }

    public ItemData GetItemData(int id)
    {
        return items.Find(s => s.id == id);
    }

    public bool GetItemData(int id, out ItemData itemData)
    {
        ItemData handler = items.Find(s => s.id == id);
        itemData = handler;
        return handler != null;
    }

    public void LoadData()
    {
        XDocument xdoc = XDocument.Parse(Resources.Load<TextAsset>("Data/xml/items_proto").text);

        var itemsXml = from item in xdoc.Descendants("DATA_RECORD")
                       select new
                       {
                           id = item.Element("id").Value,
                           name = item.Element("name").Value,
                           reqLvl = item.Element("req_level").Value
                       };

        foreach (var item in itemsXml)
        {
            try
            {
                ItemData itemData = items.Find(i => i.id == XmlConvert.ToInt32(item.id));
                itemData.ReqLevel = XmlConvert.ToInt32(item.reqLvl);
            }
            catch (Exception ex)
            {
                Debug.LogError("Couldnt load item: " + item.id);
            }
        }
    }
}
