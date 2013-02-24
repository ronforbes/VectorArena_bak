<%@ Page Title="Web Console" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VectorArenaWebRole._Default" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="BodyContent">
    <h1>Vector Arena</h1>
    <h2>Web Console</h2>
    <ul id="messages" />
    <input type="text" id="message" autofocus />
    <input type="button" id="submit" value="Submit" />

    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="Scripts/jquery.signalR-1.0.0.min.js"></script>
    <script src="/signalr/hubs" type="text/javascript"></script>
    <script>
        $(function () {
            var connection = $.connection.gameHub;

            connection.client.Log = function (message) {
                $('#messages').append('<li>' + message + '</li>');
            };

            $.connection.hub.start().done(function () {
                $('#submit').click(function () {
                    connection.server.log($('#message').val());
                });
            });
        });
    </script>
</asp:Content>