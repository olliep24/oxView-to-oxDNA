// See https://aka.ms/new-console-template for more information
using oxViewToOxDNA.src;

if (args.Length != 1)
{
    Console.WriteLine("Please provide the oxView file to convert");
}
else
{
    var inputFile = args[0];

    Console.WriteLine($"Converting file ${inputFile}");

    Converter.Convert(inputFile, "./Output/output.top", "./Output/output.dat");

    var webSocketConnect = new WebSocketConnection("./Output/output.top", "./Output/output.dat");

    webSocketConnect.Connect();
}