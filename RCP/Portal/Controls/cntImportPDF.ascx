<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntImportPDF.ascx.cs" Inherits="HRRcp.Portal.Controls.cntImportPDF" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div id="paImportPDF" runat="server" class="cntImportPDF">

            <asp:HiddenField ID="hidMode" runat="server" />  
    <style>
        .file{
            margin-bottom:7px;
        }

        .nav{
            padding:5px 5px;
        }

        .fileupload{
            padding-left:10px;
        }
    </style>

            <script type="text/javascript">
                function checkFile() {
                    /*fu = document.getElementById('');
                    if (fu != null) {
                        if (!fu.value) {
                            alert("Brak pliku do importu.");
                            return false;
                        }
                    }*/
                    return true;
                }
            </script>
           
            <div class="fileupload round5" runat="server" ID="div1" visible="false">                   
                  <div class="row">
                     <div class="col-4 col-sm-3">
                         <h6>W celu przetworzenia wszystkich plików oceny okresowej skopiuj pliki do katalogu <%=sciezkaOcena %> i naciśnij poniższy przycisk</h6>
                        <asp:Button ID="btPrzetwarzanieOcena" runat="server" CssClass="button btn-default nav" Text="Przetwarzaj" OnClick="btPrzetwarzanieOcena_Click" /><br />

                    </div>
                    <div class="col-4 col-sm-3">
                        <h6>W celu dodania pojedynczego pliku do oceny okresowej naciśnij przycisk Wybierz plik a następnie Importuj</h6>
                         <asp:FileUpload ID="FileUploadOcena" CssClass="file" runat="server" />
                        <asp:Button ID="btImportOcena" runat="server" CssClass="button btn-default nav" Text="Importuj" onclick="btImport_Click_Ocena" OnClientClick="javascript:return checkFile();" />
                    </div>
                 </div>
                       

                        
            </div>  
        
            <div class="fileupload round5" runat="server" ID="div2" visible="false">
                 <div class="row">
                     <div class="col-4 col-sm-3">
                         <h6>W celu przetworzenia wszystkich plików RMUA skopiuj pliki do katalogu <%=sciezkaRmua %> i naciśnij poniższy przycisk</h6>
                         <asp:Button ID="btPrzetwarzanieRMUA" runat="server" CssClass="button btn-default nav" Text="Przetwarzaj" OnClick="btPrzetwarzanieRMUA_Click" /> <br />
                     </div>
                     <div class="col-4 col-sm-3">
                          <h6>W celu dodania pojedynczego pliku do Dokumentów RMUA naciśnij przycisk Wybierz plik a następnie Importuj</h6>
                         <asp:FileUpload ID="FileUploadRMUA" CssClass="file" runat="server" />
                         <asp:Button ID="btImportRMUA" runat="server" CssClass="button btn-default nav" Text="Importuj" onclick="btImport_Click_RMUA" OnClientClick="javascript:return checkFile();" />
                    </div>
                </div>
           </div>     

            <div class="fileupload round5" runat="server" ID="div3" visible="false">
                <br />
                <!--<asp:Label runat="server" Text="Wybierz"/>-->
                <div class="row">
                     <div class="col-4 col-md-2">
                        <asp:Label ID="Label1" runat="server" Text="Wybierz date z listy "/><br />
                        <asp:DropDownList ID="DropDownListKwitek" CssClass="btn-default dropdown-toggle file form-control" runat="server"></asp:DropDownList>
                        <asp:FileUpload ID="FileUploadKwitek" CssClass="file" runat="server" />
                        <asp:Button ID="btImportKwitek" runat="server" CssClass="button btn-default nav" Text="Importuj" onclick="btImport_Click_Kwitek" OnClientClick="javascript:return checkFile();" /><br />
                    </div>
               </div>
            </div>
          
            <div class="fileupload round5" runat="server" ID="div4" visible="false">
                 <div class="row">
                     <div class="col-4 col-md-2">
                        <asp:Label runat="server" Text="Wybierz rok z listy"/><br />
                        <asp:DropDownList ID="DropDownListPIT" CssClass="btn-default dropdown-toggle file form-control" runat="server"></asp:DropDownList>
                        <asp:FileUpload ID="FileUploadPIT" CssClass="file" runat="server" />
                        <asp:Button ID="btImportPIT" runat="server" CssClass="button btn-default nav" Text="Importuj" onclick="btImport_Click_PIT" OnClientClick="javascript:return checkFile();" />
                </div>
                     </div>
                       <br />
            </div>   

</div>

