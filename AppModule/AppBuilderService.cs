using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 
/// </summary>
public interface IFileDirectoriesConfigurationBuilder
{
    public ICollection<FileDirectoryBindingInfo> ListFileDirectory();
    public void AddFileDirectory(params FileDirectoryBindingInfo[] options);
    public void AddFileDirectory(System.IO.FileSystemEventHandler handler, string path, string uri);
    public ICollection<FileDirectoryBindingInfo> FileDirectories { get; set; }
    public class FileDirectoryBindingInfo
    {
        public string Path;
        public string Uri;
    }
}

/// <summary>
/// 
/// </summary>
public interface IDataSourceConnectionConfigurationBuilder
{ 

    //{ ADO, ODBC, OLEDB, JDBC }
    //{ MSSQL, MYSQL, POSTGRE, ORACLE, ACCESS }
    public ICollection<DataSourceConnectionInfo> AddDataSourceConnection(
        string Name,
        DataSourceConnectionInfo.DataProvider provider,
        DataSourceConnectionInfo.ConnectionType type,
        string Connection 
    );
    public ICollection<DataSourceConnectionInfo> ListDatasourceConnections();
    public ICollection<DataSourceConnectionInfo> DatasourceConnections { get; set; }
    public class DataSourceConnectionInfo
    {
        public enum ConnectionType { ADO, ODBC, OLEDB, JDBC }
        public enum DataProvider { MSSQL, MYSQL, POSTGRE, ORACLE, ACCESS }
        public DataProvider Provider;
        public ConnectionType Type;
        public string ConnectionString;
    }
}






/// <summary>
/// 
/// </summary>
public interface IAppBuilderService:
    IDataSourceConnectionConfigurationBuilder,
    IFileDirectoriesConfigurationBuilder
{
}


/// <summary>
/// 
/// </summary>
public class AppBuilderService: MyApplicationModel
{
    
    [AppProviderService.Service] private IFileDirectoriesConfigurationBuilder IFileDirectoriesConfigurationBuilder { get; set; }
    [AppProviderService.Service] private IDataSourceConnectionConfigurationBuilder IDataSourceConnectionConfigurationBuilder { get; set; }

    public AppBuilderService ( )
    {
        this.IFileDirectoriesConfigurationBuilder = new FileDirectoriesConfigurationBuilder();
    }

    private class FileDirectoriesConfigurationBuilder : IFileDirectoriesConfigurationBuilder
    {
        public ICollection<IFileDirectoriesConfigurationBuilder.FileDirectoryBindingInfo> ListFileDirectory() => FileDirectories;

        public void AddFileDirectory(params IFileDirectoriesConfigurationBuilder.FileDirectoryBindingInfo[] options)
        {
            foreach(var option in options)
            {
                FileDirectories.Add(option);
            }
        }

        public void AddFileDirectory(FileSystemEventHandler handler, string path, string uri)
            => AddFileDirectory(new IFileDirectoriesConfigurationBuilder.FileDirectoryBindingInfo() { 
                Path = path,
                Uri = uri                
            });
        public ICollection<IFileDirectoriesConfigurationBuilder.FileDirectoryBindingInfo> FileDirectories { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}