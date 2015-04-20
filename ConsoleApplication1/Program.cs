using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{
			
		}
		static void f1(params int[] y)
		{
		}

		static void sample()
		{
			int[] i = new Int32[3];
			var c = new List<int>();
			f1(c.ToArray());
			f1(1,3,4);
			f1(new[] {1,3,4});
		}
	}
}
