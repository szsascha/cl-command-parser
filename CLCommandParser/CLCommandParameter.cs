using System.Collections.Generic;

namespace CLCommandParser
{

    public class CLCommandParameter
    {

        private int _index;

        public int Index
        {
            get { return _index; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
        }

        private List<string> _parameters;

        public List<string> Parameters
        {
            get { return _parameters; }
        }

        private Type _type;

        public Type ParameterType
        {
            get { return _type; }
        }

        public enum Type
        {
            WITH_NAME_WITHOUT_QUOTATION_MARKS, // Example: NAME(TEST)
            WITH_NAME_WITH_QUOTATION_MARKS, // Example: NAME('TEST')
            WITHOUT_NAME_WITH_QUOTATION_MARKS, // Example: 'TEST'
            WITHOUT_NAME_WITHOUT_QUOTATION_MARKS // Example: TEST
        }

        private CLCommandParameter(string name, List<string> parameters, Type type, int index)
        {
            this._name = name;
            this._parameters = parameters;
            this._type = type;
            this._index = index;
        }

        public static CLCommandParameter Create(string parameterInputString, Type type, int index)
        {
            string name = "*NONE";

            // Detect parameter name
            if (type == Type.WITH_NAME_WITHOUT_QUOTATION_MARKS || type == Type.WITH_NAME_WITH_QUOTATION_MARKS)
            {
                int firstParenthesePosition = parameterInputString.IndexOf("(");
                int lastParenthesePosition = parameterInputString.IndexOf(")");

                name = parameterInputString.Substring(0, firstParenthesePosition).Trim();
                parameterInputString = parameterInputString.Substring(firstParenthesePosition + 1, lastParenthesePosition - firstParenthesePosition - 1).Trim();
            }

            // Detect subparameters
            List<string> subparameters = new List<string>();

            // Example: NAME('TEST') || // Example: 'TEST'
            if (type == Type.WITH_NAME_WITH_QUOTATION_MARKS || type == Type.WITHOUT_NAME_WITH_QUOTATION_MARKS)
            {
                parameterInputString = parameterInputString.Substring(1);
                parameterInputString = parameterInputString.Remove(parameterInputString.Length - 1);
                subparameters.Add(parameterInputString);
            }

            // Example: TEST
            if (type == Type.WITHOUT_NAME_WITHOUT_QUOTATION_MARKS)
            {
                subparameters.Add(parameterInputString);
            }

            // Example: RECT(2 4 2 4)
            if (type == Type.WITH_NAME_WITHOUT_QUOTATION_MARKS)
            {
                foreach (string str in parameterInputString.Split(' '))
                {
                    subparameters.Add(str);
                }
            }

            CLCommandParameter parameter = new CLCommandParameter(name, subparameters, type, index);
            return parameter;
        }

        public static Type detectNextParameterType(string parameterString)
        {
            int firstBlankPosition = parameterString.IndexOf(" ");
            if (firstBlankPosition < 0) firstBlankPosition = int.MaxValue; // Set max value so that the other value is always lower

            int firstParenthesePosition = parameterString.IndexOf("(");
            if (firstParenthesePosition < 0) firstParenthesePosition = int.MaxValue; // Set max value so that the other value is always lower

            if (firstParenthesePosition < firstBlankPosition)
            {
                if (parameterString.IndexOf("('") == firstParenthesePosition)
                {
                    // Example: NAME('TEST')
                    return CLCommandParameter.Type.WITH_NAME_WITH_QUOTATION_MARKS;
                }
                else
                {
                    // Example: NAME(TEST)
                    return CLCommandParameter.Type.WITH_NAME_WITHOUT_QUOTATION_MARKS;
                }
            }
            else
            {
                if (parameterString.Substring(0, 1) == "'")
                {
                    // Example: 'TEST'
                    return CLCommandParameter.Type.WITHOUT_NAME_WITH_QUOTATION_MARKS;
                }
                else
                {
                    // Excample: TEST
                    return CLCommandParameter.Type.WITHOUT_NAME_WITHOUT_QUOTATION_MARKS;
                }
            }
        }

    }

}