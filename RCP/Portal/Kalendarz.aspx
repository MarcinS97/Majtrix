<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Kalendarz.aspx.cs" Inherits="HRRcp.Portal.Kalendarz" %>

<%@ Register Src="~/Portal/Controls/cntKalendarz.ascx" TagPrefix="uc1" TagName="cntKalendarz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/fullcalendar.min.css" rel="stylesheet" />
    <script src="Scripts/jquery.min.js"></script>
    <script src="Scripts/moment.min.js"></script>
    <script src="Scripts/fullcalendar.min.js"></script>
    <script src="Scripts/jquery-ui-1.10.4.custom.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    

    <uc1:cntKalendarz runat="server" ID="cntKalendarz" />
    <script type="text/javascript">
        $(document).ready(function () {

            $('#calendar').fullCalendar({
                theme: false,
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,basicWeek,basicDay'
                },
                lang: "pl",
                eventClick: function (calEvent, jsEvent, view) {

                    //alert('Event: ' + calEvent.title);
                    //alert('Coordinates: ' + jsEvent.pageX + ',' + jsEvent.pageY);
                    //alert('View: ' + view.name);

                    // change the border color just for fun
                    //$(this).css('border-color', 'red');
                    $.ajax({
                        type: "GET",
                        url: window.location.pathname + "/EditEventModal",
                        data: { "eventId": calEvent.id },
                        success: function (result) {
                            //alert(result);
                            $("#modal").html(result);
                            $("#myModal").modal("show");
                        }
                    });


                },
                dayClick: function (date, jsEvent, view) {
                    $.ajax({
                        type: "GET",
                        url: window.location.pathname + "/AddEventModal",
                        data: { "startDate": date.format() },
                        success: function (result) {
                            //alert(result);
                            $("#modal").html(result);
                            $("#myModal").modal("show");
                        }
                    });




                    //alert('Clicked on: ' + date.format());

                    //alert('Coordinates: ' + jsEvent.pageX + ',' + jsEvent.pageY);

                    //alert('Current view: ' + view.name);

                    // change the day's background color just for fun
                    //$(this).css('background-color', 'red');

                },
                //firstDay: 1,
                //defaultDate: '2016-01-12',
                //editable: true,
                //eventLimit: true, // allow "more" link when too many events
                events: window.location.pathname + "/GetEvents"
            });

        });
</script>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
