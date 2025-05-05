using MiniTienda.Data;
using System;
using System.Data;

namespace MiniTienda.Logic
{
    public class ProvidersLog
    {
        ProvidersData objPrv = new ProvidersData();

        public DataSet ShowProvidersDDL()
        {
            return objPrv.ShowProvidersDDL();
        }

        public DataSet ShowProviders()
        {
            return objPrv.ShowProviders();
        }

        public bool SaveProvider(string nit, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del proveedor no puede estar vacío.");

            // Evitar nombres duplicados
            var existingProviders = objPrv.ShowProviders();
            foreach (DataRow row in existingProviders.Tables[0].Rows)
            {
                if (row["prov_nombre"].ToString().Equals(name, StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Ya existe un proveedor con este nombre.");
            }

            return objPrv.SaveProvider(nit, name);
        }

        public bool UpdateProvider(int id, string nit, string name)
        {
            if (id <= 0)
                throw new ArgumentException("El ID no es válido.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede estar vacío.");

            return objPrv.UpdateProvider(id, nit, name);
        }

        public bool DeleteProvider(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID no es válido.");

            return objPrv.DeleteProvider(id); 
        }
    }
}
