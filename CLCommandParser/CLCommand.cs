using System.Collections.Generic;

using Newtonsoft.Json;

namespace CLCommandParser
{

    public class CLCommand
    {

        private string _name;

        public string Name
        {
            get { return _name; }
        }

        private List<CLCommandParameter> _parameters = new List<CLCommandParameter>();

        public List<CLCommandParameter> Parameters
        {
            get { return _parameters; }
        }

        private CLCommand(string name)
        {
            this._name = name;
        }

        public static CLCommand Create(string commandInputString)
        {

            commandInputString = commandInputString.Trim();

            // Parse commandname
            int commandNameEndPosition = commandInputString.IndexOf(" ");

            // If there are no parameters: create and return
            if(commandNameEndPosition <= -1) {
                return new CLCommand(commandInputString.Trim());
            }
            
            CLCommand command = new CLCommand(commandInputString.Substring(0, commandNameEndPosition));

            // Get parameters
            commandInputString = commandInputString.Substring(commandInputString.IndexOf(" ")).Trim();

            int parameterIndex = 1;

            // Loop for each parameter
            while (commandInputString.Length > 0)
            {
                CLCommandParameter.Type parameterType = CLCommandParameter.detectNextParameterType(commandInputString);

                string parameterInputString = "";

                // Parse next parameter string

                // Parse LIB(TEST)
                if (parameterType == CLCommandParameter.Type.WITH_NAME_WITHOUT_QUOTATION_MARKS)
                {
                    int parenthesesClose = commandInputString.IndexOf(")");
                    parameterInputString = commandInputString.Substring(0, parenthesesClose + 1);
                    commandInputString = commandInputString.Substring(parenthesesClose + 1).Trim();
                }

                // Parse LIB('TEST')
                if (parameterType == CLCommandParameter.Type.WITH_NAME_WITH_QUOTATION_MARKS)
                {
                    int parenthesesClose = commandInputString.IndexOf("')");
                    parameterInputString = commandInputString.Substring(0, parenthesesClose + 2);
                    commandInputString = commandInputString.Substring(parenthesesClose + 2).Trim();
                }

                // Parse 'TEST'
                if (parameterType == CLCommandParameter.Type.WITHOUT_NAME_WITH_QUOTATION_MARKS)
                {
                    int quotationMarkClose = commandInputString.IndexOf("'", 1);
                    parameterInputString = commandInputString.Substring(0, quotationMarkClose + 1);
                    commandInputString = commandInputString.Substring(quotationMarkClose + 1).Trim();
                }

                // Parse *TEST
                if (parameterType == CLCommandParameter.Type.WITHOUT_NAME_WITHOUT_QUOTATION_MARKS)
                {
                    int end = commandInputString.IndexOf(" ");
                    if(end < 0) end = commandInputString.Length;
                    parameterInputString = commandInputString.Substring(0, end);
                    commandInputString = commandInputString.Substring(end).Trim();
                }
                
                command._parameters.Add(CLCommandParameter.Create(parameterInputString, parameterType, parameterIndex));

                parameterIndex++;
            }

            return command;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        
    }
}
