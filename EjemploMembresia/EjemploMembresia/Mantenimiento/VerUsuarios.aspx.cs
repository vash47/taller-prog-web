﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EjemploMembresia.Mantenimiento
{
    public partial class VerUsuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            grdUsuarios.DataSource = Modelo.ObtenerUsuarios();
            grdUsuarios.DataBind();
        }
    }
}