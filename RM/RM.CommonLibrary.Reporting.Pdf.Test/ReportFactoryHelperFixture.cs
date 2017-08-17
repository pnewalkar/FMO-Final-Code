using NUnit.Framework;
using RM.CommonLibrary.Reporting.Pdf;
using System;
using System.Xml;

namespace RM.CommonLibrary.Reporting.Test
{
    /// <summary>
    /// Tests the Report Factory Helper class
    ///
    /// Note that the tests in this fixture only test the DOM changes that the methods perform
    ///   Compliance with FMO_PDFReport_Generic.xsd is not intended or tested
    /// </summary>
    [TestFixture]
    public class ReportFactoryHelperFixture
    {
        /// <summary>
        /// The name of the parent node
        /// </summary>
        private const string ParentNodeName = "parent";



        /// <summary>
        /// Test attribute name
        /// </summary>
        private const string TestAttributeName = "testAttributeName";



        /// <summary>
        /// Test node name
        /// </summary>
        private const string TestNodeName = "testNodeName";



        /// <summary>
        /// White space
        /// </summary>
        private const string WhiteSpace = "    ";





        /// <summary>
        /// Parent XML element for tests that require a parent element
        /// </summary>
        private XmlElement parent = null;



        /// <summary>
        /// Root XML document
        /// </summary>
        private XmlDocument root = null;





        /// <summary>
        /// Performs set up operations prior to each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Create the root XML document
            this.root = new XmlDocument();

            // Create the parent XML element
            this.parent = this.root.CreateElement(ParentNodeName);
        }



        /// <summary>
        /// Performs tear down operations after each test
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // Clear the parent XML element
            this.parent = null;

            // Clear the root XML document
            this.root = null;
        }



        /// <summary>
        /// Performs tear down operations after the entire fixture
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Do nothing
        }





        /// <summary>
        /// Add Attribute unit tests
        /// </summary>
        [Test]
        public void AddAttribute_WhereNameAndValueAndNotRequired_ExpectAttributeWithNameAndValue()
        {
            string value = "MyValue";
            bool required = false;
            ReportFactoryHelper.AddAttribute(this.parent, this.root, TestAttributeName, value, required);
            Assert.True(this.parent.Attributes[0].Name == TestAttributeName);
            Assert.True(this.parent.Attributes[0].Value == value);
        }

        [Test]
        public void AddAttribute_WhereNameAndValueAndIsRequired_ExpectAttributeWithNameAndValue()
        {
            string value = "MyValue";
            bool required = true;
            ReportFactoryHelper.AddAttribute(this.parent, this.root, TestAttributeName, value, required);
            Assert.True(this.parent.Attributes[0].Name == TestAttributeName);
            Assert.True(this.parent.Attributes[0].Value == value);
        }

        [Test]
        public void AddAttribute_WhereNameAndEmptyValueAndNotRequired_ExpectNoAttribute()
        {
            string value = string.Empty;
            bool required = false;
            ReportFactoryHelper.AddAttribute(this.parent, this.root, TestAttributeName, value, required);
            Assert.True(this.parent.Attributes.Count == 0);
        }

        [Test]
        public void AddAttribute_WhereNameAndNullValueAndNotRequired_ExpectNoAttribute()
        {
            string value = null;
            bool required = false;
            ReportFactoryHelper.AddAttribute(this.parent, this.root, TestAttributeName, value, required);
            Assert.True(this.parent.Attributes.Count == 0);
        }

        [Test]
        public void AddAttribute_WhereNameAndNoValueAndIsRequired_ExpectAttributeWithNameAndEmptyValue()
        {
            string value = string.Empty;
            bool required = true;
            ReportFactoryHelper.AddAttribute(this.parent, this.root, TestAttributeName, value, required);
            Assert.True(this.parent.Attributes[0].Name == TestAttributeName);
            Assert.True(this.parent.Attributes[0].Value == value);
        }

        [Test]
        public void AddAttribute_WhereNameAndNoValueAndIsRequired_ExpectAttributeWithNameAndNullValue()
        {
            string value = null;
            bool required = true;
            ReportFactoryHelper.AddAttribute(this.parent, this.root, TestAttributeName, value, required);
            Assert.True(this.parent.Attributes[0].Name == TestAttributeName);
            Assert.True(string.IsNullOrWhiteSpace(this.parent.Attributes[0].Value) == string.IsNullOrWhiteSpace(value));
        }

        [Test]
        public void AddAttribute_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddAttribute(null, this.root, TestAttributeName, "MyValue", false));
        }

        [Test]
        public void AddAttribute_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddAttribute(this.parent, null, TestAttributeName, "MyValue", false));
        }

        [Test]
        public void AddAttribute_WhereNameIsNull_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddAttribute(this.parent, this.root, null, "MyValue", false));
        }

        [Test]
        public void AddAttribute_WhereNameIsEmpty_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddAttribute(this.parent, this.root, string.Empty, "MyValue", false));
        }

        [Test]
        public void AddAttribute_WhereNameIsWhiteSpace_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddAttribute(this.parent, this.root, WhiteSpace, "MyValue", false));
        }





        /// <summary>
        /// Add Caption Element unit tests
        /// </summary>
        [Test]
        public void AddCaptionElement_WhereCaptionAndAlignLeft_ExpectCaptionElementWithCaption()
        {
            const string caption = "CaptionValue";
            ReportFactoryHelper.TextAlignOption align = ReportFactoryHelper.TextAlignOption.Left;
            ReportFactoryHelper.AddCaptionElement(this.parent, this.root, caption, align);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "caption");
            Assert.True(this.parent.ChildNodes[0].InnerText == caption);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddCaptionElement_WhereCaptionAndAlignNotLeft_ExpectCaptionElementWithCaptionAndAlignAttribute()
        {
            const string caption = "CaptionValue";
            ReportFactoryHelper.TextAlignOption align = ReportFactoryHelper.TextAlignOption.Right;
            ReportFactoryHelper.AddCaptionElement(this.parent, this.root, caption, align);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "caption");
            Assert.True(this.parent.ChildNodes[0].InnerText == caption);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "align");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == ReportFactoryHelper.GetTextAlignAttributeValue(align));
        }

        [Test]
        public void AddCaptionElement_WhereNullCaptionAndAlignLeft_ExpectCaptionElementWithEmptyCaption()
        {
            const string caption = null;
            ReportFactoryHelper.TextAlignOption align = ReportFactoryHelper.TextAlignOption.Left;
            ReportFactoryHelper.AddCaptionElement(this.parent, this.root, caption, align);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "caption");
            Assert.True(string.IsNullOrWhiteSpace(this.parent.ChildNodes[0].Value) == string.IsNullOrWhiteSpace(caption));
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddCaptionElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddCaptionElement(null, this.root, "Caption", ReportFactoryHelper.TextAlignOption.Left));
        }

        [Test]
        public void AddCaptionElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddCaptionElement(this.parent, null, "Caption", ReportFactoryHelper.TextAlignOption.Left));
        }





        /// <summary>
        /// Add Content Element unit tests
        /// </summary>
        [Test]
        public void AddContentElement_WhereHappy_ExpectContentElement()
        {
            XmlElement node = ReportFactoryHelper.AddContentElement(this.parent, this.root);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "content");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddContentElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddContentElement(null, this.root));
        }

        [Test]
        public void AddContentElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddContentElement(this.parent, null));
        }





        /// <summary>
        /// Add Element unit tests
        /// </summary>
        [Test]
        public void AddElement_WhereNodeName_ExpectElementWithNodeName()
        {
            XmlElement node = ReportFactoryHelper.AddElement(this.parent, this.root, TestNodeName);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == TestNodeName);
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddElement(null, this.root, TestNodeName));
        }

        [Test]
        public void AddElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddElement(this.parent, null, TestNodeName));
        }

        [Test]
        public void AddElement_WhereNodeNameIsNull_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddElement(this.parent, this.root, null));
        }

        [Test]
        public void AddElement_WhereNodeNameIsEmpty_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddElement(this.parent, this.root, string.Empty));
        }

        [Test]
        public void AddElement_WhereNodeNameIsWhiteSpace_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddElement(this.parent, this.root, WhiteSpace));
        }





        /// <summary>
        /// Add Full Width Section With Main Heading unit tests
        /// </summary>
        [Test]
        public void AddFullWidthSectionWithMainHeading_WhereHeading_ExpectSectionElementWithSectionColumnElementWithHeading1ElementWithHeading()
        {
            // This method calls other helper methods, so it only needs to verify that the appropriate methods are called
            const string heading = "HeadingValue";
            ReportFactoryHelper.AddFullWidthSectionWithMainHeading(this.parent, this.root, heading);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "section");
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Name == "sectionColumn");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Name == "width");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Value == "1");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].ChildNodes[0].Name == "heading1");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText == heading);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddFullWidthSectionWithMainHeading_WhereNullHeading_ExpectSectionElementWithSectionColumnElementWithHeading1ElementWithEmptyHeading()
        {
            // This method calls other helper methods, so it only needs to verify that the appropriate methods are called
            const string heading = null;
            ReportFactoryHelper.AddFullWidthSectionWithMainHeading(this.parent, this.root, heading);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "section");
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Name == "sectionColumn");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Name == "width");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Value == "1");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].ChildNodes[0].Name == "heading1");
            Assert.True(string.IsNullOrWhiteSpace(this.parent.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText) == string.IsNullOrWhiteSpace(heading));
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddFullWidthSectionWithMainHeading_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddFullWidthSectionWithMainHeading(null, this.root, "Heading"));
        }

        [Test]
        public void AddFullWidthSectionWithMainHeading_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddFullWidthSectionWithMainHeading(this.parent, null, "Heading"));
        }





        /// <summary>
        /// Add Heading 1 Element unit tests
        /// </summary>
        [Test]
        public void AddHeading1Element_WhereHeading_ExpectHeading1ElementWithHeading()
        {
            const string heading = "HeadingValue";
            ReportFactoryHelper.AddHeading1Element(this.parent, this.root, heading);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "heading1");
            Assert.True(this.parent.ChildNodes[0].InnerText == heading);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddHeading1Element_WhereNullHeading_ExpectHeading1ElementWithEmptyHeading()
        {
            const string heading = null;
            ReportFactoryHelper.AddHeading1Element(this.parent, this.root, heading);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "heading1");
            Assert.True(string.IsNullOrWhiteSpace(this.parent.ChildNodes[0].Value) == string.IsNullOrWhiteSpace(heading));
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddHeading1Element_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddHeading1Element(null, this.root, "Heading"));
        }

        [Test]
        public void AddHeading1Element_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddHeading1Element(this.parent, null, "Heading"));
        }





        /// <summary>
        /// Add Heading 2 Element unit tests
        /// </summary>
        [Test]
        public void AddHeading2Element_WhereHeading_ExpectHeading2ElementWithHeading()
        {
            const string heading = "HeadingValue";
            ReportFactoryHelper.AddHeading2Element(this.parent, this.root, heading);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "heading2");
            Assert.True(this.parent.ChildNodes[0].InnerText == heading);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddHeading2Element_WhereNullHeading_ExpectHeading2ElementWithEmptyHeading()
        {
            const string heading = null;
            ReportFactoryHelper.AddHeading2Element(this.parent, this.root, heading);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "heading2");
            Assert.True(string.IsNullOrWhiteSpace(this.parent.ChildNodes[0].Value) == string.IsNullOrWhiteSpace(heading));
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddHeading2Element_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddHeading2Element(null, this.root, "Heading"));
        }

        [Test]
        public void AddHeading2Element_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddHeading2Element(this.parent, null, "Heading"));
        }





        /// <summary>
        /// Add Image Element unit tests
        /// </summary>
        [Test]
        public void AddImageElement_WhereSource_ExpectImageElement()
        {
            const string source = "SourceValue"; // This should be a value URI but this is not validated
            ReportFactoryHelper.AddImageElement(this.parent, this.root, source);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "image");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "source");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == source);
        }

        [Test]
        public void AddImageElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddImageElement(null, this.root, "SourceValue"));
        }

        [Test]
        public void AddImageElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddImageElement(this.parent, null, "SourceValue"));
        }

        [Test]
        public void AddImageElement_WhereSourceIsNull_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddImageElement(this.parent, this.root, null));
        }

        [Test]
        public void AddImageElement_WhereSourceIsEmpty_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddImageElement(this.parent, this.root, string.Empty));
        }

        [Test]
        public void AddImageElement_WhereSourceIsWhiteSpace_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddImageElement(this.parent, this.root, WhiteSpace));
        }





        /// <summary>
        /// Add Legal Notice Element unit tests
        /// </summary>
        [Test]
        public void AddLegalNoticeElement_WhereNoticeLeftAlign_ExpectLegalNoticeElement()
        {
            string notice = "LegalNoticeValue";
            const ReportFactoryHelper.TextAlignOption align = ReportFactoryHelper.TextAlignOption.Left;
            ReportFactoryHelper.AddLegalNoticeElement(this.parent, this.root, notice, align);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "legalNotice");
            Assert.True(this.parent.ChildNodes[0].InnerText == notice);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddLegalNoticeElement_WhereNoticeRightAlign_ExpectLegalNoticeElementWithAlignAttribute()
        {
            string notice = "LegalNoticeValue";
            const ReportFactoryHelper.TextAlignOption align = ReportFactoryHelper.TextAlignOption.Right;
            ReportFactoryHelper.AddLegalNoticeElement(this.parent, this.root, notice, align);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "legalNotice");
            Assert.True(this.parent.ChildNodes[0].InnerText == notice);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "align");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == ReportFactoryHelper.GetTextAlignAttributeValue(align));
        }

        [Test]
        public void AddLegalNoticeElement_WhereEmptyNotice_ExpectLegalNoticeElement()
        {
            string notice = string.Empty;
            const ReportFactoryHelper.TextAlignOption align = ReportFactoryHelper.TextAlignOption.Left;
            ReportFactoryHelper.AddLegalNoticeElement(this.parent, this.root, notice, align);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "legalNotice");
            Assert.True(this.parent.ChildNodes[0].InnerText == notice);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddLegalNoticeElement_WhereNullLegalNotice_ExpectNoElement()
        {
            string notice = null;
            const ReportFactoryHelper.TextAlignOption align = ReportFactoryHelper.TextAlignOption.Left;
            ReportFactoryHelper.AddLegalNoticeElement(this.parent, this.root, notice, align);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "legalNotice");
            Assert.True(string.IsNullOrWhiteSpace(this.parent.ChildNodes[0].InnerText) == string.IsNullOrWhiteSpace(notice));
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddLegalNoticeElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddLegalNoticeElement(null, this.root, "LegalNoticeValue", ReportFactoryHelper.TextAlignOption.Left));
        }

        [Test]
        public void AddLegalNoticeElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddLegalNoticeElement(this.parent, null, "LegalNoticeValue", ReportFactoryHelper.TextAlignOption.Left));
        }





        /// <summary>
        /// Add Legal Notices Element unit tests
        /// </summary>
        [Test]
        public void AddLegalNoticesElement_WhereTwoLegalNoticesLeftAlign_ExpectLegalNoticesElementTwoLegalNoticeElements()
        {
            string[] legalNotices = new string[] { "LegalNoticeValue1", "LegalNoticeValue2" };
            const ReportFactoryHelper.TextAlignOption align = ReportFactoryHelper.TextAlignOption.Left;
            ReportFactoryHelper.AddLegalNoticesElement(this.parent, this.root, legalNotices, align);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "legalNotices");
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 2);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Name == "legalNotice");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].InnerText == legalNotices[0]);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes.Count == 0);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].Name == "legalNotice");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].InnerText == legalNotices[1]);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].Attributes.Count == 0);
        }

        [Test]
        public void AddLegalNoticesElement_WhereOneLegalNoticeRightAlign_ExpectLegalNoticesElementOneLegalNoticeElementWithAlignAttribute()
        {
            string[] legalNotices = new string[] { "LegalNoticeValue1" };
            const ReportFactoryHelper.TextAlignOption align = ReportFactoryHelper.TextAlignOption.Right;
            ReportFactoryHelper.AddLegalNoticesElement(this.parent, this.root, legalNotices, align);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "legalNotices");
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Name == "legalNotice");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].InnerText == legalNotices[0]);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Name == "align");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Value == ReportFactoryHelper.GetTextAlignAttributeValue(align));
        }

        [Test]
        public void AddLegalNoticesElement_WhereNoLegalNotice_ExpectNoElement()
        {
            string[] legalNotices = new string[] { };
            const ReportFactoryHelper.TextAlignOption align = ReportFactoryHelper.TextAlignOption.Left;
            ReportFactoryHelper.AddLegalNoticesElement(this.parent, this.root, legalNotices, align);
            Assert.True(this.parent.ChildNodes.Count == 0);
            Assert.True(this.parent.InnerText == string.Empty);
        }

        [Test]
        public void AddLegalNoticesElement_WhereNullLegalNotices_ExpectNoElement()
        {
            string[] legalNotices = null;
            const ReportFactoryHelper.TextAlignOption align = ReportFactoryHelper.TextAlignOption.Left;
            ReportFactoryHelper.AddLegalNoticesElement(this.parent, this.root, legalNotices, align);
            Assert.True(this.parent.ChildNodes.Count == 0);
            Assert.True(this.parent.InnerText == string.Empty);
        }

        [Test]
        public void AddLegalNoticesElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddLegalNoticesElement(null, this.root, new string[] { "LegalNoticeValue1", "LegalNoticeValue2" }, ReportFactoryHelper.TextAlignOption.Left));
        }

        [Test]
        public void AddLegalNoticesElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddLegalNoticesElement(this.parent, null, new string[] { "LegalNoticeValue1", "LegalNoticeValue2" }, ReportFactoryHelper.TextAlignOption.Left));
        }





        /// <summary>
        /// Add Map Element unit tests
        /// </summary>
        [Test]
        public void AddMapElement_WhereSourceTimestampAndScale_ExpectMapElementWithSourceTimestampAndScale()
        {
            const string source = "SourceValue"; // This should be a value URI but this is not validated
            const string timestamp = "TimestampValue";
            const string scale = "ScaleValue";
            ReportFactoryHelper.AddMapElement(this.parent, this.root, source, timestamp, scale);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "map");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "source");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == source);
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Name == "mapFooter");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes.Count == 2);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Name == "timestamp");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Value == timestamp);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[1].Name == "scale");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[1].Value == scale);
        }

        [Test]
        public void AddMapElement_WhereSourceNoTimestampAndNoScale_ExpectMapElementWithSource()
        {
            const string source = "SourceValue"; // This should be a value URI but this is not validated
            const string timestamp = null;
            string scale = string.Empty;
            ReportFactoryHelper.AddMapElement(this.parent, this.root, source, timestamp, scale);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "map");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "source");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == source);
        }

        [Test]
        public void AddMapElement_WhereSourceTimestampAndNoScale_ExpectMapElementWithSourceAndTimestamp()
        {
            const string source = "SourceValue"; // This should be a value URI but this is not validated
            const string timestamp = "TimestampValue";
            const string scale = WhiteSpace;
            ReportFactoryHelper.AddMapElement(this.parent, this.root, source, timestamp, scale);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "map");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "source");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == source);
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Name == "mapFooter");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Name == "timestamp");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Value == timestamp);
        }

        [Test]
        public void AddMapElement_WhereSourceScaleAndNoTimestamp_ExpectMapElementWithSourceAndScale()
        {
            const string source = "SourceValue"; // This should be a value URI but this is not validated
            const string timestamp = WhiteSpace;
            const string scale = "ScaleValue";
            ReportFactoryHelper.AddMapElement(this.parent, this.root, source, timestamp, scale);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "map");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "source");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == source);
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Name == "mapFooter");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Name == "scale");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Value == scale);
        }

        [Test]
        public void AddMapElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddMapElement(null, this.root, "SourceValue", "TimestampValue", "ScaleValue"));
        }

        [Test]
        public void AddMapElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddMapElement(this.parent, null, "SourceValue", "TimestampValue", "ScaleValue"));
        }

        [Test]
        public void AddMapElement_WhereSourceIsNull_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddMapElement(this.parent, this.root, null, "TimestampValue", "ScaleValue"));
        }

        [Test]
        public void AddMapElement_WhereSourceIsEmpty_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddMapElement(this.parent, this.root, string.Empty, "TimestampValue", "ScaleValue"));
        }

        [Test]
        public void AddMapElement_WhereSourceIsWhiteSpace_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddMapElement(this.parent, this.root, WhiteSpace, "TimestampValue", "ScaleValue"));
        }





        /// <summary>
        /// Add Map Section unit tests
        /// </summary>
        [Test]
        public void AddMapSection_WhereCaptionSourceTimestampScaleAndLegalNotices_ExpectMapSection()
        {
            // This method calls other helper methods, so it only needs to verify that the appropriate methods are called
            const string caption = "CaptionValue";
            const string source = "SourceValue"; // This should be a value URI but this is not validated
            const string timestamp = "TimestampValue";
            const string scale = "ScaleValue";
            string[] legalNotices = new string[] { "LegalNoticeValue1", "LegalNoticeValue2" };
            ReportFactoryHelper.AddMapSection(this.parent, this.root, caption, source, timestamp, scale, legalNotices);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "section"); // Do not validate beyond existence
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Name == "sectionColumn"); // Do not validate beyond existence
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].ChildNodes.Count == 3);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].ChildNodes[0].Name == "caption"); // Do not validate beyond existence
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].ChildNodes[1].Name == "map"); // Do not validate beyond existence
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].ChildNodes[2].Name == "legalNotices"); // Do not validate beyond existence
        }

        [Test]
        public void AddMapSection_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddMapSection(null, this.root, "CaptionValue", "SourceValue", "TimestampValue", "ScaleValue", new string[] { "LegalNoticeValue1", "LegalNoticeValue2" }));
        }

        [Test]
        public void AddMapSection_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddMapSection(this.parent, null, "CaptionValue", "SourceValue", "TimestampValue", "ScaleValue", new string[] { "LegalNoticeValue1", "LegalNoticeValue2" }));
        }

        [Test]
        public void AddMapSection_WhereSourceIsNull_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddMapSection(this.parent, this.root, "CaptionValue", null, "TimestampValue", "ScaleValue", new string[] { "LegalNoticeValue1", "LegalNoticeValue2" }));
        }





        /// <summary>
        /// Add Page Break Element unit tests
        /// </summary>
        [Test]
        public void AddPageBreakElement_WhereHappy_ExpectPageBreakElement()
        {
            ReportFactoryHelper.AddPageBreakElement(this.parent, this.root);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "pageBreak");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddPageBreakElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddPageBreakElement(null, this.root));
        }

        [Test]
        public void AddPageBreakElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddPageBreakElement(this.parent, null));
        }





        /// <summary>
        /// Add Page Footer Element unit tests
        /// </summary>
        [Test]
        public void AddPageFooterElement_WhereCaptionAndShowPageNumbers_ExpectPageFooterElementWithCaptionAndPageNumbersAttributes()
        {
            const string caption = "Caption Value";
            const bool showPageNumbers = true;
            ReportFactoryHelper.AddPageFooterElement(this.parent, this.root, caption, showPageNumbers);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "pageFooter");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 2);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "caption");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == caption);
            Assert.True(this.parent.ChildNodes[0].Attributes[1].Name == "pageNumbers");
            Assert.True(this.parent.ChildNodes[0].Attributes[1].Value == showPageNumbers.ToString().ToLower());
        }

        [Test]
        public void AddPageFooterElement_WhereCaptionAndNotShowPageNumbers_ExpectPageFooterElementWithCaptionAttribute()
        {
            const string caption = "Caption Value";
            const bool showPageNumbers = false;
            ReportFactoryHelper.AddPageFooterElement(this.parent, this.root, caption, showPageNumbers);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "pageFooter");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "caption");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == caption);
        }

        [Test]
        public void AddPageFooterElement_WhereEmptyCaptionAndShowPageNumbers_ExpectPageFooterElementWithPageNumbersAttribute()
        {
            string caption = string.Empty;
            const bool showPageNumbers = true;
            ReportFactoryHelper.AddPageFooterElement(this.parent, this.root, caption, showPageNumbers);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "pageFooter");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "pageNumbers");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == showPageNumbers.ToString().ToLower());
        }

        [Test]
        public void AddPageFooterElement_WhereEmptyCaptionAndNotShowPageNumbers_ExpectNoElement()
        {
            const string caption = "";
            const bool showPageNumbers = false;
            ReportFactoryHelper.AddPageFooterElement(this.parent, this.root, caption, showPageNumbers);
            Assert.True(this.parent.ChildNodes.Count == 0);
            Assert.True(this.parent.InnerText == string.Empty);
        }

        [Test]
        public void AddPageFooterElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddPageFooterElement(null, this.root, null, false));
        }

        [Test]
        public void AddPageFooterElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddPageFooterElement(this.parent, null, null, false));
        }





        /// <summary>
        /// Add Page Header Element unit tests
        /// </summary>
        [Test]
        public void AddPageHeaderElement_WhereCaptionAndShowPageNumbers_ExpectPageHeaderElementWithCaptionAndPageNumbersAttributes()
        {
            const string caption = "Caption Value";
            const bool showPageNumbers = true;
            ReportFactoryHelper.AddPageHeaderElement(this.parent, this.root, caption, showPageNumbers);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "pageHeader");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 2);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "caption");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == caption);
            Assert.True(this.parent.ChildNodes[0].Attributes[1].Name == "pageNumbers");
            Assert.True(this.parent.ChildNodes[0].Attributes[1].Value == showPageNumbers.ToString().ToLower());
        }

        [Test]
        public void AddPageHeaderElement_WhereCaptionAndNotShowPageNumbers_ExpectPageHeaderElementWithCaptionAttribute()
        {
            const string caption = "Caption Value";
            const bool showPageNumbers = false;
            ReportFactoryHelper.AddPageHeaderElement(this.parent, this.root, caption, showPageNumbers);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "pageHeader");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "caption");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == caption);
        }

        [Test]
        public void AddPageHeaderElement_WhereEmptyCaptionAndShowPageNumbers_ExpectPageHeaderElementWithPageNumbersAttribute()
        {
            string caption = string.Empty;
            const bool showPageNumbers = true;
            ReportFactoryHelper.AddPageHeaderElement(this.parent, this.root, caption, showPageNumbers);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "pageHeader");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "pageNumbers");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == showPageNumbers.ToString().ToLower());
        }

        [Test]
        public void AddPageHeaderElement_WhereEmptyCaptionAndNotShowPageNumbers_ExpectNoElement()
        {
            const string caption = "";
            const bool showPageNumbers = false;
            ReportFactoryHelper.AddPageHeaderElement(this.parent, this.root, caption, showPageNumbers);
            Assert.True(this.parent.ChildNodes.Count == 0);
            Assert.True(this.parent.InnerText == string.Empty);
        }

        [Test]
        public void AddPageHeaderElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddPageHeaderElement(null, this.root, null, false));
        }

        [Test]
        public void AddPageHeaderElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddPageHeaderElement(this.parent, null, null, false));
        }





        /// <summary>
        /// Add Paragraph Element unit tests
        /// </summary>
        [Test]
        public void AddParagraphElement_WhereParagraph_ExpectParagraphElementWithParagraph()
        {
            const string paragraph = "Paragraph Value";
            ReportFactoryHelper.AddParagraphElement(this.parent, this.root, paragraph);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "paragraph");
            Assert.True(this.parent.ChildNodes[0].InnerText == paragraph);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddParagraphElement_WhereNullParagraph_ExpectParagraphElementWithEmptyParagraph()
        {
            const string paragraph = null;
            ReportFactoryHelper.AddParagraphElement(this.parent, this.root, paragraph);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "paragraph");
            Assert.True(string.IsNullOrWhiteSpace(this.parent.ChildNodes[0].InnerText) == string.IsNullOrWhiteSpace(paragraph));
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddParagraphElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddParagraphElement(null, this.root, "Paragraph Value"));
        }

        [Test]
        public void AddParagraphElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddParagraphElement(this.parent, null, "Paragraph Value"));
        }





        /// <summary>
        /// Add Report Element unit tests
        /// </summary>
        [Test]
        public void AddReportElement_WhereOutputToA4PortraitAndNotAllowSpace_ExpectReportElementWithOutputToA4PortraitWithNoHeaderOrFooter()
        {
            const ReportFactoryHelper.ReportOutputToOption outputTo = ReportFactoryHelper.ReportOutputToOption.A4Portrait;
            const bool allowSpaceForHeaderAndFooter = false;
            XmlElement node = ReportFactoryHelper.AddReportElement(this.root, outputTo, allowSpaceForHeaderAndFooter);
            Assert.True(node != null);
            Assert.True(node.Name == "report");
            Assert.True(node.InnerText == string.Empty);
            Assert.True(node.Attributes.Count == 1);
            Assert.True(node.Attributes[0].Name == "outputTo");
            Assert.True(node.Attributes[0].Value == ReportFactoryHelper.GetReportOutputToAttributeValue(outputTo, allowSpaceForHeaderAndFooter));
        }

        [Test]
        public void AddReportElement_WhereOutputToA4PortraitAndAllowSpace_ExpectReportElementWithOutputToA4Portrait()
        {
            const ReportFactoryHelper.ReportOutputToOption outputTo = ReportFactoryHelper.ReportOutputToOption.A4Portrait;
            const bool allowSpaceForHeaderAndFooter = true;
            XmlElement node = ReportFactoryHelper.AddReportElement(this.root, outputTo, allowSpaceForHeaderAndFooter);
            Assert.True(node != null);
            Assert.True(node.Name == "report");
            Assert.True(node.InnerText == string.Empty);
            Assert.True(node.Attributes.Count == 1);
            Assert.True(node.Attributes[0].Name == "outputTo");
            Assert.True(node.Attributes[0].Value == ReportFactoryHelper.GetReportOutputToAttributeValue(outputTo, allowSpaceForHeaderAndFooter));
        }

        [Test]
        public void AddReportElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddReportElement(null, ReportFactoryHelper.ReportOutputToOption.A4Portrait, false));
        }

        [Test]
        public void AddReportElement_WhereOutputToUnknown_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddReportElement(this.root, ReportFactoryHelper.ReportOutputToOption.Unknown, false));
        }





        /// <summary>
        /// Add Section Column Element unit tests
        /// </summary>
        [Test]
        public void AddSectionColumnElement_WhereProportionalWidth_ExpectSectionColumnElementWithWidth()
        {
            int? proportionalWidth = 2;
            XmlElement node = ReportFactoryHelper.AddSectionColumnElement(this.parent, this.root, proportionalWidth);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "sectionColumn");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "width");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == proportionalWidth.ToString());
        }

        [Test]
        public void AddSectionColumnElement_WhereNullProportionalWidth_ExpectSectionColumnElement()
        {
            int? proportionalWidth = null;
            XmlElement node = ReportFactoryHelper.AddSectionColumnElement(this.parent, this.root, proportionalWidth);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "sectionColumn");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddSectionColumnElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddSectionColumnElement(null, this.root, null));
        }

        [Test]
        public void AddSectionColumnElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddSectionColumnElement(this.parent, null, null));
        }





        /// <summary>
        /// Add Section Element unit tests
        /// </summary>
        [Test]
        public void AddSectionElement_WhereHappy_ExpectSectionElement()
        {
            XmlElement node = ReportFactoryHelper.AddSectionElement(this.parent, this.root);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "section");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddSectionElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddSectionElement(null, this.root));
        }

        [Test]
        public void AddSectionElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddSectionElement(this.parent, null));
        }





        /// <summary>
        /// Add Table Cell Element unit tests
        /// </summary>
        [Test]
        public void AddTableCellElement_WhereContentAndLeftAlign_ExpectTableCellElementWithContent()
        {
            const string content = "Content Value";
            const ReportFactoryHelper.CellAlignOption align = ReportFactoryHelper.CellAlignOption.Left;
            ReportFactoryHelper.AddTableCellElement(this.parent, this.root, content, align);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "cell");
            Assert.True(this.parent.ChildNodes[0].InnerText == content);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddTableCellElement_WhereContentAndRightAlign_ExpectTableCellElementWithContentAndAlignAttribute()
        {
            const string content = "Content Value";
            const ReportFactoryHelper.CellAlignOption align = ReportFactoryHelper.CellAlignOption.Right;
            ReportFactoryHelper.AddTableCellElement(this.parent, this.root, content, align);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "cell");
            Assert.True(this.parent.ChildNodes[0].InnerText == content);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "align");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == ReportFactoryHelper.GetCellAlignAttributeValue(align));
        }

        [Test]
        public void AddTableCellElement_WhereNullContentAndLeftAlign_ExpectTableCellElementWithEmptyContent()
        {
            const string content = null;
            const ReportFactoryHelper.CellAlignOption align = ReportFactoryHelper.CellAlignOption.Left;
            ReportFactoryHelper.AddTableCellElement(this.parent, this.root, content, align);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "cell");
            Assert.True(string.IsNullOrWhiteSpace(this.parent.ChildNodes[0].InnerText) == string.IsNullOrWhiteSpace(content));
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddTableCellElement_WhereEmptyContentAndCenterAlign_ExpectTableCellElementWithContentAndAlignAttribute()
        {
            string content = string.Empty;
            const ReportFactoryHelper.CellAlignOption align = ReportFactoryHelper.CellAlignOption.Center;
            ReportFactoryHelper.AddTableCellElement(this.parent, this.root, content, align);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "cell");
            Assert.True(this.parent.ChildNodes[0].InnerText == content);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "align");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == ReportFactoryHelper.GetCellAlignAttributeValue(align));
        }

        [Test]
        public void AddTableCellElement_WhereWhiteSpaceContentAndCenterAlign_ExpectTableCellElementWithContentAndAlignAttribute()
        {
            const string content = WhiteSpace;
            const ReportFactoryHelper.CellAlignOption align = ReportFactoryHelper.CellAlignOption.Center;
            ReportFactoryHelper.AddTableCellElement(this.parent, this.root, content, align);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "cell");
            Assert.True(this.parent.ChildNodes[0].InnerText == content);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "align");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == ReportFactoryHelper.GetCellAlignAttributeValue(align));
        }

        [Test]
        public void AddTableCellElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableCellElement(null, this.root, "ContentValue", ReportFactoryHelper.CellAlignOption.Left));
        }

        [Test]
        public void AddTableCellElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableCellElement(this.parent, null, "ContentValue", ReportFactoryHelper.CellAlignOption.Left));
        }





        /// <summary>
        /// Add Table Column Element unit tests
        /// </summary>
        [Test]
        public void AddTableColumnElement_WhereProportionalWidth_ExpectTableColumnElementWithWidthAttribute()
        {
            int? proportionalWidth = 2;
            ReportFactoryHelper.AddTableColumnElement(this.parent, this.root, proportionalWidth);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "column");
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "width");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == proportionalWidth.Value.ToString());
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 0);
        }

        [Test]
        public void AddTableColumnElement_WhereZeroProportionalWidth_ExpectTableColumnElement()
        {
            int? proportionalWidth = 0;
            ReportFactoryHelper.AddTableColumnElement(this.parent, this.root, proportionalWidth);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "column");
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 0);
        }

        [Test]
        public void AddTableColumnElement_WhereNullProportionalWidth_ExpectTableColumnElement()
        {
            int? proportionalWidth = null;
            ReportFactoryHelper.AddTableColumnElement(this.parent, this.root, proportionalWidth);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "column");
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 0);
        }

        [Test]
        public void AddTableColumnElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableColumnElement(null, this.root, null));
        }

        [Test]
        public void AddTableColumnElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableColumnElement(this.parent, null, null));
        }





        /// <summary>
        /// Add Table Columns Element unit tests
        /// </summary>
        [Test]
        public void AddTableColumnsElement_WhereTwoExplicitColumns_ExpectTableColumnsElementAndTwoColumnElementsWithWidthAttribute()
        {
            int?[] columnProportionalWidths = new int?[] { 1, 2 };
            ReportFactoryHelper.AddTableColumnsElement(this.parent, this.root, columnProportionalWidths);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "columns");
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 2);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Name == "column");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Name == "width");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Value == columnProportionalWidths[0].Value.ToString());
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].Name == "column");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].Attributes[0].Name == "width");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].Attributes[0].Value == columnProportionalWidths[1].Value.ToString());
        }

        [Test]
        public void AddTableColumnsElement_WhereOneExplicitColumn_ExpectTableColumnsElementAndOneColumnElementWithWidthAttribute()
        {
            int?[] columnProportionalWidths = new int?[] { 3 };
            ReportFactoryHelper.AddTableColumnsElement(this.parent, this.root, columnProportionalWidths);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "columns");
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Name == "column");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Name == "width");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes[0].Value == columnProportionalWidths[0].Value.ToString());
        }

        [Test]
        public void AddTableColumnsElement_WhereTwoImplicitColumns_ExpectTableColumnsElementAndTwoColumnElements()
        {
            int?[] columnProportionalWidths = new int?[] { null, null };
            ReportFactoryHelper.AddTableColumnsElement(this.parent, this.root, columnProportionalWidths);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "columns");
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 2);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Name == "column");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes.Count == 0);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].Name == "column");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].Attributes.Count == 0);
        }

        [Test]
        public void AddTableColumnsElement_WhereOneImplicitAndOneExplicitColumn_ExpectTableColumnsElementAndTwoColumnElementsSecondWithWidthAttribute()
        {
            int?[] columnProportionalWidths = new int?[] { null, 2 };
            ReportFactoryHelper.AddTableColumnsElement(this.parent, this.root, columnProportionalWidths);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "columns");
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
            Assert.True(this.parent.ChildNodes[0].ChildNodes.Count == 2);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Name == "column");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[0].Attributes.Count == 0);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].Name == "column");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].Attributes[0].Name == "width");
            Assert.True(this.parent.ChildNodes[0].ChildNodes[1].Attributes[0].Value == columnProportionalWidths[1].Value.ToString());
        }

        [Test]
        public void AddTableColumnsElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableColumnsElement(null, this.root, new int?[] { 1, 2 }));
        }

        [Test]
        public void AddTableColumnsElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableColumnsElement(this.parent, null, new int?[] { 1, 2 }));
        }

        [Test]
        public void AddTableColumnsElement_WhereColumnProportionalWidthsIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableColumnsElement(this.parent, this.root, null));
        }

        [Test]
        public void AddTableColumnsElement_WhereColumnProportionalWidthsIsEmpty_ExpectArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ReportFactoryHelper.AddTableColumnsElement(this.parent, this.root, new int?[] { }));
        }





        /// <summary>
        /// Add Table Element unit tests
        /// </summary>
        [Test]
        public void AddTableElement_WhereNullWidthNotBordersNotShading_ExpectTableElement()
        {
            int? width = null;
            bool useBorders = false;
            bool useShading = false;
            XmlElement node = ReportFactoryHelper.AddTableElement(this.parent, this.root, width, useBorders, useShading);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "table");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddTableElement_WhereWidthBordersShading_ExpectTableElementWithWidthBordersAndUseShadingAttributes()
        {
            int? width = 5;
            bool useBorders = true;
            bool useShading = true;
            XmlElement node = ReportFactoryHelper.AddTableElement(this.parent, this.root, width, useBorders, useShading);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "table");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 3);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "width");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == width.Value.ToString() + "%");
            Assert.True(this.parent.ChildNodes[0].Attributes[1].Name == "borders");
            Assert.True(this.parent.ChildNodes[0].Attributes[1].Value == useBorders.ToString().ToLower());
            Assert.True(this.parent.ChildNodes[0].Attributes[2].Name == "useShading");
            Assert.True(this.parent.ChildNodes[0].Attributes[2].Value == useShading.ToString().ToLower());
        }

        [Test]
        public void AddTableElement_WhereWidthNoBordersNoShading_ExpectTableElementWithWidthAttribute()
        {
            int? width = 5;
            bool useBorders = false;
            bool useShading = false;
            XmlElement node = ReportFactoryHelper.AddTableElement(this.parent, this.root, width, useBorders, useShading);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "table");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "width");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == width.Value.ToString() + "%");
        }

        [Test]
        public void AddTableElement_WhereNullWidthBordersNoShading_ExpectTableElementWithBordersAttribute()
        {
            int? width = null;
            bool useBorders = true;
            bool useShading = false;
            XmlElement node = ReportFactoryHelper.AddTableElement(this.parent, this.root, width, useBorders, useShading);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "table");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "borders");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == useBorders.ToString().ToLower());
        }

        [Test]
        public void AddTableElement_WhereNullWidthNoBordersShading_ExpectTableElementWithUseShadingAttribute()
        {
            int? width = null;
            bool useBorders = false;
            bool useShading = true;
            XmlElement node = ReportFactoryHelper.AddTableElement(this.parent, this.root, width, useBorders, useShading);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "table");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "useShading");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == useShading.ToString().ToLower());
        }

        [Test]
        public void AddTableElement_WhereTooLowWidthNoBordersNoShading_ExpectTableElementWithWidthAttributeWithMinimumValue()
        {
            int? width = -1;
            bool useBorders = false;
            bool useShading = false;
            XmlElement node = ReportFactoryHelper.AddTableElement(this.parent, this.root, width, useBorders, useShading);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "table");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "width");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == 0.ToString() + "%");
        }

        [Test]
        public void AddTableElement_WhereTooHighWidthNoBordersNoShading_ExpectTableElementWithWidthAttributeWithMaximumValue()
        {
            int? width = 101;
            bool useBorders = false;
            bool useShading = false;
            XmlElement node = ReportFactoryHelper.AddTableElement(this.parent, this.root, width, useBorders, useShading);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "table");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "width");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == 100.ToString() + "%");
        }

        [Test]
        public void AddTableElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableElement(null, this.root, null, false, false));
        }

        [Test]
        public void AddTableElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableElement(this.parent, null, null, false, false));
        }





        /// <summary>
        /// Add Table Footer Element unit tests
        /// </summary>
        [Test]
        public void AddTableFooterElement_WhereHappy_ExpectTableFooterElement()
        {
            XmlElement node = ReportFactoryHelper.AddTableFooterElement(this.parent, this.root);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "footer");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddTableFooterElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableFooterElement(null, this.root));
        }

        [Test]
        public void AddTableFooterElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableFooterElement(this.parent, null));
        }





        /// <summary>
        /// Add Table Header Element unit tests
        /// </summary>
        [Test]
        public void AddTableHeaderElement_WhereHappy_ExpectTableHeaderElement()
        {
            XmlElement node = ReportFactoryHelper.AddTableHeaderElement(this.parent, this.root);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "header");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddTableHeaderElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableHeaderElement(null, this.root));
        }

        [Test]
        public void AddTableHeaderElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableHeaderElement(this.parent, null));
        }





        /// <summary>
        /// Add Table Row Element unit tests
        /// </summary>
        [Test]
        public void AddTableRowElement_WhereNotUseShadingAndNotShaded_ExpectTableRowElement()
        {
            bool useShading = false;
            bool shaded = false;
            XmlElement node = ReportFactoryHelper.AddTableRowElement(this.parent, this.root, useShading, shaded);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "row");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddTableRowElement_WhereUseShadingAndNotShaded_ExpectTableRowElement()
        {
            bool useShading = true;
            bool shaded = false;
            XmlElement node = ReportFactoryHelper.AddTableRowElement(this.parent, this.root, useShading, shaded);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "row");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddTableRowElement_WhereNotUseShadingAndShaded_ExpectTableRowElement()
        {
            bool useShading = false;
            bool shaded = true;
            XmlElement node = ReportFactoryHelper.AddTableRowElement(this.parent, this.root, useShading, shaded);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "row");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 0);
        }

        [Test]
        public void AddTableRowElement_WhereUseShadingAndShaded_ExpectTableRowElementWithShadeAttribute()
        {
            bool useShading = true;
            bool shaded = true;
            XmlElement node = ReportFactoryHelper.AddTableRowElement(this.parent, this.root, useShading, shaded);
            Assert.True(node != null);
            Assert.True(this.parent.ChildNodes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Name == "row");
            Assert.True(this.parent.ChildNodes[0].InnerText == string.Empty);
            Assert.True(this.parent.ChildNodes[0].Attributes.Count == 1);
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Name == "shade");
            Assert.True(this.parent.ChildNodes[0].Attributes[0].Value == true.ToString().ToLower());
        }

        [Test]
        public void AddTableRowElement_WhereParentIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableRowElement(null, this.root, false, false));
        }

        [Test]
        public void AddTableRowElement_WhereRootIsNull_ExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReportFactoryHelper.AddTableRowElement(this.parent, null, false, false));
        }





        /// <summary>
        /// Get Cell Align Attribute Value unit tests
        /// </summary>
        [Test]
        public void GetCellAlignAttributeValue_WhereHappy_ExpectStringValue()
        {
            const ReportFactoryHelper.CellAlignOption align = ReportFactoryHelper.CellAlignOption.Left;
            string result = ReportFactoryHelper.GetCellAlignAttributeValue(align);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "left");
        }

        [Test]
        public void GetCellAlignAttributeValue_WhereCoverageTest_ExpectPass()
        {
            string result = null;

            // Left
            result = ReportFactoryHelper.GetCellAlignAttributeValue(ReportFactoryHelper.CellAlignOption.Left);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "left");

            // Center
            result = ReportFactoryHelper.GetCellAlignAttributeValue(ReportFactoryHelper.CellAlignOption.Center);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "center");

            // Right
            result = ReportFactoryHelper.GetCellAlignAttributeValue(ReportFactoryHelper.CellAlignOption.Right);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "right");
        }





        /// <summary>
        /// Get Formatted Boolean unit tests
        /// </summary>
        [Test]
        public void GetFormattedBoolean_WhereTrue_ExpectStringTrueValue()
        {
            const bool value = true;
            string result = ReportFactoryHelper.GetFormattedBoolean(value);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "true");
        }

        [Test]
        public void GetFormattedBoolean_WhereFalse_ExpectStringFalseValue()
        {
            const bool value = false;
            string result = ReportFactoryHelper.GetFormattedBoolean(value);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "false");
        }





        /// <summary>
        /// Get Report Output To Attribute Value unit tests
        /// </summary>        
        [Test]
        public void GetReportOutputToAttributeValue_WhereA4PortraitAndNoSpaceForHeaderAndFooter_ExpectStringValue()
        {
            ReportFactoryHelper.ReportOutputToOption outputTo = ReportFactoryHelper.ReportOutputToOption.A4Portrait;
            bool allowSpaceForHeaderAndFooter = false;
            string result = ReportFactoryHelper.GetReportOutputToAttributeValue(outputTo, allowSpaceForHeaderAndFooter);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result.StartsWith("A4Portrait"));
            Assert.True(result.EndsWith("NoHeaderOrFooter"));
        }

        [Test]
        public void GetReportOutputToAttributeValue_WhereA4PortraitAndSpaceForHeaderAndFooter_ExpectStringValue()
        {
            ReportFactoryHelper.ReportOutputToOption outputTo = ReportFactoryHelper.ReportOutputToOption.A4Portrait;
            bool allowSpaceForHeaderAndFooter = true;
            string result = ReportFactoryHelper.GetReportOutputToAttributeValue(outputTo, allowSpaceForHeaderAndFooter);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "A4Portrait");
        }

        [Test]
        public void GetReportOutputToAttributeValue_WhereA0LandscapeAndNoSpaceForHeaderAndFooter_ExpectStringValue()
        {
            ReportFactoryHelper.ReportOutputToOption outputTo = ReportFactoryHelper.ReportOutputToOption.A0Landscape;
            bool allowSpaceForHeaderAndFooter = false;
            string result = ReportFactoryHelper.GetReportOutputToAttributeValue(outputTo, allowSpaceForHeaderAndFooter);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result.StartsWith("A0Landscape"));
            Assert.True(result.EndsWith("NoHeaderOrFooter"));
        }

        [Test]
        public void GetReportOutputToAttributeValue_WhereA0LandscapeAndSpaceForHeaderAndFooter_ExpectStringValue()
        {
            ReportFactoryHelper.ReportOutputToOption outputTo = ReportFactoryHelper.ReportOutputToOption.A0Landscape;
            bool allowSpaceForHeaderAndFooter = true;
            string result = ReportFactoryHelper.GetReportOutputToAttributeValue(outputTo, allowSpaceForHeaderAndFooter);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "A0Landscape");
        }

        [Test]
        public void GetReportOutputToAttributeValue_WhereUnknownAndNoSpaceForHeaderAndFooter_ExpectNull()
        {
            ReportFactoryHelper.ReportOutputToOption outputTo = ReportFactoryHelper.ReportOutputToOption.Unknown;
            bool allowSpaceForHeaderAndFooter = false;
            string result = ReportFactoryHelper.GetReportOutputToAttributeValue(outputTo, allowSpaceForHeaderAndFooter);
            Assert.True(result == null);
        }

        [Test]
        public void GetReportOutputToAttributeValue_WhereUnknownAndSpaceForHeaderAndFooter_ExpectStringValue()
        {
            ReportFactoryHelper.ReportOutputToOption outputTo = ReportFactoryHelper.ReportOutputToOption.Unknown;
            bool allowSpaceForHeaderAndFooter = true;
            string result = ReportFactoryHelper.GetReportOutputToAttributeValue(outputTo, allowSpaceForHeaderAndFooter);
            Assert.True(result == null);
        }

        [Test]
        public void GetReportOutputToAttributeValue_WhereCoverageTest_ExpectPass()
        {
            string result = null;

            // A0 Portrait with no space for header or footer
            result = ReportFactoryHelper.GetReportOutputToAttributeValue(ReportFactoryHelper.ReportOutputToOption.A0Portrait, false);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result.StartsWith("A0Portrait"));
            Assert.True(result.EndsWith("NoHeaderOrFooter"));

            // A0 Landscape with space for header or footer
            result = ReportFactoryHelper.GetReportOutputToAttributeValue(ReportFactoryHelper.ReportOutputToOption.A0Landscape, true);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "A0Landscape");


            // A1 Portrait with no space for header or footer
            result = ReportFactoryHelper.GetReportOutputToAttributeValue(ReportFactoryHelper.ReportOutputToOption.A1Portrait, false);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result.StartsWith("A1Portrait"));
            Assert.True(result.EndsWith("NoHeaderOrFooter"));

            // A1 Landscape with space for header or footer
            result = ReportFactoryHelper.GetReportOutputToAttributeValue(ReportFactoryHelper.ReportOutputToOption.A1Landscape, true);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "A1Landscape");


            // A2 Portrait with no space for header or footer
            result = ReportFactoryHelper.GetReportOutputToAttributeValue(ReportFactoryHelper.ReportOutputToOption.A2Portrait, false);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result.StartsWith("A2Portrait"));
            Assert.True(result.EndsWith("NoHeaderOrFooter"));

            // A2 Landscape with space for header or footer
            result = ReportFactoryHelper.GetReportOutputToAttributeValue(ReportFactoryHelper.ReportOutputToOption.A2Landscape, true);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "A2Landscape");


            // A3 Portrait with no space for header or footer
            result = ReportFactoryHelper.GetReportOutputToAttributeValue(ReportFactoryHelper.ReportOutputToOption.A3Portrait, false);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result.StartsWith("A3Portrait"));
            Assert.True(result.EndsWith("NoHeaderOrFooter"));

            // A3 Landscape with space for header or footer
            result = ReportFactoryHelper.GetReportOutputToAttributeValue(ReportFactoryHelper.ReportOutputToOption.A3Landscape, true);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "A3Landscape");


            // A4 Portrait with no space for header or footer
            result = ReportFactoryHelper.GetReportOutputToAttributeValue(ReportFactoryHelper.ReportOutputToOption.A4Portrait, false);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result.StartsWith("A4Portrait"));
            Assert.True(result.EndsWith("NoHeaderOrFooter"));

            // A4 Landscape with space for header or footer
            result = ReportFactoryHelper.GetReportOutputToAttributeValue(ReportFactoryHelper.ReportOutputToOption.A4Landscape, true);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "A4Landscape");
        }






        /// <summary>
        /// Get Text Align Attribute Value unit tests
        /// </summary>
        [Test]
        public void GetTextAlignAttributeValue_WhereHappy_ExpectStringValue()
        {
            const ReportFactoryHelper.TextAlignOption align = ReportFactoryHelper.TextAlignOption.Left;
            string result = ReportFactoryHelper.GetTextAlignAttributeValue(align);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "left");
        }

        [Test]
        public void GetTextAlignAttributeValue_WhereCoverageTest_ExpectPass()
        {
            string result = null;

            // Left
            result = ReportFactoryHelper.GetTextAlignAttributeValue(ReportFactoryHelper.TextAlignOption.Left);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "left");

            // Center
            result = ReportFactoryHelper.GetTextAlignAttributeValue(ReportFactoryHelper.TextAlignOption.Center);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "center");

            // Right
            result = ReportFactoryHelper.GetTextAlignAttributeValue(ReportFactoryHelper.TextAlignOption.Right);
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.True(result == "right");
        }
    }
}
