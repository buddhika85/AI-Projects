using System;
using System.Linq;
using Encog.MathUtil.Randomize;
using Encog.ML.EA.Genome;
using Encog.ML.EA.Opp;
using Encog.ML.EA.Train;
using Encog.ML.Genetic.Genome;

namespace _7_GA_Power_unit_schedulling.EncogExtensions
{
    /// <summary>
    /// Ref - https://github.com/encog/encog-dotnet-core/blob/master/encog-core-cs/ML/Genetic/Mutate/MutateShuffle.cs
    /// </summary>
    public class CustomMutation : IEvolutionaryOperator
    {
        /// <summary>
        /// The owner.
        /// </summary>
        private IEvolutionaryAlgorithm owner;

        /// <inheritdoc/>
        public void PerformOperation(EncogRandom rnd, IGenome[] parents, int parentIndex,
            IGenome[] offspring, int offspringIndex)
        {
            try
            {
                FourBitCustomGenome mum = (FourBitCustomGenome)parents[0];
                FourBitCustomGenome dad = (FourBitCustomGenome)parents[1];
                FourBitCustomGenome offSping1 = PerformErrorlessSmartCustomMutation(mum);
                FourBitCustomGenome offSping2 = PerformErrorlessSmartCustomMutation(dad);
            
                //for (var geneIndex = 0; geneIndex < dad.Size; geneIndex++)
                //{
                //    if (geneIndex != mutateIndex1)
                //    {
                //        offSping2.Data[geneIndex] = dad.Data[geneIndex];
                //    }
                //    else
                //    {
                //        // mutation
                //        offSping2.Data[geneIndex] = dad.Data[geneIndex];
                //    }
                //}


                //IArrayGenome parent = (IArrayGenome)parents[parentIndex];
                //offspring[offspringIndex] = this.owner.Population.GenomeFactory.Factor();
                //IArrayGenome child = (IArrayGenome)offspring[offspringIndex];

                //child.Copy(parent);

                //int length = parent.Size;
                //int iswap1 = (int)(rnd.NextDouble() * length);
                //int iswap2 = (int)(rnd.NextDouble() * length);

                //// can't be equal
                //if (iswap1 == iswap2)
                //{
                //    // move to the next, but
                //    // don't go out of bounds
                //    if (iswap1 > 0)
                //    {
                //        iswap1--;
                //    }
                //    else
                //    {
                //        iswap1++;
                //    }

                //}

                //// make sure they are in the right order
                //if (iswap1 > iswap2)
                //{
                //    int temp = iswap1;
                //    iswap1 = iswap2;
                //    iswap2 = temp;
                //}

                //child.Swap(iswap1, iswap2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private FourBitCustomGenome PerformErrorlessSmartCustomMutation(FourBitCustomGenome parent)
        {
            try
            {
                FourBitCustomGenome offSping = new FourBitCustomGenome(parent.Data.Length);
                var random = new Random();
                var mutateIndex1 = random.Next(1, parent.Data.Length);        // random number is between 0 and 
                for (var geneIndex = 0; geneIndex < parent.Size; geneIndex++)
                {
                    if (geneIndex != mutateIndex1)
                    {
                        offSping.Data[geneIndex] = parent.Data[geneIndex];
                    }
                    else
                    {
                        // mutation
                        FourBitGene geneToMutate = parent.Data[geneIndex];
                        var bitZeroCount = geneToMutate.Gene.Count(x => x == 0);
                        var randomNumber = random.Next(1, 101); // random number is between 0 and 100
                        switch (bitZeroCount)
                        {
                            case 0:
                                {
                                    offSping.Data[geneIndex] = geneToMutate;
                                    break;
                                }
                            case 1:
                                {
                                    var remainder = randomNumber % 4;        // either zero or one or two or three
                                    int [] mutatedGene = remainder == 0 ? new[] { 0, 0, 0, 1 } :
                                        remainder == 1 ? new[] { 0, 0, 1, 0 } :
                                        remainder == 2 ? new[] { 0, 1, 0, 0 } : new[] { 1, 0, 0, 0 };
                                    offSping.Data[geneIndex] = new FourBitGene { Gene = mutatedGene };
                                    break;
                                }
                            case 2:
                                {
                                    var remainder = randomNumber % 3;        // either zero or one or two
                                    int[] mutatedGene = remainder == 0 ? new[] { 0, 0, 1, 1 } :
                                        remainder == 1 ? new[] { 0, 1, 1, 0 } : new[] { 1, 1, 0, 0 };
                                    offSping.Data[geneIndex] = new FourBitGene { Gene = mutatedGene };
                                    break;
                                }
                            case 3:
                                {
                                    var remainder = randomNumber % 2;        // either zero or one
                                    int[] mutatedGene = remainder == 0 ? new[] { 0, 1, 1, 1 } : new[] { 1, 1, 1, 0 };
                                    offSping.Data[geneIndex] = new FourBitGene { Gene = mutatedGene };
                                    break;
                                }
                            case 4:
                                {
                                    offSping.Data[geneIndex] = geneToMutate;
                                    break;
                                }
                            default:
                                {
                                    offSping.Data[geneIndex] = geneToMutate;
                                    break;
                                }
                        }
                    }
                }

                return offSping;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// The number of offspring produced, which is 1 for this mutation.
        /// </summary>
        public int OffspringProduced
        {
            get
            {
                return 1;
            }
        }

        /// <inheritdoc/>
        public int ParentsNeeded
        {
            get
            {
                return 1;
            }
        }

        /// <inheritdoc/>
        public void Init(IEvolutionaryAlgorithm theOwner)
        {
            this.owner = theOwner;
        }
    }
}
