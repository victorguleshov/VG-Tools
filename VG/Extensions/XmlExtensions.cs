using System.Collections.Generic;
using System.Xml;

namespace VG.Extensions
{
    public static class XmlExtensions
    {
        public static XmlNode GetNamedItem(this XmlAttributeCollection collection, string localName,
            bool ignoreCase = true)
        {
            if (ignoreCase)
            {
                localName = localName.ToLower();
                foreach (XmlNode item in collection)
                    if (item.Name.ToLower().Equals(localName))
                        return item;
                return null;
            }

            return collection.GetNamedItem(localName);
        }

        public static bool TryGetNamedItem(this XmlAttributeCollection collection, string localName, out XmlNode node,
            bool ignoreCase = true)
        {
            node = collection.GetNamedItem(localName, ignoreCase);
            return node != null;
        }

        public static IEnumerable<string> SplitAttributeValuesByWords(this XmlAttributeCollection collection,
            string localName) =>
            collection.TryGetNamedItem(localName, out var node) ? node.Value.SplitByWords() : new string[0];
    }
}