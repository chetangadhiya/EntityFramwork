//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityFramwork
{
    using System;
    using System.Collections.Generic;
    
    public partial class Student
    {
        public int StudentId { get; set; }
        public string S_Name { get; set; }
        public string S_Email { get; set; }
        public string S_Course { get; set; }
        public int CourseId { get; set; }
        public int TeacherId { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
