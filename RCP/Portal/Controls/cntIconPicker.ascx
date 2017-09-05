<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntIconPicker.ascx.cs" Inherits="HRRcp.Portal.Controls.cntIconPicker" %>

<style>
    .cntIconPicker {
    width: 270px;
    height: 230px;
    overflow-y: scroll;
    display: inline-block;
}

    .cntIconPicker .wrapper {
        width: 50px;
        height: 50px;
        text-align: center;
        border: solid 1px #ccc;
        border-radius: 6px;
        margin: 3px;
        display: inline-block;
        color: #000;
    }

        .cntIconPicker .wrapper:hover {
            cursor: pointer;
            background-color: #dedede;
        }

    .cntIconPicker .selected {
        background-color: #246;
        color: #fff !important;
        border: solid 1px #246;
    }

        .cntIconPicker .selected:hover {
            background-color: #468;
        }

    .cntIconPicker .wrapper a {
        font-size: 32px;
        line-height: 50px;
        width: 100%;
        color: inherit;
        text-decoration: none;
        vertical-align: top;
    }
</style>

<div id="ctIconPicker" runat="server" class="cntIconPicker">
    <asp:HiddenField ID="hidSelected" runat="server" />
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Repeater ID="rpIcons" runat="server">
                <ItemTemplate>
                    <div class='<%# "wrapper" + GetSelectedClass(Container.DataItem.ToString())  %>'>
                        <asp:LinkButton ID="lnkIcon" runat="server" CssClass='<%# "fa " + Container.DataItem.ToString() %>' />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel>    
</div>
