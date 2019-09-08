CL command parser
=================

Parse CL commands and convert them to json.

Example
-------

```
CLCommand cmd = CLCommand.Create("CALL TEST 'VALUE' INVALID(*YES) DEBUG('NO')");
Console.WriteLine(cmd.ToJson());
```

The output will look like:

```
{
	"Name": "CALL",
	"Parameters": [{
		"Index": 1,
		"Name": "*NONE",
		"Parameters": ["TEST"],
		"ParameterType": 3
	}, {
		"Index": 2,
		"Name": "*NONE",
		"Parameters": ["VALUE"],
		"ParameterType": 2
	}, {
		"Index": 3,
		"Name": "INVALID",
		"Parameters": ["*YES"],
		"ParameterType": 0
	}, {
		"Index": 4,
		"Name": "DEBUG",
		"Parameters": ["NO"],
		"ParameterType": 1
	}]
}
```

In the test cases there are many examples. 