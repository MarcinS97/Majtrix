namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.WebPages.Html;

    [Table("obdFiltrPoleDict")]
    public partial class obdFiltrPoleDict
    {
        ObiegDokumentow ob = new ObiegDokumentow();
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nazwa { get; set; }

        [Required]
        [StringLength(100)]
        public string ZmiennaSQL { get; set; }

        [Required]
        public string WarunekSQL { get; set; }

        public string ddlSelect { get; set; }

        public int Typ { get; set; }

        public List<SelectListItem> GetDdlItems()
        {
            if(ddlSelect != null && ddlSelect != "")
            {
                try
                {
                    return ob.Database.SqlQuery<SelectListItem>(ddlSelect).ToList();
                }catch(Exception ex){}
            }

            return new List<SelectListItem>();
        }
    }
}
