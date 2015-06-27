using System.Collections.Generic;

namespace CommandLine.Tests.Fakes
{
    class FakeOptionsWithSequence
    {
        [Option('i')]
        public IEnumerable<int> IntSequence { get; set; }
    }
}
