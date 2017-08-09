using NUnit.Framework;
using RM.CommonLibrary.Reporting.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.CommonLibrary.Reporting.Test
{
    /// <summary>
    /// Tests the PDF Generator class
    /// </summary>
    [TestFixture]
    public class PdfGeneratorFixture
    {
        /// <summary>
        /// Performs set up operations prior to each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Do nothing
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
            // Do nothing
        }





        // No tests implemented because:
        // * External dependencies - requires an output folder and application configuration
        // * Limited value - the tests would only be able to determine whether or not a file was created as 
        //     verifying the content of the PDF requires human interaction
    }
}
