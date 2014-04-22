<%@ Page Language="C#" MasterPageFile="~/MasterPages/PageExt.master" AutoEventWireup="true"
  CodeFile="CreateQuote.aspx.cs" Inherits="MetraNet.Quoting.CreateQuote" Title="MetraNet - Create quote"
  Culture="auto" UICulture="auto" %>

<%@ Register Assembly="MetraTech.UI.Controls" Namespace="MetraTech.UI.Controls" TagPrefix="MT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <asp:PlaceHolder ID="PlaceHolderPOJavaScript" runat="server"></asp:PlaceHolder> 
  <MT:MTTitle ID="CreateQuoteTitle" Text="Create Quote" runat="server" />
  <br />
  <%--<MTCDT:MTGenericForm ID="CreateQuoteForm" runat="server" TemplateName="CreateQuote" DataBinderInstanceName="MTDataBinder1" meta:resourcekey="BMEInstanceForm">
  </MTCDT:MTGenericForm>--%>
  <br />
  
  <MT:MTPanel ID="MTPanelQuoteParameters" runat="server" Text="Quote parameters" Collapsible="True"
    Collapsed="False" meta:resourcekey="MTPanelQuoteParametersResource">
    <MT:MTTextBoxControl ID="MTtbQuoteDescription" Label = "Quote Description" LabelWidth="200" runat="server"/>
    <MT:MTDatePicker AllowBlank="False" Enabled="True" HideLabel="False" ID="MTdpStartDate"
      Label="Start date" LabelWidth="200" meta:resourcekey="dpStartDateResource1" ReadOnly="False"
      runat="server"></MT:MTDatePicker>
    <MT:MTDatePicker AllowBlank="False" Enabled="True" HideLabel="False" ID="MTdpEndDate"
      Label="End date" LabelWidth="200" meta:resourcekey="dpEndDateResource1" ReadOnly="False"
      runat="server"></MT:MTDatePicker>
    <MT:MTCheckBoxControl ID="MTcbPdf" BoxLabel = "Generate PDF" runat="server" LabelWidth="200" meta:resourcekey="MTCheckBoxPdfResource" />
  </MT:MTPanel>
  <br />
  <MT:MTPanel ID="MTPanelQuoteAccounts" runat="server" Text="Accounts for quote" Collapsible="True"
    Collapsed="False" meta:resourcekey="MTPanelQuoteAccountsResource">
    <asp:PlaceHolder ID="PlaceHolderAccountsGrid" runat="server"></asp:PlaceHolder> 
     <br />GRID WITH ACCOUNT IDs QUOTE (for add account to the list uses MTInlineSearch component and grid rendered by JS like on ubscriptions\SetUDRCValues.aspx page)<br /> <br />
    <MT:MTCheckBoxControl BoxLabel="Is GroupSubscription" ID="MTcbIsGroupSubscription" Text = "Is GroupSubscription" runat="server" LabelWidth="200"  />
    <MT:MTDropDown ID="MTddCorporateAccount" Label = "Corporate Account" LabelWidth="200" runat="server" meta:resourcekey="MTDropDownCorporateAccountResource" />
    <%--<MT:MTInlineSearch ID="MTisAddAccount" runat="server" TabIndex="210" AllowBlank="False"
      Label="Add account to quote" LabelWidth="200" HideLabel="False" meta:resourcekey="tbAncestorAccountResource1"></MT:MTInlineSearch>--%>
  </MT:MTPanel>
  <MT:MTPanel ID="MTPanelProductOfferings" runat="server" Text="Product offerings for quote" Collapsible="True"
    Collapsed="False" meta:resourcekey="MTPanelProductOfferingsResource">
    <asp:PlaceHolder ID="PlaceHolderProductOfferingsGrid" runat="server">      
    </asp:PlaceHolder> 
    <div id="ProductOfferingsGrid"></div>
    <br />GRID WITH PO IDs and NAMES FOR QUOTE (for select PO uses code based on Subscriptions\SetUDRCValues.aspx page) <br /> <br />
  </MT:MTPanel>
  <MT:MTPanel ID="MTPanelUDRCMetrics" runat="server" Text="UDRC metrics for quote" Collapsible="True"
    Collapsed="False" meta:resourcekey="MTPanelUDRCResource">
     <asp:PlaceHolder ID="PlaceHolderUDRCMetricsGrid" runat="server"></asp:PlaceHolder> 
     <br />GRID WITH UDRC METRICS (GENERATED DYNAMICALLY ACCORDING TO SET OF POS ABOVE, uses code based on Subscriptions\SetUDRCValues.aspx page<br /> <br />
  </MT:MTPanel>
  <MT:MTPanel ID="MTPanelICBs" runat="server" Text="ICBs for quote" Collapsible="True"
    Collapsed="False" meta:resourcekey="MTPanelICBResource">
    <br />GRID WITH ICB METRICS. (most likely we should create a new screen for rates edit instead of existed ASP page for Rates similar to Subscriptions\SetUDRCValues.aspx< page<br /> <br />
  </MT:MTPanel>
  <div class="x-panel-btns-ct">
    <div style="width: 630px" class="x-panel-btns x-panel-btns-center">
      <div style="text-align: center;">
        <table>
          <tr>
            <td class="x-panel-btn-td">
              <MT:MTCheckBoxControl ID="MTCheckBoxControl1" BoxLabel = "View result" runat="server" LabelWidth="100" meta:resourcekey="MTCheckBoxPdfResource" />
            </td>
            <td class="x-panel-btn-td">
              <MT:MTButton ID="MTbtnGenerateQuote" runat="server" OnClick="btnGenerateQuote_Click"
                TabIndex="150" meta:resourcekey="btnGenerateQuoteResource1" />
            </td>
            <td class="x-panel-btn-td">
              <MT:MTButton ID="MTbtnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="False"
                TabIndex="160" meta:resourcekey="btnCancelResource1" />
            </td>
            
          </tr>
        </table>
      </div>
    </div>
  </div>  
</asp:Content>