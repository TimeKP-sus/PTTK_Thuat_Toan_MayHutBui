using Godot;
using System;
using System.Linq;

public partial class Test : Node2D
{
	// Called when the node enters the scene tree for the first time.
	DataContext context = new DataContext();
	
	// Define the required variables
	private System.Collections.Generic.List<string> offspring1 = new System.Collections.Generic.List<string>();
	private System.Collections.Generic.List<string> parent1 = ["3","3","7","2","6","7","7","0","5","7","4","5","2","4","5","1","3","2","0","7","4","7","0","6","7","4","6","2","4","0","4","0","1","1","1","3","7","0"];
	private System.Collections.Generic.List<string> parent2 = ["3","3","7","2","6","7","7","0","5","7","4","5","2","4","5","1","3","2","0","7","4","7","0","6","7","4","6","2","4","0","4","0","1","1","1","3","7","0"];
	private int diem_cat_parent1 = 3;
	private int diem_cat_parent2 = 3;
	// 3_3_7_2_6_7_7_0_5_7_4_5_2_4_5_1_3_2_0_7_4_7_0_6_7_4_6_2_4_0_4_0_1_1_1_3_7_0

	public override void _Ready()
	{
		offspring1.AddRange(parent1.Take(diem_cat_parent1+1));
		offspring1.AddRange(parent2.Skip(diem_cat_parent2-1));
		if (offspring1.SequenceEqual(parent1)){
			GD.Print("dung");
		}
		else{
			GD.Print("sai");
		}
		GD.Print(string.Join("_",offspring1));
	}

	// Example Parent class definition
}
