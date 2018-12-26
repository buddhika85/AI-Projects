using Encog.MathUtil.Randomize;
using Encog.ML.EA.Genome;
using Encog.ML.EA.Opp;
using Encog.ML.EA.Train;
using System;

namespace _7_GA_Power_unit_schedulling.EncogExtensions
{

    /// <summary>
    /// Ref - https://github.com/encog/encog-dotnet-core/blob/master/encog-core-cs/ML/Genetic/Crossover/Splice.cs
    /// </summary>
    public class CustomCrossOver : IEvolutionaryOperator
    {
      
        private IEvolutionaryAlgorithm owner;

      
        /// <inheritdoc/>
        public void PerformOperation(EncogRandom rnd, IGenome[] parents, int parentIndex,
                IGenome[] offspring, int offspringIndex)
        {
            try
            {
                FourBitCustomGenome mum = (FourBitCustomGenome) parents[0];
                FourBitCustomGenome dad = (FourBitCustomGenome)parents[1];
                FourBitCustomGenome offSping1 = new FourBitCustomGenome(mum.Data.Length);
                FourBitCustomGenome offSping2 = new FourBitCustomGenome(dad.Data.Length);
                var random = new Random();
                var spliceStartIndex = random.Next(1, mum.Data.Length);        // random number is between 0 and 7

                for (var geneIndex = 0; geneIndex < mum.Size; geneIndex++)
                {
                    if (geneIndex < spliceStartIndex)
                    {
                        offSping1.Data[geneIndex] = mum.Data[geneIndex];
                        offSping2.Data[geneIndex] = dad.Data[geneIndex];
                    }
                    else
                    {
                        // cross over
                        offSping1.Data[geneIndex] = dad.Data[geneIndex];
                        offSping2.Data[geneIndex] = mum.Data[geneIndex];
                    }
                }
            
                //IArrayGenome mother = (IArrayGenome)parents[parentIndex];
                //IArrayGenome father = (IArrayGenome)parents[parentIndex + 1];
                //IArrayGenome offspring1 = (IArrayGenome)this.owner.Population.GenomeFactory.Factor();
                //IArrayGenome offspring2 = (IArrayGenome)this.owner.Population.GenomeFactory.Factor();

                //offspring[offspringIndex] = offspring1;
                //offspring[offspringIndex + 1] = offspring2;

                //int geneLength = mother.Size;

                //// the chromosome must be cut at two positions, determine them
                //int cutpoint1 = (int)(rnd.Next(geneLength - this.cutLength));
                //int cutpoint2 = cutpoint1 + this.cutLength;

                //// handle cut section
                //for (int i = 0; i < geneLength; i++)
                //{
                //    if (!((i < cutpoint1) || (i > cutpoint2)))
                //    {
                //        offspring1.Copy(father, i, i);
                //        offspring2.Copy(mother, i, i);
                //    }
                //}

                //// handle outer sections
                //for (int i = 0; i < geneLength; i++)
                //{
                //    if ((i < cutpoint1) || (i > cutpoint2))
                //    {
                //        offspring1.Copy(mother, i, i);
                //        offspring2.Copy(father, i, i);
                //    }
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <inheritdoc/>
        public int OffspringProduced
        {
            get
            {
                return 2;
            }
        }

        /// <inheritdoc/>
        public int ParentsNeeded
        {
            get
            {
                return 2;
            }
        }

        /// <inheritdoc/>
        public void Init(IEvolutionaryAlgorithm theOwner)
        {
            this.owner = theOwner;

        }
    }
}
