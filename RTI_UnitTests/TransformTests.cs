using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RTI;

namespace RTI_UnitTests
{
    public class TransformTests
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

            float beamVel = 1.00f;
            float beamVelInc = 0.2f;
            // Create the Beam Velocity Data
            // 4 Beams, 5 Bins
            for(int binNum = 0; binNum < 5; binNum++)
            {
                for(int beamNum = 0; beamNum < 4; beamNum++)
                {
                    ensemble1.BeamVelocityData.BeamVelocityData[binNum, beamNum] = beamVel + beamVelInc;
                    beamVelInc *= 2;
                }
            }
        }

        [Test]
        public void TestBeam()
        {
            Assert.AreEqual(ensemble1.EnsembleData.EnsembleNumber, 12);
            Assert.AreEqual(ensemble1.AncillaryData.Heading, 10.2f);
            Assert.AreEqual(ensemble1.AncillaryData.Pitch, 1.02f);
            Assert.AreEqual(ensemble1.AncillaryData.Roll, 2.123f);

            float beamVel = 1.00f;
            float beamVelInc = 0.2f;
            // Check the Beam Velocity Data
            // 4 Beams, 5 Bins
            for (int binNum = 0; binNum < 5; binNum++)
            {
                for (int beamNum = 0; beamNum < 4; beamNum++)
                {
                    Assert.AreEqual(beamVel + beamVelInc, ensemble1.BeamVelocityData.BeamVelocityData[binNum, beamNum]);
                    beamVelInc *= 2;
                }
            }
        }

        /// <summary>
        /// Correlation Threshold failed.
        /// </summary>
        [Test]
        public void TransformBadCorrelation()
        {
            // Transform the data
            Transform.ProfileTransform(ref ensemble1, AdcpCodec.CodecEnum.Binary, 0.25f, Transform.HeadingSource.ADCP, 0, 0);

            Assert.IsTrue(ensemble1.IsInstrumentVelocityAvail);

            float beamVel = 1.00f;
            float beamVelInc = 0.2f;
            // Check the Beam Velocity Data
            // 4 Beams, 5 Bins
            for (int binNum = 0; binNum < 5; binNum++)
            {
                for (int beamNum = 0; beamNum < 4; beamNum++)
                {
                    Assert.AreEqual(beamVel + beamVelInc, ensemble1.BeamVelocityData.BeamVelocityData[binNum, beamNum]);
                    beamVelInc *= 2;
                }
            }

            // Check the Instrument Velocity Data
            // 4 Beams, 5 Bins
            for (int binNum = 0; binNum < 5; binNum++)
            {
                for (int beamNum = 0; beamNum < 4; beamNum++)
                {
                    Assert.AreEqual(88.888f, ensemble1.InstrumentVelocityData.InstrumentVelocityData[binNum, beamNum]);
                }
            }

            // Check the Earth Velocity Data
            // 4 Beams, 5 Bins
            for (int binNum = 0; binNum < 5; binNum++)
            {
                for (int beamNum = 0; beamNum < 4; beamNum++)
                {
                    Assert.AreEqual(88.888f, ensemble1.EarthVelocityData.EarthVelocityData[binNum, beamNum]);
                }
            }
        }


        /// <summary>
        /// Set Correlation to 0 to ignore
        /// </summary>
        [Test]
        public void TransformCorrelation0()
        {
            float correlationThreshold = 0.0f;

            // Transform the data
            Transform.ProfileTransform(ref ensemble1, AdcpCodec.CodecEnum.Binary, correlationThreshold, Transform.HeadingSource.ADCP, 0, 0);

            Assert.IsTrue(ensemble1.IsInstrumentVelocityAvail);

            //float beamVel = 1.00f;
            //float beamVelInc = 0.2f;
            //// Check the Beam Velocity Data
            //// 4 Beams, 5 Bins
            //for (int binNum = 0; binNum < 5; binNum++)
            //{
            //    for (int beamNum = 0; beamNum < 4; beamNum++)
            //    {
            //        Assert.AreEqual(beamVel + beamVelInc, ensemble1.BeamVelocityData.BeamVelocityData[binNum, beamNum]);
            //        beamVelInc *= 2;
            //    }
            //}

            // Check the Instrument Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(0.29238f, ensemble1.InstrumentVelocityData.InstrumentVelocityData[0, 0], 0.001);
            Assert.AreEqual(1.16952, ensemble1.InstrumentVelocityData.InstrumentVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.862311, ensemble1.InstrumentVelocityData.InstrumentVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble1.InstrumentVelocityData.InstrumentVelocityData[0, 3], 0.001);

            Assert.AreEqual(4.6780, ensemble1.InstrumentVelocityData.InstrumentVelocityData[1, 0], 0.001);
            Assert.AreEqual(18.71234, ensemble1.InstrumentVelocityData.InstrumentVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.8343, ensemble1.InstrumentVelocityData.InstrumentVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble1.InstrumentVelocityData.InstrumentVelocityData[1, 3], 0.001);

            // Check the Earth Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(-1.160649, ensemble1.EarthVelocityData.EarthVelocityData[0, 0], 0.001);
            Assert.AreEqual(0.53873, ensemble1.EarthVelocityData.EarthVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.812215, ensemble1.EarthVelocityData.EarthVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble1.EarthVelocityData.EarthVelocityData[0, 3], 0.001);

            Assert.AreEqual(-18.038677, ensemble1.EarthVelocityData.EarthVelocityData[1, 0], 0.001);
            Assert.AreEqual(8.23564, ensemble1.EarthVelocityData.EarthVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.04626, ensemble1.EarthVelocityData.EarthVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble1.EarthVelocityData.EarthVelocityData[1, 3], 0.001);

            // All error velocities should be the same between instrument and earth velocity
            Assert.AreEqual(ensemble1.InstrumentVelocityData.InstrumentVelocityData[2, 3], ensemble1.EarthVelocityData.EarthVelocityData[2, 3], 0.001);
            Assert.AreEqual(ensemble1.InstrumentVelocityData.InstrumentVelocityData[3, 3], ensemble1.EarthVelocityData.EarthVelocityData[3, 3], 0.001);
            Assert.AreEqual(ensemble1.InstrumentVelocityData.InstrumentVelocityData[4, 3], ensemble1.EarthVelocityData.EarthVelocityData[4, 3], 0.001);

        }

        /// <summary>
        /// Set Correlation to larger then correlation threshold.
        /// </summary>
        [Test]
        public void TransformCorrelationSetValue()
        {
            float correlationThreshold = 25.0f;

            // Clone the current ensemble
            RTI.DataSet.Ensemble ensemble2 = ensemble1.Clone();

            // Set the Correlation Data
            // 4 Beams, 5 Bins
            for (int binNum = 0; binNum < 5; binNum++)
            {
                for (int beamNum = 0; beamNum < 4; beamNum++)
                {
                    ensemble2.CorrelationData.CorrelationData[binNum, beamNum] = correlationThreshold + 1.0f;
                }
            }

            Assert.IsTrue(ensemble2.IsInstrumentVelocityAvail);
            Assert.IsTrue(ensemble2.IsCorrelationAvail);

            // Transform the data
            Transform.ProfileTransform(ref ensemble2, AdcpCodec.CodecEnum.Binary, correlationThreshold, Transform.HeadingSource.ADCP, 0, 0);

            // Check the Instrument Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(0.29238f, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 0], 0.001);
            Assert.AreEqual(1.16952, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.862311, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 3], 0.001);

            Assert.AreEqual(4.6780, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 0], 0.001);
            Assert.AreEqual(18.71234, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.8343, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 3], 0.001);

            // Check the Earth Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(-1.160649, ensemble2.EarthVelocityData.EarthVelocityData[0, 0], 0.001);
            Assert.AreEqual(0.53873, ensemble2.EarthVelocityData.EarthVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.812215, ensemble2.EarthVelocityData.EarthVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.EarthVelocityData.EarthVelocityData[0, 3], 0.001);

            Assert.AreEqual(-18.038677, ensemble2.EarthVelocityData.EarthVelocityData[1, 0], 0.001);
            Assert.AreEqual(8.23564, ensemble2.EarthVelocityData.EarthVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.04626, ensemble2.EarthVelocityData.EarthVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.EarthVelocityData.EarthVelocityData[1, 3], 0.001);

            // All error velocities should be the same between instrument and earth velocity
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[2, 3], ensemble2.EarthVelocityData.EarthVelocityData[2, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[3, 3], ensemble2.EarthVelocityData.EarthVelocityData[3, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[4, 3], ensemble2.EarthVelocityData.EarthVelocityData[4, 3], 0.001);

        }

        /// <summary>
        /// Set the heading offset for new transformed data
        /// </summary>
        [Test]
        public void TransformHeadingOffset()
        {
            // Clone the current ensemble
            RTI.DataSet.Ensemble ensemble2 = ensemble1.Clone();

            // Set the Correlation Data
            // 4 Beams, 5 Bins
            for (int binNum = 0; binNum < 5; binNum++)
            {
                for (int beamNum = 0; beamNum < 4; beamNum++)
                {
                    ensemble2.CorrelationData.CorrelationData[binNum, beamNum] = 25.6f;
                }
            }

            Assert.IsTrue(ensemble2.IsInstrumentVelocityAvail);
            Assert.IsTrue(ensemble2.IsCorrelationAvail);

            float correlationThreshold = 25.0f;
            // Transform the data
            Transform.ProfileTransform(ref ensemble2, AdcpCodec.CodecEnum.Binary, correlationThreshold, Transform.HeadingSource.ADCP, 10, 0);

            // Check new heading value
            Assert.AreEqual(20.2, ensemble2.AncillaryData.Heading, 0.01f);

            // Check the Instrument Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(0.29238f, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 0], 0.001);
            Assert.AreEqual(1.16952, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.862311, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 3], 0.001);

            Assert.AreEqual(4.6780, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 0], 0.001);
            Assert.AreEqual(18.71234, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.8343, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 3], 0.001);

            // Check the Earth Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(-1.04946, ensemble2.EarthVelocityData.EarthVelocityData[0, 0], 0.001);
            Assert.AreEqual(0.732099, ensemble2.EarthVelocityData.EarthVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.81222, ensemble2.EarthVelocityData.EarthVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.EarthVelocityData.EarthVelocityData[0, 3], 0.001);

            Assert.AreEqual(-16.334524, ensemble2.EarthVelocityData.EarthVelocityData[1, 0], 0.001);
            Assert.AreEqual(11.242907, ensemble2.EarthVelocityData.EarthVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.04626, ensemble2.EarthVelocityData.EarthVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.EarthVelocityData.EarthVelocityData[1, 3], 0.001);

            // All error velocities should be the same between instrument and earth velocity
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[2, 3], ensemble2.EarthVelocityData.EarthVelocityData[2, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[3, 3], ensemble2.EarthVelocityData.EarthVelocityData[3, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[4, 3], ensemble2.EarthVelocityData.EarthVelocityData[4, 3], 0.001);

        }

        /// <summary>
        /// Set the heading offset for new transformed data
        /// </summary>
        [Test]
        public void AddHeadingOffset()
        {
            // Clone the current ensemble
            RTI.DataSet.Ensemble ensemble2 = ensemble1.Clone();

            // Set the Correlation Data
            // 4 Beams, 5 Bins
            for (int binNum = 0; binNum < 5; binNum++)
            {
                for (int beamNum = 0; beamNum < 4; beamNum++)
                {
                    ensemble2.CorrelationData.CorrelationData[binNum, beamNum] = 25.6f;
                }
            }

            Assert.IsTrue(ensemble2.IsInstrumentVelocityAvail);
            Assert.IsTrue(ensemble2.IsCorrelationAvail);

            // Give a heading offset
            RTI.VesselMount.VmHeadingOffset.AddAncillaryHeadingOffset(ref ensemble2, 10);

            float correlationThreshold = 25.0f;
            // Transform the data
            Transform.ProfileTransform(ref ensemble2, AdcpCodec.CodecEnum.Binary, correlationThreshold, Transform.HeadingSource.ADCP, 0, 0);

            // Check new heading value
            Assert.AreEqual(20.2, ensemble2.AncillaryData.Heading, 0.01f);

            // Check the Instrument Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(0.29238f, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 0], 0.001);
            Assert.AreEqual(1.16952, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.862311, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 3], 0.001);

            Assert.AreEqual(4.6780, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 0], 0.001);
            Assert.AreEqual(18.71234, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.8343, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 3], 0.001);

            // Check the Earth Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(-1.04946, ensemble2.EarthVelocityData.EarthVelocityData[0, 0], 0.001);
            Assert.AreEqual(0.732099, ensemble2.EarthVelocityData.EarthVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.81222, ensemble2.EarthVelocityData.EarthVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.EarthVelocityData.EarthVelocityData[0, 3], 0.001);

            Assert.AreEqual(-16.334524, ensemble2.EarthVelocityData.EarthVelocityData[1, 0], 0.001);
            Assert.AreEqual(11.242907, ensemble2.EarthVelocityData.EarthVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.04626, ensemble2.EarthVelocityData.EarthVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.EarthVelocityData.EarthVelocityData[1, 3], 0.001);

            // All error velocities should be the same between instrument and earth velocity
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[2, 3], ensemble2.EarthVelocityData.EarthVelocityData[2, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[3, 3], ensemble2.EarthVelocityData.EarthVelocityData[3, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[4, 3], ensemble2.EarthVelocityData.EarthVelocityData[4, 3], 0.001);

        }

        /// <summary>
        /// Set the heading offset for new transformed data.
        /// Let the offset call transform the data.
        /// </summary>
        [Test]
        public void HeadingOffset()
        {
            // Clone the current ensemble
            RTI.DataSet.Ensemble ensemble2 = ensemble1.Clone();

            // Set the Correlation Data
            // 4 Beams, 5 Bins
            for (int binNum = 0; binNum < 5; binNum++)
            {
                for (int beamNum = 0; beamNum < 4; beamNum++)
                {
                    ensemble2.CorrelationData.CorrelationData[binNum, beamNum] = 25.6f;
                }
            }

            Assert.IsTrue(ensemble2.IsInstrumentVelocityAvail);
            Assert.IsTrue(ensemble2.IsCorrelationAvail);

            // Give a heading offset
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.HeadingOffsetMag = 10.0f;
            RTI.VesselMount.VmHeadingOffset.HeadingOffset(ref ensemble2, vmOptions);

            //float correlationThreshold = 25.0f;
            // Transform the data
            //Transform.ProfileTransform(ref ensemble2, AdcpCodec.CodecEnum.Binary, correlationThreshold, Transform.HeadingSource.ADCP, 0, 0);

            // Check new heading value
            Assert.AreEqual(20.2, ensemble2.AncillaryData.Heading, 0.01f);

            // Check the Instrument Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(0.29238f, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 0], 0.001);
            Assert.AreEqual(1.16952, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.862311, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 3], 0.001);

            Assert.AreEqual(4.6780, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 0], 0.001);
            Assert.AreEqual(18.71234, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.8343, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 3], 0.001);

            // Check the Earth Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(-1.04946, ensemble2.EarthVelocityData.EarthVelocityData[0, 0], 0.001);
            Assert.AreEqual(0.732099, ensemble2.EarthVelocityData.EarthVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.81222, ensemble2.EarthVelocityData.EarthVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.EarthVelocityData.EarthVelocityData[0, 3], 0.001);

            Assert.AreEqual(-16.334524, ensemble2.EarthVelocityData.EarthVelocityData[1, 0], 0.001);
            Assert.AreEqual(11.242907, ensemble2.EarthVelocityData.EarthVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.04626, ensemble2.EarthVelocityData.EarthVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.EarthVelocityData.EarthVelocityData[1, 3], 0.001);

            // All error velocities should be the same between instrument and earth velocity
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[2, 3], ensemble2.EarthVelocityData.EarthVelocityData[2, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[3, 3], ensemble2.EarthVelocityData.EarthVelocityData[3, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[4, 3], ensemble2.EarthVelocityData.EarthVelocityData[4, 3], 0.001);

        }

        /// <summary>
        /// Set the heading offset for new transformed data.
        /// Do not transform data when the offset is done.  Transform the data seperately.
        /// </summary>
        [Test]
        public void HeadingOffsetTransform()
        {
            // Clone the current ensemble
            RTI.DataSet.Ensemble ensemble2 = ensemble1.Clone();

            // Set the Correlation Data
            // 4 Beams, 5 Bins
            for (int binNum = 0; binNum < 5; binNum++)
            {
                for (int beamNum = 0; beamNum < 4; beamNum++)
                {
                    ensemble2.CorrelationData.CorrelationData[binNum, beamNum] = 25.6f;
                }
            }

            Assert.IsTrue(ensemble2.IsInstrumentVelocityAvail);
            Assert.IsTrue(ensemble2.IsCorrelationAvail);

            // Give a heading offset
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.HeadingOffsetMag = 10.0f;
            RTI.VesselMount.VmHeadingOffset.HeadingOffset(ref ensemble2, vmOptions, false);

            float correlationThreshold = 25.0f;
            // Transform the data
            Transform.ProfileTransform(ref ensemble2, AdcpCodec.CodecEnum.Binary, correlationThreshold, Transform.HeadingSource.ADCP, 0, 0);

            // Check new heading value
            Assert.AreEqual(20.2, ensemble2.AncillaryData.Heading, 0.01f);

            // Check the Instrument Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(0.29238f, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 0], 0.001);
            Assert.AreEqual(1.16952, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.862311, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 3], 0.001);

            Assert.AreEqual(4.6780, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 0], 0.001);
            Assert.AreEqual(18.71234, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.8343, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 3], 0.001);

            // Check the Earth Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(-1.04946, ensemble2.EarthVelocityData.EarthVelocityData[0, 0], 0.001);
            Assert.AreEqual(0.732099, ensemble2.EarthVelocityData.EarthVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.81222, ensemble2.EarthVelocityData.EarthVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.EarthVelocityData.EarthVelocityData[0, 3], 0.001);

            Assert.AreEqual(-16.334524, ensemble2.EarthVelocityData.EarthVelocityData[1, 0], 0.001);
            Assert.AreEqual(11.242907, ensemble2.EarthVelocityData.EarthVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.04626, ensemble2.EarthVelocityData.EarthVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.EarthVelocityData.EarthVelocityData[1, 3], 0.001);

            // All error velocities should be the same between instrument and earth velocity
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[2, 3], ensemble2.EarthVelocityData.EarthVelocityData[2, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[3, 3], ensemble2.EarthVelocityData.EarthVelocityData[3, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[4, 3], ensemble2.EarthVelocityData.EarthVelocityData[4, 3], 0.001);

        }

        /// <summary>
        /// Set the Pitch offset for new transformed data.
        /// Do not transform data when the offset is done.  Transform the data seperately.
        /// </summary>
        [Test]
        public void TiltOffsetPitch()
        {
            // Clone the current ensemble
            RTI.DataSet.Ensemble ensemble2 = ensemble1.Clone();

            // Set the Correlation Data
            // 4 Beams, 5 Bins
            for (int binNum = 0; binNum < 5; binNum++)
            {
                for (int beamNum = 0; beamNum < 4; beamNum++)
                {
                    ensemble2.CorrelationData.CorrelationData[binNum, beamNum] = 25.6f;
                }
            }

            Assert.IsTrue(ensemble2.IsInstrumentVelocityAvail);
            Assert.IsTrue(ensemble2.IsCorrelationAvail);

            // Give a heading offset
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.PitchOffset = 10.0f;
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble2, vmOptions, false);

            float correlationThreshold = 25.0f;
            // Transform the data
            Transform.ProfileTransform(ref ensemble2, AdcpCodec.CodecEnum.Binary, correlationThreshold, Transform.HeadingSource.ADCP, 0, 0);

            // Check new Pitch value
            Assert.AreEqual(11.02, ensemble2.AncillaryData.Pitch, 0.01f);

            // Check the Instrument Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(0.29238f, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 0], 0.001);
            Assert.AreEqual(1.16952, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.862311, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 3], 0.001);

            Assert.AreEqual(4.6780, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 0], 0.001);
            Assert.AreEqual(18.71234, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.8343, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 3], 0.001);

            // Check the Earth Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(-1.1058, ensemble2.EarthVelocityData.EarthVelocityData[0, 0], 0.001);
            Assert.AreEqual(0.843599, ensemble2.EarthVelocityData.EarthVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.7283, ensemble2.EarthVelocityData.EarthVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.EarthVelocityData.EarthVelocityData[0, 3], 0.001);

            // All error velocities should be the same between instrument and earth velocity
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[2, 3], ensemble2.EarthVelocityData.EarthVelocityData[2, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[3, 3], ensemble2.EarthVelocityData.EarthVelocityData[3, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[4, 3], ensemble2.EarthVelocityData.EarthVelocityData[4, 3], 0.001);

        }

        /// <summary>
        /// Set the Roll offset for new transformed data.
        /// Do not transform data when the offset is done.  Transform the data seperately.
        /// </summary>
        [Test]
        public void TiltOffsetRoll()
        {
            // Clone the current ensemble
            RTI.DataSet.Ensemble ensemble2 = ensemble1.Clone();

            // Set the Correlation Data
            // 4 Beams, 5 Bins
            for (int binNum = 0; binNum < 5; binNum++)
            {
                for (int beamNum = 0; beamNum < 4; beamNum++)
                {
                    ensemble2.CorrelationData.CorrelationData[binNum, beamNum] = 25.6f;
                }
            }

            Assert.IsTrue(ensemble2.IsInstrumentVelocityAvail);
            Assert.IsTrue(ensemble2.IsCorrelationAvail);

            // Give a heading offset
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.RollOffset = 10.0f;
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble2, vmOptions, false);

            float correlationThreshold = 25.0f;
            // Transform the data
            Transform.ProfileTransform(ref ensemble2, AdcpCodec.CodecEnum.Binary, correlationThreshold, Transform.HeadingSource.ADCP, 0, 0);

            // Check new Pitch value
            Assert.AreEqual(12.123, ensemble2.AncillaryData.Roll, 0.01f);

            // Check the Instrument Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(0.29238f, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 0], 0.001);
            Assert.AreEqual(1.16952, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.862311, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 3], 0.001);

            Assert.AreEqual(4.6780, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 0], 0.001);
            Assert.AreEqual(18.71234, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.8343, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 3], 0.001);

            // Check the Earth Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(-1.45356, ensemble2.EarthVelocityData.EarthVelocityData[0, 0], 0.001);
            Assert.AreEqual(0.587056, ensemble2.EarthVelocityData.EarthVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.56971, ensemble2.EarthVelocityData.EarthVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.EarthVelocityData.EarthVelocityData[0, 3], 0.001);

            // All error velocities should be the same between instrument and earth velocity
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[2, 3], ensemble2.EarthVelocityData.EarthVelocityData[2, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[3, 3], ensemble2.EarthVelocityData.EarthVelocityData[3, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[4, 3], ensemble2.EarthVelocityData.EarthVelocityData[4, 3], 0.001);

        }


        /// <summary>
        /// Set the Pitch and Roll offset for new transformed data.
        /// Do not transform data when the offset is done.  Transform the data seperately.
        /// </summary>
        [Test]
        public void TiltOffsetPitchAndRoll()
        {
            // Clone the current ensemble
            RTI.DataSet.Ensemble ensemble2 = ensemble1.Clone();

            // Set the Correlation Data
            // 4 Beams, 5 Bins
            for (int binNum = 0; binNum < 5; binNum++)
            {
                for (int beamNum = 0; beamNum < 4; beamNum++)
                {
                    ensemble2.CorrelationData.CorrelationData[binNum, beamNum] = 25.6f;
                }
            }

            Assert.IsTrue(ensemble2.IsInstrumentVelocityAvail);
            Assert.IsTrue(ensemble2.IsCorrelationAvail);

            // Give a heading offset
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.RollOffset = 10.0f;
            vmOptions.PitchOffset = 10.0f;
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble2, vmOptions, false);

            float correlationThreshold = 25.0f;
            // Transform the data
            Transform.ProfileTransform(ref ensemble2, AdcpCodec.CodecEnum.Binary, correlationThreshold, Transform.HeadingSource.ADCP, 0, 0);

            // Check new Pitch value
            Assert.AreEqual(12.123, ensemble2.AncillaryData.Roll, 0.01f);

            // Check the Instrument Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(0.29238f, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 0], 0.001);
            Assert.AreEqual(1.16952, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.862311, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 3], 0.001);

            Assert.AreEqual(4.6780, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 0], 0.001);
            Assert.AreEqual(18.71234, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.8343, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 3], 0.001);

            // Check the Earth Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(-1.40615, ensemble2.EarthVelocityData.EarthVelocityData[0, 0], 0.001);
            Assert.AreEqual(0.850535, ensemble2.EarthVelocityData.EarthVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.49023, ensemble2.EarthVelocityData.EarthVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.EarthVelocityData.EarthVelocityData[0, 3], 0.001);

            // All error velocities should be the same between instrument and earth velocity
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[2, 3], ensemble2.EarthVelocityData.EarthVelocityData[2, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[3, 3], ensemble2.EarthVelocityData.EarthVelocityData[3, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[4, 3], ensemble2.EarthVelocityData.EarthVelocityData[4, 3], 0.001);

        }

        /// <summary>
        /// Set the Pitch and Roll offset for new transformed data.
        /// Transform in offset.
        /// </summary>
        [Test]
        public void TiltOffsetPitchAndRollWithTransform()
        {
            // Clone the current ensemble
            RTI.DataSet.Ensemble ensemble2 = ensemble1.Clone();

            // Set the Correlation Data
            // 4 Beams, 5 Bins
            for (int binNum = 0; binNum < 5; binNum++)
            {
                for (int beamNum = 0; beamNum < 4; beamNum++)
                {
                    ensemble2.CorrelationData.CorrelationData[binNum, beamNum] = 25.6f;
                }
            }

            Assert.IsTrue(ensemble2.IsInstrumentVelocityAvail);
            Assert.IsTrue(ensemble2.IsCorrelationAvail);

            // Give a Pitch and Roll offset
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.RollOffset = 10.0f;
            vmOptions.PitchOffset = 10.0f;
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble2, vmOptions, true);

            // Heading offset
            RTI.VesselMount.VmHeadingOffset.HeadingOffset(ref ensemble2, vmOptions, true);

            //float correlationThreshold = 25.0f;
            // Transform the data
            //Transform.ProfileTransform(ref ensemble2, AdcpCodec.CodecEnum.Binary, correlationThreshold, Transform.HeadingSource.ADCP, 0, 0);

            // Check new Pitch value
            Assert.AreEqual(10.2, ensemble2.AncillaryData.Heading, 0.01f);
            Assert.AreEqual(11.02, ensemble2.AncillaryData.Pitch, 0.01f);
            Assert.AreEqual(12.123, ensemble2.AncillaryData.Roll, 0.01f);

            // Check the Instrument Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(0.29238f, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 0], 0.001);
            Assert.AreEqual(1.16952, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.862311, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 3], 0.001);

            Assert.AreEqual(4.6780, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 0], 0.001);
            Assert.AreEqual(18.71234, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.8343, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 3], 0.001);

            // Check the Earth Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(-1.40615, ensemble2.EarthVelocityData.EarthVelocityData[0, 0], 0.001);
            Assert.AreEqual(0.850535, ensemble2.EarthVelocityData.EarthVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.49023, ensemble2.EarthVelocityData.EarthVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.EarthVelocityData.EarthVelocityData[0, 3], 0.001);

            // All error velocities should be the same between instrument and earth velocity
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[2, 3], ensemble2.EarthVelocityData.EarthVelocityData[2, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[3, 3], ensemble2.EarthVelocityData.EarthVelocityData[3, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[4, 3], ensemble2.EarthVelocityData.EarthVelocityData[4, 3], 0.001);

        }

        /// <summary>
        /// Set the Heading, Pitch and Roll offset for new transformed data.
        /// Transform in offset.
        /// </summary>
        [Test]
        public void TiltOffsetHeadingPitchAndRollWithTransform()
        {
            // Clone the current ensemble
            RTI.DataSet.Ensemble ensemble2 = ensemble1.Clone();

            // Set the Correlation Data
            // 4 Beams, 5 Bins
            for (int binNum = 0; binNum < 5; binNum++)
            {
                for (int beamNum = 0; beamNum < 4; beamNum++)
                {
                    ensemble2.CorrelationData.CorrelationData[binNum, beamNum] = 25.6f;
                }
            }

            Assert.IsTrue(ensemble2.IsInstrumentVelocityAvail);
            Assert.IsTrue(ensemble2.IsCorrelationAvail);

            // Give a Pitch and Roll offset
            VesselMountOptions vmOptions = new VesselMountOptions();
            vmOptions.HeadingOffsetMag = 10.0f;
            vmOptions.RollOffset = 10.0f;
            vmOptions.PitchOffset = 10.0f;
            RTI.VesselMount.VmTiltOffset.TiltOffset(ref ensemble2, vmOptions, true);

            // Heading offset
            RTI.VesselMount.VmHeadingOffset.HeadingOffset(ref ensemble2, vmOptions, true);

            //float correlationThreshold = 25.0f;
            // Transform the data
            //Transform.ProfileTransform(ref ensemble2, AdcpCodec.CodecEnum.Binary, correlationThreshold, Transform.HeadingSource.ADCP, 0, 0);

            // Check new Pitch value
            Assert.AreEqual(20.2, ensemble2.AncillaryData.Heading, 0.01f);
            Assert.AreEqual(11.02, ensemble2.AncillaryData.Pitch, 0.01f);
            Assert.AreEqual(12.123, ensemble2.AncillaryData.Roll, 0.01f);

            // Check the Instrument Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(0.29238f, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 0], 0.001);
            Assert.AreEqual(1.16952, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.862311, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.InstrumentVelocityData.InstrumentVelocityData[0, 3], 0.001);

            Assert.AreEqual(4.6780, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 0], 0.001);
            Assert.AreEqual(18.71234, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 1], 0.001);
            Assert.AreEqual(-13.8343, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 2], 0.001);
            Assert.AreEqual(-7.2, ensemble2.InstrumentVelocityData.InstrumentVelocityData[1, 3], 0.001);

            // Check the Earth Velocity Data
            // 4 Beams, 5 Bins
            Assert.AreEqual(-1.2371, ensemble2.EarthVelocityData.EarthVelocityData[0, 0], 0.001);
            Assert.AreEqual(1.08179, ensemble2.EarthVelocityData.EarthVelocityData[0, 1], 0.001);
            Assert.AreEqual(-1.49023, ensemble2.EarthVelocityData.EarthVelocityData[0, 2], 0.001);
            Assert.AreEqual(-0.45, ensemble2.EarthVelocityData.EarthVelocityData[0, 3], 0.001);

            // All error velocities should be the same between instrument and earth velocity
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[2, 3], ensemble2.EarthVelocityData.EarthVelocityData[2, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[3, 3], ensemble2.EarthVelocityData.EarthVelocityData[3, 3], 0.001);
            Assert.AreEqual(ensemble2.InstrumentVelocityData.InstrumentVelocityData[4, 3], ensemble2.EarthVelocityData.EarthVelocityData[4, 3], 0.001);

        }

    }
}
