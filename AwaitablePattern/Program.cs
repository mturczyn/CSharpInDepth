using FirstEncounterWithAsync.AwaitablePattern;
using System;

var myTask = new MyTask<int>();
var three = await myTask;
Console.WriteLine($"Task returned value {three}");
