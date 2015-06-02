using System.Collections.Generic;

namespace CommandLine.Tests.Fakes
{
    class FakeOptionsWithSequenceWithMinZero
    {
        [Option('i', Min = 0)]
        public IEnumerable<int> IntSequence { get; set; }
    }
}
