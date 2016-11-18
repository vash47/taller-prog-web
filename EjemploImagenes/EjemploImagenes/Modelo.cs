﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace EjemploImagenes
{
    public static class Modelo
    {
        const string con_str = @"Data Source=.\SQLEXPRESS;AttachDbFilename=D:\taller-prog-web\EjemploImagenes\EjemploImagenes\App_Data\UsuariosBD.mdf;Integrated Security=True;User Instance=True";
        
        public static int AgregarUsuario(Usuario u)
        {
            using (var con = new SqlConnection(con_str))
            using (var da = new SqlDataAdapter("SELECT * FROM Usuario WHERE 0 = 1", con))
            {
                var ds = new DataSet();
                da.Fill(ds);

                var nuevaFila = ds.Tables[0].NewRow();
     
                nuevaFila["Nombre"] = u.Nombre;
                nuevaFila["Apellido"] = u.Apellido;
                nuevaFila["URLImagen"] = u.URLImagen;

                ds.Tables[0].Rows.Add(nuevaFila);

                new SqlCommandBuilder(da);

                da.Update(ds);
            }
            return 0;
        }
    }
}