<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HFCDatePicker.ascx.cs" Inherits="Modules_HFCDatePicker" %>

<asp:TextBox runat="server" ID="txtDateTime" />
<asp:ImageButton runat="Server" ID="btnCalendar" ImageUrl="~/images/Calendar_scheduleHS.png" AlternateText="Calendar" />
<br />
<ajaxToolkit:CalendarExtender runat="server" ID="ajaxCalendar" TargetControlID="txtDateTime" PopupButtonID="btnCalendar" />