using System.Collections.Generic;

namespace ProgramRunners
{
	public class RouteTestRunner
	{
		public void RunRouteTests()
		{
			RunSimpleTest();
			RunComplexTest();
		}

		public void RunSimpleTest()
		{
			System.Console.WriteLine("Testing routes lookup simple");
			Shared.Node n1 = new Shared.Node() { NodeIndex = 1 };
			Shared.Node n2 = new Shared.Node() { NodeIndex = 2 };
			Shared.Node n3 = new Shared.Node() { NodeIndex = 3 };
			n1.LinkedNodes.AddRange(new [] {n2, n3});
			n2.LinkedNodes.AddRange(new [] { n3});
			// N1 -> N2
			//  |    V
			//    -> N3
			var routeCalculator = new Shared.RouteCalculator<Shared.Node<Shared.Node>, Shared.Node>();
			var nodeList = new List<Shared.Node<Shared.Node>>() {n1,n2,n3};
			var routes = routeCalculator.CalculateRoutes(nodeList, n1, n3, 20);

			System.Console.WriteLine("Pre filter:");
			routes.ForEach(r => System.Console.WriteLine(r));

			System.Console.WriteLine("Post filter");
			routes = routeCalculator.FilterRoutesOnDestinationReached(routes);
			routes.ForEach(r => System.Console.WriteLine(r));
		}

		public void RunComplexTest()
		{
			System.Console.WriteLine("Testing routes lookup complex");

		}
	}
}