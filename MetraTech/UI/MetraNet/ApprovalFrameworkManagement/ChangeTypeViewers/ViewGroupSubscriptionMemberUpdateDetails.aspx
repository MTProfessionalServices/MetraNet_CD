<%@ Page Language="C#" MasterPageFile="~/MasterPages/NoMenuPageExt.master" AutoEventWireup="true" Inherits="ApprovalFrameworkManagement_ViewGroupSubscriptionMemberUpdateDetails"
  Title="Group Subscription Membership Change Details" Culture="auto" UICulture="auto" CodeFile="ViewGroupSubscriptionMemberUpdateDetails.aspx.cs" meta:resourcekey="PageResource1" %>
  <%@ Import Namespace="MetraTech.UI.Tools" %>
  <%@ Register Assembly="MetraTech.UI.Controls" Namespace="MetraTech.UI.Controls" TagPrefix="MT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<%--  <div class="CaptionBar">
    <asp:Label ID="lblShowChangesSummaryTitle" runat="server" Text="Group Subscription Membership Change Details"
      meta:resourcekey="lblShowAllChangesSummaryTitleResource1"></asp:Label>
  </div>
  <br />--%>

    <MT:MTFilterGrid runat="Server" ID="GroupSubscriptionChangeDetails" ExtensionName="Core" 
    TemplateFileName="Approvals.GroupSubscriptionChangeDetails.xml" 
    ButtonAlignment="Center" Buttons="None" DefaultSortDirection="Ascending" 
    DisplayCount="True" EnableColumnConfig="True" EnableFilterConfig="True" 
    EnableLoadSearch="False" EnableSaveSearch="False" Expandable="False" 
    ExpansionCssClass="" Exportable="False" FilterColumnWidth="350" 
    FilterInputWidth="220" FilterLabelWidth="75" FilterPanelCollapsed="False" 
    FilterPanelLayout="MultiColumn" meta:resourcekey="GroupSubscriptionChangeDetailsResource1" 
    MultiSelect="False" NoRecordsText="No records found" PageSize="10" 
    Resizable="True" RootElement="Items" SearchOnLoad="True" 
    SelectionModel="Standard" ShowBottomBar="True" ShowColumnHeaders="True" 
    ShowFilterPanel="True" ShowGridFrame="True" ShowGridHeader="True" 
    ShowTopBar="True" 
    TotalProperty="TotalRows"></MT:MTFilterGrid>

    <script language="javascript" type="text/javascript">


 </script>
  </asp:Content>