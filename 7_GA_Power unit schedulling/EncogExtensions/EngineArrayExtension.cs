using System;
using Encog.Util;

namespace _7_GA_Power_unit_schedulling.EncogExtensions
{
    /// <summary>
    /// https://github.com/encog/encog-dotnet-core/blob/master/encog-core-cs/Util/EngineArray.cs
    /// </summary>
    public class EngineArrayExtension : EngineArray
    {
        /// <summary>
        /// Completely copy Copy a 4 bit Gene Chromosome.
        /// </summary>
        /// <param name="src">Source array.</param>
        /// <param name="dst">Destination array.</param>
        public static void ArrayCopy(FourBitGene[] src, FourBitGene[] dst)
        {
            Array.Copy(src, dst, src.Length);
        }
    }
}
