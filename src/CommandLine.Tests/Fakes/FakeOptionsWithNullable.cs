namespace CommandLine.Tests.Fakes
{
    class FakeOptionsWithNullable
    {
        [Option('n')]
        public int? NullableIntValue { get; set; }

        [Option('c')]
        public Colors? NullableColorsValue { get; set; }
    }
}
