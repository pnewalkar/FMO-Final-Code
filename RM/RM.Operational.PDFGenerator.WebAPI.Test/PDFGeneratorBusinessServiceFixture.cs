using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Operational.PDFGenerator.WebAPI.BusinessService;
using RM.CommonLibrary.EntityFramework.DTO;
using System.Collections.Generic;
using System.Xml;

namespace RM.Operational.PDFGenerator.WebAPI.Test
{
    [TestFixture]
    public class PDFGeneratorBusinessServiceFixture : TestFixtureBase
    {
        /// <summary>
        /// The name of the PDF report folder path configuration key
        /// </summary>
        private const string PdfReportFolderPathConfigurationKey = "PdfReportFolderPath";



        /// <summary>
        /// Mock configuration helper
        /// </summary>
        private Mock<IConfigurationHelper> mockConfigurationHelper;



        /// <summary>
        /// Mock logging helper
        /// </summary>
        private Mock<ILoggingHelper> mockLoggingHelper;



        /// <summary>
        /// Test candidate
        /// </summary>
        private IPDFGeneratorBusinessService testCandidate;



        /// <summary>
        /// Default report XML - for use with the default XSLT template
        /// </summary>
        private const string DefaultReportXml = "<report outputTo=\"A4Landscape\"><!-- xmlns=\"http://tempuri.org/FMO_PDFReport_Generic.xsd\" -->" +
                                                "<pageHeader caption=\"Unit Test Report\" /><pageFooter caption=\"Source: PDFGenerator\" pageNumbers=\"true\" />" +
                                                "<content><section><sectionColumn width=\"1\"><heading1>Unit Test Report</heading1></sectionColumn>" +
                                                "</section></content></report>";



        /// <summary>
        /// Custom report XML
        /// </summary>
        private const string CustomReportXml = "<report><content><section></section></content></report>";



        /// <summary>
        /// Custom XSLT
        /// </summary>
        private const string CustomXslt = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" xmlns:fo=\"http://www.w3.org/1999/XSL/Format\">" +
                                          "<xsl:output method=\"xml\" indent=\"yes\"/><xsl:template match=\"/report\"><fo:root><fo:layout-master-set>" +
                                          "<fo:simple-page-master master-name=\"A4Landscape\" page-width=\"297mm\" page-height=\"210mm\" margin-top=\"1cm\" margin-bottom=\"1cm\" margin-left=\"1cm\"  margin-right=\"1cm\">" +
                                          "<fo:region-body margin-top=\"1cm\" margin-bottom=\"1cm\" margin-left=\"0cm\" margin-right=\"0cm\" column-count=\"1\"/>" +
                                          "<fo:region-before extent=\"0.5cm\"/><fo:region-after extent=\"0.5cm\"/><fo:region-start extent=\"0.5cm\"/><fo:region-end extent=\"0.5cm\"/>" +
                                          "</fo:simple-page-master></fo:layout-master-set><fo:page-sequence font-family=\"Calibri\" font-size=\"10pt\" master-reference=\"A4Landscape\">" +
                                          "<fo:flow flow-name=\"xsl-region-body\"><xsl:apply-templates/></fo:flow></fo:page-sequence></fo:root></xsl:template>" +
                                          "<xsl:template match=\"section\"><fo:block>Custom XSLT</fo:block></xsl:template></xsl:stylesheet>";





        /// <summary>
        /// Performs set up operations prior to each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Set up the configuration helper
            this.mockConfigurationHelper = CreateMock<IConfigurationHelper>();
            const string PdfReportFolderPath = @"C:\RMData\UnitTest\PDFReports";
            this.mockConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(PdfReportFolderPathConfigurationKey)).Returns(PdfReportFolderPath);

            // Set up the logging helper
            var mockTraceManager = new Mock<IRMTraceManager>();
            mockTraceManager.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            this.mockLoggingHelper = CreateMock<ILoggingHelper>();
            this.mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(mockTraceManager.Object);

            // Set up the test candidate
            testCandidate = new PDFGeneratorBusinessService(mockConfigurationHelper.Object, mockLoggingHelper.Object);
        }



        /// <summary>
        /// Performs tear down operations after each test
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // Do nothing
        }



        /// <summary>
        /// Performs tear down operations after the entire fixture
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Clear all PDF files and XSLT files from the PDF report folder path in case any have been left behind
            string pdfReportFolderPath = mockConfigurationHelper.Object.ReadAppSettingsConfigurationValues(PdfReportFolderPathConfigurationKey);
            if (Directory.Exists(pdfReportFolderPath))
            {
                // Verify that the folder path is not obviously malicious
                DirectoryInfo folder = new DirectoryInfo(pdfReportFolderPath);
                if (folder.FullName.StartsWith(@"C:\RMData") && folder.FullName == pdfReportFolderPath)
                {
                    // Step through the files in the folder
                    var files = folder.EnumerateFiles();
                    foreach (var file in files)
                    {
                        // Only delete if the file is actually in the PDF report folder path - this ensures that only direct paths
                        //   are processed and prevents attacks where navigation back up the folder hierarchy might allow a path
                        //   that contains system files
                        // Also check that the folder path doesn't point to the Windows folder or is unexpectedly short
                        if (file.Directory.FullName == pdfReportFolderPath && file.Directory.FullName != @"C:\Windows" && file.Directory.FullName.Length > 10)
                        {
                            // Only delete PDF and XSLT files - these are the only file types that the unit test creates - anything
                            //   else should be left alone
                            string extension = file.Extension.ToLower();
                            if (extension == ".pdf" || extension == ".xslt")
                            {
                                try
                                {
                                    // Delete the file
                                    file.Delete();
                                }
                                catch (IOException)
                                {
                                    // Do nothing
                                    //
                                    // Since this just clearing up resources that are uniquely identified there is no problem if
                                    //   any files fail to delete
                                }
                            }
                        }
                    }
                }
            }
        }





        /// <summary>
        /// Environment unit tests to verify that any external dependencies are configures correctly
        /// </summary>
        [Test]
        public void Environment_WhereExternalDependenciesAreValid_ExpectPass()
        {
            // Verify that the PDF report folder path exists
            string pdfReportFolderPath = mockConfigurationHelper.Object.ReadAppSettingsConfigurationValues(PdfReportFolderPathConfigurationKey);
            Assert.True(Directory.Exists(pdfReportFolderPath));
        }



        /// <summary>
        /// Create PDF Report unit tests
        /// The 'happy' case unit tests are covered by:
        ///   PdfGenerationLifecycle_WhereValidReportXmlAndDefaultXslt_ExpectPdfFileDeletedAndPass
        ///   PdfGenerationLifecycle_WhereValidReportXmlAndCustomXslt_ExpectPdfFileDeletedAndPass
        /// </summary>
        [Test]
        public void CreatePdfReport_WhereEmptyReportXmlAndDefaultXslt_ExpectArgumentException()
        {
            foreach (string reportXml in GetEmptyArgumentValues())
            {
                Assert.Throws<ArgumentException>(() => testCandidate.CreatePdfReport(reportXml));
            }
        }

        [Test]
        public void CreatePdfReport_WhereInvalidReportXmlAndDefaultXslt_ExpectXmlException()
        {
            // Test all the invalid xml for the report XML
            foreach (string reportXml in GetInvalidXml())
            {
                Assert.Throws<XmlException>(() => testCandidate.CreatePdfReport(reportXml));
            }
        }

        [Test]
        public void CreatePdfReport_WhereEmptyReportXmlAndCustomXslt_ExpectArgumentException()
        {
            // Create the custom XSLT file
            string customXsltFilePath = this.CreateCustomXsltFile();
            Assert.True(File.Exists(customXsltFilePath));

            // Test all the empty argument values for the report XML
            foreach (string reportXml in GetEmptyArgumentValues())
            {
                Assert.Throws<ArgumentException>(() => testCandidate.CreatePdfReport(reportXml, customXsltFilePath));
            }

            // Delete the custom XSLT file
            this.DeleteCustomXsltFile(customXsltFilePath);
            Assert.True(!File.Exists(customXsltFilePath));
        }

        [Test]
        public void CreatePdfReport_WhereInvalidReportXmlAndCustomXslt_ExpectXmlException()
        {
            // Create the custom XSLT file
            string customXsltFilePath = this.CreateCustomXsltFile();
            Assert.True(File.Exists(customXsltFilePath));

            // Test all the invalid xml for the report XML
            foreach (string reportXml in GetInvalidXml())
            {
                Assert.Throws<XmlException>(() => testCandidate.CreatePdfReport(reportXml, customXsltFilePath));
            }

            // Delete the custom XSLT file
            this.DeleteCustomXsltFile(customXsltFilePath);
            Assert.True(!File.Exists(customXsltFilePath));
        }

        [Test]
        public void CreatePdfReport_WhereValidReportXmlAndEmptyCustomXslt_ExpectArgumentException()
        {
            string reportXml = CustomReportXml;
            foreach (string xsltFilePath in GetEmptyArgumentValues())
            {
                Assert.Throws<ArgumentException>(() => testCandidate.CreatePdfReport(reportXml, xsltFilePath));
            }
        }

        [Test]
        public void CreatePdfReport_WhereValidReportXmlAndMissingCustomXslt_ExpectArgumentException()
        {
            // Create the custom XSLT file
            string customXsltFilePath = this.CreateCustomXsltFile();
            Assert.True(File.Exists(customXsltFilePath));

            // Delete the custom XSLT file
            this.DeleteCustomXsltFile(customXsltFilePath);
            Assert.True(!File.Exists(customXsltFilePath));

            string reportXml = CustomReportXml;
            Assert.Throws<ArgumentException>(() => testCandidate.CreatePdfReport(reportXml, customXsltFilePath));
        }



        /// <summary>
        /// Delete PDF Report unit tests
        /// The 'happy' case unit tests are covered by:
        ///   PdfGenerationLifecycle_WhereValidReportXmlAndDefaultXslt_ExpectPdfFileDeletedAndPass
        ///   PdfGenerationLifecycle_WhereValidReportXmlAndCustomXslt_ExpectPdfFileDeletedAndPass
        /// The 'sad' case where the PDF file does not exist does not have a meaningful test as 
        ///   the outcome is that nothing happens
        /// </summary>
        [Test]
        public void DeletePdfReport_WherePdfFileNameIsNullEmptyOrWhiteSpace_ExpectArgumentException()
        {
            foreach (string pdfFileName in GetEmptyArgumentValues())
            {
                Assert.Throws<ArgumentException>(() => testCandidate.DeletePdfReport(pdfFileName));
            }
        }

        [Test]
        public void DeletePdfReport_WherePdfFileNameIsNotValid_ExpectArgumentException()
        {
            foreach (string pdfFileName in GetInvalidFileNames(".pdf"))
            {
                Assert.Throws<ArgumentException>(() => testCandidate.DeletePdfReport(pdfFileName));
            }
        }




        /// <summary>
        /// Get PDF Report unit tests
        /// The 'happy' case unit tests are covered by:
        ///   PdfGenerationLifecycle_WhereValidReportXmlAndDefaultXslt_ExpectPdfFileDeletedAndPass
        ///   PdfGenerationLifecycle_WhereValidReportXmlAndCustomXslt_ExpectPdfFileDeletedAndPass
        /// </summary>
        [Test]
        public void GetPdfReport_WherePdfFileNotExists_ExpectNull()
        {
            string pdfFileName = Guid.NewGuid().ToString() + ".pdf";
            PdfFileDTO result = testCandidate.GetPdfReport(pdfFileName);
            Assert.True(result == null);
        }

        [Test]
        public void GetPdfReport_WherePdfFileNameIsNullEmptyOrWhiteSpace_ExpectArgumentException()
        {
            foreach (string pdfFileName in GetEmptyArgumentValues())
            {
                Assert.Throws<ArgumentException>(() => testCandidate.GetPdfReport(pdfFileName));
            }
        }

        [Test]
        public void GetPdfReport_WherePdfFileNameIsNotValid_ExpectArgumentException()
        {
            foreach (string pdfFileName in GetInvalidFileNames(".pdf"))
            {
                Assert.Throws<ArgumentException>(() => testCandidate.GetPdfReport(pdfFileName));
            }
        }



        /// <summary>
        /// Lifecycle unit tests - to verify overall lifecycle of a PDF file
        /// </summary>
        [Test]
        public void PdfGenerationLifecycle_WhereValidReportXmlAndDefaultXslt_ExpectPdfFileDeletedAndPass()
        {
            // Create a PDF report from the default report XML
            string pdfFileName = testCandidate.CreatePdfReport(DefaultReportXml);
            Assert.True(!string.IsNullOrWhiteSpace(pdfFileName));

            // Verify that the PDF report file has been created
            string pdfReportFolderPath = mockConfigurationHelper.Object.ReadAppSettingsConfigurationValues(PdfReportFolderPathConfigurationKey);
            string pdfFilePath = Path.Combine(pdfReportFolderPath, pdfFileName);
            Assert.True(File.Exists(pdfFilePath));

            // Retrieve the PDF report file
            PdfFileDTO response = testCandidate.GetPdfReport(pdfFileName);
            Assert.True(response != null);
            Assert.True(response.Data != null);
            Assert.True(response.Data.Length > 0);

            // Delete the PDF report file
            testCandidate.DeletePdfReport(pdfFileName);
            Assert.True(!File.Exists(pdfFilePath));
        }

        [Test]
        public void PdfGenerationLifecycle_WhereValidReportXmlAndCustomXslt_ExpectPdfFileDeletedAndPass()
        {
            // Create the custom XSLT file
            string customXsltFilePath = this.CreateCustomXsltFile();
            Assert.True(File.Exists(customXsltFilePath));

            // Create a PDF report from the custom report XML using a custom XSLT file
            string pdfFileName = testCandidate.CreatePdfReport(CustomReportXml, customXsltFilePath);
            Assert.True(!string.IsNullOrWhiteSpace(pdfFileName));

            // Verify that the PDF report file has been created
            string pdfReportFolderPath = mockConfigurationHelper.Object.ReadAppSettingsConfigurationValues(PdfReportFolderPathConfigurationKey);
            string pdfFilePath = Path.Combine(pdfReportFolderPath, pdfFileName);
            Assert.True(File.Exists(pdfFilePath));

            // Retrieve the PDF report file
            PdfFileDTO response = testCandidate.GetPdfReport(pdfFileName);
            Assert.True(response != null);
            Assert.True(response.Data != null);
            Assert.True(response.Data.Length > 0);

            // Delete the PDF report file
            testCandidate.DeletePdfReport(pdfFileName);
            Assert.True(!File.Exists(pdfFilePath));

            // Delete the custom XSLT file
            this.DeleteCustomXsltFile(customXsltFilePath);
            Assert.True(!File.Exists(customXsltFilePath));
        }





        /// <summary>
        /// Creates an XSLT file containing the custom XSLT
        /// </summary>
        /// <returns>The path to the custom XSLT file</returns>
        private string CreateCustomXsltFile()
        {
            // Get the PDF report folder path
            string pdfReportFolderPath = mockConfigurationHelper.Object.ReadAppSettingsConfigurationValues(PdfReportFolderPathConfigurationKey);

            // Create the custom XSLT file in the PDF report folder path
            string customXsltFilePath = Path.Combine(pdfReportFolderPath, (Guid.NewGuid()).ToString() + ".xslt");
            using (StreamWriter sw = File.CreateText(customXsltFilePath))
            {
                sw.Write(CustomXslt);
                sw.Flush();
                sw.Close();
            }

            // Return the path to the custom XSLT file
            return customXsltFilePath;
        }



        /// <summary>
        /// Deletes a custom XSLT file
        /// Note: this is not a general purpose delete file method - it includes checks to ensure that the 
        ///   path points to an xslt file under the PDF report folder path
        /// </summary>
        /// <param name="path">The path to the custom XSLT file</param>
        private void DeleteCustomXsltFile(string path)
        {
            // Get the PDF report folder path
            string pdfReportFolderPath = mockConfigurationHelper.Object.ReadAppSettingsConfigurationValues(PdfReportFolderPathConfigurationKey);

            // If the path is in the expected location and has the expected extension
            FileInfo target = new FileInfo(path);
            if (target.FullName.StartsWith(pdfReportFolderPath) && target.Extension.ToLower() == ".xslt")
            {
                try
                {
                    // Delete the file
                    target.Delete();
                }
                catch (IOException)
                {
                    // Do nothing
                    //
                    // The failure to delete will be flagged by a failed test
                }
            }
        }



        /// <summary>
        /// Returns a list of empty (null, empty or whitespace) file names for validation testing
        /// </summary>
        /// <returns>List of empty file names</returns>
        private List<string> GetEmptyArgumentValues()
        {
            List<string> fileNames = new List<string>();
            fileNames.Add(null);
            fileNames.Add(string.Empty);
            fileNames.Add("   ");
            fileNames.Add(" ");
            return fileNames;
        }



        /// <summary>
        /// Returns a list of invalid file names for validation testing
        /// </summary>
        /// <param name="extension">The file extension to apply</param>
        /// <returns>List of invalid file names</returns>
        private List<string> GetInvalidFileNames(string extension)
        {
            List<string> fileNames = new List<string>();
            fileNames.Add(Guid.NewGuid().ToString() + @"*" + extension);
            fileNames.Add(Guid.NewGuid().ToString() + @"?" + extension);
            fileNames.Add(Guid.NewGuid().ToString() + @"/" + extension);
            fileNames.Add(Guid.NewGuid().ToString() + @"\" + extension);
            fileNames.Add(Guid.NewGuid().ToString() + @".." + extension);
            return fileNames;
        }



        /// <summary>
        /// Returns a list of invalid XML document strings for validation testing
        /// </summary>
        /// <returns>List of invalid XML document strings</returns>
        private List<string> GetInvalidXml()
        {
            List<string> xmlStrings = new List<string>();
            xmlStrings.Add("<xml>");
            xmlStrings.Add("xml>");
            xmlStrings.Add("This is not valid XML");
            xmlStrings.Add("</xml><xml>");
            return xmlStrings;
        }





        // TODO remove this once it is removed from the base class
        protected override void OnSetup()
        {
            // TODO delete this method
        }
    }
}