
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Eproject3.Models
{

using System;
    using System.Collections.Generic;
    
public partial class Users
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Users()
    {

        this.Contester = new HashSet<Contester>();

        this.FeedBack = new HashSet<FeedBack>();

        this.Tips = new HashSet<Tips>();

        this.Recipes = new HashSet<Recipes>();

    }


    public int id { get; set; }

    public string UPhone { get; set; }

    public string UPass { get; set; }

    public string UAdress { get; set; }

    public string Img { get; set; }

    public Nullable<int> Roll_id { get; set; }

    public Nullable<int> Pack_id { get; set; }

    public Nullable<System.DateTime> Exp_Date { get; set; }

    public string AccNum { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Contester> Contester { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<FeedBack> FeedBack { get; set; }

    public virtual Packs Packs { get; set; }

    public virtual Roles Roles { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Tips> Tips { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Recipes> Recipes { get; set; }

}

}
