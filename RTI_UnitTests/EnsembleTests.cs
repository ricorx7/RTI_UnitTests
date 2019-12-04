using NUnit.Framework;
using RTI;

namespace RTI_UnitTests
{
    public class EnsembleTests
    {
        /// <summary>
        /// Ensemble 1.
        /// </summary>
        RTI.DataSet.Ensemble ensemble1;

        [SetUp]
        public void Setup()
        {
            // Create the ensemble
            ensemble1 = EnsembleHelper.GenerateEnsemble(5, 4, true);

            ensemble1.EnsembleData.EnsembleNumber = 12;
        }

        [Test]
        public void SetupEnsemble()
        {
            Assert.IsTrue(ensemble1.IsEnsembleAvail);
            Assert.AreEqual(12, ensemble1.EnsembleData.EnsembleNumber);
        }
    }
}