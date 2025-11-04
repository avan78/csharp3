# BreakoutRoom 06.1

Cíl: Procvičit si práci s generiky.

1. Implementujte generickou metodu Max<T>, která porovná dva parametry typu IComparable<T> a vrátí jejich maximum. //Řešení přes CompareTo()?
2. Implementujte generickou metodu Max<T>, která přijme IEnumerable<IComparable<T>> a vrátí jejich maximum. //Řešení přes .Max()?
3. Implementujte generickou metodu IsDistinct<T>, která přijme IEnumerable<IComparable<T>> a vrátí true, pokud neobsahuje duplikáty //Řešení přes .Distinct()?


using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
	static void Main()
	{
		
		
		
		public static T Max<T>(T firstParam, T secondParam) 
		
			where T : IComparable<T>
		{
				//return items.Max(); jestliže je na vstupu list
				
			if (a.CompareTo(b) > 0) {
			return a;
			}
			else (a.CompareTo(b) < 0)  {
			return b;
			};
				return a; 
		}
		
		Console.WriteLine(Max(3,1));
				Console.WriteLine(Max("a","c"));

			public static T Max2<T>(IEnumerable<T> items) 
		
			where T : IComparable<T>
		{
				var max = items.First();
			foreach (var item in items)
			{
				if(item.CompareTo(max) > 0 ) { 
					max = item;
				}
			}
								return max;
							//	return a.CompareTo(b) > 0;


		}
		
		Console.WriteLine(Max2("a","c", "g"));
		
			public static bool IsDistinct<T>(IEnumerable<T> items) 
		
			where T : IComparable<T>
		{
				return items.Distinct().Count() == items.Count();

		}
		
		Console.WriteLine(Max2("a","c", "g"));
		
		
		
		
		var numbers = new List<int>
		{
			1,
			4,
			7,
			10
		};
		var words = new List<string>
		{
			"Ahoj",
			"Czechitas",
			"Programovani"
		};
		Console.WriteLine("Čísla větší než 5:");
		PrintGreaterThanInt(numbers, 5);
		
		Console.WriteLine("\nSlova 'větší' než 'Czechitas':");
		PrintGreaterThanString(words, "Czechitas");
		
		Console.WriteLine("\n(Přes generiku) Čísla větší než 5:");
		PrintGreaterThan(numbers, 5);
		
		Console.WriteLine("\n(Přes generiku) Slova 'větší' než 'Czechitas':");
		PrintGreaterThan(words, "Czechitas");
	}

	static void PrintGreaterThanInt(List<int> items, int minValue)
	{
		foreach (var item in items)
		{
			if (item > minValue)
				Console.WriteLine(item);
		}
	}

	static void PrintGreaterThanString(List<string> items, string minValue)
	{
		foreach (var item in items)
		{
			if (string.Compare(item, minValue) > 0)
				Console.WriteLine(item);
		}
	}

	static void PrintGreaterThan<T>(List<T> items, T minValue)
		where T : IComparable<T>
	{
		foreach (var item in items)
		{
			if (item.CompareTo(minValue) > 0)
				Console.WriteLine(item);
		}
	}
}
