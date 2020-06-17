using Dapper;
using EasyAssetManagerCore.Model.CommonModel;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyAssetManagerCore.Repository.Common
{

    public abstract class GenericRepository<TEntity> : BaseRepository where TEntity : class
    {
        public GenericRepository(OracleConnection connection, OracleTransaction transaction = null)
            : base(connection, transaction)
        {

        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            string query = QB<TEntity>.Select();
            if (Transaction != null)
            {
                return Connection.Query<TEntity>(query, transaction: Transaction);
            }
            return Connection.Query<TEntity>(query);
        }

        public virtual TEntity Get(int id)
        {
            string query = QB<TEntity>.SelectById(id);
            if (Transaction != null)
            {
                return Connection.Query<TEntity>(query, transaction: Transaction).FirstOrDefault();
            }

            return Connection.Query<TEntity>(query).FirstOrDefault();
        }

        public virtual int Insert(TEntity entity)
        {
            dynamic id;
            int primaryKeyValue = 0;
            string query = QB<TEntity>.Insert();

            if (Transaction != null)
            {
                id = Connection.Query(query, entity, transaction: Transaction).FirstOrDefault();

            }
            else
            {
                id = Connection.Query(query, entity).FirstOrDefault();
            }



            if (id != null)
            {
                var firstItem = (IDictionary<string, object>)id;
                foreach (var v in firstItem)
                {
                    primaryKeyValue = Convert.ToInt32(v.Value);
                }
            }

            return primaryKeyValue;
        }

        public virtual int InsertWithPrimaryKey(TEntity entity)
        {
            dynamic id;
            int primaryKeyValue = 0;
            string query = QB<TEntity>.InsertWithPrimaryKey();

            if (Transaction != null)
            {
                id = Connection.Query(query, entity, transaction: Transaction).FirstOrDefault();

            }
            else
            {
                id = Connection.Query(query, entity).FirstOrDefault();
            }



            if (id != null)
            {
                var firstItem = (IDictionary<string, object>)id;
                foreach (var v in firstItem)
                {
                    primaryKeyValue = Convert.ToInt32(v.Value);
                }
            }

            return primaryKeyValue;
        }

        public virtual int Update(TEntity entity)
        {
            string query = QB<TEntity>.Update();

            if (Transaction != null)
            {
                return Connection.Execute(query, entity, transaction: Transaction);

            }
            else
            {
                return Connection.Execute(query, entity);
            }


        }

        public virtual void Delete(int id)
        {
            string query = QB<TEntity>.Delete();
            string primaryKey = QB<TEntity>.GetPrimaryKeyColumns().FirstOrDefault();

            var p = new DynamicParameters();
            p.Add("@" + primaryKey, id);

            if (Transaction != null)
            {
                Connection.Execute(query, p, transaction: Transaction);

            }
            else
            {
                Connection.Execute(query, p);
            }
        }

        public virtual void SetTransction(OracleTransaction transaction)
        {
            Transaction = transaction;
        }
    }
}
