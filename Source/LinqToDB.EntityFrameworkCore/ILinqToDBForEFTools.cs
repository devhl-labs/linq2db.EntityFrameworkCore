﻿using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace LinqToDB.EntityFrameworkCore
{
	using DataProvider;
	using Mapping;
	using Metadata;
	using Data;

	/// <summary>
	/// Interface for EF.Core - LINQ To DB integration bridge.
	/// </summary>
	public interface ILinqToDBForEFTools
	{
		/// <summary>
		/// Clears internal caches
		/// </summary>
		void ClearCaches();

		/// <summary>
		/// Returns LINQ To DB provider, based on provider data from EF.Core.
		/// </summary>
		/// <param name="providerInfo">Provider information, extracted from EF.Core.</param>
		/// <param name="connectionInfo">Database connection information.</param>
		/// <returns>LINQ TO DB provider instance.</returns>
		IDataProvider GetDataProvider(EFProviderInfo providerInfo, EFConnectionInfo connectionInfo);

		/// <summary>
		/// Creates metadata provider for specified EF.Core data model.
		/// </summary>
		/// <param name="model">EF.Core data model.</param>
		/// <param name="dependencies"></param>
		/// <param name="mappingSource"></param>
		/// <param name="logger"></param>
		/// <returns>LINQ To DB metadata provider for specified EF.Core model. Can return <c>null</c>.</returns>
		IMetadataReader CreateMetadataReader(IModel model, RelationalSqlTranslatingExpressionVisitorDependencies dependencies, 
			IRelationalTypeMappingSource mappingSource,
			IDiagnosticsLogger<DbLoggerCategory.Query> logger);

		/// <summary>
		/// Creates mapping schema using provided EF.Core data model and metadata provider.
		/// </summary>
		/// <param name="model">EF.Core data model.</param>
		/// <param name="metadataReader">Additional optional LINQ To DB database metadata provider.</param>
		/// <param name="convertorSelector">EF Core registry for type conversion.</param>
		/// <returns>Mapping schema for provided EF.Core model.</returns>
		MappingSchema CreateMappingSchema(IModel model, IMetadataReader metadataReader, IValueConverterSelector convertorSelector);

		/// <summary>
		/// Returns mapping schema using provided EF.Core data model and metadata provider.
		/// </summary>
		/// <param name="model">EF.Core data model.</param>
		/// <param name="metadataReader">Additional optional LINQ To DB database metadata provider.</param>
		/// <param name="convertorSelector">EF Core registry for type conversion.</param>
		/// <returns>Mapping schema for provided EF.Core model.</returns>
		MappingSchema GetMappingSchema(IModel model, IMetadataReader metadataReader, IValueConverterSelector convertorSelector);

		/// <summary>
		/// Returns EF.Core <see cref="IDbContextOptions"/> for specific <see cref="DbContext"/> instance.
		/// </summary>
		/// <param name="context">EF.Core <see cref="DbContext"/> instance.</param>
		/// <returns><see cref="IDbContextOptions"/> instance.</returns>
		IDbContextOptions GetContextOptions(DbContext context);

		/// <summary>
		/// Transforms EF.Core expression tree to LINQ To DB expression.
		/// </summary>
		/// <param name="expression">EF.Core expression tree.</param>
		/// <param name="dc">LINQ To DB <see cref="IDataContext"/> instance.</param>
		/// <param name="ctx">Optional DbContext instance.</param>
		/// <param name="model">EF.Core data model instance.</param>
		/// <returns>Transformed expression.</returns>
		Expression TransformExpression(Expression expression, IDataContext dc, DbContext ctx, IModel model);

		/// <summary>
		/// Extracts <see cref="DbContext"/> instance from <see cref="IQueryable"/> object.
		/// </summary>
		/// <param name="query">EF.Core query.</param>
		/// <returns>Current <see cref="DbContext"/> instance.</returns>
		DbContext GetCurrentContext(IQueryable query);

		/// <summary>
		/// Extracts EF.Core connection information object from <see cref="IDbContextOptions"/>.
		/// </summary>
		/// <param name="options"><see cref="IDbContextOptions"/> instance.</param>
		/// <returns>EF.Core connection data.</returns>
		EFConnectionInfo ExtractConnectionInfo(IDbContextOptions options);

		/// <summary>
		/// Extracts EF.Core data model instance from <see cref="IDbContextOptions"/>.
		/// </summary>
		/// <param name="options"><see cref="IDbContextOptions"/> instance.</param>
		/// <returns>EF.Core data model instance.</returns>
		IModel ExtractModel(IDbContextOptions options);

		/// <summary>
		/// Creates logger used for logging Linq To DB connection calls.
		/// </summary>
		/// <param name="options"><see cref="IDbContextOptions"/> instance.</param>
		/// <returns>Logger instance.</returns>
		ILogger CreateLogger(IDbContextOptions options);

		/// <summary>
		/// Logs DataConnection information.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="logger"></param>
		void LogConnectionTrace(TraceInfo info, ILogger logger);

		/// <summary>
		/// Enables attaching entities to change tracker.
		/// Entities will be attached only if AsNoTracking() is not used in query and DbContext is configured to track entities. 
		/// </summary>
		bool EnableChangeTracker { get; set; }
		
	}
}
