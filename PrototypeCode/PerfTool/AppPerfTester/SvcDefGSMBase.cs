﻿using System;
using System.IO;
using MetraTech.DomainModel.Enums;
using MetraTech.DomainModel.Enums.GSM.Metratech_com_GSM;
using MetraTech.DomainModel.Enums.GSM.Metratech_com_GSMReference;

namespace BaselineGUI
{
    public class SvcDefGSMBase : SvcDefBase
    {
        public RecordType RecordType; // length:0;
        public string IMSI; // length:15;
        public string MSISDN; // length:15;
        public string CalledNumber; // length:20;
        public string DialedDigits; // length:20;
        public string OriginatingNetwork; // length:20;
        public string DestinationNetwork; // length:20;
        public DateTime CallEventStartTimestamp; // length:0;
        public int TotalCallEventDuration; // length:0;
        public int TotalUnits1; // length:0;
        public string UOM1; // length:20;
        public int TotalUnits2; // length:0;
        public string UOM2; // length:20;
        public string QoS; // length:20;
        public string HomeZoneIndicator; // length:20;
        public int HomeBID; // length:0;
        public string HomeLocationDesc; // length:255;
        public string HomeNDC; // length:20;
        public MobileStationClassmark MobileStationClassmark; // length:0;
        public TeleServiceCode TeleServiceCode; // length:0;
        public BearerServiceCode BearerServiceCode; // length:0;
        public FNUR FNUR; // length:0;
        public AIUR AIUR; // length:0;
        public string RateScenarioID1; // length:20;
        public string RateScenarioID2; // length:20;
        public string RateScenarioID3; // length:20;
        public string TariffID; // length:20;
        public double EndUserCharge; // length:0;
        public string ServiceDescription; // length:255;
        public string ContentProvider; // length:255;
        public string AccessPointName; // length:30;
        public double ScenarioRate; // length:0;
        public double AmountBilled; // length:0;
        public double AmountSettled; // length:0;
        public void init()
        {
            initEnum(ref RecordType);
            IMSI = "";
            MSISDN = "";
            CalledNumber = "";
            DialedDigits = "";
            OriginatingNetwork = "";
            DestinationNetwork = "";
            // CallEventStartTimestamp = DateTime;
            TotalCallEventDuration = 0;
            TotalUnits1 = 0;
            UOM1 = "";
            TotalUnits2 = 0;
            UOM2 = "";
            QoS = "";
            HomeZoneIndicator = "";
            HomeBID = 0;
            HomeLocationDesc = "";
            HomeNDC = "";
            initEnum(ref MobileStationClassmark);
            initEnum(ref TeleServiceCode);
            initEnum(ref BearerServiceCode);
            initEnum(ref FNUR);
            initEnum(ref AIUR);
            RateScenarioID1 = "";
            RateScenarioID2 = "";
            RateScenarioID3 = "";
            TariffID = "";
            EndUserCharge = 0.0;
            ServiceDescription = "";
            ContentProvider = "";
            AccessPointName = "";
            ScenarioRate = 0.0;
            AmountBilled = 0.0;
            AmountSettled = 0.0;
        }
        public void print(TextWriter writer)
        {
            writer.Write("{0}", EnumHelper.GetEnumEntryName(RecordType));
            writer.Write("|");
            writer.Write("{0}", IMSI);
            writer.Write("|");
            writer.Write("{0}", MSISDN);
            writer.Write("|");
            writer.Write("{0}", CalledNumber);
            writer.Write("|");
            writer.Write("{0}", DialedDigits);
            writer.Write("|");
            writer.Write("{0}", OriginatingNetwork);
            writer.Write("|");
            writer.Write("{0}", DestinationNetwork);
            writer.Write("|");
            writer.Write("{0}", FLS_ISO8601(CallEventStartTimestamp));
            writer.Write("|");
            writer.Write("{0}", TotalCallEventDuration);
            writer.Write("|");
            writer.Write("{0}", TotalUnits1);
            writer.Write("|");
            writer.Write("{0}", UOM1);
            writer.Write("|");
            writer.Write("{0}", TotalUnits2);
            writer.Write("|");
            writer.Write("{0}", UOM2);
            writer.Write("|");
            writer.Write("{0}", QoS);
            writer.Write("|");
            writer.Write("{0}", HomeZoneIndicator);
            writer.Write("|");
            writer.Write("{0}", HomeBID);
            writer.Write("|");
            writer.Write("{0}", HomeLocationDesc);
            writer.Write("|");
            writer.Write("{0}", HomeNDC);
            writer.Write("|");
            writer.Write("{0}", EnumHelper.GetEnumEntryName(MobileStationClassmark));
            writer.Write("|");
            writer.Write("{0}", EnumHelper.GetEnumEntryName(TeleServiceCode));
            writer.Write("|");
            writer.Write("{0}", EnumHelper.GetEnumEntryName(BearerServiceCode));
            writer.Write("|");
            writer.Write("{0}", EnumHelper.GetEnumEntryName(FNUR));
            writer.Write("|");
            writer.Write("{0}", EnumHelper.GetEnumEntryName(AIUR));
            writer.Write("|");
            writer.Write("{0}", RateScenarioID1);
            writer.Write("|");
            writer.Write("{0}", RateScenarioID2);
            writer.Write("|");
            writer.Write("{0}", RateScenarioID3);
            writer.Write("|");
            writer.Write("{0}", TariffID);
            writer.Write("|");
            writer.Write("{0}", EndUserCharge);
            writer.Write("|");
            writer.Write("{0}", ServiceDescription);
            writer.Write("|");
            writer.Write("{0}", ContentProvider);
            writer.Write("|");
            writer.Write("{0}", AccessPointName);
            writer.Write("|");
            writer.Write("{0}", ScenarioRate);
            writer.Write("|");
            writer.Write("{0}", AmountBilled);
            writer.Write("|");
            writer.Write("{0}", AmountSettled);
            writer.WriteLine();
        }
    }

}
