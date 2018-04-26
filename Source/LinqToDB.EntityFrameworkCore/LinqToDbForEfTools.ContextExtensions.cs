﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using LinqToDB.Linq;
using Microsoft.EntityFrameworkCore;

namespace LinqToDB.EntityFrameworkCore
{
	using Data;

	public static partial class LinqToDbForEfTools
	{
		#region BulkCopy

		/// <summary>
		/// Performs bulk insert operation.
		/// </summary>
		/// <typeparam name="T">Mapping type of inserted record.</typeparam>
		/// <param name="context">Database context.</param>
		/// <param name="options">Operation options.</param>
		/// <param name="source">Records to insert.</param>
		/// <returns>Bulk insert operation status.</returns>
		public static BulkCopyRowsCopied BulkCopy<T>([NotNull] this DbContext context, BulkCopyOptions options, IEnumerable<T> source)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			return context.CreateLinqToDbConnection().BulkCopy(options, source);
		}

		/// <summary>
		/// Performs bulk insert operation.
		/// </summary>
		/// <typeparam name="T">Mapping type of inserted record.</typeparam>
		/// <param name="context">Database context.</param>
		/// <param name="maxBatchSize">Number of rows in each batch. At the end of each batch, the rows in the batch are sent to the server. </param>
		/// <param name="source">Records to insert.</param>
		/// <returns>Bulk insert operation status.</returns>
		public static BulkCopyRowsCopied BulkCopy<T>([NotNull] this DbContext context, int maxBatchSize, IEnumerable<T> source)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			var dataConnection = context.CreateLinqToDbConnection();

			return dataConnection.DataProvider.BulkCopy(
				dataConnection,
				new BulkCopyOptions { MaxBatchSize = maxBatchSize },
				source);
		}

		/// <summary>
		/// Performs bulk insert operation.
		/// </summary>
		/// <typeparam name="T">Mapping type of inserted record.</typeparam>
		/// <param name="context">Database context.</param>
		/// <param name="source">Records to insert.</param>
		/// <returns>Bulk insert operation status.</returns>
		public static BulkCopyRowsCopied BulkCopy<T>([NotNull] this DbContext context, IEnumerable<T> source)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			var dataConnection = context.CreateLinqToDbConnection();

			return dataConnection.DataProvider.BulkCopy(
				dataConnection,
				new BulkCopyOptions(),
				source);
		}

		#endregion

		#region Value Insertable

		/// <summary>
		/// Starts insert operation LINQ query definition.
		/// </summary>
		/// <typeparam name="T">Target table mapping class.</typeparam>
		/// <param name="context">Database context.</param>
		/// <param name="target">Target table.</param>
		/// <returns>Insertable source query.</returns>
		[LinqTunnel]
		[Pure]
		public static IValueInsertable<T> Into<T>([NotNull] this DbContext context, [NotNull] ITable<T> target)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));
			if (target  == null) throw new ArgumentNullException(nameof(target));

			return context.CreateLinqToDbConnection().Into(target);
		}

		#endregion
	}
}
