using System.Collections.Generic;

namespace ProgramRunners
{
	public class RouteTestRunner
	{
		public void Run()
		{
			RunSimpleTest();
			RunComplexTest();
		}

		public void RunSimpleTest()
		{
			Shared.Node n1 = new Shared.Node() { NodeIndex = 0 };
			Shared.Node n2 = new Shared.Node() { NodeIndex = 1 };
			Shared.Node n3 = new Shared.Node() { NodeIndex = 2 };
			n1.LinkedNodes.AddRange(new [] {n2, n3});
			n2.LinkedNodes.AddRange(new [] { n3});
			// N1 -> N2
			//  |    V
			//    -> N3

			var nodeList = new List<Shared.Node>() {n1,n2,n3};
			new Shared.RouteCalculator<Shared.NodeRoute<Shared.Node>>().CalculateRoutes(nodeList, n1, n3, 20);
		}

		public void RunComplexTest()
		{

		}
	}
}