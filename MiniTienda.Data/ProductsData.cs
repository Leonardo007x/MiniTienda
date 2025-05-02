using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;

namespace MiniTienda.Data
{
    internal class ProductsData
    {
        Persistence objPer = new Persistence();

        public DataSet showUsers()
        {
            MySqlDataAdapter objAdapter = new MySqlDataAdapter();
            DataSet objData = new DataSet();

            MySqlCommand objSelectCmd = new MySqlCommand();
            objSelectCmd.Connection = objPer.openConnection();
            objSelectCmd.CommandText = "spSelectProvidersDDL";
            objSelectCmd.CommandType = CommandType.StoredProcedure;
            objAdapter.SelectCommand = objSelectCmd;
            objAdapter.Fill(objData);
            objPer.closeConnection();
            return objData;
        }

        public bool saveProducts(string _code, string _description, int _quantity, double _price, int _fkCategory, int _fkProvider)
        {
            bool executed = false;
            int row;

            MySqlCommand objSelectCmd = new MySqlCommand();
            objSelectCmd.Connection = objPer.openConnection();

            // Asignar el nombre del procedimiento almacenado que inserta productos
            objSelectCmd.CommandText = "spInsertProducts"; // <-- Asegúrate de que este procedimiento exista en MySQL
            objSelectCmd.CommandType = CommandType.StoredProcedure;

            // Agregar los parámetros requeridos por el procedimiento con sus respectivos valores
            objSelectCmd.Parameters.Add("p_code", MySqlDbType.VarString).Value = _code;               
            objSelectCmd.Parameters.Add("p_description", MySqlDbType.VarString).Value = _description; 
            objSelectCmd.Parameters.Add("p_quantity", MySqlDbType.Int32).Value = _quantity;           
            objSelectCmd.Parameters.Add("p_price", MySqlDbType.Double).Value = _price;                
            objSelectCmd.Parameters.Add("p_fkcategory", MySqlDbType.Int32).Value = _fkCategory;       
            objSelectCmd.Parameters.Add("p_fkprovider", MySqlDbType.Int32).Value = _fkProvider;      

            try
            {
                // Ejecutar el comando y obtener cuántas filas fueron afectadas
                row = objSelectCmd.ExecuteNonQuery();

                if (row == 1)
                {
                    executed = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e.ToString());
            }


            objPer.closeConnection();
            return executed;
        }

        public bool updateProducts(int _id, string _code, string _description, int _quantity, double _price, int _fkCategory, int _fkProvider)
        {
            bool executed = false; 
            int row;  

            MySqlCommand objSelectCmd = new MySqlCommand();
            objSelectCmd.Connection = objPer.openConnection();

            // Se asigna el nombre del procedimiento almacenado encargado de actualizar productos
            objSelectCmd.CommandText = "spUpdateProduct"; // <-- Asegúrate de que este procedimiento exista en tu base de datos
            objSelectCmd.CommandType = CommandType.StoredProcedure;

            // Se agregan los parámetros requeridos por el procedimiento almacenado con sus valores correspondientes
            objSelectCmd.Parameters.Add("p_id", MySqlDbType.Int32).Value = _id;                      
            objSelectCmd.Parameters.Add("p_code", MySqlDbType.VarString).Value = _code;              
            objSelectCmd.Parameters.Add("p_description", MySqlDbType.VarString).Value = _description;
            objSelectCmd.Parameters.Add("p_quantity", MySqlDbType.Int32).Value = _quantity;          
            objSelectCmd.Parameters.Add("p_price", MySqlDbType.Double).Value = _price;               
            objSelectCmd.Parameters.Add("p_fkcategory", MySqlDbType.Int32).Value = _fkCategory;      
            objSelectCmd.Parameters.Add("p_fkprovider", MySqlDbType.Int32).Value = _fkProvider;      

            try
            {
                // Se ejecuta el procedimiento almacenado y se obtiene cuántas filas fueron afectadas
                row = objSelectCmd.ExecuteNonQuery();

                if (row == 1)
                {
                    executed = true;
                }
            }
            catch (Exception e)
            {
                // Si ocurre una excepción, se muestra el mensaje de error en consola
                Console.WriteLine("Error " + e.ToString());
            }

            objPer.closeConnection();
            return executed;
        }



    }
}
