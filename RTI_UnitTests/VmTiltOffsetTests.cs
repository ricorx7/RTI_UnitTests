using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RTI;

namespace RTI_UnitTests
{
    public class VmTiltOffsetTests
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
        public void TestPitchOffset()
        {
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.PitchOffset = 10.0f;

            // Heading offset
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(11.02f, ensemble1.AncillaryData.Pitch, 0.001);
        }

        [Test]
        public void TestRollOffset()
        {
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.RollOffset = 10.0f;

            // Heading offset
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(12.123f, ensemble1.AncillaryData.Roll, 0.001);
        }

        [Test]
        public void TestPitchOffset0()
        {
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.PitchOffset = 0.0f;

            // Heading offset
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(1.02f, ensemble1.AncillaryData.Pitch, 0.001);
        }

        [Test]
        public void TestRollOffset0()
        {
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.RollOffset = 0.0f;

            // Heading offset
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(2.123f, ensemble1.AncillaryData.Roll, 0.001);
        }

        [Test]
        public void TestPitchOffset90()
        {
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.PitchOffset = 90.0f;

            // Heading offset
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(-88.98f, ensemble1.AncillaryData.Pitch, 0.001);
        }

        [Test]
        public void TestRollOffset180()
        {
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.RollOffset = 180.0f;

            // Heading offset
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(-177.877f, ensemble1.AncillaryData.Roll, 0.001);
        }

        [Test]
        public void TestPitchOffsetNeg90()
        {
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.PitchOffset = -90.0f;

            // Heading offset
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(-88.98f, ensemble1.AncillaryData.Pitch, 0.001);
        }

        [Test]
        public void TestRollOffsetNeg180()
        {
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.RollOffset = -180.0f;

            // Heading offset
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(-177.877f, ensemble1.AncillaryData.Roll, 0.001);
        }

        [Test]
        public void TestRollOffsetNegVal()
        {
            ensemble1.AncillaryData.Roll = -20.0f;

            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.RollOffset = 30.0f;

            // Heading offset
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(10.0f, ensemble1.AncillaryData.Roll, 0.001);
        }

        [Test]
        public void TestRollOffsetWrap()
        {
            ensemble1.AncillaryData.Roll = 170.0f;

            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.RollOffset = 30.0f;

            // Heading offset
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(-160.0f, ensemble1.AncillaryData.Roll, 0.001);
        }

        [Test]
        public void TestPitchOffsetNegVal()
        {
            ensemble1.AncillaryData.Pitch = -20.0f;

            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.PitchOffset = 30.0f;

            // Heading offset
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(10.0f, ensemble1.AncillaryData.Pitch, 0.001);
        }

        [Test]
        public void TestPItchOffsetWrap()
        {
            ensemble1.AncillaryData.Pitch = 70.0f;

            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.PitchOffset = 30.0f;

            // Heading offset
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble1, vmOptions, true);

            Assert.AreEqual(-80.0f, ensemble1.AncillaryData.Pitch, 0.001);
        }
    }
}
