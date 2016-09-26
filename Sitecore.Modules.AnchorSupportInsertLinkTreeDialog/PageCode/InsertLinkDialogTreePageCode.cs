namespace Sitecore.Modules.AnchorSupportInsertLinkTreeDialog.PageCode
{
    using System;
    using System.Net;
    using System.Xml.Linq;
    using Configuration;
    using Data.Items;
    using Diagnostics;
    using Mvc.Presentation;
    using Shell;
    using Speak.Applications;
    using Web;
    using Web.PageCodes;

    public class InsertLinkDialogTreePageCode : PageCodeBase
    {
        #region Original Implementation

        private string targetActiveBrowserItemId = "{C5FA4571-37B3-472C-BDA1-0FADC2D2EFA7}";
        private string targetCustomItemId = "{07CF2A84-9C22-4E85-8F3F-C301AADF5218}";
        private string targetNewBrowserItemId = "{02A6C72E-17BB-48C5-8D35-AF9C494ED6BA}";

        private static string GetXmlAttributeValue(XElement element, string attrName)
        {
            if (element.Attribute(attrName) == null)
            {
                return string.Empty;
            }
            return element.Attribute(attrName).Value;
        }

        public override void Initialize()
        {
            string setting = Settings.GetSetting("BucketConfiguration.ItemBucketsEnabled");
            this.ListViewToggleButton.Parameters["IsVisible"] = setting;
            this.TreeViewToggleButton.Parameters["IsVisible"] = setting;
            this.TreeView.Parameters["ShowHiddenItems"] = UserOptions.View.ShowHiddenItems.ToString();
            this.ReadQueryParamsAndUpdatePlaceholders();
        }

        #endregion

        public Rendering Anchor { get; set; }

        private void ReadQueryParamsAndUpdatePlaceholders()
        {
            #region Original Implementation

            string queryString = WebUtil.GetQueryString("ro");
            string str2 = WebUtil.GetQueryString("hdl");
            if (!string.IsNullOrEmpty(queryString) && (queryString != "{0}"))
            {
                this.TreeView.Parameters["RootItem"] = queryString;
            }
            this.InsertAnchorButton.Parameters["Click"] = string.Format(this.InsertAnchorButton.Parameters["Click"], WebUtility.UrlEncode(queryString), WebUtility.UrlEncode(str2));
            this.InsertEmailButton.Parameters["Click"] = string.Format(this.InsertEmailButton.Parameters["Click"], WebUtility.UrlEncode(queryString), WebUtility.UrlEncode(str2));
            this.ListViewToggleButton.Parameters["Click"] = string.Format(this.ListViewToggleButton.Parameters["Click"], WebUtility.UrlEncode(queryString), WebUtility.UrlEncode(str2));
            bool flag = str2 != string.Empty;
            string text = string.Empty;

            if (flag)
            {
                text = UrlHandle.Get()["va"];
            }

            if (text != string.Empty)
            {
                XElement element = XElement.Parse(text);
                if (GetXmlAttributeValue(element, "linktype") == "internal")
                {
                    string xmlAttributeValue = GetXmlAttributeValue(element, "id");
                    if (!string.IsNullOrWhiteSpace(xmlAttributeValue))
                    {
                        Item mediaItemFromQueryString = SelectMediaDialog.GetMediaItemFromQueryString(xmlAttributeValue);
                        this.TreeView.Parameters["PreLoadPath"] = mediaItemFromQueryString.Paths.LongID.Substring(1);
                        this.TextDescription.Parameters["Text"] = GetXmlAttributeValue(element, "text");
                        this.AltText.Parameters["Text"] = GetXmlAttributeValue(element, "title");
                        this.StyleClass.Parameters["Text"] = GetXmlAttributeValue(element, "class");
                        this.QueryString.Parameters["Text"] = GetXmlAttributeValue(element, "querystring");
                        this.SetupTargetDropbox(element);

                        #endregion

                        this.Anchor.Parameters["Text"] = GetXmlAttributeValue(element, "anchor");
                    }
                }
            }
        }

        #region Original Implementation

        private void SetupTargetDropbox(XElement fieldContent)
        {
            string targetNewBrowserItemId;
            string xmlAttributeValue = GetXmlAttributeValue(fieldContent, "target");
            if (xmlAttributeValue.Equals("_blank", StringComparison.OrdinalIgnoreCase))
            {
                targetNewBrowserItemId = this.TargetNewBrowserItemId;
            }
            else if (string.IsNullOrWhiteSpace(xmlAttributeValue))
            {
                targetNewBrowserItemId = this.TargetActiveBrowserItemId;
            }
            else
            {
                targetNewBrowserItemId = this.TargetCustomItemId;
                this.CustomUrl.Parameters["Text"] = xmlAttributeValue;
            }
            this.TargetLoadedValue.Parameters["Text"] = targetNewBrowserItemId;
        }

        public Sitecore.Mvc.Presentation.Rendering AltText { get; set; }

        public Sitecore.Mvc.Presentation.Rendering CustomUrl { get; set; }

        public Sitecore.Mvc.Presentation.Rendering InsertAnchorButton { get; set; }

        public Sitecore.Mvc.Presentation.Rendering InsertEmailButton { get; set; }

        public Sitecore.Mvc.Presentation.Rendering ListViewToggleButton { get; set; }

        public Sitecore.Mvc.Presentation.Rendering QueryString { get; set; }

        public Sitecore.Mvc.Presentation.Rendering StyleClass { get; set; }

        public Sitecore.Mvc.Presentation.Rendering Target { get; set; }

        public string TargetActiveBrowserItemId
        {
            get
            {
                return this.targetActiveBrowserItemId;
            }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                this.targetActiveBrowserItemId = value;
            }
        }

        public string TargetCustomItemId
        {
            get
            {
                return this.targetCustomItemId;
            }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                this.targetCustomItemId = value;
            }
        }

        public Sitecore.Mvc.Presentation.Rendering TargetLoadedValue { get; set; }

        public string TargetNewBrowserItemId
        {
            get
            {
                return this.targetNewBrowserItemId;
            }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                this.targetNewBrowserItemId = value;
            }
        }

        public Sitecore.Mvc.Presentation.Rendering TextDescription { get; set; }

        public Sitecore.Mvc.Presentation.Rendering TreeView { get; set; }

        public Sitecore.Mvc.Presentation.Rendering TreeViewToggleButton { get; set; }

        #endregion
    }
}