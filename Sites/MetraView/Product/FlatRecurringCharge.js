﻿OverrideRenderer_ctl00_ContentPlaceHolder1_MTFilterGrid1 = function(cm) {

}

BeforeExpanderRender_ctl00_ContentPlaceHolder1_MTFilterGrid1 = function(tplString) {
  var xTemplate = "";

  // Render proration details
  xTemplate += '<div id="ProrationDetails"><div id="ProrationDetails{SessionID}"></div></div>';

  tplString = xTemplate;
  return tplString;
};


function reformatDate(date, addDays) {
  var parsedDate = new Date();
  parsedDate = Date.parseDate(date, EXTJS_DATE_LONG_FORMAT);
  parsedDate = parsedDate.add(Date.DAY, addDays);
  return parsedDate.format(EXTJS_DATE_FORMAT);
}

// On expand get proration details
onExpand = function (record) {

  var proratedRecurringChargeMessage = TEXT_NO_PRORATION;

  if (record.data.ProratedOnSubscription == 1 || record.data.ProratedOnUnsubscription == 1 || record.data.ProratedInstantly == 1) {
    if (record.data.DisplayAmount >= 0) {
      proratedRecurringChargeMessage = TEXT_PRORATED_RECURRING_CHARGE_MESSAGE;
    }
    else {
      proratedRecurringChargeMessage = TEXT_PRORATED_RECURRING_CHARGE_CREDIT_MESSAGE;
    }
  }
  else {
    if (record.data.DisplayAmount < 0) {
      proratedRecurringChargeMessage = TEXT_RECURRING_CHARGE_CREDIT;
    }
  }

  // Replace values
  var str = proratedRecurringChargeMessage.replace("[ProratedIntervalStartNextDay]", reformatDate(record.data.ProratedIntervalStart,1));
  str = str.replace("[ProratedIntervalEnd]", reformatDate(record.data.ProratedIntervalEnd,0));
  str = str.replace("[ProratedDays]", record.data.ProratedDays);
  str = str.replace("[ProratedDays]", record.data.ProratedDays);
  str = str.replace("[ProratedDailyRate]", record.data.ProratedDailyRate);
  str = str.replace("[ProratedDailyRate]", record.data.ProratedDailyRate);
  str = str.replace("[Amount]", record.data.DisplayAmountAsString);
  str = str.replace("[RCAmount]", record.data.RCAmount);
  str = str.replace("[RCAmount]", record.data.RCAmount);
  str = str.replace("[RCIntervalStart]", reformatDate(record.data.RCIntervalStart,0));
  str = str.replace("[ProratedIntervalStart]", reformatDate(record.data.ProratedIntervalStart,0));
  str = str.replace("[RCIntervalSubscriptionStart]", reformatDate(record.data.RCIntervalSubscriptionStart,0));
  str = str.replace("[RCIntervalSubscriptionEnd]", reformatDate(record.data.RCIntervalSubscriptionEnd,0));

  // Insert string
  var el = Ext.fly("ProrationDetails" + record.data.SessionID);
  el.dom.innerHTML = str;
};
