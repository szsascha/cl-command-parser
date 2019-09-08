using Xunit;
using System;

namespace CLCommandParser.Tests
{
    public class CLCommandTest
    {
        [Fact]
        public void TestParseCommandWithoutParameters()
        {
            CLCommand cmd = CLCommand.Create("CALL");
            Assert.Equal("CALL", cmd.Name);
        }

        [Fact]
        public void TestParseCommandWithParenthesesAndQuotationMarks()
        {
            CLCommand cmd = CLCommand.Create("CALL PGM('TEST')");
            Assert.Equal("CALL", cmd.Name);
            Assert.Equal("PGM", cmd.Parameters[0].Name);
            Assert.Equal("TEST", cmd.Parameters[0].Parameters[0]);
            Assert.Equal(1, cmd.Parameters[0].Index);
        }

        [Fact]
        public void TestParseCommandWithParenthesesAndWithoutQuotationMarks()
        {
            CLCommand cmd = CLCommand.Create("CALL PGM(TEST)");
            Assert.Equal("CALL", cmd.Name);
            Assert.Equal("PGM", cmd.Parameters[0].Name);
            Assert.Equal("TEST", cmd.Parameters[0].Parameters[0]);
            Assert.Equal(1, cmd.Parameters[0].Index);
        }

        [Fact]
        public void TestParseCommandWithoutParameterName()
        {
            CLCommand cmd = CLCommand.Create("CALL 'TEST'");
            Assert.Equal("CALL", cmd.Name);
            Assert.Equal("*NONE", cmd.Parameters[0].Name);
            Assert.Equal("TEST", cmd.Parameters[0].Parameters[0]);
            Assert.Equal(1, cmd.Parameters[0].Index);
        }

        [Fact]
        public void TestParseCommandWithoutParameterNameAndWithoutQuotationMarks()
        {
            CLCommand cmd = CLCommand.Create("CALL TEST");
            Assert.Equal("CALL", cmd.Name);
            Assert.Equal("*NONE", cmd.Parameters[0].Name);
            Assert.Equal("TEST", cmd.Parameters[0].Parameters[0]);
            Assert.Equal(1, cmd.Parameters[0].Index);
        }

        [Fact]
        public void TestParseCommandWithManyBlanks01()
        {
            CLCommand cmd = CLCommand.Create("CALL      TEST     ");
            Assert.Equal("CALL", cmd.Name);
            Assert.Equal("*NONE", cmd.Parameters[0].Name);
            Assert.Equal("TEST", cmd.Parameters[0].Parameters[0]);
            Assert.Equal(1, cmd.Parameters[0].Index);
        }

        [Fact]
        public void TestParseCommandWithManyBlanks02()
        {
            CLCommand cmd = CLCommand.Create("CALL     PGM(TEST)    ");
            Assert.Equal("CALL", cmd.Name);
            Assert.Equal("PGM", cmd.Parameters[0].Name);
            Assert.Equal("TEST", cmd.Parameters[0].Parameters[0]);
            Assert.Equal(1, cmd.Parameters[0].Index);
        }

        [Fact]
        public void TestParseCommandWithCarriageReturn()
        {
            CLCommand cmd = CLCommand.Create("CALL \r TEST \r");
            Assert.Equal("CALL", cmd.Name);
            Assert.Equal("*NONE", cmd.Parameters[0].Name);
            Assert.Equal("TEST", cmd.Parameters[0].Parameters[0]);
            Assert.Equal(1, cmd.Parameters[0].Index);
        }

        [Fact]
        public void TestParseCommandWithMultipleParametersInParenthesesAndQuotationMarks()
        {
            CLCommand cmd = CLCommand.Create("CALL PGM('TEST') PARM('Just a test parameter!')");
            Assert.Equal("CALL", cmd.Name);
            Assert.Equal("PGM", cmd.Parameters[0].Name);
            Assert.Equal("TEST", cmd.Parameters[0].Parameters[0]);
            Assert.Equal(1, cmd.Parameters[0].Index);
            Assert.Equal("PARM", cmd.Parameters[1].Name);
            Assert.Equal("Just a test parameter!", cmd.Parameters[1].Parameters[0]);
            Assert.Equal(2, cmd.Parameters[1].Index);
        }

        [Fact]
        public void TestParseCommandWithMultipleParametersParenthesesAndWithoutQuotationMarks()
        {
            CLCommand cmd = CLCommand.Create("CALL PGM(TEST) PARM(1234)");
            Assert.Equal("CALL", cmd.Name);
            Assert.Equal("PGM", cmd.Parameters[0].Name);
            Assert.Equal("TEST", cmd.Parameters[0].Parameters[0]);
            Assert.Equal(1, cmd.Parameters[0].Index);
            Assert.Equal("PARM", cmd.Parameters[1].Name);
            Assert.Equal("1234", cmd.Parameters[1].Parameters[0]);
            Assert.Equal(2, cmd.Parameters[1].Index);
        }

        [Fact]
        public void TestParseCommandWithMultipleParametersWithoutParameterName()
        {
            CLCommand cmd = CLCommand.Create("CALL 'TEST' 'This is a testvalue'");
            Assert.Equal("CALL", cmd.Name);
            Assert.Equal("*NONE", cmd.Parameters[0].Name);
            Assert.Equal("TEST", cmd.Parameters[0].Parameters[0]);
            Assert.Equal(1, cmd.Parameters[0].Index);
            Assert.Equal("*NONE", cmd.Parameters[1].Name);
            Assert.Equal("This is a testvalue", cmd.Parameters[1].Parameters[0]);
            Assert.Equal(2, cmd.Parameters[1].Index);
        }

        [Fact]
        public void TestParseCommandWithMultipleParametersWithoutParameterNameAndWithoutQuotationMarks()
        {
            CLCommand cmd = CLCommand.Create("CALL TEST VALUE");
            Assert.Equal("CALL", cmd.Name);
            Assert.Equal("*NONE", cmd.Parameters[0].Name);
            Assert.Equal("TEST", cmd.Parameters[0].Parameters[0]);
            Assert.Equal(1, cmd.Parameters[0].Index);
            Assert.Equal("*NONE", cmd.Parameters[1].Name);
            Assert.Equal("VALUE", cmd.Parameters[1].Parameters[0]);
            Assert.Equal(2, cmd.Parameters[1].Index);
        }

        [Fact]
        public void TestParseCommandWitMixedParameters()
        {
            CLCommand cmd = CLCommand.Create("CALL TEST 'VALUE' INVALID(*YES) DEBUG('NO')");
            Assert.Equal("CALL", cmd.Name);
            Assert.Equal("*NONE", cmd.Parameters[0].Name);
            Assert.Equal("TEST", cmd.Parameters[0].Parameters[0]);
            Assert.Equal(1, cmd.Parameters[0].Index);
            Assert.Equal("*NONE", cmd.Parameters[1].Name);
            Assert.Equal("VALUE", cmd.Parameters[1].Parameters[0]);
            Assert.Equal(2, cmd.Parameters[1].Index);
            Assert.Equal("INVALID", cmd.Parameters[2].Name);
            Assert.Equal("*YES", cmd.Parameters[2].Parameters[0]);
            Assert.Equal(3, cmd.Parameters[2].Index);
            Assert.Equal("DEBUG", cmd.Parameters[3].Name);
            Assert.Equal("NO", cmd.Parameters[3].Parameters[0]);
            Assert.Equal(4, cmd.Parameters[3].Index);
        }

        [Fact]
        public void TestParseCommandWithMultipleSubparameters()
        {
            CLCommand cmd = CLCommand.Create("TSTCMD RECT(2 6 1 7)");
            Assert.Equal("TSTCMD", cmd.Name);
            Assert.Equal("RECT", cmd.Parameters[0].Name);
            Assert.Equal("2", cmd.Parameters[0].Parameters[0]);
            Assert.Equal("6", cmd.Parameters[0].Parameters[1]);
            Assert.Equal("1", cmd.Parameters[0].Parameters[2]);
            Assert.Equal("7", cmd.Parameters[0].Parameters[3]);
            Assert.Equal(1, cmd.Parameters[0].Index);
        }

        [Fact]
        public void TestJsonOutput()
        {
            CLCommand cmd = CLCommand.Create("CALL TEST 'VALUE' INVALID(*YES) DEBUG('NO')");
            Console.WriteLine(cmd.ToJson());
            Assert.Equal("{\"Name\":\"CALL\",\"Parameters\":[{\"Index\":1,\"Name\":\"*NONE\",\"Parameters\":[\"TEST\"],\"ParameterType\":3},{\"Index\":2,\"Name\":\"*NONE\",\"Parameters\":[\"VALUE\"],\"ParameterType\":2},{\"Index\":3,\"Name\":\"INVALID\",\"Parameters\":[\"*YES\"],\"ParameterType\":0},{\"Index\":4,\"Name\":\"DEBUG\",\"Parameters\":[\"NO\"],\"ParameterType\":1}]}", cmd.ToJson());
        }

    }
}
