using Godot;
using System;

public partial class OCham : Panel
{
	[Export] public bool la_vat_can = false;
	[Export] public bool da_duoc_quet = false;
	[Export] public bool o_song = false;
	public bool dang_bi_cham = false;


	public ColorRect colorRect;

	public override void _Ready() {
		colorRect = GetNodeOrNull<ColorRect>("Control/ColorRect");

	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.ButtonIndex == MouseButton.Right){
			if (eventMouseButton.IsPressed()) {
				
				GD.Print("da bam vao o");
				if (la_vat_can == false && dang_bi_cham == true){
					GD.Print("da la vat can");
					colorRect.Color = new Color(0, 0, 0);
					la_vat_can = true;
					
				}
			}
		}
	}

	// khi may hut di qua
	public void _on_area_2d_body_entered(Node2D body){
		if (la_vat_can == false){
			colorRect.Color = new Color(0, 0.5f, 0);
			GetNode<Main>("../../..").mang_o_da_di.Add(Name);
			da_duoc_quet = true;
		}
	}




	public void _on_color_rect_mouse_entered(){
		dang_bi_cham = true;
	}

	public void _on_color_rect_mouse_exited(){
		dang_bi_cham = false;
	}


}
