﻿using System;
using System.Collections.Generic;
using System.Linq;
using MetraTech.DataAccess;
using MetraTech.Domain.DataAccess;
using MetraTech.Domain.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetraTech.Domain.Test.DataAccess
{
  /// <summary>
  /// Summary description for NotificationConfigurationTest
  /// </summary>
  [TestClass]
  public class NotificationConfigurationTest
  {
    #region Additional test attributes

    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //

    #endregion

    [TestMethod]
    [TestCategory("FunctionalTest")]
    public void SaveNotificationConfigurationTest()
    {
      var notificationEndpoint = EntityFactory.CreateTestNotificationEndpoint();
      var notificationConfiguration = EntityFactory.CreateTestNotificationConfiguration(notificationEndpoint,
          EmailTemplates.ChangeApprovedTemplateSubject, EmailTemplates.ChangeApprovedTemplateBody,
          "event.SubmitterEmail", "\"en-us\"", "ChangeNotificationEvent", "Approval Notification");
      NotificationConfiguration notificationConfigurationFromDb;
      var connectionInfo = new ConnectionInfo("NetMeter");
      var connection = ConnectionBase.GetDbConnection(connectionInfo, false);

      using (var db = new MetraNetContext(connection))
      {
        db.Entities.Add(notificationConfiguration);
        db.SaveChanges();
        notificationConfigurationFromDb = db.NotificationConfigurations.SingleOrDefault(x => x.EntityId == notificationConfiguration.EntityId);
      }

      Assert.IsNotNull(notificationConfigurationFromDb, "notificationConfiguration not loaded from DB");
      CompareNotificationConfigurations(notificationConfiguration, notificationConfigurationFromDb);
    }

    private static void CompareNotificationConfigurations(NotificationConfiguration notificationConfiguration, NotificationConfiguration notificationConfigurationFromDb)
    {
      Comparers.CompareDictionaries(notificationConfiguration.Name, notificationConfigurationFromDb.Name, "Name");
      Comparers.CompareDictionaries(notificationConfiguration.Description, notificationConfigurationFromDb.Description, "Description");
      Assert.AreEqual(notificationConfiguration.CreationDate, notificationConfigurationFromDb.CreationDate, "CreationDate");
      Assert.AreEqual(notificationConfiguration.ExternalId, notificationConfigurationFromDb.ExternalId, "ExternalId");
      Assert.AreEqual(notificationConfiguration.EventType, notificationConfigurationFromDb.EventType, "EventType");
      Assert.AreEqual(notificationConfiguration.ModifiedDate, notificationConfigurationFromDb.ModifiedDate, "ModifiedDate");
      Assert.AreEqual(notificationConfiguration.NotificationEndpoint.EntityId, 
        notificationConfigurationFromDb.NotificationEndpointEntityId, "NotificationEndpointEntityId");
      Assert.AreEqual(notificationConfiguration.NotificationType, notificationConfigurationFromDb.NotificationType, "NotificationType");

      CompareMessageTemplate(notificationConfiguration.MessageTemplate, notificationConfigurationFromDb.MessageTemplate);
    }

    private static void CompareMessageTemplate(MessageTemplate messageTemplate1, MessageTemplate messageTemplate2)
    {
      var emailTemplate1 = messageTemplate1 as EmailTemplate;
      if (emailTemplate1 != null)
      {
        var emailTemplate2 = messageTemplate2 as EmailTemplate;
        Assert.IsNotNull(emailTemplate2, "EmailTemplates have different types");

        Assert.AreEqual(emailTemplate1.ToRecipient, emailTemplate2.ToRecipient, "EmailTemplate.ToRecipient");
        Assert.AreEqual(emailTemplate1.DeliveryLanguage, emailTemplate2.DeliveryLanguage, "EmailTemplate.DeliveryLanguage");
        CompareEmailTemplateDictionary(emailTemplate1.EmailTemplateDictionary, emailTemplate2.EmailTemplateDictionary, "EmailTemplate.EmailTemplateDictionary");
        Comparers.CompareStringArrays(emailTemplate1.CarbonCopyRecipients, emailTemplate2.CarbonCopyRecipients, "EmailTemplate.CarbonCopyRecipients");
        return;
      }
      Assert.Fail("Unsupported EmailTemplate type");
    }

    private static void CompareEmailTemplateDictionary(EmailTemplateDictionary dictionary1, EmailTemplateDictionary dictionary2, string propertyName)
    {
      if (dictionary1 == null && dictionary2 == null)
        return;

      if (dictionary1 == null || dictionary2 == null || (dictionary1.Count != dictionary2.Count))
        Assert.Fail(propertyName + " have deferent element count");

      foreach (var key in dictionary1.Keys)
      {
        if (!dictionary2.ContainsKey(key))
          Assert.Fail("{0}. Key {1} not found in the second array.", propertyName, key);

        var localizedEmailTemplate1 = dictionary1[key];
        var localizedEmailTemplate2 = dictionary2[key];

        Assert.AreEqual(localizedEmailTemplate1.BodyTemplate, localizedEmailTemplate2.BodyTemplate,
          string.Format("{0}. Key: {1}. LocalizedEmailTemplate", propertyName, key));
        Assert.AreEqual(localizedEmailTemplate1.SubjectTemplate, localizedEmailTemplate2.SubjectTemplate,
          string.Format("{0}. Key: {1}. SubjectTemplate", propertyName, key));
      }
    }
  }
}
