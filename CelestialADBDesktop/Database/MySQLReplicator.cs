using Harris.CelestialADB.ApiData;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harris.CelestialADB.Desktop.Database
{
    enum DataSource
    {
        Celestial,
        User
    }

    interface ISQLReplicator
    {
        /// <summary>
        /// Check to see if the database is configured, for use when connecting to a database for the first time
        /// </summary>
        /// <returns>if the database appears to be setup as an altiumdb</returns>
        Task<bool> IsDatabaseConfigured();

        /// <summary>
        /// Creates the database from scratch
        /// </summary>
        /// <param name="columns">Columns needed in the components tables</param>
        /// <param name="views">View definitions</param>
        /// <param name="name">Name of the database</param>
        /// <returns>Success or Failure</returns>
        Task<bool> CreateDatabase(List<DatabaseTableColumnDefinition> columns, List<DatabaseViewDefinition> views, string name = "altium_library");
        /// <summary>
        /// Update the whole database to match the Celestial data
        /// </summary>
        /// <param name="columns">Columns in the components tables</param>
        /// <param name="views">View definitions to be created/updated</param>
        /// <returns></returns>
        Task<bool> UpdateDatabaseDefinitions(List<DatabaseTableColumnDefinition> columns, List<DatabaseViewDefinition> views);

        /// <summary>
        /// Syncronise new columns from Celestial source to user database
        /// </summary>
        /// <param name="columns">Columns required for the database</param>
        /// <returns>Success or failure</returns>
        Task<bool> UpdateTableDefinition(List<DatabaseTableColumnDefinition> columns);
        /// <summary>
        /// Adds a single column to the components tables
        /// </summary>
        /// <param name="column">Column definition</param>
        /// <returns>Whether the action was successful</returns>
        Task<bool> AddColumn(DatabaseTableColumnDefinition column);

        /// <summary>
        /// Create a new view in the database
        /// </summary>
        /// <param name="view">View definition</param>
        /// <param name="source">Where the view data came from</param>
        /// <returns>Success of the action</returns>
        Task<bool> AddView(DatabaseViewDefinition view, DataSource source = DataSource.Celestial);
        /// <summary>
        /// Update a view definition which has changed
        /// </summary>
        /// <param name="view">View data</param>
        /// <returns></returns>
        Task<bool> UpdateView(DatabaseViewDefinition view);

        /// <summary>
        /// Add multiple components to the database
        /// </summary>
        /// <param name="components">Component Defintions</param>
        /// <param name="source">Source the components came from</param>
        /// <returns></returns>
        Task<bool> AddComponents(List<AltiumComponent> components, DataSource source = DataSource.Celestial);
        /// <summary>
        /// Add a single component to the database
        /// </summary>
        /// <param name="component">Component details</param>
        /// <param name="source">Component information source</param>
        /// <returns></returns>
        Task<bool> AddComponent(AltiumComponent component, DataSource source = DataSource.Celestial);
        /// <summary>
        /// Update a component's definition
        /// </summary>
        /// <param name="component">Component to be updated</param>
        /// <param name="source">datasource of the component</param>
        /// <returns></returns>
        Task<bool> UpdateComponent(AltiumComponent component, DataSource source = DataSource.Celestial);

        /// <summary>
        /// Get the last component ID from the Celestial component table
        /// </summary>
        /// <returns>Highest component ID from the local celestial data source</returns>
        Task<int> LatestComponentId();

        /// <summary>
        /// Retrieve all the local user created components
        /// </summary>
        /// <returns>User created components</returns>
        Task<List<AltiumComponent>> UserComponents();

        /// <summary>
        /// Retrieve all the local user created views
        /// </summary>
        /// <returns>User created views</returns>
        Task<List<DatabaseViewDefinition>> UserViews();
        

        /// <summary>
        /// Test the connection string in the config to see if the database can be contacted
        /// </summary>
        /// <returns>Success of the connection</returns>
        Task<bool> TestConnection();
    }

    class MySQLReplicator : ISQLReplicator, IDisposable
    {
        //https://dev.mysql.com/downloads/windows/installer/5.7.html
        //https://dev.mysql.com/get/Downloads/MySQLInstaller/mysql-installer-web-community-5.7.18.1.msi

        MySqlConnection conn;

        public MySQLReplicator()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.MySqlConnectionString))
            {
                conn = new MySqlConnection(Properties.Settings.Default.MySqlConnectionString);
                conn.Open();
            }
        }


        public async Task<bool> TestConnection()
        {
            return await CheckConnection();
        }

        async Task<bool> CheckConnection()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.MySqlConnectionString))
                return false; // no connection string

            if (conn == null)
            {
                conn = new MySqlConnection(Properties.Settings.Default.MySqlConnectionString);
                conn.Open();
            }

            if (conn.State == System.Data.ConnectionState.Open)
            {
                return true;
            }

            conn.ConnectionString = Properties.Settings.Default.MySqlConnectionString;
            await conn.OpenAsync();

            if (conn.State == System.Data.ConnectionState.Open)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> AddColumn(DatabaseTableColumnDefinition column)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddComponent(AltiumComponent component, DataSource source = DataSource.Celestial)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddComponents(List<AltiumComponent> components, DataSource source = DataSource.Celestial)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddView(DatabaseViewDefinition view, DataSource source = DataSource.Celestial)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateDatabase(List<DatabaseTableColumnDefinition> columns, List<DatabaseViewDefinition> views, string name = "altium_library")
        {
            if (!(await CheckConnection()))
                return false;

            //todo: sanity check name.
            MySqlCommand cmd = new MySqlCommand(String.Format("CREATE DATABASE IF NOT EXISTS {0}", name), conn);
            await cmd.ExecuteNonQueryAsync();

            throw new NotImplementedException();
        }

        public async Task<int> LatestComponentId()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateComponent(AltiumComponent component, DataSource source = DataSource.Celestial)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateTableDefinition(List<DatabaseTableColumnDefinition> columns)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateView(DatabaseViewDefinition view)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AltiumComponent>> UserComponents()
        {
            throw new NotImplementedException();
        }

        public async Task<List<DatabaseViewDefinition>> UserViews()
        {
            throw new NotImplementedException();
        }
        public async Task<bool> IsDatabaseConfigured()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateDatabaseDefinitions(List<DatabaseTableColumnDefinition> columns, List<DatabaseViewDefinition> views)
        {
            throw new NotImplementedException();
        }
        

        public void Dispose()
        { 
            if (conn != null)
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Dispose();
            }
        }

    }
}
