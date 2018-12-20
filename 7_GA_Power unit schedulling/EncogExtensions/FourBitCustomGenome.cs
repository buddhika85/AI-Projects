using Encog.ML.EA.Genome;
using Encog.ML.Genetic.Genome;

namespace _7_GA_Power_unit_schedulling.EncogExtensions
{
    /// <summary>
    /// Ref - https://github.com/encog/encog-dotnet-core/blob/master/encog-core-cs/ML/Genetic/Genome/IntegerArrayGenome.cs
    /// </summary>
    public class FourBitCustomGenome : BasicGenome, IArrayGenome
    {
        /// <summary>
        /// The genome data.
        /// </summary>
        private FourBitGene [] data;

        /// <summary>
        /// Construct the genome.
        /// </summary>
        /// <param name="size">The size of the genome.</param>
        public FourBitCustomGenome(int size)
        {
            this.data = new FourBitGene[size];
        }

        /// <summary>
        /// Construct the genome by copying another.
        /// </summary>
        /// <param name="other">The other genome.</param>
        public FourBitCustomGenome(FourBitCustomGenome other)
        {
            this.data = (FourBitGene[])other.Data.Clone();
        }

        /// <inheritdoc/>
        public override int Size
        {
            get
            {
                return this.data.Length;
            }
        }

        /// <inheritdoc/>
        public void Copy(IArrayGenome source, int sourceIndex, int targetIndex)
        {
            FourBitCustomGenome sourceInt = (FourBitCustomGenome)source;
            this.data[targetIndex] = sourceInt.data[sourceIndex];

        }

        /// <summary>
        /// The data.
        /// </summary>
        public FourBitGene[] Data
        {
            get
            {
                return this.data;
            }
        }

        /// <inheritdoc/>
        /// https://github.com/encog/encog-dotnet-core/blob/master/encog-core-cs/Util/EngineArray.cs
        public override void Copy(IGenome source)
        {
            FourBitCustomGenome sourceInt = (FourBitCustomGenome)source;
            EngineArrayExtension.ArrayCopy(sourceInt.data, this.data);
            this.Score = source.Score;
            this.AdjustedScore = source.AdjustedScore;

        }

        /// <inheritdoc/>
        public void Swap(int iswap1, int iswap2)
        {
            FourBitGene temp = this.data[iswap1];
            this.data[iswap1] = this.data[iswap2];
            this.data[iswap2] = temp;

        }
    }
}
