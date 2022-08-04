using System;
using System.Data;
using System.Data.Common;

namespace EntityFrameworkCore.AutoFixture.Tests.Common;

public class FakeConnection : DbConnection
{
    public override string ConnectionString { get; set; }

    public override string Database => throw new NotImplementedException();

    public override string DataSource => throw new NotImplementedException();

    public override string ServerVersion => throw new NotImplementedException();

    public override ConnectionState State => throw new NotImplementedException();

    public Action OnOpen { get; set; }

    public override void ChangeDatabase(string databaseName)
    {
        throw new NotImplementedException();
    }

    public override void Close()
    {
        throw new NotImplementedException();
    }

    public override void Open()
    {
        this.OnOpen?.Invoke();
    }

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
        throw new NotImplementedException();
    }

    protected override DbCommand CreateDbCommand()
    {
        throw new NotImplementedException();
    }
}
