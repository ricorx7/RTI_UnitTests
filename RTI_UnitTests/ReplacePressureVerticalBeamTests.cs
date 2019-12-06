using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RTI;


namespace RTI_UnitTests
{
    public class ReplacePressureVerticalBeamTests
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
            ensemble1.AncillaryData.TransducerDepth = 5.5f;
        }

        /// <summary>
        /// Not the correct subsystem type.
        /// </summary>
        [Test]
        public void TestReplaceFailNotCorrectSS()
        {
            bool result = RTI.ScreenData.ReplacePressureVerticalBeam.Replace(ref ensemble1);

            Assert.AreEqual(10.2f, ensemble1.AncillaryData.Heading, 0.001);
            Assert.AreEqual(5.5f, ensemble1.AncillaryData.TransducerDepth, 0.001);
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Correct Subsystem code but no good RT Value
        /// </summary>
        [Test]
        public void TestReplaceSSNoRT()
        {
            RTI.DataSet.Ensemble ensemble2 = ensemble1.Clone();

            // Change Subsystem
            ensemble2.EnsembleData.SubsystemConfig.SubSystem.Code = Subsystem.SUB_300KHZ_VERT_PISTON_C;

            // Add Range Tracking
            bool result = RTI.ScreenData.ReplacePressureVerticalBeam.Replace(ref ensemble2);

            Assert.AreEqual(5.5f, ensemble2.AncillaryData.TransducerDepth, 0.001);
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Correct Subsystem code but no good RT Value
        /// </summary>
        [Test]
        public void TestReplace()
        {
            // Create a new ensemble with 1 beam
            RTI.DataSet.Ensemble ensemble2 = EnsembleHelper.GenerateEnsemble(5, 1, true);

            // Change Subsystem
            ensemble2.EnsembleData.SubsystemConfig.SubSystem.Code = Subsystem.SUB_300KHZ_VERT_PISTON_C;

            // Set the Transducer Depth
            ensemble2.AncillaryData.TransducerDepth = 7.5f;

            // Range Track
            ensemble2.RangeTrackingData.Range[RTI.DataSet.Ensemble.BEAM_0_INDEX] = 3.123f;

            // Add Range Tracking
            bool result = RTI.ScreenData.ReplacePressureVerticalBeam.Replace(ref ensemble2);

            Assert.AreEqual(3.123f, ensemble2.AncillaryData.TransducerDepth, 0.001);
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Correct Subsystem code but no good RT Value
        /// </summary>
        [Test]
        public void TestReplaceBadTransducerDepth()
        {
            // Create a new ensemble with 1 beam
            RTI.DataSet.Ensemble ensemble2 = EnsembleHelper.GenerateEnsemble(5, 1, true);

            // Change Subsystem
            ensemble2.EnsembleData.SubsystemConfig.SubSystem.Code = Subsystem.SUB_300KHZ_VERT_PISTON_C;

            // Set the Transducer Depth
            ensemble2.AncillaryData.TransducerDepth = 0.0f;

            // Range Track
            ensemble2.RangeTrackingData.Range[RTI.DataSet.Ensemble.BEAM_0_INDEX] = 3.123f;

            // Add Range Tracking
            bool result = RTI.ScreenData.ReplacePressureVerticalBeam.Replace(ref ensemble2);

            Assert.AreEqual(3.123f, ensemble2.AncillaryData.TransducerDepth, 0.001);
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Correct Subsystem code but no good RT Value
        /// </summary>
        [Test]
        public void TestReplaceBadRT()
        {
            // Create a new ensemble with 1 beam
            RTI.DataSet.Ensemble ensemble2 = EnsembleHelper.GenerateEnsemble(5, 1, true);

            // Change Subsystem
            ensemble2.EnsembleData.SubsystemConfig.SubSystem.Code = Subsystem.SUB_300KHZ_VERT_PISTON_C;

            // Set the Transducer Depth
            ensemble2.AncillaryData.TransducerDepth = 8.7f;

            // Range Track
            ensemble2.RangeTrackingData.Range[RTI.DataSet.Ensemble.BEAM_0_INDEX] = RTI.DataSet.Ensemble.BAD_RANGE;

            // Add Range Tracking
            bool result = RTI.ScreenData.ReplacePressureVerticalBeam.Replace(ref ensemble2);

            Assert.AreEqual(8.7f, ensemble2.AncillaryData.TransducerDepth, 0.001);
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Correct Subsystem code but no good RT Value
        /// </summary>
        [Test]
        public void TestReplace0RT()
        {
            // Create a new ensemble with 1 beam
            RTI.DataSet.Ensemble ensemble2 = EnsembleHelper.GenerateEnsemble(5, 1, true);

            // Change Subsystem
            ensemble2.EnsembleData.SubsystemConfig.SubSystem.Code = Subsystem.SUB_300KHZ_VERT_PISTON_C;

            // Set the Transducer Depth
            ensemble2.AncillaryData.TransducerDepth = 8.7f;

            // Range Track
            ensemble2.RangeTrackingData.Range[RTI.DataSet.Ensemble.BEAM_0_INDEX] = 0.0f;

            // Add Range Tracking
            bool result = RTI.ScreenData.ReplacePressureVerticalBeam.Replace(ref ensemble2);

            Assert.AreEqual(8.7f, ensemble2.AncillaryData.TransducerDepth, 0.001);
            Assert.AreEqual(false, result);
        }

    }
}
