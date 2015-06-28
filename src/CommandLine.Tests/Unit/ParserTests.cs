// Copyright 2005-2013 Giacomo Stelluti Scala & Contributors. All rights reserved. See doc/License.md in the project root for license information.

using System.IO;
using System.Linq;
using CommandLine.Tests.Fakes;
using FluentAssertions;
using Xunit;

namespace CommandLine.Tests.Unit
{
    public class ParserTests
    {
        [Fact]
        public void When_HelpWriter_is_set_help_screen_is_generated()
        {
            // Fixture setup
            var writer = new StringWriter();
            var sut = new Parser(with => with.HelpWriter = writer);

            // Exercize system
            sut.ParseArguments<FakeOptionWithRequired>(new string[] { });

            // Verify outcome
            var text = writer.ToString();
            Assert.True(text.Length > 0);
            // Teardown
        }

        [Fact]
        public void When_HelpWriter_is_set_help_screen_is_generated_in_verbs_scenario()
        {
            // Fixture setup
            var writer = new StringWriter();
            var sut = new Parser(with => with.HelpWriter = writer);

            // Exercize system
            sut.ParseArguments(new string[] { }, typeof(AddOptions), typeof(CommitOptions), typeof(CloneOptions));

            // Verify outcome
            var text = writer.ToString();
            Assert.True(text.Length > 0);
            // Teardown
        }

        [Fact]
        public void When_HelpWriter_is_set_help_screen_is_generated_in_verbs_scenario_using_generic_overload()
        {
            // Fixture setup
            var writer = new StringWriter();
            var sut = new Parser(with => with.HelpWriter = writer);

            // Exercize system
            sut.ParseArguments<AddOptions, CommitOptions, CloneOptions>(new string[] { });

            // Verify outcome
            var text = writer.ToString();
            Assert.True(text.Length > 0);
            // Teardown
        }

        [Fact]
        public void Parse_options()
        {
            // Fixture setup
            var expectedOptions = new FakeOptions
                {
                    StringValue = "strvalue", IntSequence = new[] { 1, 2, 3 }
                };
            var sut = new Parser();

            // Exercize system
            var result = sut.ParseArguments<FakeOptions>(new[] { "--stringvalue=strvalue", "-i1", "2", "3" });

            // Verify outcome
            result.Value.ShouldHave().AllProperties().EqualTo(expectedOptions);
            Assert.False(result.Errors.Any());
            // Teardown
        }

        [Fact]
        public void Parameters_names_are_inferred()
        {
            // Fixture setup
            var expectedOptions = new FakeOptions
            {
                StringValue = "strvalue",
                IntSequence = new [] { 1, 2, 3 }
            };
            var sut = new Parser();

            // Exercize system
            var result = sut.ParseArguments<FakeOptions>(new[] { "--stringvalue", "strvalue", "-i", "1", "2", "3" });

            // Verify outcome
            result.Value.ShouldHave().AllProperties().EqualTo(expectedOptions);
            Assert.False(result.Errors.Any());
            // Teardown
        }

        [Fact]
        public void Same_value_in_two_parameters_are_parsed()
        {
            // Fixture setup
            var expectedOptions = new FakeOptionsWithTwoIntegers
            {
                A = 2,
                B = 2
            };
            var sut = new Parser();

            // Exercize system
            var result = sut.ParseArguments<FakeOptionsWithTwoIntegers>(new[] { "-a", "2", "-b", "2" });

            // Verify outcome
            result.Value.ShouldHave().AllProperties().EqualTo(expectedOptions);
            Assert.False(result.Errors.Any());
            // Teardown
        }

        [Fact]
        public void Short_Parameters_names_are_used()
        {
            // Fixture setup
            var expectedOptions = new FakeOptions
            {
                IntSequence = new[] { 1, 2, 3 },
                BoolValue = true
            };
            var sut = new Parser();

            // Exercize system
            var result = sut.ParseArguments<FakeOptions>(new[] { "-x", "-i1", "2", "3" });

            // Verify outcome
            result.Value.ShouldHave().AllProperties().EqualTo(expectedOptions);
            Assert.False(result.Errors.Any());
            // Teardown
        }

        [Fact]
        public void Parse_options_with_double_dash()
        {
            // Fixture setup
            var expectedOptions = new FakeOptionsWithValues
                {
                    StringValue = "astring",
                    LongValue = 20L,
                    StringSequence = new[] { "--aaa", "-b", "--ccc" },
                    IntValue = 30
                };
            var sut = new Parser(with => with.EnableDashDash = true);

            // Exercize system
            var result = sut.ParseArguments<FakeOptionsWithValues>(
                new[] { "--stringvalue", "astring", "--", "20", "--aaa", "-b", "--ccc", "30" });

            // Verify outcome
            result.Value.ShouldHave().AllProperties().EqualTo(expectedOptions);
            Assert.False(result.Errors.Any());
            // Teardown
        }

        [Fact]
        public void Parse_options_with_double_dash_in_verbs_scenario()
        {
            // Fixture setup
            var expectedOptions = new AddOptions
                {
                    Patch = true,
                    FileName = "--strange-fn"
                };
            var sut = new Parser(with => with.EnableDashDash = true);

            // Exercize system
            var result = sut.ParseArguments(
                new[] { "add", "-p", "--", "--strange-fn" },
                typeof(AddOptions), typeof(CommitOptions), typeof(CloneOptions));

            // Verify outcome
            Assert.IsType<AddOptions>(result.Value);
            result.Value.ShouldHave().AllRuntimeProperties().EqualTo(expectedOptions);
            Assert.False(result.Errors.Any());
            // Teardown
        }

        [Fact]
        public void Parse_verbs()
        {
            // Fixture setup
            var expectedOptions = new CloneOptions
                {
                    Quiet = true,
                    Urls = new[] { "http://gsscoder.github.com/", "http://yes-to-nooo.github.com/" }
                };
            var sut = new Parser();

            // Exercize system
            var result = sut.ParseArguments(
                new[] { "clone", "-q", "http://gsscoder.github.com/", "http://yes-to-nooo.github.com/" },
                typeof(AddOptions), typeof(CommitOptions), typeof(CloneOptions));

            // Verify outcome
            Assert.IsType<CloneOptions>(result.Value);
            result.Value.ShouldHave().AllRuntimeProperties().EqualTo(expectedOptions);
            Assert.False(result.Errors.Any());
            // Teardown
        }

        [Fact]
        public void Parse_verbs_using_generic_overload()
        {
            // Fixture setup
            var expectedOptions = new CloneOptions
            {
                Quiet = true,
                Urls = new[] { "http://gsscoder.github.com/", "http://yes-to-nooo.github.com/" }
            };
            var sut = new Parser();

            // Exercize system
            var result = sut.ParseArguments<AddOptions, CommitOptions, CloneOptions>(
                new[] { "clone", "-q", "http://gsscoder.github.com/", "http://yes-to-nooo.github.com/" });

            // Verify outcome
            Assert.IsType<CloneOptions>(result.Value);
            result.Value.ShouldHave().AllRuntimeProperties().EqualTo(expectedOptions);
            Assert.False(result.Errors.Any());
            // Teardown
        }

        [Fact]
        public void Parse_nullable_options()
        {
            // Fixture setup
            var expectedOptions = new FakeOptionsWithNullable
            {
                NullableIntValue = 60,
                NullableColorsValue = Colors.Red
            };
            var sut = new Parser();

            // Exercize system
            var result = sut.ParseArguments<FakeOptionsWithNullable>(new[] { "-n", "60", "-c", "Red" });

            // Verify outcome
            Assert.Empty(result.Errors);
            result.Value.ShouldHave().AllProperties().EqualTo(expectedOptions);
            // Teardown
        }

        [Fact]
        public void Parse_check_empty_sequence()
        {
            // Fixture setup
            var sut = new Parser();

            // Exercize system
            var result = sut.ParseArguments<FakeOptionsWithSequence>(new string[0]);

            // Verify outcome
            Assert.NotNull(result.Value.IntSequence);
            Assert.Empty(result.Value.IntSequence);
            // Teardown
        }

        [Fact]
        public void Parse_check_nonempty_sequence()
        {
            // Fixture setup
            var expectedOptions = new FakeOptionsWithSequence
            {
                IntSequence = new[] { 60, 120 }
            };
            var sut = new Parser();

            // Exercize system
            var result = sut.ParseArguments<FakeOptionsWithSequence>(new[] { "-i", "60", "120" });

            // Verify outcome
            Assert.Empty(result.Errors);
            result.Value.ShouldHave().AllProperties().EqualTo(expectedOptions);
            // Teardown
        }

        [Fact]
        public void Parse_allow_null_default_value()
        {
            // Fixture setup
            var sut = new Parser();

            // Exercize system
            var result = sut.ParseArguments<FakeOptionsWithNullDefault>(new string[0]);

            // Verify outcome
            Assert.Null(result.Value.IntSequence);
            // Teardown
        }

        [Fact]
        public void Parse_reject_sequence_without_values()
        {
            // Fixture setup
            var sut = new Parser();

            // Exercize system
            var result = sut.ParseArguments<FakeOptionsWithSequence>(new[] { "-i" });

            // Verify outcome
            Assert.NotEmpty(result.Errors);
            // Teardown
        }

        [Fact]
        public void Parse_allow_min_equal_zero_empty()
        {
            // Fixture setup
            var sut = new Parser();

            // Exercize system
            var result = sut.ParseArguments<FakeOptionsWithSequenceWithMinZero>(new[] { "-i" });

            // Verify outcome
            Assert.NotNull(result.Value.IntSequence);
            Assert.Empty(result.Value.IntSequence);
            // Teardown
        }

        [Fact]
        public void Parse_allow_min_equal_zero_nonempty()
        {
            // Fixture setup
            var expectedOptions = new FakeOptionsWithSequence
            {
                IntSequence = new[] { 60, 120 }
            };
            var sut = new Parser();

            // Exercize system
            var result = sut.ParseArguments<FakeOptionsWithSequenceWithMinZero>(new[] { "-i", "60", "120" });

            // Verify outcome
            Assert.Empty(result.Errors);
            result.Value.ShouldHave().AllProperties().EqualTo(expectedOptions);
            // Teardown
        }

        [Fact]
        public void Parse_allow_set_option_with_non_set()
        {
            // Fixture setup
            var sut = new Parser();

            // Exercize system
            var result = sut.ParseArguments<FakeOptionsWithSetAndNonSet>(new[] { "-s", "-n" });

            // Verify outcome
            Assert.Empty(result.Errors);
            // Teardown
        }
    }
}
