using Optimizations;

Console.WriteLine("Hello, World!");
var list = new List<bool>()
{
    true, false, false, false, false, false, true
};

Console.WriteLine("Original vs New collection");
foreach(var item in list)
    Console.Write(item.ToString() + "\t");

var alg = new SpaciousInsert();

int optimalIndex = alg.NextOptimalIndex(list);    //Simple example with none of the optional params passed.
//int optimalIndex = alg.NextOptimalIndex(list,(x) => !x);  //Example of call with delegate passed.
list[optimalIndex] = true;

Console.WriteLine();
foreach (var item in list)
    Console.Write(item.ToString() + "\t");