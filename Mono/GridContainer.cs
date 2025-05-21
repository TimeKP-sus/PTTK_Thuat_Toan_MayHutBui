using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class GridContainer : Godot.GridContainer
{
	public PackedScene o_cham = GD.Load<PackedScene>("res://o_cham.tscn");
	public MayHut may_hut;

	public Godot.Collections.Array<int> mang_o_trong = new Godot.Collections.Array<int> {};
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		may_hut = GetNode<MayHut>("may_hut");
		may_hut.Position = new Vector2(0,0);
		System.Collections.Generic.List<int> vat_can = new System.Collections.Generic.List<int> { 2,8,10,15,32,35,19,22,27,30 };
		

		// int sl_o = GetNode<GridContainer>(".").Columns;


		// GD.Print(GetNode<GridContainer>(".").Columns.ToString());
		for (int i = 1; i <= 36; i++)
		{
			var oChamInstance = o_cham.Instantiate<OCham>();
			oChamInstance.GetNode<Label>("Control/Label").Text = i.ToString();
			oChamInstance.Name = "o" + i.ToString();
			mang_o_trong.Add(i);
			AddChild(oChamInstance);
			// GD.Print($"OCham: {oChamInstance.Name} da duoc them");
		}
		foreach (int i in vat_can)
		{
			mang_o_trong.Remove(i);
			if (GetNode<OCham>("o" + i.ToString()) is OCham ocham)
			{
				ocham.la_vat_can = true;
				ocham.GetNode<ColorRect>("Control/ColorRect").Color = new Color(0, 0, 0);
			}
		}
					
			// Random random = new Random();
			// int x = random.Next(0, 10);

			// // if (x < 3)
			// // {
			// // 	oChamInstance.la_vat_can = true;
			// // 	oChamInstance.GetNode<ColorRect>("Control/ColorRect").Color = new Color(0, 0, 0);
			// // }
		
	}

}
