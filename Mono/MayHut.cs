using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class MayHut : CharacterBody2D
{
	[Export] public bool cho_phep_di_lai = false;
	[Export] public bool duoc_hanh_dong = false;
	[Export] public Godot.Collections.Dictionary<string, Vector2> vi_tri_o_huong = 
		new Godot.Collections.Dictionary<string, Vector2>
		{
			{ "0", Vector2.Inf },
			{ "1", Vector2.Inf },
			{ "2", Vector2.Inf },
			{ "3", Vector2.Inf },
			{ "4", Vector2.Inf },
			{ "5", Vector2.Inf },
			{ "6", Vector2.Inf },
			{ "7", Vector2.Inf }
		};
	[Export] public Godot.Collections.Dictionary<string, bool> co_the_di = new Godot.Collections.Dictionary <string, bool> {
		{ "0", false },
		{ "1", false },
		{ "2", false },
		{ "3", false },
		{ "4", false },
		{ "5", false },
		{ "6", false },
		{ "7", false }
	};

	// public bool da_in_ra = false;
	public Array<string> huong_di = new Array<string> { "0", "1", "2", "3", "4", "5", "6", "7" };
	
	public override void _Process(double _delta) {
		if (duoc_hanh_dong == true){
			foreach (string huong in huong_di) {
				//huong co vat the
				
				if (GetNode<RayCast2D>("qlcambien/" + huong).IsColliding() == true) {
					
					var collider = GetNode<RayCast2D>("qlcambien/" + huong).GetCollider();
					if (collider is Area2D area) {
						var o_cham = area.GetParent();

						//quet
						if (o_cham is OCham ocham) {
							// khong 0 vat can va chua duoc quet
							if (ocham.la_vat_can == false && ocham.da_duoc_quet == false) {
								co_the_di[huong] = true;
								GetNode<Label>("qlcambien/" + huong + "/Label").Text = o_cham.Name;
								vi_tri_o_huong[huong] = ocham.Position;
							}

							// khong 0 la vat can nhung da duoc quet
							if (ocham.la_vat_can == false && ocham.da_duoc_quet == true) {
								co_the_di[huong] = cho_phep_di_lai;
								GetNode<Label>("qlcambien/" + huong + "/Label").Text = "da quet";
								vi_tri_o_huong[huong] = ocham.Position;
							}

							// la vat can
							if (ocham.la_vat_can == true) {
								co_the_di[huong] = false;
								GetNode<Label>("qlcambien/" + huong + "/Label").Text = "vat can";
							}
						}
					}
				}
				//khong co gi
				else{
					co_the_di[huong] = false;
					GetNode<Label>("qlcambien/" + huong + "/Label").Text = "tuong";
				}
			}
		}
	}
}
