using System;
using System.ComponentModel.DataAnnotations;

public class tblDuLieu{
    [Key]
    public int Id { get; set; }

    // public string DuongDi { get; set; } 
    public System.Collections.Generic.List<string> DuongDi { get; set; } 
    public System.Collections.Generic.List<string> ODaDi { get; set; } 
    public float TrongSo { get; set; }
    public int DaQuethet { get; set; } //1: da quet het, 0: chua quet het
    public int LaConLai {get; set;}
    
}