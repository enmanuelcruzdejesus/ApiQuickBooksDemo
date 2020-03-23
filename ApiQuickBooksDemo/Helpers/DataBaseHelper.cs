
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ApiQuickBooksDemo.Helpers
{
    public class DataBaseHelper
    {
        public static DateTime GetLastUpdateDate(IDbConnection dbConnection, int vendorId, string tableName)
        {
            using (var dbCmd = dbConnection.CreateCommand())
            {
                dbCmd.CommandText = String.Format("SELECT LastUpdateSync FROM synctables WHERE idvendor = {0} AND tablename = '{1}'", vendorId, tableName);
                var result = dbCmd.ExecuteScalar();
                if (result is DBNull) return DateTime.MinValue;
                return (DateTime)result;
            }
        }

        public static bool Exists(IDbConnection dbConnection, string tableName, string filter)
        {
            using (var dbCmd = dbConnection.CreateCommand())
            {
                dbCmd.CommandText =
                    String.Format("SELECT CASE WHEN EXISTS (SELECT 1 FROM {0} WHERE {1}) THEN 1 ELSE 0 END", tableName,
                                  filter);
                var result = dbCmd.ExecuteScalar();
                if (result is DBNull) return false;
                return Convert.ToBoolean(result);
            }


        }


        private static object GetDataValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }

            return value;
        }


        private static string LastUpdateFilter(IDbConnectionFactory dbFactory, int vendorId, string tableName)
        {
            DateTime lastUpdate;
            using (var db = dbFactory.Open())
            {
                lastUpdate = GetLastUpdateDate(db, vendorId, tableName);
            }
            return lastUpdate.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }


        //public static void PedidoMerge(string connectionString, pdpedidoshd Pedido)
        //{
        //    SqlConnection.ClearAllPools();
        //    using (var oConn = new SqlConnection(connectionString))
        //    {
        //        oConn.Open();

        //        var oCmd = new SqlCommand(@"MERGE pdpedidoshd WITH(HOLDLOCK) as dest
        //                                USING (VALUES (
        //                                        @cia,
        //                                        @transaccion,
        //                                        @sucursal,
        //                                        @pedido,
        //                                        @tipo_pedido,
        //                                        @vendedor,
        //                                        @cedula_rnc,
        //                                        @nombre,
        //                                        @celular,
        //                                        @telefono_residencia,
        //                                        @direccion,
        //                                        @sector,
        //                                        @ciudad,
        //                                        @comentario,
        //                                        @tipo_ncf,
        //                                        @fecha,
        //                                        @total_general,
        //                                        @estatus,
        //                                        @userid,
        //                                        @created,
        //                                        @lastupdate)) as src
                                                
        //                                    (cia,
        //                                     transaccion,
        //                                     sucursal,
        //                                     pedido,
        //                                     tipo_pedido,
        //                                     vendedor,
        //                                     cedula_rnc,
        //                                     nombre,
        //                                     celular,
        //                                     telefono_residencia,
        //                                     direccion,
        //                                     sector,
        //                                     ciudad,
        //                                     comentario,
        //                                     tipo_ncf,
        //                                     fecha,
        //                                     total_general,
        //                                     estatus,
        //                                     userid,
        //                                     created,
        //                                     lastupdate)
        //                                    ON src.cia = dest.cia AND src.transaccion = dest.transaccion 
        //                                WHEN MATCHED THEN
        //                                    UPDATE SET 
	       //                                    [cedula_rnc] = src.cedula_rnc
        //                                      ,[nombre] = src.nombre
        //                                      ,[telefono_residencia] = src.telefono_residencia
        //                                      ,[comentario] = src.comentario
        //                                      ,[tipo_ncf] = src.tipo_ncf
        //                                      ,[fecha] = src.fecha
        //                                      ,[total_general] = src.total_general
        //                                      ,[estatus] = src.estatus
                                       
        //                                WHEN NOT MATCHED THEN
        //                                    INSERT VALUES 
		      //                                     (   
        //                                             src.[cia],
        //                                             src.[transaccion],
        //                                             src.[sucursal],
        //                                             src.[pedido],
        //                                             src.[tipo_pedido],
        //                                             src.[vendedor],
        //                                             src.[cedula_rnc],
        //                                             src.[nombre],
        //                                             src.[celular],
        //                                             src.[telefono_residencia],
        //                                             src.[direccion],
        //                                             src.[sector],
        //                                             src.[ciudad],
        //                                             src.[comentario],
        //                                             src.[tipo_ncf],
        //                                             src.[fecha],
        //                                             src.[total_general],
        //                                             src.[estatus],
        //                                             src.[userid],
        //                                             src.[created],
        //                                             src.[lastupdate]);",
        //                                            oConn);


        //        var cia = oCmd.CreateParameter(); cia.ParameterName = "@cia"; oCmd.Parameters.Add(cia);
        //        var transaccion = oCmd.CreateParameter(); transaccion.ParameterName = "@transaccion"; oCmd.Parameters.Add(transaccion);
        //        var sucursal = oCmd.CreateParameter(); sucursal.ParameterName = "@sucursal"; oCmd.Parameters.Add(sucursal);
        //        var pedido = oCmd.CreateParameter(); pedido.ParameterName = "@pedido"; oCmd.Parameters.Add(pedido);
        //        var tipo_pedido = oCmd.CreateParameter(); tipo_pedido.ParameterName = "@tipo_pedido"; oCmd.Parameters.Add(tipo_pedido);
        //        var vendedor = oCmd.CreateParameter(); vendedor.ParameterName = "@vendedor"; oCmd.Parameters.Add(vendedor);
        //        var cedula_rnc = oCmd.CreateParameter(); cedula_rnc.ParameterName = "@cedula_rnc"; oCmd.Parameters.Add(cedula_rnc);
        //        var nombre = oCmd.CreateParameter(); nombre.ParameterName = "@nombre"; oCmd.Parameters.Add(nombre);
        //        var celular = oCmd.CreateParameter(); celular.ParameterName = "@celular"; oCmd.Parameters.Add(celular);
        //        var telefono_residencia = oCmd.CreateParameter(); telefono_residencia.ParameterName = "@telefono_residencia"; oCmd.Parameters.Add(telefono_residencia);
        //        var direccion = oCmd.CreateParameter(); direccion.ParameterName = "@direccion"; oCmd.Parameters.Add(direccion);
        //        var sector = oCmd.CreateParameter(); sector.ParameterName = "@sector"; oCmd.Parameters.Add(sector);
        //        var ciudad = oCmd.CreateParameter(); ciudad.ParameterName = "@ciudad"; oCmd.Parameters.Add(ciudad);
        //        var comentario = oCmd.CreateParameter(); comentario.ParameterName = "@comentario"; oCmd.Parameters.Add(comentario);
        //        var tipo_ncf = oCmd.CreateParameter(); tipo_ncf.ParameterName = "@tipo_ncf"; oCmd.Parameters.Add(tipo_ncf);
        //        var fecha = oCmd.CreateParameter(); fecha.ParameterName = "@fecha"; oCmd.Parameters.Add(fecha);
        //        var total_general = oCmd.CreateParameter(); total_general.ParameterName = "@total_general"; oCmd.Parameters.Add(total_general);
        //        var estatus = oCmd.CreateParameter(); estatus.ParameterName = "@estatus"; oCmd.Parameters.Add(estatus);
        //        var userid = oCmd.CreateParameter(); userid.ParameterName = "@userid"; oCmd.Parameters.Add(userid);
        //        var created = oCmd.CreateParameter(); created.ParameterName = "@created"; oCmd.Parameters.Add(created);
        //        var lastupdate = oCmd.CreateParameter(); lastupdate.ParameterName = "@lastupdate"; oCmd.Parameters.Add(lastupdate);

        //        cia.Value = Pedido.cia;
        //        transaccion.Value = Pedido.transaccion;
        //        sucursal.Value = Pedido.sucursal;
        //        pedido.Value = Pedido.pedido;
        //        tipo_pedido.Value = Pedido.tipo_pedido;
        //        vendedor.Value = Pedido.vendedor;
        //        cedula_rnc.Value = Pedido.cedula_rnc;
        //        nombre.Value = Pedido.nombre;
        //        celular.Value = GetDataValue(Pedido.celular);
        //        telefono_residencia.Value = GetDataValue(Pedido.telefono_residencia);
        //        direccion.Value = GetDataValue(Pedido.direccion);
        //        sector.Value = GetDataValue(Pedido.sector);
        //        ciudad.Value = GetDataValue(Pedido.ciudad);
        //        comentario.Value = GetDataValue(Pedido.comentario);
        //        tipo_ncf.Value = GetDataValue(Pedido.tipo_ncf);
        //        fecha.Value = Pedido.fecha;
        //        total_general.Value = Pedido.total_general;
        //        estatus.Value = GetDataValue(Pedido.estatus);
        //        userid.Value = GetDataValue(Pedido.userid);
        //        created.Value = GetDataValue(Pedido.created);
        //        lastupdate.Value = GetDataValue(Pedido.lastupdate);

        //        oCmd.ExecuteNonQuery();
                

        //        oCmd.Dispose();

        //        oConn.Close();
        //        oConn.Dispose();
        //    }
        //}

        //public static void PedidoDetailBulkMerge(string connectionString, List<pdpedidosdt> Detail)
        //{
        //    SqlConnection.ClearAllPools();
        //    using (var oConn = new SqlConnection(connectionString))
        //    {
        //        oConn.Open();

        //        var oCmd = new SqlCommand(@"MERGE pdpedidosdt WITH(HOLDLOCK) as dest
        //                                USING (VALUES (
        //                                        @cia,
        //                                        @transaccion,
        //                                        @secuencia,
        //                                        @fecha,
        //                                        @producto,
        //                                        @cantidad_pedido,
        //                                        @precio_pedido,
        //                                        @precio_contado,
        //                                        @total,
        //                                        @id_oferta_credito,
        //                                        @inicial_credito,
        //                                        @cant_cuotas_credito,
        //                                        @valor_cuotas_credito,
        //                                        @reg_activo,
        //                                        @estatus_pedido,
        //                                        @userid,
        //                                        @created,
        //                                        @lastupdate)) as src
                                                                                                
        //                                    (cia,
        //                                     transaccion,
        //                                     secuencia,
        //                                     fecha,
        //                                     producto,
        //                                     cantidad_pedido,
        //                                     precio_pedido,
        //                                     precio_contado,
        //                                     total,
        //                                     id_oferta_credito,
        //                                     inicial_credito,
        //                                     cant_cuotas_credito,
        //                                     valor_cuotas_credito,
        //                                     reg_activo,
        //                                     estatus_pedido,
        //                                     userid,
        //                                     created,
        //                                     lastupdate)
        //                                    ON src.cia = dest.cia AND src.transaccion = dest.transaccion and src.producto = dest.producto
        //                                WHEN MATCHED THEN
        //                                    UPDATE SET 
                                                   
	       //                                    [cantidad_pedido] = src.cantidad_pedido
        //                                      ,[precio_pedido] = src.precio_pedido
        //                                      ,[precio_contado] = src.precio_contado
        //                                      ,[total] = src.total
        //                                      ,[id_oferta_credito] = src.id_oferta_credito
        //                                      ,[inicial_credito] = src.inicial_credito
        //                                      ,[cant_cuotas_credito] = src.cant_cuotas_credito
        //                                      ,[valor_cuotas_credito] = src.valor_cuotas_credito
        //                                      ,[reg_activo] = src.reg_activo
        //                                      ,[lastupdate] = src.lastupdate
                                       
        //                                WHEN NOT MATCHED THEN
        //                                    INSERT VALUES 
		      //                                     (   
        //                                             src.[cia],
        //                                             src.[transaccion],
        //                                             src.[secuencia],
        //                                             src.[fecha],
        //                                             src.[producto],
        //                                             src.[cantidad_pedido],
        //                                             src.[precio_pedido],
        //                                             src.[precio_contado],
        //                                             src.[total],
        //                                             src.[id_oferta_credito],
        //                                             src.[inicial_credito],
        //                                             src.[cant_cuotas_credito],
        //                                             src.[valor_cuotas_credito],
        //                                             src.[reg_activo],
        //                                             src.[estatus_pedido],
        //                                             src.[userid],
        //                                             src.[created],
        //                                             src.[lastupdate]);",
        //                                            oConn);


        //        var cia = oCmd.CreateParameter(); cia.ParameterName = "@cia"; oCmd.Parameters.Add(cia);
        //        var transaccion = oCmd.CreateParameter(); transaccion.ParameterName = "@transaccion"; oCmd.Parameters.Add(transaccion);
        //        var secuencia = oCmd.CreateParameter(); secuencia.ParameterName = "@secuencia"; oCmd.Parameters.Add(secuencia);
        //        var fecha = oCmd.CreateParameter(); fecha.ParameterName = "@fecha"; oCmd.Parameters.Add(fecha);
        //        var producto = oCmd.CreateParameter(); producto.ParameterName = "@producto"; oCmd.Parameters.Add(producto);
        //        var cantidad_pedido = oCmd.CreateParameter(); cantidad_pedido.ParameterName = "@cantidad_pedido"; oCmd.Parameters.Add(cantidad_pedido);
        //        var precio_pedido = oCmd.CreateParameter(); precio_pedido.ParameterName = "@precio_pedido"; oCmd.Parameters.Add(precio_pedido);
        //        var precio_contado = oCmd.CreateParameter(); precio_contado.ParameterName = "@precio_contado"; oCmd.Parameters.Add(precio_contado);
        //        var total = oCmd.CreateParameter(); total.ParameterName = "@total"; oCmd.Parameters.Add(total);
        //        var id_oferta_credito = oCmd.CreateParameter(); id_oferta_credito.ParameterName = "@id_oferta_credito"; oCmd.Parameters.Add(id_oferta_credito);
        //        var inicial_credito = oCmd.CreateParameter(); inicial_credito.ParameterName = "@inicial_credito"; oCmd.Parameters.Add(inicial_credito);
        //        var cant_cuotas_credito = oCmd.CreateParameter(); cant_cuotas_credito.ParameterName = "@cant_cuotas_credito"; oCmd.Parameters.Add(cant_cuotas_credito);
        //        var valor_cuotas_credito = oCmd.CreateParameter(); valor_cuotas_credito.ParameterName = "@valor_cuotas_credito"; oCmd.Parameters.Add(valor_cuotas_credito);
        //        var reg_activo = oCmd.CreateParameter(); reg_activo.ParameterName = "@reg_activo"; oCmd.Parameters.Add(reg_activo);
        //        var estatus_pedido = oCmd.CreateParameter(); estatus_pedido.ParameterName = "@estatus_pedido"; oCmd.Parameters.Add(estatus_pedido);
        //        var userid = oCmd.CreateParameter(); userid.ParameterName = "@userid"; oCmd.Parameters.Add(userid);
        //        var created = oCmd.CreateParameter(); created.ParameterName = "@created"; oCmd.Parameters.Add(created);
        //        var lastupdate = oCmd.CreateParameter(); lastupdate.ParameterName = "@lastupdate"; oCmd.Parameters.Add(lastupdate);



        //        foreach (var item in Detail)
        //        {
        //            cia.Value = item.cia;
        //            transaccion.Value = item.transaccion;
        //            secuencia.Value = item.secuencia;
        //            fecha.Value = GetDataValue(item.fecha);
        //            producto.Value = item.producto;
        //            cantidad_pedido.Value = item.cantidad_pedido;
        //            precio_pedido.Value = item.precio_pedido;
        //            precio_contado.Value = item.precio_contado;
        //            total.Value = item.total;
        //            id_oferta_credito.Value = item.id_oferta_credito;
        //            inicial_credito.Value = item.inicial_credito;
        //            cant_cuotas_credito.Value = item.cant_cuotas_credito;
        //            valor_cuotas_credito.Value = item.valor_cuotas_credito;
        //            reg_activo.Value = GetDataValue(item.reg_activo);
        //            userid.Value = GetDataValue(item.userid);
        //            estatus_pedido.Value = GetDataValue(item.estatus_pedido);
        //            created.Value = GetDataValue(item.created);
        //            lastupdate.Value = GetDataValue(item.lastupdate);

        //            oCmd.ExecuteNonQuery();


        //        }
               


              


        //        oCmd.Dispose();

        //        oConn.Close();
        //        oConn.Dispose();
        //    }
        //}




    }
}