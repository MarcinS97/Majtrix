<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntKiosSlider.ascx.cs" Inherits="HRRcp.Controls.Portal.cntKiosSlider" %>

<div id="paKioskSlider" runat="server" class="cntKioskSlider">

    <div id="content1">
        <div id="slideshow" class="center1280">
            <div class="sliderimage">
                <table class="table0">
                    <tr>
                        <td>
                            <h3>Pomysł!</h3>
                            <ul>
                                <li>Masz pomysł na usprawnienie procesu produkcji ?</li>
                                <li>Wymyśl co można byłoby poprawić</li>
                                <li>To Ci się opłaci!</li>
                            </ul>
                        </td>
                        <td class="image">
                            <asp:Image ID="Image1" ImageUrl="~/images/icons/sl_pomysl.png" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="sliderimage">
                <table class="table0">
                    <tr>
                        <td>
                            <h3>Złóż wniosek</h3>
                            <ul>
                                <li>Opisz swoje rozwiąznie lub Twojego zespołu</li>
                                <li>Określ udział % członków zespołu</li>
                                <li>Podaj obszary funkcjonowania</li>
                                <li>Wyślij wniosek do weryfikacji</li>
                            </ul>
                        </td>
                        <td class="image">
                            <asp:Image ID="Image2" ImageUrl="~/images/icons/sl_zloz_wniosek.png" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="sliderimage">
                <table class="table0">
                    <tr>
                        <td>
                            <h3>Weryfikacja</h3>
                            <ul>
                                <li>Poczekaj na opinię przełożonych i technologów</li>
                                <li>Wyjaśnij i uzupełnij dane</li>
                                <li>Przyjmij punkty za złożenie wniosku</li>
                            </ul>
                        </td>
                        <td class="image">
                            <asp:Image ID="Image3" ImageUrl="~/images/icons/sl_weryfikacja.png" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="sliderimage">
                <table class="table0">
                    <tr>
                        <td>
                            <h3>Zbierz punkty za każdy z etapów</h3>
                            <ul>
                                <li>Złożenie wniosku</li>
                                <li>Weryfikacja formalna</li>
                                <li>Realizacja rozwiązania</li>
                                <li>Rozliczenie</li>
                            </ul>
                        </td>
                        <td class="image">
                            <asp:Image ID="Image4" ImageUrl="~/images/icons/sl_punkty.png" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="sliderimage">
                <table class="table0">
                    <tr>
                        <td>
                            <h3>Wymień punkty na nagrody</h3>
                            <ul>
                                <li>Wybierz produkt, który chcesz otrzymać za posiadane punkty</li>
                                <li>Zamów</li>
                                <li>Czekaj na informację od Administratora</li>
                                <li>Odbierz nagrodę</li>
                            </ul>
                        </td>
                        <td class="image">
                            <asp:Image ID="Image5" ImageUrl="~/images/icons/sl_odbierz_nagrody.png" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="scontrolwrapper" >
            <div id="scontrol" class="center1280" >
                <div id="slide1" class="sliderbutton">
                    <div class="first">
                    Krok 1<br />
                    Pomysł!
                    </div>
                </div>
                <div id="slide2" class="sliderbutton">
                    <div class="middle">
                    Krok 2<br />
                    Złóż wniosek
                    </div>
                </div>
                <div id="slide3" class="sliderbutton">
                    <div class="middle">
                    Krok 3<br />
                    Weryfikacja
                    </div>
                </div>
                <div id="slide4" class="sliderbutton">
                    <div class="middle">
                    Krok 4<br />
                    Zbierz punkty
                    </div>
                </div>
                <div id="slide5" class="sliderbutton">
                    <div class="last">
                    Krok 5<br />
                    Wymień punkty na nagrody
                    </div>
                </div>
                <script type="text/javascript">
                    $('#slide1').click(function() { $('#slideshow').cycle(0); return false; });
                    $('#slide2').click(function() { $('#slideshow').cycle(1); return false; });
                    $('#slide3').click(function() { $('#slideshow').cycle(2); return false; }); 
                    $('#slide4').click(function() { $('#slideshow').cycle(3); return false; });
                    $('#slide5').click(function() { $('#slideshow').cycle(4); return false; });
                </script>
            </div>
        </div>
    </div>
    <div id="content2">
        <div id="loginpanel" class="center1280">
            <table class="table0">
                <tr>
                    <td class="col1">
                        <div>
                            <span>aby się zalogować, przyłóż swoją kartę identyfikacyjną do czytnika</span><br />
                            <span>możesz również podać swój numer<br />ewidencyjny i hasło, otrzymane u Administratora</span><br />
                            <span class="lastrow">zamiast myszki - dotknij ekran</span>       
                        </div>
                    </td>
                    <td id="tdPracLogin1" runat="server" class="col2">
                        <asp:HiddenField ID="hidUniqueId" runat="server" />
                        <input type="hidden" id="login" name="login"/>
                        <input type="hidden" id="pass" name="pass"/>
                        <div class="line1">
                            <span id="lblogin">numer ewidencyjny</span> 
                            <span id="erlogin">podaj numer ewidencyjny</span>
                        </div>
                        <asp:TextBox ID="tbLogin" runat="server" MaxLength="30" AutoCompleteType="Disabled"></asp:TextBox>
                        <div class="line2">
                            <span id="lbpass">hasło</span> 
                            <span id="erpass">podaj hasło</span>
                        </div>
                        <asp:TextBox ID="tbPass" runat="server" MaxLength="30" TextMode="Password" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                    <td id="tdPracLogin2" runat="server" class="col3">
                        <asp:Button ID="tbtClear" CssClass="tbtClear" runat="server" Text="Czyść" OnClientClick="javascript:clearClick();" Visible="false"/>
                        <asp:Button ID="tbtLogin" CssClass="tbtLogin" runat="server" Text="Zaloguj" OnClientClick="javascript:loginClick();return false;"/>
                    </td>
                    <td id="tdKierLogin" runat="server" class="col4" visible="false">
                        <div class="info">
                            <asp:Label ID="lbWitaj" CssClass="label" runat="server" Text="witaj:"></asp:Label>
                            <asp:Label ID="lbUser" CssClass="user" runat="server" Text="Nieznajomy"></asp:Label><br />
                            <asp:Label ID="lbInfoKier" runat="server" Text="zaloguj się do Panelu Przełożonego"></asp:Label>
                            <asp:Label ID="lbInfoAdm" runat="server" Text="zaloguj się do Panelu Administratora" Visible="false"></asp:Label>
                            <br />
                            albo poznaj Panel Pracownika
                        </div>
                        <div class="buttons">
                            <asp:Button ID="tbtEnter" CssClass="tbtLogin" runat="server" Text="Zaloguj" OnClick="tbtEnter_Click"/>
                            <asp:Button ID="tbtTest" CssClass="tbtLogin tbtTest" runat="server" Text="Zaloguj jako<br />Użytkownik Testowy" OnClick="tbtTest_Click" />
                        </div>
                    </td>
                </tr>
            </table>            
        </div>
    </div>

</div>