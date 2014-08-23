// Based on the work of crdx https://github.com/crdx/PortableSettingsProvider
//The MIT License (MIT)
//Copyright (c) crdx
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;

namespace KCT.HotkeyKanban.Settings
{
    public sealed class PortableSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        private const string RootNodeName = "settings";
        private const string LocalSettingsNodeName = "localSettings";
        private const string GlobalSettingsNodeName = "globalSettings";
        private const string ClassName = "PortableSettingsProvider";
        private XmlDocument xmlDocument;

        private string FilePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    string.Format("{0}.settings", ApplicationName));
            }
        }

        private XmlNode LocalSettingsNode
        {
            get
            {
                XmlNode settingsNode = GetSettingsNode(LocalSettingsNodeName);
                XmlNode machineNode = settingsNode.SelectSingleNode(Environment.MachineName.ToLowerInvariant());
                if (machineNode == null)
                {
                    machineNode = rootDocument.CreateElement(Environment.MachineName.ToLowerInvariant());
                    settingsNode.AppendChild(machineNode);
                }
                return machineNode;
            }
        }

        private XmlNode GlobalSettingsNode
        {
            get { return GetSettingsNode(GlobalSettingsNodeName); }
        }

        private XmlNode RootNode
        {
            get { return rootDocument.SelectSingleNode(RootNodeName); }
        }

        private XmlDocument rootDocument
        {
            get
            {
                if (xmlDocument == null)
                {
                    try
                    {
                        xmlDocument = new XmlDocument();
                        xmlDocument.Load(FilePath);
                    }
                    catch (Exception)
                    {
                    }
                    if (xmlDocument.SelectSingleNode(RootNodeName) != null)
                        return xmlDocument;
                    xmlDocument = GetBlankXmlDocument();
                }
                return xmlDocument;
            }
        }

        public override string ApplicationName
        {
            get { return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().GetName().Name); }
            set { }
        }

        public override string Name
        {
            get { return ClassName; }
        }

        public void Reset(SettingsContext context)
        {
            LocalSettingsNode.RemoveAll();
            GlobalSettingsNode.RemoveAll();
            xmlDocument.Save(FilePath);
        }

        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            // do nothing
            return new SettingsPropertyValue(property);
        }

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(Name, config);
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue propertyValue in collection)
                SetValue(propertyValue);
            try
            {
                rootDocument.Save(FilePath);
            }
            catch (Exception)
            {
                /*
                    * If this is a portable application and the device has been
                    * removed then this will fail, so don't do anything. It's
                    * probably better for the application to stop saving settings
                    * rather than just crashing outright. Probably.
                    */
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context,
            SettingsPropertyCollection collection)
        {
            var values = new SettingsPropertyValueCollection();
            foreach (SettingsProperty property in collection)
            {
                values.Add(new SettingsPropertyValue(property)
                {
                    SerializedValue = GetValue(property)
                });
            }
            return values;
        }

        private void SetValue(SettingsPropertyValue propertyValue)
        {
            XmlNode targetNode = IsGlobal(propertyValue.Property)
                ? GlobalSettingsNode
                : LocalSettingsNode;
            XmlNode settingNode = targetNode.SelectSingleNode(string.Format("setting[@name='{0}']", propertyValue.Name));
            if (settingNode != null)
                settingNode.InnerText = propertyValue.SerializedValue.ToString();
            else
            {
                settingNode = rootDocument.CreateElement("setting");
                XmlAttribute nameAttribute = rootDocument.CreateAttribute("name");
                nameAttribute.Value = propertyValue.Name;
                settingNode.Attributes.Append(nameAttribute);
                settingNode.InnerText = propertyValue.SerializedValue.ToString();
                targetNode.AppendChild(settingNode);
            }
        }

        private string GetValue(SettingsProperty property)
        {
            XmlNode targetNode = IsGlobal(property) ? GlobalSettingsNode : LocalSettingsNode;
            XmlNode settingNode = targetNode.SelectSingleNode(string.Format("setting[@name='{0}']", property.Name));
            if (settingNode == null)
                return property.DefaultValue != null ? property.DefaultValue.ToString() : string.Empty;
            return settingNode.InnerText;
        }

        private bool IsGlobal(SettingsProperty property)
        {
            foreach (DictionaryEntry attribute in property.Attributes)
            {
                if ((Attribute) attribute.Value is SettingsManageabilityAttribute)
                    return true;
            }
            return false;
        }

        private XmlNode GetSettingsNode(string name)
        {
            XmlNode settingsNode = RootNode.SelectSingleNode(name);
            if (settingsNode == null)
            {
                settingsNode = rootDocument.CreateElement(name);
                RootNode.AppendChild(settingsNode);
            }
            return settingsNode;
        }

        public XmlDocument GetBlankXmlDocument()
        {
            var blankXmlDocument = new XmlDocument();
            blankXmlDocument.AppendChild(blankXmlDocument.CreateXmlDeclaration("1.0", "utf-8", string.Empty));
            blankXmlDocument.AppendChild(blankXmlDocument.CreateElement(RootNodeName));
            return blankXmlDocument;
        }
    }
}