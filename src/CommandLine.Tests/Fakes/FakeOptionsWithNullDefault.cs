using System.Collections.Generic;

namespace CommandLine.Tests.Fakes
{
    class FakeOptionsWithNullDefault
    {
        [Option('i', DefaultValue = null)]
        public IEnumerable<int> IntSequence { get; set; }
    }
}
