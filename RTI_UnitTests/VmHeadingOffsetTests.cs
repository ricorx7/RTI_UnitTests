using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RTI;

namespace RTI_UnitTests
{
    public class VmHeadingOffsetTests
    {

        /// <summary>
        /// Ensemble 1.
        /// </summary>
        RTI.DataSet.Ensemble ensemble1;

        [SetUp]
        public void Setup()
        {
            // Create the ensemble
            // 4 Beams, 5 Bins
            ensemble1 = EnsembleHelper.GenerateEnsemble(5, 4, true);
            ensemble1.EnsembleData.EnsembleNumber = 12;
            ensemble1.AncillaryData.Heading = 10.2f;
            ensemble1.AncillaryData.Pitch = 1.02f;
            ensemble1.AncillaryData.Roll = 2.123f;
        }

        [Test]
        public void TestProfileHeadingOffset()
        {
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.HeadingOffsetMag = 10.0f;

            // Heading offset
            RTI.VesselMount.VmHeadingOffset.HeadingOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(20.2f, ensemble1.AncillaryData.Heading, 0.001);
        }

        [Test]
        public void TestProfileHeadingOffset0()
        {
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.HeadingOffsetMag = 0.0f;

            // Heading offset
            RTI.VesselMount.VmHeadingOffset.HeadingOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(10.2f, ensemble1.AncillaryData.Heading, 0.001);
        }

        [Test]
        public void TestProfileHeadingOffset360()
        {
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.HeadingOffsetMag = 360.0f;

            // Heading offset
            RTI.VesselMount.VmHeadingOffset.HeadingOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(10.2f, ensemble1.AncillaryData.Heading, 0.001);
        }

        [Test]
        public void TestProfileHeadingOffsetNeg()
        {
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.HeadingOffsetMag = -10.0f;

            // Heading offset
            RTI.VesselMount.VmHeadingOffset.HeadingOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(0.2f, ensemble1.AncillaryData.Heading, 0.001);
        }

        [Test]
        public void TestProfileHeadingOffsetLargeNeg()
        {
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.HeadingOffsetMag = -30.0f;

            // Heading offset
            RTI.VesselMount.VmHeadingOffset.HeadingOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(340.2f, ensemble1.AncillaryData.Heading, 0.001);
        }

        [Test]
        public void TestProfileHeadingOffsetLargePos()
        {
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.HeadingOffsetMag = 370.0f;

            // Heading offset
            RTI.VesselMount.VmHeadingOffset.HeadingOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(20.2f, ensemble1.AncillaryData.Heading, 0.001);
        }

    }
}
