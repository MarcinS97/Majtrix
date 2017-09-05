<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Portal3.Master" CodeBehind="Dashboard.aspx.cs" Inherits="HRRcp.Portal.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="Scripts/Chart.min.js"></script>
    <script type="text/javascript" src="Scripts/Chart.bundle.min.js"></script>

    <style type="text/css">
        .dcard { padding: 16px 32px 32px 32px; background-color: #fff; border-radius: 4px; box-shadow: 0px 1px 2px rgba(0, 0, 0, 0.1); margin: 15px; float: left; }

        .first-chart { width: 40%; }
        .second-chart { width: 45%; }
        .chart { margin: 32px 0px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-dashboard">
        <div class="page-title">Dashboard</div>
        <div class="container wide" style="margin: 0 !important;">
            <div class="dcard first-chart">
                <h4>Analiza stanu zatrudnienia</h4>
                <canvas id="myChart" class="chart" width="400" height="200"></canvas>
            </div>


            <div class="dcard second-chart">
                <h4>Analiza absencji</h4>
                <canvas id="myChart2" class="chart" width="400" height="200"></canvas>
            </div>


            <div class="dcard third-chart">
                <h4>Struktura zatrudnienia</h4>
                <canvas id="myChart3" class="chart" width="400" height="200"></canvas>
            </div>

        </div>
    </div>


    <script type="text/javascript">
        var ctx = $("#myChart");

        var myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ['t41', 't42', 't43', 't44', 't45'],
                datasets: [{
                    label: 'Suma z ilości pracowników',
                    data: [952, 940, 930, 920, 940, 900],
                    backgroundColor:
                        'rgba(54, 162, 235, 0.2)',

                    borderColor:
                        'rgba(54, 162, 235, 1)',

                }]
            },
            options: {
                legend: {
                    position: 'bottom'
                }
            }
        });

        var ctx2 = $('#myChart2');

        var myChart2 = new Chart(ctx2, {
            type: 'line',
            data: {
                labels: ['t41', 't42', 't43', 't44', 't45'],
                datasets: [{
                    label: 'Średnia % z absencji chorobowych',
                    data: [7.5, 9, 9.1, 5.5, 5],
                    backgroundColor: 'transparent',
                    //backgroundColor: '#796AEE',
                    borderColor: '#FF6384'

                },
                {
                    label: 'Średnia % z absencji urlopowyh',
                    data: [8.5, 9, 8.5, 6, 8],
                    //backgroundColor: "rgba(75,192,192,0.4)",
                    backgroundColor: 'transparent',
                    borderColor: "rgba(75,192,192,1)",

                }]
            },
            options: {
                legend: {
                    position: 'bottom'
                }
            }
        });

        var ctx3 = $('#myChart3');

        var myChart3 = new Chart(ctx3, {
            type: 'pie',
            data: {
                labels: ['Podstawowe', 'Średnie', 'Wyższe'],
                datasets: [{
                    label: 'Średnia % z absencji chorobowych',
                    data: [10, 30, 60],
                    backgroundColor: [
                        "#FF6384",
                        "#36A2EB",
                        "#FFCE56"
                    ],
                    hoverBackgroundColor: [
                        "#FF6384",
                        "#36A2EB",
                        "#FFCE56"
                    ]

                }]
            },
            options: {
                legend: {
                    position: 'bottom'
                }
            }
        });




    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
