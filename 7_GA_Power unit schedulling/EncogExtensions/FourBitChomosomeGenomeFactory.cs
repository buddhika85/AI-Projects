using Encog.ML.EA.Genome;

namespace _7_GA_Power_unit_schedulling.EncogExtensions
{
    /// <summary>
    /// https://github.com/encog/encog-dotnet-core/blob/master/encog-core-cs/ML/Genetic/Genome/IntegerArrayGenomeFactory.cs
    /// </summary>
    public class FourBitChomosomeGenomeFactory : IGenomeFactory
    {
        /// <summary>
        /// The size of genome to create.
        /// </summary>
        private int size;

        /// <summary>
        /// Create the integer genome of a fixed size.
        /// </summary>
        /// <param name="theSize">The size to use.</param>
        public FourBitChomosomeGenomeFactory(int theSize)
        {
            this.size = theSize;
        }

        /// <inheritdoc/>
        public IGenome Factor()
        {
            return new FourBitCustomGenome(this.size);
        }

        /// <inheritdoc/>
        public IGenome Factor(IGenome other)
        {
            return new FourBitCustomGenome(((FourBitCustomGenome)other));
        }
    }
}