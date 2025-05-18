using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiniTienda.Presentation
{
    public partial class WFSales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar si el usuario está autenticado
                if (!User.Identity.IsAuthenticated)
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
        }
    }
} 