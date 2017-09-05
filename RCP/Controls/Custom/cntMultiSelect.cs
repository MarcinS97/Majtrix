using HRRcp.App_Code;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace HRRcp.Controls.Custom
{
    public class cntMultiSelect : ListBox
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        class Options
        {
            public bool disableIfEmpty { get; set;}
            public bool includeSelectAllOption { get; set; }
            public string selectAllText { get; set; }
            public string disabledText { get; set; }
            public bool dropRight { get; set; }
            public bool dropUp { get; set; }
            public int maxHeight { get; set; }
            public string nonSelectedText { get; set; }
            public string nSelectedText { get; set; }
            public string allSelectedText { get; set; }
            public int numberDisplayed { get; set; }
            public string delimiterText { get; set; }
            public string buttonWidth { get; set; }
            public object buttonText { 
                get
                {
        
            // use JRaw to set the value of the anonymous function
                    return new JRaw(String.Format(@"     
        function(options, select) {{
                if (options.length === 0) {{
                    //return 'xxx No option selected ...';
                    return '{0}';
                }}
                if (options.length > 2) {{
                    return 'Zaznaczone: ' + options.length;
                }}
                 else {{
                     var labels = [];
                     options.each(function() {{
                         if ($(this).attr('label') !== undefined) {{
                             labels.push($(this).attr('label'));
                         }}
                         else {{
                             labels.push($(this).html());
                         }}
                     }});
                     return labels.join(', ') + '';
                 }}
            }}", nonSelectedText));
        
                }
            }
            public object onDropdownHidden
            {
                get
                {
                    return new JRaw(String.Format(@"
                        function(event) {{
                                    //alert('Dropdown closed.');
                        }}
                    "));

                }
            }
        }
        //includeSelectAllOption: true
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Options opt = new Options();

            opt.disableIfEmpty = DisableIfEmpty;
            opt.includeSelectAllOption = IncludeSelectAllOption;
            opt.selectAllText = SelectAllText;
            opt.disabledText = DisabledText;
            opt.dropRight = DropRight;
            opt.dropUp = DropUp;
            opt.maxHeight = MaxHeight;
            opt.nonSelectedText = NonSelectedText;
            opt.nSelectedText = NSelectedText;
            opt.allSelectedText = AllSelectedText;
            opt.numberDisplayed = NumberDisplayed;
            opt.delimiterText = DelimiterText;
            opt.buttonWidth = ButtonWidth;
            //opt.buttonText = ButtonText;
            Tools.ExecuteJavascript(String.Format(@"$('#{0}').multiselect({1});", this.ClientID, Newtonsoft.Json.JsonConvert.SerializeObject(opt)));
            
            //Tools.ShowMessage("asd");
        }

        public string SelectedItems
        {
            get
            {
                return String.Join(",", GetSelectedItems());
            }
        }

        public string[] GetSelectedItems()
        {
            List<string> selectedItems = new List<string>();
            foreach(ListItem item in this.Items)
            {
                if(item.Selected)
                {
                    if(!String.IsNullOrEmpty(item.Value))
                        selectedItems.Add(item.Value);
                }
            }
            return selectedItems.ToArray();
        }

        #region PROPERTIES

        public Boolean DisableIfEmpty
        {
            get { return Tools.GetViewStateBool(ViewState["vDisableIfEmpty"], false); }
            set { ViewState["vDisableIfEmpty"] = value; }
        }

        public Boolean IncludeSelectAllOption
        {
            get { return Tools.GetViewStateBool(ViewState["vIncludeSelectAllOption"], false); }
            set { ViewState["vIncludeSelectAllOption"] = value; }
        }

        public String SelectAllText
        {
            get { return Tools.GetStr(ViewState["vSelectAllText"], "Zaznacz wszystko..."); }
            set { ViewState["vSelectAllText"] = value; }
        }

        public String DisabledText
        {
            get { return Tools.GetStr(ViewState["vDisabledText"], ""); }
            set { ViewState["vDisabledText"] = value; }
        }

        public Boolean DropRight
        {
            get { return Tools.GetViewStateBool(ViewState["vDropRight"], false); }
            set { ViewState["vDropRight"] = value; }
        }

        public Boolean DropUp
        {
            get { return Tools.GetViewStateBool(ViewState["vDropUp"], false); }
            set { ViewState["vDropUp"] = value; }
        }

        public int MaxHeight
        {
            get { return Tools.GetViewStateInt(ViewState["VMaxHeight"], 100000); }
            set { ViewState["VMaxHeight"] = value; }
        }

        public String NonSelectedText
        {
            get { return Tools.GetStr(ViewState["vNonSelectedText"], "wybierz ..."); }
            set { ViewState["vNonSelectedText"] = value; }
        }
        
        public String NSelectedText
        {
            get { return Tools.GetStr(ViewState["vNSelectedText"], "Opcje"); }
            set { ViewState["vNSelectedText"] = value; }
        }

        public String AllSelectedText
        {
            get { return Tools.GetStr(ViewState["vAllSelectedText"], "Wszystko"); }
            set { ViewState["vAllSelectedText"] = value; }
        }

        public int NumberDisplayed
        {
            get { return Tools.GetViewStateInt(ViewState["vNumberDisplayed"], 2); }
            set { ViewState["vNumberDisplayed"] = value; }
        }

        public String DelimiterText
        {
            get { return Tools.GetStr(ViewState["vDelimiterText"], ","); }
            set { ViewState["vDelimiterText"] = value; }
        }

        public String ButtonWidth
        {
            get { return Tools.GetStr(ViewState["vButtonWidth"], "400px"); }
            set { ViewState["vButtonWidth"] = value; }
        }
        
        public String ButtonText
        {
            get { return Tools.GetStr(ViewState["vButtonText"], @"
        function(options, select) {
                if (options.length === 0) {
                    return 'xxx No option selected ...';
                }
                else if (options.length > 3) {
                    return 'More than 3 options selected!';
                }
                 else {
                     var labels = [];
                     options.each(function() {
                         if ($(this).attr('label') !== undefined) {
                             labels.push($(this).attr('label'));
                         }
                         else {
                             labels.push($(this).html());
                         }
                     });
                     return labels.join(', ') + '';
                 }
            }
"); }
            set { ViewState["vButtonText"] = value; }
        }

        #endregion
    }
}