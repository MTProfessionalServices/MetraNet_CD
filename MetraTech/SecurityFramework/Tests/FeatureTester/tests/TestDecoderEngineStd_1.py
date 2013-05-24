
import pprint
import yaml
import AppTestBaseCommon
from MetraTech.SecurityFramework import *

       
class TestDecoderEngineStd1(AppTestBaseCommon.AppTestBaseCommon):            
    def setUp(self):
        print ' '
    def tearDown(self):
        print ' '
	 
    def testDefaultHtmlDecoderXMLAttributeMain(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.XML_Attribute.Main.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Xml.ToString() + '.V1',testData['payload']) 
                    
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])    
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)         
	 
    def testDefaultHtmlDecoderXMlAttributeAlphanumeric(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.XML_Attribute.Alphanumeric.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Xml.ToString() + '.V1',testData['payload']) 
                    
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])    
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)         
   
    def testDefaultHtmlDecoderXMLCharacters(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.XML.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Xml.ToString() + '.V1',testData['payload']) 
                    
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])    
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)         
   
    def testDefaultHtmlAttributeDecoderDecHex(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.Html_Attribute.Dec.Hex.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Html.ToString() + '.V1',testData['payload'])
                    
					
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)    

    def testDefaultHtmlAttributeDecoderEntityCharacters(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.Html_Attribute.Entity.Character.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Html.ToString() + '.V1',testData['payload']) 
                    
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])    
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)         

			
    def testDefaultHtmlDecoderEntityCharacters(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.Html.Entity.Character.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Html.ToString() + '.V1',testData['payload']) 
                    
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])    
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)         

    def testDefaultHtmlDecoderMain(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.Html.Character.main.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Html.ToString() + '.V1',testData['payload'])
                    
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])                  
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures) 
                   
    def testDefaultHtmlDecoderCharacters(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.Html.Character.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Html.ToString() + '.V1',testData['payload']) 
                    
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)        

    def testDefaultHtmlDecoderDecHex(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.Html.Dec.Hex.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Html.ToString() + '.V1',testData['payload'])
                    
					
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)    
                   
    def testDefaultHtmlDecoderString(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.Html.Strings.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Html.ToString() + '.V1',testData['payload'])
                    
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)    
                   
    def testDefaultBase64DecoderAlphanumerical(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.Base64.alphanumerical.yaml')):		
																    
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Base64.ToString() + '.Standart',testData['payload']) 
                    
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)         
			 
    #def testDefaultBase64DecoderModified(self):
    #    failures = []
    #    try:
    #        for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.Base64.modified.yaml')):		
#																    
#                try:      
#                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Base64.ToString() + '.Modified',testData['payload']) 
#                    
#                    if testData['expect'] != testResult.Value:
#                        failures.append(testData['id'])
#                except TypeError, x:
#                    print 'Decoding problem for character: ', testData['id'], x
#        except TypeError, x:
#            print x
#        if len(failures) > 0:
            #self.assertTrue(False,failures)

    def testDefaultCssDecoderAlphanumerical(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.CSS.alphanumerical.yaml')):
                try: 
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Css.ToString() + '.V1',testData['payload'])
                    
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)   

    def testDefaultJavaScriptDecoderMain(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.JavaScript.Main.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.JavaScript.ToString() + '.V1',testData['payload']) 
                    
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)         

    def testDefaultJavaScriptDecoderAlphanumerical(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.JavaScript.alphanumerical.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.JavaScript.ToString() + '.V1',testData['payload'])
                    
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures) 

    def testDefaultLDAPDecoderAlphanumerical(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.LDAP.alphanumerical.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Ldap.ToString() + '.V1',testData['payload']) 
                    #print testResult
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)         

    def testDefaultUrlDecoderAlphanumerical(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.URL.alphanumerical.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Url.ToString() + '.V1',testData['payload'])
                    
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures) 
            
    def testDefaultUrlDecoderMain(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.URL.Main.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Url.ToString() + '.V1',testData['payload']) 
                    
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)         
    
    def testDefaultXMLDecoder(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.XML.Main.yaml')):
                try:      
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.Xml.ToString() + '.V1',testData['payload'])
                    #print 'expect:[' + testData['expect'] + ']'
                    #print 'test result:[' + testResult.Value + ']'
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)		

    def testDefaultVbsDecoderAlphanumerical(self):
        failures = []
        try:
            for testData in yaml.load_all(open(self.testDataPath + 'Test.Decoder.VBS.alphanumerical.yaml')):
                try: 
                    testResult = SecurityKernel.Decoder.Api.Execute(DecoderEngineCategory.VbScript.ToString() + '.V1',testData['payload'])
                    #print testResult
                    if testData['expect'] != testResult.Value:
                        failures.append(testData['id'])
                except TypeError, x:
                    print 'Decoding problem for character: ', testData['id'], ' => ', testData['payload'], ' => ', x
        except TypeError, x:
            print x
        if len(failures) > 0:
            self.assertTrue(False,failures)  			
######################

if __name__ == '__main__':
    unittest.main()




