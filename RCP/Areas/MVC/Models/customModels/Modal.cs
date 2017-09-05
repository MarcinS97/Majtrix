using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HRRcp.App_Code;

namespace HRRcp.Areas.MVC.Models.customModels
{

[NotMapped]
public class Modal
    {
        public Modal()
        {
            ConfirmButtonText = L.p("Tak");
            CancelButtonText = L.p("Nie");
            ShowXButton = true;
            ShowConfirmButton = true;
            ShowTextBox = false;
            ShowTextBox2 = false;
            ShowDDLInModal2 = false;
            CommentTextValue = "";
            CommentText2Value = "";
        }

        public string Header { get; set; }
        public string Text { get; set; }
        public string CommentText { get; set; }
        public string CommentTextValue { get; set; }
        public string CommentText2 { get; set; }
        public string CommentText2Value { get; set; }
        public string DateInModalText { get; set; }
        public string DDLInModalText { get; set; }
        public string DDLInModalText2 { get; set; }

        public string Javascript { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? DateInModal { get; set; }

        public string ConfirmButtonText { get; set; }
        public string CancelButtonText { get; set; }

        public string ConfirmButtonController { get; set; }
        public string CancelButtonController { get; set; }

        public string ConfirmButtonAction { get; set; }
        public string CancelButtonAction { get; set; }

        public object ConfirmButtonRouteValues { get; set; }
        public object CancelButtonRouteValues { get; set; }



        public bool ShowXButton { get; set; }
        public bool ShowConfirmButton { get; set; }
        public bool ShowTextBox { get; set; }
        public bool ShowTextBox2 { get; set; }
        public bool ShowDateInModal { get; set; }
        public bool ShowDDLInModal { get; set; }
        public bool ShowDDLInModal2 { get; set; }
    }
}
