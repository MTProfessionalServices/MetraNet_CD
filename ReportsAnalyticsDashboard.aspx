<%@ Page Language="C#" MasterPageFile="~/MasterPages/PageExt.master" AutoEventWireup="true"
  Inherits="ReportsAnalyticsDashboard" Title="MetraNet" CodeFile="ReportsAnalyticsDashboard.aspx.cs" %>

<%@ Register Assembly="MetraTech.UI.Controls.CDT" Namespace="MetraTech.UI.Controls.CDT"
  TagPrefix="MTCDT" %>
<%@ Register Assembly="MetraTech.UI.Controls" Namespace="MetraTech.UI.Controls" TagPrefix="MT" %>
<asp:Content ID="ReportsAnalyticsDashboardPage" ContentPlaceHolderID="ContentPlaceHolder1"
  runat="Server">
  <link rel="stylesheet" type="text/css" href="/Res/Styles/jquery.gridster.css">
  <link rel="stylesheet" type="text/css" href="/Res/Styles/dc.css">
  <script type="text/javascript" src="MetraControl/ControlCenter/js/D3Visualize.js"></script>
  <script type="text/javascript" src="/Res/JavaScript/jquery.min.js"></script>
  <script type="text/javascript" src="/Res/JavaScript/jquery.gridster.min.js"></script>
  <script type="text/javascript" src="/Res/JavaScript/crossfilter.min.js"></script>
  <script type="text/javascript" src="/Res/JavaScript/dc.min.js"></script>
  <script type="text/javascript" src="/Res/JavaScript/Renderers.js"></script>
  <MT:MTTitle ID="MTTitle1" Text="Finance Dashboard" runat="server" />
  <%--<div style="margin: 40px; border: 1px solid; padding: 15px 10px 15px 35px; background-repeat: no-repeat;
    background-position: 10px center; color: #9F6000; background-color: #FEEFB3; background-image: url('/Res/Images/icons/error.png');">
    Under development
  </div>--%>
  <br />
  <div class="CaptionBar remaining-graphs span8">
    <h2>
      Revenue</h2>
    <div>
      <p>
        Select currency:
        <select id="selRevenueCurrency">
        </select>
      </p>
    </div>
    <div class="row-fluid">
      <div id='RevenueChart' class="pie-graph span4 dc-chart" style="float: none !important;">
      </div>
    </div>
  </div>
  <div class="CaptionBar remaining-graphs span8">
    <h2>
      MRR</h2>
    <div>
      <p>
        Select currency:
        <select id="selMRRCurrency">
        </select>
      </p>
    </div>
    <div class="row-fluid">
      <div id='MRRChart' class="pie-graph span4 dc-chart" style="float: none !important;">
      </div>
    </div>
  </div>
  <div class="CaptionBar remaining-graphs span8">
    <h2>
      New Customers</h2>
    <div class="row-fluid">
      <div id='NewCustomersChart' class="pie-graph span4 dc-chart" style="float: none !important;">
      </div>
    </div>
  </div>
  <br />
  <script type="text/javascript">
  
      var MRRChart = dc.barChart("#MRRChart");
      var RevenueChart = dc.barChart("#RevenueChart");
      var MRRJSONData;
      var RevenueJSONData;

      var margin = {top: 10,right: 50,bottom: 30,left: 55},
          width = 920 - margin.left - margin.right,
          height = 340 - margin.top - margin.bottom,
          paddingX = 10,
          previousMonth = getUTCDate(new Date(<%= previousMonth %>)),
          firstMonth = getUTCDate(new Date(<%= firstMonth %>));
          

    function SetCurrencyOptions(JSONData, selectId, currencyFieldName)
    {
      var currencies = [];
      $.each(JSONData, function(){
    	  if ($.inArray(this[currencyFieldName],currencies) === -1) {
    		  currencies.push(this[currencyFieldName]);
    	  }
      });

      for (var i=0; i<currencies.length; i++)
      {
        $("#"+selectId).append($('<option>', {
                                              value: i,
                                              text:currencies[i]
                                              }));
      }
    }

    $(function () {
      $("#selMRRCurrency").change(function() {
          var currentCurrency = $( "#selMRRCurrency option:selected" ).text();
          RenderMRRChart(MRRJSONData);
          dc.redrawAll();
        });

        $("#selRevenueCurrency").change(function() {
          var currentCurrency = $( "#selRevenueCurrency option:selected" ).text();
          RenderRevenueChart(RevenueJSONData);
          dc.redrawAll();
        });

      getRevenue();
      getMRR();
      getNewCustomers();
    });
    
    

    function getUTCDate(dateForConvert) {
      return new Date(dateForConvert.getUTCFullYear(), dateForConvert.getUTCMonth(), dateForConvert.getUTCDate(),dateForConvert.getUTCHours(), dateForConvert.getUTCMinutes(), dateForConvert.getUTCSeconds());
    }

    function getRevenue() {
      $.ajax({
        type: 'GET',
        async: true,
        url: 'Report/Revenue',
        success: function(data) {
          RevenueJSONData = data;
          SetCurrencyOptions(RevenueJSONData, "selRevenueCurrency", "Currency");
          RenderRevenueChart(RevenueJSONData);

          RevenueChart.render();
          dc.redrawAll();
        },
        error: function () {
          alert("Error getting Data");
        }
      });
    };

    function RenderRevenueChart(JSONData) {

      var currentCurrency = $( "#selRevenueCurrency option:selected" ).text();

      var ndx = crossfilter(JSONData),
          runDimension = ndx.dimension(function(d) {return new Date(parseInt(d.Date.substr(6))); });
          /*usdGroup = runDimension.group().reduceSum(function(d) {if (d.Currency === 'USD') {return d.Amount;} else {return 0;}}),
          cadGroup = runDimension.group().reduceSum(function(d) {if (d.Currency === 'CAD') {return d.Amount;} else {return 0;}}),
          eurGroup = runDimension.group().reduceSum(function(d) {if (d.Currency === 'EUR') {return d.Amount;} else {return 0;}}),
          yenGroup = runDimension.group().reduceSum(function(d) {if (d.Currency === 'YEN') {return d.Amount;} else {return 0;}});*/
          currencyGroup = runDimension.group().reduceSum(function(d) {if (d.Currency == currentCurrency) return d.Amount;return 0;});

      RevenueChart.width(width)
        .height(height)
        .margins(margin)
        .x(d3.time.scale().domain([firstMonth, previousMonth]))
        .round(d3.time.month.round)
        .xUnits(d3.time.months)
        .brushOn(false)
        .title(function(d){return d.value;})
        .xAxisLabel("Months")
        .yAxisLabel("Amount")
        .elasticY(true)
        //.elasticX(true)
        .xAxisPadding(paddingX)
        .dimension(runDimension)
        .group(currencyGroup, currentCurrency)
        /*.stack(cadGroup, "CAD")
        .stack(eurGroup, "EUR")
        .stack(yenGroup, "YEN")*/
        .barPadding(0.1)
        .outerPadding(0.05)
        .transitionDuration(1500)
        .centerBar(true)
        .gap(15)
        .legend(dc.legend().x(750).y(0))
        .xAxis().tickFormat(d3.time.format("%b-%Y"));
    }

    function getMRR() {
      $.ajax({
        type: 'GET',
        async: true,
        url: 'Report/MRRByProduct',
        success: function(data) {
          MRRJSONData = data;
          SetCurrencyOptions(MRRJSONData, "selMRRCurrency", "CurrencyCode");
          RenderMRRChart(MRRJSONData);

          MRRChart.render();
          dc.redrawAll();
        },
        error: function() {
        }
      });
    };

    function RenderMRRChart(JSONData) {

      var currentCurrency = $( "#selMRRCurrency option:selected" ).text();

      var ndx = crossfilter(JSONData);

      var startMonthMRR = getUTCDate(new Date(<%= startMonthMRR %>));
      var endMonthMRR = getUTCDate(new Date(<%= endMonthMRR %>));

      var mrrValue = ndx.dimension(function(d) {
        return new Date(parseInt(d.Date.substr(6)));
      });
      
      var currencyGroup = mrrValue.group().reduceSum(function(d) {
        if (d.CurrencyCode == currentCurrency) return d.Amount;
        return 0;
      });

      MRRChart.width(width)
        .height(height)
        .dimension(mrrValue)
        .group(currencyGroup, currentCurrency)
        .transitionDuration(350)
        .margins(margin)
        .centerBar(true)
        .gap(10)
        .round(d3.time.month.round)
        .x(d3.time.scale().domain([startMonthMRR, endMonthMRR]))
        .brushOn(false)
        .title(function(d){return d.value;})
        .xAxisLabel("Months")
        .yAxisLabel("Amount")
        .elasticY(true)
        .xUnits(d3.time.months)
        .legend(dc.legend().x(750).y(0))
        .xAxis().tickFormat(d3.time.format("%b-%Y"));
    }

    function getNewCustomers() {
      $.ajax({
        type: 'GET',
        async: true,
        url: 'Report/NewCustomers',
        success: function (data) {
          RenderNewCustomersChart(data);

          dc.renderAll();
          dc.redrawAll();
        },
        error: function () {
          alert("Error getting Data");
        }
      });
    };

    function RenderNewCustomersChart(JSONData) {
      var volumeChart = dc.barChart("#NewCustomersChart");
      var ndx = crossfilter(JSONData);

      var startValue = ndx.dimension(function(d) {
        return new Date(parseInt(d.Date.substr(6)));
      });
      var startValueGroup = startValue.group();

      volumeChart.width(width)
        .height(height)
        .dimension(startValue)
        .group(startValueGroup, "New Customers")
        .transitionDuration(1000)
        .margins(margin)
        .centerBar(true)
        .gap(15)
        .round(d3.time.month.round)
        .x(d3.time.scale().domain([firstMonth, previousMonth]))
        .brushOn(false)
        .title(function(d){return d.value;})
        .xAxisLabel("Months")
        .yAxisLabel("Customers")
        .xUnits(d3.time.months)
        .legend(dc.legend().x(680).y(0))
        .elasticY(true)
        .xAxis().tickFormat(d3.time.format("%b-%Y"));
    }

  </script>
</asp:Content>
