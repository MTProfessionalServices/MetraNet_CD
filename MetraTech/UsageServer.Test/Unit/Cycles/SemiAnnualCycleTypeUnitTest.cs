﻿using System;
using System.Linq;
using MetraTech.TestCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetraTech.UsageServer.Test.Unit.Cycles
{
  [TestClass]
  public class SemiAnnualCycleTypeUnitTest
  {
    private DateTime _start;
    private DateTime _end;
    private DateTime _refDate;
    private readonly ICycleType _cycleType = new SemiAnnualCycleType();

    [TestMethod, MTUnitTest]
    public void ComputeStartAndEndDateWhenCycleStartDateIsJan01AndEqualRefDateTest()
    {
      _refDate = new DateTime(2013, 01, 01);
      var cycle = new Cycle { StartDay = 1, StartMonth = 1 };
      _cycleType.ComputeStartAndEndDate(_refDate, cycle, out _start, out _end);

      Assert.AreEqual(new DateTime(2013, 01, 01), _start);
      Assert.AreEqual(new DateTime(2013, 06, 30), _end);
    }

    [TestMethod, MTUnitTest]
    public void ComputeStartAndEndDateWhenCycleStartDateJan01AndRefDateIsCycleEndDateTest()
    {
      _refDate = new DateTime(2013, 06, 30);
      var cycle = new Cycle { StartDay = 1, StartMonth = 1 };
      _cycleType.ComputeStartAndEndDate(_refDate, cycle, out _start, out _end);

      Assert.AreEqual(new DateTime(2013, 01, 01), _start);
      Assert.AreEqual(new DateTime(2013, 06, 30), _end);
    }

    [TestMethod, MTUnitTest]
    public void ComputeStartAndEndDateWhenRefDateBeforeCycleStartDateTest()
    {
      _refDate = new DateTime(2012, 12, 31);
      var cycle = new Cycle { StartDay = 1, StartMonth = 1 };
      _cycleType.ComputeStartAndEndDate(_refDate, cycle, out _start, out _end);

      Assert.AreEqual(new DateTime(2012, 07, 01), _start);
      Assert.AreEqual(new DateTime(2012, 12, 31), _end);
    }

    [TestMethod, MTUnitTest]
    public void ComputeStartAndEndDateWhenRefDateAfterCycleEndDateTest()
    {
      _refDate = new DateTime(2013, 07, 01);
      var cycle = new Cycle { StartDay = 1, StartMonth = 1 };
      _cycleType.ComputeStartAndEndDate(_refDate, cycle, out _start, out _end);

      Assert.AreEqual(new DateTime(2013, 07, 01), _start);
      Assert.AreEqual(new DateTime(2013, 12, 31), _end);
    }

    [TestMethod, MTUnitTest]
    public void ComputeStartAndEndDateShouldReturnSameIntervalAsIfCycleStartDateJan01Test()
    {
      _refDate = new DateTime(2013, 06, 30);
      var cycle = new Cycle { StartDay = 1, StartMonth = 7 };
      _cycleType.ComputeStartAndEndDate(_refDate, cycle, out _start, out _end);

      Assert.AreEqual(new DateTime(2013, 01, 01), _start);
      Assert.AreEqual(new DateTime(2013, 06, 30), _end);
    }

    [TestMethod, MTUnitTest]
    public void ComputeStartAndEndDateWhenCycleStartDateDec31AndEqualRefDateTest()
    {
      _refDate = new DateTime(2013, 12, 31);
      var cycle = new Cycle { StartDay = 31, StartMonth = 12 };
      _cycleType.ComputeStartAndEndDate(_refDate, cycle, out _start, out _end);

      Assert.AreEqual(new DateTime(2013, 12, 31), _start);
      Assert.AreEqual(new DateTime(2014, 06, 29), _end);
    }

    [TestMethod, MTUnitTest]
    public void ComputeStartAndEndDateWhenCycleStartDateDec31AndRefDateBeforeStartDateTest()
    {
      _refDate = new DateTime(2013, 12, 30);
      var cycle = new Cycle { StartDay = 31, StartMonth = 12 };
      _cycleType.ComputeStartAndEndDate(_refDate, cycle, out _start, out _end);

      Assert.AreEqual(new DateTime(2013, 06, 30), _start);
      Assert.AreEqual(new DateTime(2013, 12, 30), _end);
    }

    [TestMethod, MTUnitTest]
    public void ComputeStartAndEndDateWhenCycleStartDateFeb28Test()
    {
      _refDate = new DateTime(2013, 09, 30);
      var cycle = new Cycle { StartDay = 28, StartMonth = 2 };
      _cycleType.ComputeStartAndEndDate(_refDate, cycle, out _start, out _end);

      Assert.AreEqual(new DateTime(2013, 08, 28), _start);
      Assert.AreEqual(new DateTime(2014, 02, 27), _end);
    }

    [TestMethod, MTUnitTest]
    public void ComputeStartAndEndDateShouldThrowExceptionWhenCycleStartDateFeb29Test()
    {
      _refDate = new DateTime(2013, 09, 30);
      var cycle = new Cycle { StartDay = 29, StartMonth = 2 };
      
      ExceptionAssert.Expected<ArgumentException>(
        () => _cycleType.ComputeStartAndEndDate(_refDate, cycle, out _start, out _end),
        "StartDay 29 must be a valid day of month");
    }

    [TestMethod, MTUnitTest]
    public void GenerateCyclesTest()
    {
      var cycles = _cycleType.GenerateCycles();

      // Should be 365 semi-annual cycles
      Assert.AreEqual(365, cycles.Length);

      foreach (var cycle in cycles)
      {
        //Validate required cycle properties
        Assert.IsTrue(cycle.StartMonth >= 1 && cycle.StartMonth <= 12);
        Assert.IsTrue(cycle.StartDay >= 1 && cycle.StartDay <= DateTime.DaysInMonth(1999, cycle.StartMonth));
        Assert.AreEqual(cycle.CycleType, CycleType.SemiAnnual);
        //Cycle's properties listed below should not be set
        Assert.AreEqual(cycle.StartYear, -1);
        Assert.AreEqual(cycle.DayOfMonth, -1);
        Assert.AreEqual(cycle.DayOfWeek, DayOfWeek.Monday);
        Assert.AreEqual(cycle.DayOfYear, -1);
        Assert.AreEqual(cycle.FirstDayOfMonth, -1);
        Assert.AreEqual(cycle.SecondDayOfMonth, -1);
        //Validate that current cycle is uniqe in the collection of cycles
        // ReSharper disable ReturnValueOfPureMethodIsNotUsed
        cycles.Single(c => c.StartMonth == cycle.StartMonth && c.StartDay == cycle.StartDay);
        // ReSharper restore ReturnValueOfPureMethodIsNotUsed
      }
    }
  }
}