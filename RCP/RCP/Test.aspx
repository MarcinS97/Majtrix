<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="HRRcp.RCP.Test" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register Src="~/RCP/Controls/Adm/cntSchematy.ascx" TagPrefix="cc" TagName="Schematy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
       
        .tytul
        {
        text-align:center;
        font-size:30px;
        padding:20px;   
        }
        
        .panel
        {
        word-wrap:break-word;
        max-width:100%;
        }
        
        .panel:hover
        {
        cursor:pointer;
       
        }    
        
        
        
        .panel-info > .panel-heading
        {
            border-color: #3f51b5;
            color: #ffffff;
            background-color: #3f51b5;
        }  

        .panel-info
        {
            border-color: #3f51b5;
        }

        span
        {
            width:80%;
        }
        
        .panel-warning > .panel-heading
        {
            border-color: #016459;
            color: #ffffff;
            background-color: #016459;
        }  

        .panel-warning
        {
            border-color: #016459;
        }
       
        .panel-danger > .panel-heading
        {
            border-color: #098d7f;
            color: #ffffff;
            background-color: #098d7f;
        }  

        .panel-danger
        {
            border-color: #098d7f;
        }

        .prawa
        {
            float:right;
        }


        </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Test" SubText1="Formatka testowa" />

    <div class="tytul"> Przykladowe raporty</div>
    
    

    <div class="row">
        <div class="col-md-2 col-md-offset-1">
        <div class="panel panel-info">
        <div class="panel-heading"><span>tytul raportutytul raportutytul raportu tytul raportu</span><div class="prawa"><i class="glyphicon glyphicon-cog"></i></div></div>
        <div class="panel-body">
                lorem ipsum <br />
                lorem ipsum
        </div>
        </div>
    </div>

        <div class="col-md-2">
        <div class="panel panel-danger">
        <div class="panel-heading">tytul raportu</div>
        <div class="panel-body">
             lorem ipsum <br />
        lorem ipsum
        </div>
        </div>
        </div>

        <div class="col-md-2">
        <div class="panel panel-info">
        <div class="panel-heading">tytullorem ipsum lorem ipsum lorem ipsum  raportu</div>
        <div class="panel-body">
                lorem ipsum 
                lorem ipsum 
                lorem ipsum 
                lorem ipsum 

        </div>
        </div>
        </div>

        <div class="col-md-2">
        <div class="panel panel-warning">
        <div class="panel-heading">tytulrtutyturaportutytul  raportutytul raportu <div class="prawa"><i class="glyphicon glyphicon-home"></i></div></div>
        <div class="panel-body">
             lorem ipsum <br />
            lorem ipsumlorem ipsumlorem ipsumlorem ipsumlorem ipsumlorem ipsumlorem ipsumlorem ipsumlorem ipsumlorem ipsumlorem ipsum
        </div>
        </div>
        </div>

        <div class="col-md-2">
        <div class="panel panel-danger">
        <div class="panel-heading">tytul raportu</div>
        <div class="panel-body">
             lorem ipsum <br />
        lorem ipsum
        </div>
        </div>
        </div>
    </div> 



    <div class="tytul"> Przykladowe raporty</div>
    
    

    <div class="row">
        <div class="col-md-2 col-md-offset-1">
        <div class="panel panel-warning">
        <div class="panel-heading">tytul raportu</div>
        <div class="panel-body">
             lorem ipsum <br />
             lorem ipsum
        </div>
        </div>
        </div>

        <div class="col-md-2">
        <div class="panel panel-info">
        <div class="panel-heading">tytul raportu</div>
        <div class="panel-body">
             lorem ipsum <br />
        lorem ipsum
        </div>
        </div>
        </div>

        <div class="col-md-2">
        <div class="panel panel-warning">
        <div class="panel-heading">tytul raportu<div class="prawa"><i class="glyphicon glyphicon-remove-sign"></i></div></div>
        <div class="panel-body">
             lorem ipsum <br />
            lorem ipsum
        </div>
        </div>
        </div>

        <div class="col-md-2">
        <div class="panel panel-danger">
        <div class="panel-heading">tytul raportu</div>
        <div class="panel-body">
                 lorem ipsum <br />
            lorem ipsum
        </div>
        </div>
        </div>

        <div class="col-md-2">
        <div class="panel panel-info">
        <div class="panel-heading">tytul raportu</div>
        <div class="panel-body">
                 lorem ipsum 
            lorem ipsum 
            lorem ipsum 
            lorem ipsum 
            lorem ipsum 
            lorem ipsum
        </div>
        </div>
        </div>
    </div> 


    <div class="row">
        <div class="col-md-2 col-md-offset-1">
        <div class="panel panel-info">
        <div class="panel-heading">tytul raportu<div class="prawa"><i class="glyphicon glyphicon-tree-conifer"></i></div></div>
        <div class="panel-body">
             lorem ipsum <br />
             lorem ipsum
        </div>
        </div>
        </div>

        <div class="col-md-2">
        <div class="panel panel-warning">
        <div class="panel-heading">tytul raportu</div>
        <div class="panel-body">
                 lorem ipsum <br />
                 lorem ipsum
        </div>
        </div>
        </div>

        <div class="col-md-2">
        <div class="panel panel-danger">
        <div class="panel-heading">tytul raportu</div>
        <div class="panel-body">
                 lorem ipsum <br />
                 lorem ipsum
            lorem ipsum 
            lorem ipsum 
            lorem ipsum 
        </div>
        </div>
        </div>

        <div class="col-md-2">
        <div class="panel panel-info">
        <div class="panel-heading">tytul raportu</div>
        <div class="panel-body">
                 lorem ipsum <br />
            lorem ipsum
        </div>
        </div>
        </div>

        <div class="col-md-2">
        <div class="panel panel-danger">
        <div class="panel-heading">tytul raportu</div>
        <div class="panel-body">
                 lorem ipsum <br />
            lorem ipsum
        </div>
        </div>
        </div>
    </div> 




</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
