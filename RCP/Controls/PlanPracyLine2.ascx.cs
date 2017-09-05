using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class PlanPracyLine2 : System.Web.UI.UserControl
    {
        private int FMode;
        public string cntPrefix;  // prefix nazwy kontrolek
        public string hidZmianyId;
        public bool Selected;

        public PlanPracyLineHeader LineHeader;

#if SIEMENS
        private const bool noaccBlink = true;   // blinkowanie jak alert
#else
        private const bool noaccBlink = false;  // obwoluta
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        //----------------------
        /* raporty mają inny 
        private PlanPracy GetParentPP()
        {4
            PlanPracy pp = (PlanPracy)Parent.NamingContainer.Parent;
            return pp;
        }
        */
        //----- div wersja -----
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("tdPP");
                HtmlGenericControl div = (HtmlGenericControl)e.Item.FindControl("divPP");

                //PlanPracy pp = GetParentPP();

                if (td != null && div != null)
                {
                    string dataItem = (string)e.Item.DataItem;
                    string id, s, c, wt, info, absencja, p5, p6, p7, p8, p9;
                    bool noDay = false;
                    bool pracOff = false;
                    bool isInfo = false;
                    bool isAbsencja = false;
                    bool isHoliday = false;
                    string absHint = null;
                    string tdClass = null;
                    string divClass = null;

                    /*  PlanUrlopówRok
                        days[i] = Tools.SetLineParams(D[3],   // 0 kod
                                                      D[4],   // 1 symbol 
                                                      D[5],   // 2 kolor
                                                      info,   // 3 info
                                                      hint,   // 4 hint 
                                                      dno,    // 5 numer dnia ~ absencja w pp
                                                      D[3],   // 6 dla znacznika modyfikacji
                                                      D[8],   // 7 absencja: symbol
                                                      kolor); // 8 absencja: kolor - jeżeli nie jest wybierana w PU                     
                     */
                    Tools.GetLineParams(dataItem, out id, out s, out c, out info, out wt, out p5, out p6, out p7, out p8, out p9);  
                    absencja = p5;
                    //----- no day - do raportów -----
                    if (!String.IsNullOrEmpty(info))
                    {
                        if (info.IndexOf(PlanPracy.maNoDay) >= 0            //"x" -> td
                            //|| info.IndexOf(PlanPracy.maOff) >= 0
                            )             //"0" -> td nie liczony pracownik, tym samym mechanizmem zrobię
                        {
                            noDay = true;
                            //c = "#E0E0E0";  // na sztywno zeby print zadziałał ...
                            c = "#EEEEEE";  // na sztywno zeby print zadziałał ...
                            tdClass += " noday";
                        }
                        else if (info.IndexOf(PlanPracy.maOff) >= 0)
                        {
                            pracOff = true;
                            c = "#E0E0E0";  // na sztywno zeby print zadziałał ...
                            tdClass += " noday";
                        }
                    }
                    //----- brak dnia np 30.02 -----
                    if (noDay)
                    {
                        //----- headerline info -----
                        //*zzzzzzzzzzz
                        if (e.Item.ItemIndex < LineHeader.HeaderData.Count())   // HeaderData moze nie występować, ale powinien ...
                        {
                            string hinfo = LineHeader.HeaderData[e.Item.ItemIndex];
                            if (!String.IsNullOrEmpty(hinfo))
                            {
                                if (hinfo.IndexOf(PlanPracy.maToday) >= 0)
                                    tdClass += " today";                                //"t" -> td
                            }
                        }
                        /*
                        if (e.Item.ItemIndex < pp.HeaderData.Count())   // HeaderData moze nie występować 
                        {
                            string hinfo = pp.HeaderData[e.Item.ItemIndex];
                            if (!String.IsNullOrEmpty(hinfo))
                            {
                                if (hinfo.IndexOf(PlanPracy.maToday) >= 0)
                                    tdClass += " today";                                //"t" -> td
                            }
                        }
                        */
                    }
                    //----- pracownik nie liczony - algorytm = bez liczenia czasu pracy -----
                    else if (pracOff)
                    {
                        if (e.Item.ItemIndex < LineHeader.HeaderData.Count())   // HeaderData moze nie występować, ale powinien ...
                        {
                            string hinfo = LineHeader.HeaderData[e.Item.ItemIndex];
                            if (!String.IsNullOrEmpty(hinfo))
                            {
                                if (hinfo.IndexOf(PlanPracy.maToday) >= 0)
                                    tdClass += " today";                                //"t" -> td
                            }
                        }
                        //----- absencja -----
                        if (!String.IsNullOrEmpty(absencja))    //hint do absencji - odcinam symbol
                        {
                            int p = absencja.IndexOf('-');
                            if (p != -1)
                            {
                                if (p < absencja.Length)
                                    absHint = absencja.Substring(p + 1).Trim();
                                absencja = absencja.Substring(0, p).Trim();
                            }
                        }
                        isAbsencja = info.IndexOf(PlanPracy.maAbsencja) >= 0;   //"u" -> td
                        if (isAbsencja) tdClass += " absencja";
                        Label lb = Tools.SetText(e.Item, "lbOverlap", absencja);
                        if (lb != null && isAbsencja) lb.ToolTip = absHint;
                    }
                    //----- dane czasu pracy ----- 
                    else
                    {
                        //----- absencja -----
                        if (!String.IsNullOrEmpty(absencja))    //hint do absencji - odcinam symbol
                        {
                            int p = absencja.IndexOf('-');
                            if (p != -1)
                            {
                                if (p < absencja.Length)
                                    absHint = absencja.Substring(p + 1).Trim();
                                absencja = absencja.Substring(0, p).Trim();
                            }
                        }
                        //----- day - cell info -----
                        if (!String.IsNullOrEmpty(info))
                        {
                            isInfo = info.IndexOf(PlanPracy.maInfo) >= 0;           //"i" -> div
                            if (!isInfo && (FMode == PlanPracy.moPlanUrlopow || FMode == PlanPracy.moPlanUrlopowRok))
                                if (info.IndexOf(PlanPracy.maUrlopKorekta) >= 0)
                                    isInfo = true;
                            if (isInfo) divClass = " info";

                            isAbsencja = info.IndexOf(PlanPracy.maAbsencja) >= 0;   //"u" -> td
                            if (isAbsencja) tdClass += " absencja";
                            else
                            {
                                isAbsencja = info.IndexOf(PlanPracy._maAbsencjaKier) >= 0;  //"U" -> td
                                if (isAbsencja) tdClass += " absK";
                            }

                            if (info.IndexOf(PlanPracy.maAlert) >= 0)               //"A" -> div
                                divClass += " alert";

                            if (info.IndexOf(PlanPracy.maAlert2) >= 0)              //"B" -> div
                                divClass += " alert2";

                            if (info.IndexOf(PlanPracy.maBlockEdit) >= 0)           //"p" -> td
                                tdClass += " noedit";
                            if (info.IndexOf(PlanPracy.maPrzesun) >= 0)             //"P" -> td
                                tdClass += " move";

                            if (info.IndexOf(PlanPracy.maAccepted) >= 0)            //"a" -> td
                                tdClass += " acc";

                            if (info.IndexOf(PlanPracy.maNoAcc) >= 0)               //"n" -> td
                            {
                                tdClass += " noacc";
                                if (noaccBlink)
                                    divClass += " alert";
                            }
                            if (info.IndexOf(PlanPracy._maSelected) >= 0)            //"s" -> td
                                tdClass += " selected";

                            isHoliday = info.IndexOf(PlanPracy.maHoliday) >= 0;
                            if (FMode == PlanPracy.moKartaRoczna || FMode == PlanPracy.moPlanUrlopowRok)
                                if (isHoliday)
                                    tdClass += " holiday";                          //"h" -> td
                        }
                        //----- headerline info -----
                        if (FMode != PlanPracy.moKartaRoczna)
                            if (e.Item.ItemIndex < LineHeader.HeaderData.Count())   // HeaderData moze nie występować 
                            {
                                string hinfo = LineHeader.HeaderData[e.Item.ItemIndex];
                                if (!String.IsNullOrEmpty(hinfo))
                                {
                                    if (hinfo.IndexOf(PlanPracy.maHoliday) >= 0)
                                        tdClass += " holiday";                              //"h" -> td
                                    if (hinfo.IndexOf(PlanPracy.maToday) >= 0)
                                        tdClass += " today";                                //"t" -> td
                                }
                            }

                        /*
                        if (e.Item.ItemIndex < pp.HeaderData.Count())   // HeaderData moze nie występować 
                        {
                            string hinfo = pp.HeaderData[e.Item.ItemIndex];
                            if (!String.IsNullOrEmpty(hinfo))
                            {
                                if (hinfo.IndexOf(PlanPracy.maHoliday) >= 0)
                                    tdClass += " holiday";                              //"h" -> td
                                if (hinfo.IndexOf(PlanPracy.maToday) >= 0)
                                    tdClass += " today";                                //"t" -> td
                            }
                        }
                          */
                        //----- zmiana --------------------
                        Label lb = Tools.SetText(e.Item, "lbTop", s);

                        /*
                        if (FMode == PlanPracy.moZmiany)
                        {
                            lb.ToolTip = _wt;
                            _wt = null;          //<<<< tymczas
                        }
                        Tools.SetText(e.Item, "lbBottom", _wt);
                        lb = Tools.SetText(e.Item, "lbOverlap", absencja);
                        if (lb != null && isAbsencja) lb.ToolTip = absHint;
                        */
                        //----- zmiana i mysz -----    
                        switch (FMode)
                        {
                            default:
                            case PlanPracy.moZmiany:
                                td.Attributes["title"] = wt;
                                lb.Text = s;
                                //lb.ToolTip = wt;
                                Tools.SetText(e.Item, "lbOverlap", p5);     // nr dnia (data)
                                lb = Tools.SetText(e.Item, "lbBottom", p7); // rzeczywista absencja

                                PlanPracy pp = (PlanPracy)Parent.NamingContainer.Parent;
                                if (pp.EditMode)
                                {
                                    if (Selected) tdClass += " selPrac";
                                    RepeaterItem item = LineHeader.Repeater.Items[e.Item.ItemIndex];
                                    CheckBox cb = (CheckBox)item.FindControl("cbDay");
                                    if (cb != null && cb.Checked)
                                        tdClass += " selDay";

                                    div.Attributes["onclick"] = String.Format("javascript:selectCellPP(this,{0},{1},'{2}','{3}');",
                                        PracId,
                                        e.Item.ItemIndex,
                                        cntPrefix,
                                        hidZmiany.ClientID.Substring(cntPrefix.Length));
                                }
                                break;
                            case PlanPracy.moPlanUrlopow:
                                td.Attributes["title"] = wt;
                                lb.Text = s;
                                //lb.ToolTip = wt;
                                Tools.SetText(e.Item, "lbOverlap", p5);     // nr dnia (data)
                                lb = Tools.SetText(e.Item, "lbBottom", p7); // rzeczywista absencja
                                lb.BackColor = ColorTranslator.FromHtml(p8); 

                                //if (!String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(p8)) c = p8;      // kolor - jezeli jest plan i kolor to go ustawiam (UŻ), przy braku planu wszystko bez kolorów

                                pp = (PlanPracy)Parent.NamingContainer.Parent;
                                if (pp.EditMode)
                                {
                                    if (Selected) tdClass += " selPrac";
                                    RepeaterItem item = LineHeader.Repeater.Items[e.Item.ItemIndex];
                                    CheckBox cb = (CheckBox)item.FindControl("cbDay");
                                    if (cb != null && cb.Checked)
                                        tdClass += " selDay";
                                    
                                    div.Attributes["onclick"] = String.Format("javascript:selectCellPP(this,{0},{1},'{2}','{3}');",
                                        PracId, 
                                        e.Item.ItemIndex, 
                                        cntPrefix, 
                                        hidZmiany.ClientID.Substring(cntPrefix.Length));
                                }
                                break;
                            case PlanPracy.moPlanUrlopowRok:
                                /*
                        days[i] = Tools.SetLineParams(D[3],   // 0 id - kod
                                                      D[4],   // 1 s  - symbol 
                                                      D[5],   // 2 c  - kolor
                                                      info,   // 3 info
                                                      hint,   // 4 hint 
                                                      dno,    // 5 numer dnia ~ absencja w pp
                                                      D[3],   // 6 dla znacznika modyfikacji
                                                      D[8],   // 7 absencja: symbol
                                                      kolor); // 8 absencja: kolor - jeżeli nie jest wybierana w PU                     
                                */

                                td.Attributes["title"] = wt;
                                lb.Text = s;
                                //lb.ToolTip = wt;
                                Tools.SetText(e.Item, "lbOverlap", p5);     // nr dnia (data)
                                lb = Tools.SetText(e.Item, "lbBottom", p7); // rzeczywista absencja
                                lb.BackColor = ColorTranslator.FromHtml(p8); 
                               
                                //if (!String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(p8)) c = p8;      // kolor - jezeli jest plan i kolor to go ustawiam (UŻ), przy braku planu wszystko bez kolorów

                                cntPlanRoczny2 pr = (cntPlanRoczny2)Parent.NamingContainer.Parent;
                                if (pr.EditMode)
                                {
                                    if (Selected) tdClass += " selPrac";
                                    RepeaterItem item = LineHeader.Repeater.Items[e.Item.ItemIndex];
                                    CheckBox cb = (CheckBox)item.FindControl("cbDay");
                                    if (cb != null && cb.Checked)
                                        tdClass += " selDay";
                                
                                    if (!noDay && !isHoliday)
                                        div.Attributes["onclick"] = String.Format("javascript:selectCellPU(this,{0},'{1}','{2}');",   // PU - Plan Urlopów
                                            e.Item.ItemIndex, 
                                            cntPrefix, 
                                            hidZmiany.ClientID.Substring(cntPrefix.Length));
                                }
                                break;
                            case PlanPracy.moAccept:
                                Tools.SetText(e.Item, "lbBottom", wt);
                                lb = Tools.SetText(e.Item, "lbOverlap", absencja);
                                if (lb != null && isAbsencja) lb.ToolTip = absHint;

                                div.Attributes["onclick"] = String.Format("javascript:selectCellPPAcc(this,{0},{1},'{2}');",
                                    PracId, e.Item.ItemIndex, cntPrefix);
                                break;
                            case PlanPracy.moKartaRoczna:
                                string br = !String.IsNullOrEmpty(wt) && !String.IsNullOrEmpty(absencja) ? "<br />" : null;
                                lb = Tools.SetText(e.Item, "lbBottom", wt + br + absencja);
                                if (lb != null && isAbsencja) lb.ToolTip = absHint;

                                //RepPlanPracy rpp = (RepPlanPracy)Parent.NamingContainer.Parent;
                                break;
                            case PlanPracy.moRozliczenie:
                                break;
                        }
                    }  //czas pracy
                    //----- kolor -----
                    if (!String.IsNullOrEmpty(c))  //zmiana
                    {
                        td.Attributes["style"] = String.Format("background-color: {0}; background-image: none;", c);
                        //td.Attributes["style"] = String.Format("background-image: none;", c);
                        //x div.Attributes["style"] = String.Format("border: solid 2px {0};", c);
                    }
                    HtmlGenericControl bck = (HtmlGenericControl)e.Item.FindControl("bckPP");   // kolor do wydruku
                    if (bck != null)
                        if (!String.IsNullOrEmpty(c))  //zmiana
                            bck.Attributes["style"] = String.Format("border-color: {0};", c);
                        else 
                            bck.Visible = false;
                    //----- class ----------------
                    td.Attributes["class"] += tdClass;
                    div.Attributes["class"] += divClass;
                }
            }
        }


        /* kopia
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("tdPP");
                HtmlGenericControl div = (HtmlGenericControl)e.Item.FindControl("divPP");
                if (td != null && div != null)
                {
                    string dataItem = (string)e.Item.DataItem;
                    string id, s, c, wt, info, absencja;
                    string absHint = null;
                    Tools.GetLineParams(dataItem, out id, out s, out c, out info, out wt, out absencja);
                    //----- no day - do raportów -----
                    bool noDay = !String.IsNullOrEmpty(info) && info.IndexOf(PlanPracy.maNoDay) >= 0;
                    //----- absencja -----
                    if (!String.IsNullOrEmpty(absencja))    //hint do absencji - odcinam symbol
                    {
                        int p = absencja.IndexOf('-');
                        if (p != -1)
                        {
                            if (p < absencja.Length)
                                absHint = absencja.Substring(p + 1).Trim();
                            absencja = absencja.Substring(0, p).Trim();
                        }
                    }
                    //----- holiday & infomarker ------
                    bool isInfo = false;
                    bool isAbsencja = false;
                    bool xisAlert = false;
                    string tdClass = null;
                    string divClass = null;
                    //----- day - cell info -----
                    if (!String.IsNullOrEmpty(info))
                    {
                        isInfo = info.IndexOf(PlanPracy.maInfo) >= 0;           //"i" -> div
                        if (isInfo) divClass = " info";

                        isAbsencja = info.IndexOf(PlanPracy.maAbsencja) >= 0;   //"u" -> td
                        if (isAbsencja) tdClass += " absencja";

                        if (info.IndexOf(PlanPracy.maAlert) >= 0)               //"A" -> div
                            divClass += " alert";

                        if (info.IndexOf(PlanPracy.maAlert2) >= 0)              //"B" -> div
                            divClass += " alert2";

                        if (info.IndexOf(PlanPracy.maAccepted) >= 0)            //"a" -> td
                            tdClass += " acc";

                        if (info.IndexOf(PlanPracy.maNoDay) >= 0)               //"x" -> td
                            tdClass += " noday";
                    }
                    //----- headerline info -----
                    if (e.Item.ItemIndex < LineHeader.HeaderData.Count())
                    {
                        string hinfo = LineHeader.HeaderData[e.Item.ItemIndex];
                        if (!String.IsNullOrEmpty(hinfo))
                        {
                            if (hinfo.IndexOf(PlanPracy.maHoliday) >= 0)
                                tdClass += " holiday";                              //"h" -> td
                            if (hinfo.IndexOf(PlanPracy.maToday) >= 0)
                                tdClass += " today";                                //"t" -> td
                        }
                    }
                    else
                    {
                        int x = 0;
                    }
                    //----- zmiana --------------------
                    Label lb = Tools.SetText(e.Item, "lbTop", s);
                    if (FMode == PlanPracy.moZmiany)
                    {
                        lb.ToolTip = wt;
                        wt = null;          //<<<< tymczas
                    }
                    Tools.SetText(e.Item, "lbBottom", wt);
                    lb = Tools.SetText(e.Item, "lbOverlap", absencja);
                    if (lb != null && isAbsencja) lb.ToolTip = absHint;
                    
                     /*
                    Label lb;
                    if (!String.IsNullOrEmpty(s))
                    {
                        lb = Tools.FindLabel(e.Item, "lbTop");
                        if (lb != null) lb.Text = s;
                    }
                    if (!String.IsNullOrEmpty(wt))
                    {
                        lb = Tools.FindLabel(e.Item, "lbBottom");
                        if (lb != null) lb.Text = wt;  // work + overtime
                    }
                    if (isAbsencja)
                    {
                        lb = Tools.FindLabel(e.Item, "lbOverlap");
                        if (lb != null)
                        {
                            lb.Text = absencja;
                            lb.ToolTip = absHint;
                        }
                    }
                    * / zz
          
          
                    if (!String.IsNullOrEmpty(c))  //zmiana
                    {
                        td.Attributes["style"] = String.Format("background-color: {0}; background-image: none;", c);
                        //td.Attributes["style"] = String.Format("background-image: none;", c);
                        //x div.Attributes["style"] = String.Format("border: solid 2px {0};", c);
                    }

                    HtmlGenericControl bck = (HtmlGenericControl)e.Item.FindControl("bckPP");
                    if (bck != null)
                        if (!String.IsNullOrEmpty(c))  //zmiana
                        {
                            bck.Attributes["style"] = String.Format("border-color: {0};", c);
                        }
                        else bck.Visible = false;

                        
                    switch (FMode)
                    {
                        default:
                        case PlanPracy.moZmiany:
                            PlanPracy pp = (PlanPracy)Parent.NamingContainer.Parent;
                            if (pp.EditMode)
                            {
                                if (Selected) tdClass += " selPrac";

                                RepeaterItem item = LineHeader.Repeater.Items[e.Item.ItemIndex];
                                CheckBox cb = (CheckBox)item.FindControl("cbDay");
                                if (cb != null && cb.Checked)
                                    tdClass += " selDay";
                                
                                div.Attributes["onclick"] = String.Format("javascript:selectCellPP(this,{0},{1},'{2}','{3}');",
                                    PracId, e.Item.ItemIndex, cntPrefix, hidZmiany.ClientID.Substring(cntPrefix.Length));
                            }
                            break;
                        case PlanPracy.moAccept:
                            div.Attributes["onclick"] = String.Format("javascript:selectCellPPAcc(this,{0},{1},'{2}');",
                                PracId, e.Item.ItemIndex, cntPrefix);
                            break;
                        case PlanPracy.moKartaRoczna:
                            //RepPlanPracy rpp = (RepPlanPracy)Parent.NamingContainer.Parent;
                            break;
                    }
                    //----- class ----------------
                    td.Attributes["class"] += tdClass;
                    div.Attributes["class"] += divClass;
                }
            }
        }
         */



        /*----- td wersja ------
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("tdPP");
                if (td != null)
                {
                    string dataItem = (string)e.Item.DataItem;
                    string id, s, c, wt, info;
                    Tools.GetLineParams(dataItem, out id, out s, out c, out wt, out info);
                    //----- holiday & infomarker ------
                    bool isInfo = false;
                    bool isAbsencja = false;
                    bool isAlert = false;
                    string css = null;
                    if (!String.IsNullOrEmpty(info))
                    {
                        isInfo = info.IndexOf("i") >= 0;
                        if (isInfo) css = " info";
                        isAbsencja = info.IndexOf("u") >= 0;
                        if (isAbsencja) 
                        {
                            s = "Abs";
                            c = "Aqua";
                        }
                        isAlert = info.IndexOf("A") >= 0;
                        if (isAlert) css += " alert";
                    }
                    string hinfo = LineHeader.HeaderData[e.Item.ItemIndex];
                    if (!String.IsNullOrEmpty(hinfo))
                    {
                        if (hinfo.IndexOf("h") >= 0)
                            if (isInfo && String.IsNullOrEmpty(id))
                                css = " holidayinfo";
                            else 
                                css += " holiday";
                        if (hinfo.IndexOf("t") >= 0) 
                            css += " today";
                    }
                    td.Attributes["class"] += css;
                    //----- zmiana --------------------
                    td.InnerHtml = s + (wt != null ? "<br />" + wt : null);
                    if (!String.IsNullOrEmpty(c))
                        if (isInfo)
                            td.Attributes["style"] = String.Format("background-color: {0};", c);
                        else
                            td.Attributes["style"] = String.Format("background-color: {0}; background-image: none;", c);

                    switch (FMode)
                    {
                        default:
                        case PlanPracy.moZmiany:
                            td.Attributes["onclick"] = String.Format("javascript:selectCellPP(this,{0},'{1}','{2}');", 
                                e.Item.ItemIndex, hidZmiany.ClientID, SelZmianaCtrlId);
                            break;
                        case PlanPracy.moAccept:
                            break;
                    }
                }
            }
        }
         */

        protected void Repeater1_PreRender(object sender, EventArgs e)
        {
            string[] zm = hidZmiany.Value.Split(',');
            Repeater1.DataSource = zm;
            Repeater1.DataBind();       // w Render binduje
        }
        //----- SUMY ----------
        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (SumyVisible)
                {
                    if (e.Item.ItemIndex == 0)
                    {
                        HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("tdSuma");
                        if (td != null)
                            Tools.AddClass(td, "suma1");
                    }

                    Label lb1 = (Label)e.Item.FindControl("lb1");
                    if (lb1 != null)
                    {
                        string dataItem = (string)e.Item.DataItem;
                        if (e.Item.ItemIndex == 0)
                        {
                            Label lb0 = (Label)e.Item.FindControl("lb0");
                            if (lb0 != null)
                            {
                                string[] ss = dataItem.Split(';');
                                int cnt = ss.Count();
                                if (cnt > 1)
                                {
                                    lb0.Text = ss[0];
                                    dataItem = ss[1];
                                }
                            }
                        }
                        lb1.Text = dataItem;
                    }
                }
            }
        }

        protected void Repeater2_PreRender(object sender, EventArgs e)
        {
            if (SumyVisible)
            {
                string[] sumy = Sumy.Split('|');
                Repeater2.DataSource = sumy;
                Repeater2.DataBind();       // w Render binduje
            }
        }
        //---------------
        public void SelectCell(int dayIndex, bool select)   // <<<<, będzie można tak upate zrobić bez odświeżania całości może ...
        {
            string[] days = Zmiany.Split(',');
            if (0 <= dayIndex && dayIndex < days.Count())
            {
                string[] par = days[dayIndex].Split('|');
                if (!select)
                    par[3] = par[3].Replace(PlanPracy._maSelected, "");
                else
                    if (!par[3].Contains(PlanPracy._maSelected))
                        par[3] += PlanPracy._maSelected;
                days[dayIndex] = String.Join("|", par);
                Zmiany = String.Join(",", days);
            }                
        }
        //---------------
        public string Zmiany
        {
            set { hidZmiany.Value = value; }
            get { return hidZmiany.Value; }
            //set { ViewState["zmiany"] = value; }
            //get { return Tools.GetStr(ViewState["zmiany"]); }
        }

        public string Sumy
        {
            set { hidSumy.Value = value; }
            get { return hidSumy.Value; }
            //set { ViewState["sumy"] = value; }
            //get { return Tools.GetStr(ViewState["sumy"]); }
        }

        public bool SumyVisible
        {
            set 
            {
                if (!value)
                {
                    Sumy = null;
                    //Repeater2.DataBind();
                }
            }
            get { return !String.IsNullOrEmpty(Sumy); }
        }

        /*
        public bool SumyVisible
        {
            set { ViewState[ID + "_SumyVisible"] = value; }
            get 
            {
                object sv = ViewState[ID + "_SumyVisible"];
                return sv == null ? true : (bool)sv;
            }
        }
        /**/
        public string PracId
        {
            set { hidPracId.Value = value; }
            get { return hidPracId.Value; }
            //set { ViewState["pracid"] = value; }
            //get { return Tools.GetStr(ViewState["pracid"]); }
        }

        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }

        public Repeater Repeater
        {
            get { return Repeater1; }
        }
    }
}










/*

ArrayList al = new ArrayList();
        al.Add("1");
        al.Add("2");
        al.Add("3");
rpt.DataSource = al;
        rpt.DataBind();



protected void Repeater1_OnItemDataBound(object sender, RepeaterItemEventArgs e) 
        { 
                // break from function if it is not Item or Alternate Item template 
                if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) 
                        return; 
 
 
                if (e.Item.ItemIndex > 0) 
                { 
                        // get current row 
                        DataRowView CurrentRow = (DataRowView)e.Item.DataItem; 
 
                        // get previous row 
                        Repeater r = (Repeater)sender; 
                        DataRowView PreviousRow = (DataRowView)r.Items[e.Item.ItemIndex - 1].DataItem; 
 
 
                        // this works 
                        Response.Write(CurrentRow["Viewed"].ToString()); 
 
                        // this throws an exception (PreviousRow is null, r is not null) 
                        // System.NullReferenceException: Object reference not set to an instance of an object. 
                        Response.Write(PreviousRow["Viewed"].ToString()); 
                } 
        }

*/