﻿using System.Collections.Generic;

namespace Shuttle.Core.Data.Tests.Fakes
{
	public class OrderAssembler : IAssembler<Order>
	{
		public IEnumerable<Order> Assemble(MappedData data)
		{
			var result = new List<Order>();

			foreach (var orderRow in data.MappedRows<Order>())
			{
				var order = orderRow.Result;

				foreach (var orderLineRow in data.MappedRows<OrderLine>())
				{
					if (orderLineRow.Row["OrderNumber"].Equals(order.OrderNumber))
					{
						order.AddLine(orderLineRow.Result);
					}
				}

				result.Add(order);
			}

			return result;
		}
	}
}