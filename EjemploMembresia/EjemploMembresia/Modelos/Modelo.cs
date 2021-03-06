﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;

namespace EjemploMembresia
{
    public static class Modelo
    {
        public static string[] nombresRoles = { "administrador", "usuario", "invitado" };
        
        //así podemos leer la cadena de conexión que está especificada en Web.config
        public static string con_str = null;
        
        //el constructor en una clase estática solo se llama la primera vez que se usa la clase,
        //es decir, debería llamarse una sola vez
        static Modelo()
        {
            con_str = System.Configuration.ConfigurationManager.ConnectionStrings["CadenaConexionEjemploMembresia"].ConnectionString; 
            CargarRoles();
        }
        
        //Crea los roles especificados en el arreglo nombresRoles si es que no existen
        public static void CargarRoles()
        {
            foreach (var nombreRol in nombresRoles)
            {
                if (!Roles.RoleExists(nombreRol))
                {
                    Roles.CreateRole(nombreRol);
                }
            }
        }

        public static int CrearNuevoUsuario(Usuario nuevoUsu)
        {
            MembershipUser nuevoUsuMembresia = null;
            //primero que todo intentamos crear nuestro usuario de membresía

            //si el usuario ya existe
            if (Membership.GetUser(nuevoUsu.nombre_usuario) != null)
            {
                return -1;
            }

            //intentamos crear el usuario en la tabla de membresía
            try
            {
                //creamos el usuario de membresía
                nuevoUsuMembresia = Membership.CreateUser(nuevoUsu.nombre_usuario, nuevoUsu.contraseña);
                //y le asignamos su rol
                Roles.AddUserToRole(nuevoUsu.nombre_usuario, nuevoUsu.rol);
            }
            catch (Exception err)
            {
                //similar a hacer un Console.WriteLine, pero el texto aparece en la ventan Output
                System.Diagnostics.Debug.WriteLine(err.Message);
                return -1;
            }

            try
            {
                using (var con = new SqlConnection(con_str))
                using (var da = new SqlDataAdapter("SELECT * FROM Usuarios WHERE 0 = 1", con))
                {
                    var ds = new DataSet();
                    da.Fill(ds);

                    var nuevaFila = ds.Tables[0].NewRow();
                    nuevaFila["id_usuario_asp"] = nuevoUsuMembresia.ProviderUserKey;
                    nuevaFila["dni"] = nuevoUsu.dni;
                    nuevaFila["nombres"] = nuevoUsu.nombres;
                    nuevaFila["apellidos"] = nuevoUsu.apellidos;
                    nuevaFila["telefono"] = nuevoUsu.telefono;

                    ds.Tables[0].Rows.Add(nuevaFila);

                    new SqlCommandBuilder(da);

                    da.Update(ds);
                }
                return 0;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                
                //si falló la inserción en nuestra tabla borramos al usuario creado
                //en la tabla de membresía para mantener la consistencia
                Membership.DeleteUser(nuevoUsu.nombre_usuario);

                return -1;
            }
        }

        public static List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> usus = null;

            using (var con = new SqlConnection(con_str))
            // esta consulta es bastante ineficiente; muchos joins
            using (var da = new SqlDataAdapter(
                @"select u.dni, u.nombres, u.apellidos, u.telefono, u.id_usuario_asp,
                    au.UserName as nombre_usuario, r.RoleName as rol from Usuarios u
                    inner join aspnet_UsersInRoles uir
                    on u.id_usuario_asp=uir.UserId
                    inner join aspnet_Roles r
                    on uir.RoleId = r.RoleId
                    inner join aspnet_Users au
                    on au.UserId = u.id_usuario_asp", con))
            {
                var ds = new DataSet();
                da.Fill(ds);

                usus = ds.Tables[0].AsEnumerable().Select(
                    fila => new Usuario
                    {
                        dni = fila.Field<string>("dni"),
                        nombres = fila.Field<string>("nombres"),
                        apellidos = fila.Field<string>("apellidos"),
                        telefono = fila.Field<string>("dni"),
                        id_usuario_asp = fila.Field<Guid>("id_usuario_asp").ToString(),
                        nombre_usuario = fila.Field<string>("nombre_usuario"),
                        rol = fila.Field<string>("rol")
                    }).ToList();
            }
            return usus;
        }
    }
}