using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CS_HOSPITALARIO_Front_end.Reports;

namespace CS_HOSPITALARIO_Front_end.Forms
{
    public partial class ReportCrystalTest : System.Web.UI.Page
    {
        rptTest re = new rptTest();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Cargar();
            }
        }
        protected void Cargar()
        {
            //using (rptCRecetaConsulta reportDocument = new rptCRecetaConsulta())
            //{
            //    reportDocument.Load(Server.MapPath("~/Reports/rptCRecetaConsulta.rpt"));
            //    CRViewer.ReportSource = reportDocument;
            //    CRViewer.RefreshReport();
            //    reportDocument.Close();
            //}

            //int consulta;
            //String valor = Request.QueryString["consulta"];
            //consulta = int.Parse(valor);
            //re.SetParameterValue("@CONSULTA", consulta);
            CRViewer.ReportSource = re;
            CRViewer.RefreshReport();
        }
    }
}