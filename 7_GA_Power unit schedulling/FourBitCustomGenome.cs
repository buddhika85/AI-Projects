using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encog.ML.EA.Genome;

namespace _7_GA_Power_unit_schedulling
{
    public class FourBitCustomGenome : BasicGenome
    {
        public override void Copy(IGenome source)
        {
            throw new NotImplementedException();
        }

        public override int Size { get; }
    }
}
