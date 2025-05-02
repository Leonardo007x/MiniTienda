using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;

namespace MiniTienda.Data
{
    internal class UsersData
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

        public bool saveUsers(string _mail, string _password, string _salt, string _state)
        {
            bool executed = false;  // Variable que indica si la operación fue exitosa
            int row;  // Almacena el número de filas afectadas por la ejecución del comando

            MySqlCommand objSelectCmd = new MySqlCommand();
            objSelectCmd.Connection = objPer.openConnection();

            // Se indica el nombre del procedimiento almacenado a ejecutar
            objSelectCmd.CommandText = "spInsertUsers"; // <-- Aquí debe ir el nombre exacto del procedimiento en MySQL

            // Se indica que el comando es un procedimiento almacenado
            objSelectCmd.CommandType = CommandType.StoredProcedure;

            // Se agregan los parámetros requeridos por el procedimiento almacenado, junto con sus valores
            objSelectCmd.Parameters.Add("p_mail", MySqlDbType.VarString).Value = _mail;
            objSelectCmd.Parameters.Add("p_password", MySqlDbType.VarString).Value = _password;
            objSelectCmd.Parameters.Add("p_salt", MySqlDbType.VarString).Value = _salt;
            objSelectCmd.Parameters.Add("p_state", MySqlDbType.VarString).Value = _state;

            try
            {
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

        public bool updateUsers(int _id, string _mail, string _password, string _salt, string _state)
        {
            bool executed = false; 
            int row; 

            MySqlCommand objSelectCmd = new MySqlCommand();
            objSelectCmd.Connection = objPer.openConnection();

            objSelectCmd.CommandText = "spUpdateUsers"; // <-- Este debe coincidir con el nombre del procedimiento en MySQL

            // Indicar que se ejecutará un procedimiento almacenado
            objSelectCmd.CommandType = CommandType.StoredProcedure;

            // Agregar parámetros al comando, que serán enviados al procedimiento almacenado
            objSelectCmd.Parameters.Add("p_id", MySqlDbType.Int32).Value = _id;              
            objSelectCmd.Parameters.Add("p_mail", MySqlDbType.VarString).Value = _mail;     
            objSelectCmd.Parameters.Add("p_password", MySqlDbType.VarString).Value = _password; 
            objSelectCmd.Parameters.Add("p_salt", MySqlDbType.VarString).Value = _salt;     
            objSelectCmd.Parameters.Add("p_state", MySqlDbType.VarString).Value = _state;    

            try
            {
                row = objSelectCmd.ExecuteNonQuery();

                // Si se afectó exactamente una fila, se considera una actualización exitosa
                if (row == 1)
                {
                    executed = true;
                }
            }
            catch (Exception e)
            {
                // En caso de error, mostrar el mensaje en la consola
                Console.WriteLine("Error " + e.ToString());
            }


            objPer.closeConnection();
            return executed;
        }


    }
}
