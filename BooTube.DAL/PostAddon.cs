//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BooTube.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class PostAddon
    {
        public int AddonID { get; set; }
        public int AddonType { get; set; }
        public Nullable<int> UserID { get; set; }
        public int PostID { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual AddonType AddonType1 { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
