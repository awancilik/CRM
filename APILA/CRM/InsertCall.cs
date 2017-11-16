using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APILA.CRM
{
    public class InsertCall
    {

        public static int Insert(CRMCall obj)
        {
            var newID = 0;
            using (var _connection = new SqlConnection(Database.GetConnection))
            {
                _connection.Open();
                var tr = _connection.BeginTransaction(IsolationLevel.Serializable);
                using (var command = new SqlCommand())
                {
                    command.Transaction = tr;
                    command.Connection = _connection;
                    command.CommandText = @"INSERT INTO [Call](_event
                                               ,callId
                                               ,incoming
                                               ,[type]
                                               ,acdId
                                               ,displayNumber
                                               ,displayLabel
                                               ,displayNumberE164
                                               ,AddedOn
                                               ,terminalId
                                               ,userCrm)
                                         VALUES
                                               (@_event
                                               ,@callId
                                               ,@incoming
                                               ,@type
                                               ,@acdId
                                               ,@displayNumber
                                               ,@displayLabel
                                               ,@displayNumberE164
                                               ,@AddedOn
                                               ,@terminalId
                                               ,@userCrm)";

                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@_event", obj._event);
                    command.Parameters.AddWithValue("@callId", obj.callId);
                    command.Parameters.AddWithValue("@incoming", obj.incoming);
                    command.Parameters.AddWithValue("@type", obj.type);
                    command.Parameters.AddWithValue("@acdId", obj.acdId);
                    command.Parameters.AddWithValue("@displayNumber", obj.displayNumber);
                    command.Parameters.AddWithValue("@displayLabel", obj.displayLabel);
                    command.Parameters.AddWithValue("@displayNumberE164", obj.displayNumberE164);
                    command.Parameters.AddWithValue("@AddedOn", DateTime.Now);
                    command.Parameters.AddWithValue("@terminalId", obj.terminalId);
                    command.Parameters.AddWithValue("@userCrm", obj.userCrm);

                    try
                    {
                        command.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        throw ex;
                    }
                }
            }
            return newID;
        }

        public static int Delete()
        {
            var newID = 0;
            using (var _connection = new SqlConnection(Database.GetConnection))
            {
                _connection.Open();
                var tr = _connection.BeginTransaction(IsolationLevel.Serializable);
                using (var command = new SqlCommand())
                {
                    command.Transaction = tr;
                    command.Connection = _connection;
                    command.CommandText = @"DELETE FROM Call";
                    try
                    {
                        command.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        throw ex;
                    }
                }
            }
            return newID;
        }

        public static CRMCall GetCall()
        {
            var obj = new CRMCall();
            using (var _connection = new SqlConnection(Database.GetConnection))
            {
                _connection.Open();
                var tr = _connection.BeginTransaction(IsolationLevel.Serializable);
                using (var command = new SqlCommand())
                {
                    command.Transaction = tr;
                    command.Connection = _connection;
                    command.CommandText = @"select TOP (1) [displayNumberE164], _event
                                          ,[AddedOn]
                                          ,[terminalId]
                                          ,[userCrm] from [Call] where _event = 'callRinging'  order by [AddedOn] desc";
                    try
                    {
                        using (var rd = command.ExecuteReader())
                        {
                            if (rd.HasRows)
                            {
                                if (rd.Read())
                                {
                                    obj.displayNumberE164 = rd["displayNumberE164"].ToString();
                                    obj._event = rd["_event"].ToString();
                                    obj.AddedOn = Convert.ToDateTime(rd["AddedOn"]);
                                    obj.terminalId = rd["terminalId"].ToString();
                                    obj.userCrm = rd["userCrm"].ToString();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        throw ex;
                    }
                }
            }
            return obj;
        }
    }
}
