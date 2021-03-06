﻿using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Moq;
using NUnit.Framework;

namespace Shuttle.Core.Data.Tests
{
    [TestFixture]
    public abstract class Fixture
    {
	    protected static string DefaultConnectionStringName = "Shuttle";
		protected static string DefaultProviderName = "System.Data.SqlClient";
		protected static string DefaultConnectionString = "Data Source=.\\sqlexpress;Initial Catalog=Shuttle;Integrated Security=SSPI";

        protected Fixture()
        {
#if NETCOREAPP2_1
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
#endif
        }

        protected DbConnectionFactory GetDbConnectionFactory()
        {
            return new DbConnectionFactory();
        }

	    protected IDatabaseContext GetDatabaseContext()
		{
			return new DatabaseContextFactory(GetConnectionConfigurationProvider(), GetDbConnectionFactory(), new DbCommandFactory(), new ThreadStaticDatabaseContextCache()).Create(DefaultConnectionStringName);
		}

        protected IConnectionConfigurationProvider GetConnectionConfigurationProvider()
        {
            var provider = new Mock<IConnectionConfigurationProvider>();

            provider.Setup(m => m.Get("Shuttle")).Returns(new ConnectionConfiguration(
                "Shuttle",
                "System.Data.SqlClient",
                "Data Source=.\\sqlexpress;Initial Catalog=Shuttle;Integrated Security=SSPI"));

            return provider.Object;
        }

        protected IDatabaseContext GetDatabaseContext(Mock<IDbCommand> command)
	    {
		    var commandFactory = new Mock<IDbCommandFactory>();

		    commandFactory.Setup(m => m.CreateCommandUsing(It.IsAny<IDbConnection>(), It.IsAny<IQuery>())).Returns(command.Object);

			return new DatabaseContextFactory(GetConnectionConfigurationProvider(), GetDbConnectionFactory(), commandFactory.Object, new ThreadStaticDatabaseContextCache()).Create(DefaultConnectionStringName);
	    }
    }
}
