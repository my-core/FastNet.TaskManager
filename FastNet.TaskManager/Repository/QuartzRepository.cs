using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using FastNet.Framework.Dapper;
using FastNet.Framework.Dapper.Generator;
using MySql.Data.MySqlClient;

namespace FastNet.TaskManager.Repository
{
    /// <summary>
    /// 运营库仓储
    /// </summary>
    public class QuartzRepository : BaseRepository, IBaseRepository
    {
        private string _connString { get; set; }
        public QuartzRepository(string connString)
            : base(connString, new MySqlGenerator())
        {
            _connString = connString;
        }
        /// <summary>
        /// OpenConnection
        /// </summary>
        /// <returns></returns>
        public override IDbConnection OpenConnection()
        {
            var conn = new MySqlConnection(_connString);
            conn.Open();
            return conn;            
        }
    }
}
