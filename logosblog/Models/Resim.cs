//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace logosblog.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Resim
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Resim()
        {
            this.Kullanicis = new HashSet<Kullanici>();
            this.Makales = new HashSet<Makale>();
        }
    
        public int ResimId { get; set; }
        public string KucukBoyut { get; set; }
        public string OrtaBoyut { get; set; }
        public string BuyukBoyut { get; set; }
        public string Video { get; set; }
        public Nullable<int> MakaleID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kullanici> Kullanicis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Makale> Makales { get; set; }
        public virtual Makale Makale { get; set; }
    }
}