
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------


namespace GameServer
{

using System;
    using System.Collections.Generic;
    
public partial class TGuild
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TGuild()
    {

        this.Applies = new HashSet<TGuildApply>();

        this.Members = new HashSet<TGuildMember>();

    }


    public int Id { get; set; }

    public string Name { get; set; }

    public int LeaderId { get; set; }

    public string LeaderName { get; set; }

    public string Notice { get; set; }

    public System.DateTime CreateTime { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<TGuildApply> Applies { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<TGuildMember> Members { get; set; }

}

}
