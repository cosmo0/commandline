namespace CommandLine.Tests.Fakes
{
	class FakeOptionsWithSetAndNonSet
	{
		[Option('s', "set", SetName = "Set")]
		public bool Set { get; set; }

		[Option('n', "nonset")]
		public bool NonSet { get; set; }
	}
}
