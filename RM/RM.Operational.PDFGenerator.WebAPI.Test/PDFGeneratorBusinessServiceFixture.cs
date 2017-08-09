using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Operational.PDFGenerator.WebAPI.BusinessService;

namespace RM.Operational.PDFGenerator.WebAPI.Test
{
    // TODO clean up
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
        private const string defaultReportXml = "<report outputTo=\"A4Landscape\"><!-- xmlns=\"http://tempuri.org/FMO_PDFReport_Generic.xsd\" -->" +
                                                "<pageHeader caption=\"Unit Test Report\" /><pageFooter caption=\"Source: PDFGenerator\" pageNumbers=\"true\" />" +
                                                "<content><section><sectionColumn width=\"1\"><heading1>Unit Test Report</heading1></sectionColumn>" +
                                                "</section></content></report>";



        /// <summary>
        /// Custom report XML
        /// </summary>
        private const string customReportXml = "<report><content><section></section></content></report>";



        /// <summary>
        /// Custom XSLT
        /// </summary>
        private const string customXslt = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" xmlns:fo=\"http://www.w3.org/1999/XSL/Format\">" +
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
            const string PdfReportFolderPath = @"C:\FMOData\UnitTest\PDFReports";
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
                // TODO enhance this and move it to the validation library
                DirectoryInfo folder = new DirectoryInfo(pdfReportFolderPath);
                if (folder.FullName.Length >= 20 && !folder.FullName.StartsWith(@"C:\Windows") && folder.FullName == pdfReportFolderPath)
                {
                    // Step through the files in the folder
                    var files = folder.EnumerateFiles();
                    foreach (var file in files)
                    {
                        // Only delete if the file is actually in the PDF report folder path - this ensures that only direct paths
                        //   are processed and prevents attacks where navigation back up the folder hierarchy might allow a path
                        //   that contains system files
                        if (file.Directory.FullName == pdfReportFolderPath)
                        {
                            // Only delete PDF and XSLT files - these are the only file types that the unit test creates - anything
                            //   else should be left alone
                            string extension = file.Extension.ToLower();
                            if (extension == "pdf" || extension == "xslt")
                            {
                                try
                                {
                                    // Delete the file
                                    // TODO test that the protective code works then enable this line
                                    //file.Delete();
                                }
                                catch (IOException)
                                {
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



        ///// <summary>
        ///// Create PDF Report unit tests
        ///// </summary>
        //[Test]
        //public void CreatePdfReport_WhereValidReportXmlAndDefaultXslt_ExpectPdfFileExists()
        //{
        //    //testCandidate.CreatePdfReport(reportXml)
        //    Assert.Fail(); // TODO
        //}

        //[Test]
        //public void CreatePdfReport_WhereValidReportXmlAndCustomXslt_ExpectPdfFileExists()
        //{
        //    //testCandidate.CreatePdfReport(reportXml, xsltFilePath)
        //    Assert.Fail(); // TODO
        //}



        ///// <summary>
        ///// Delete PDF Report unit tests
        ///// </summary>
        //[Test]
        //public void DeletePdfReport_WherePdfFileExists_ExpectPdfFileDeleted()
        //{
        //    //testCandidate.DeletePdfReport(pdfFileName)
        //    Assert.Fail(); // TODO
        //}

        //[Test]
        //public void DeletePdfReport_WherePdfFileNotExists_ExpectTODO()
        //{
        //    //testCandidate.DeletePdfReport(pdfFileName)
        //    Assert.Fail(); // TODO
        //}

        //[Test]
        //public void DeletePdfReport_WherePdfFileNameIsNull_ExpectArgumentNullException()
        //{
        //    //testCandidate.DeletePdfReport(pdfFileName)
        //    Assert.Fail(); // TODO
        //}

        //[Test]
        //public void DeletePdfReport_WherePdfFileNameIsNotValid_ExpectArgumentException()
        //{
        //    //testCandidate.DeletePdfReport(pdfFileName)
        //    Assert.Fail(); // TODO
        //}



        ///// <summary>
        ///// Get PDF Report unit tests
        ///// </summary>
        //[Test]
        //public void GetPdfReport_WherePdfFileExists_ExpectPdfFileDtoWithByteArrayData()
        //{
        //    //testCandidate.GetPdfReport(pdfFileName)
        //    Assert.Fail(); // TODO
        //}

        //[Test]
        //public void GetPdfReport_WherePdfFileNotExists_ExpectNull()
        //{
        //    //testCandidate.GetPdfReport(pdfFileName)
        //    Assert.Fail(); // TODO
        //}

        //[Test]
        //public void GetPdfReport_WherePdfFileNameIsNull_ExpectArgumentNullException()
        //{
        //    //testCandidate.GetPdfReport(pdfFileName)
        //    Assert.Fail(); // TODO
        //}

        //[Test]
        //public void GetPdfReport_WherePdfFileNameIsNotValid_ExpectArgumentException()
        //{
        //    //testCandidate.GetPdfReport(pdfFileName)
        //    Assert.Fail(); // TODO
        //}



        ///// <summary>
        ///// Lifecycle unit tests - to verify overall lifecycle of a PDF file
        ///// </summary>
        //[Test]
        //public void Lifecycle_WhereValidReportXmlAndDefaultXslt_ExpectPdfFileDeletedAndPass()
        //{
        //    //testCandidate.CreatePdfReport(reportXml)
        //    //testCandidate.GetPdfReport(pdfFileName)
        //    //testCandidate.DeletePdfReport(pdfFileName)
        //    Assert.Fail(); // TODO
        //}

        //[Test]
        //public void Lifecycle_WhereValidReportXmlAndCustomXslt_ExpectPdfFileDeletedAndPass()
        //{
        //    //testCandidate.CreatePdfReport(reportXml, xsltFilePath)
        //    //testCandidate.GetPdfReport(pdfFileName)
        //    //testCandidate.DeletePdfReport(pdfFileName)
        //    Assert.Fail(); // TODO
        //}





        // TODO remove this once it is removed from the base class
        protected override void OnSetup()
        {
            // TODO delete this method
        }
    }
}