namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StrOrg")]
    public partial class StrOrg
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StrOrg()
        {
            Pracownicy = new HashSet<Pracownicy>();
            Przelozeni_x = new HashSet<Przelozeni_x>();
        }

        [Key]
        public int Id_Str_Org { get; set; }

        public int? Id_Parent { get; set; }

        [StringLength(255)]
        public string Symb_Jedn { get; set; }

        [StringLength(255)]
        public string Nazwa_Jedn { get; set; }

        [StringLength(255)]
        public string Nazwa_JednEN { get; set; }

        public int? ID_Upr_Przel { get; set; }

        public int Typ { get; set; }

        [StringLength(500)]
        public string Path { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pracownicy> Pracownicy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Przelozeni_x> Przelozeni_x { get; set; }
    }
}
