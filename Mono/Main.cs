using Godot;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public partial class Main : Control
{
	DataContext dataContext = new DataContext();

	[Export] public Godot.Collections.Array<string> ngan_cho = new Godot.Collections.Array<string>();
	public float quang_duong_di_duoc = 0;

	public System.Collections.Generic.List<string> cac_huong = new System.Collections.Generic.List<string> { "0", "1", "2", "3", "4", "5", "6", "7" };
	public CheckBox check_phai;
	public CheckBox check_trai;
	public CheckBox check_tren;
	public CheckBox check_duoi;
	public CheckBox check_trentrai;
	public CheckBox check_trenphai;
	public CheckBox check_duoitrai;
	public CheckBox check_duoiphai;

	public MayHut may_hut;

	public TextEdit text_ngan_cho;
	public Label ket_thuc;
	public Label quang_duong_di_duoc_label;
	public Label so_buoc_label;
	public Label so_lan_da_reload;


	public Label test_label;
	// public Godot.Timer timer;
	// public int s;

	// Called when the node enters the scene tree for the first time.
	private Godot.Collections.Array<int> mang_random = new Godot.Collections.Array<int> { 0, 1, 2, 3, 4 };
	private System.Collections.Generic.List<string> offspring1 = new System.Collections.Generic.List<string> { };
	private float offspring1Weight = 0;

	public override void _Ready()
	{
		so_lan_da_reload = GetNode<Label>("so_lan_da_reload");

		// int s = GetNode<GridContainer>("NinePatchRect/GridContainer").Columns;
		using (var context = new DataContext())
		{
			// context.Database.EnsureDeleted();
			context.Database.EnsureCreated();
		}

		check_phai = GetNodeOrNull<CheckBox>("GridContainer/0"); //huongphai
		check_trai = GetNodeOrNull<CheckBox>("GridContainer/1"); //huongtrai
		check_tren = GetNodeOrNull<CheckBox>("GridContainer/2"); //huongtren
		check_duoi = GetNodeOrNull<CheckBox>("GridContainer/3"); //huongduoi
		check_trentrai = GetNodeOrNull<CheckBox>("GridContainer/4"); //huongtrentrai
		check_trenphai = GetNodeOrNull<CheckBox>("GridContainer/5"); //huongtrenphai
		check_duoitrai = GetNodeOrNull<CheckBox>("GridContainer/6"); //huongduoitrai
		check_duoiphai = GetNodeOrNull<CheckBox>("GridContainer/7"); //huongduoiphai

		may_hut = GetNodeOrNull<MayHut>("NinePatchRect/GridContainer/may_hut");

		text_ngan_cho = GetNode<TextEdit>("text_ngan_cho");

		test_label = GetNode<Label>("test");

		ket_thuc = GetNodeOrNull<Label>("ket_thuc");
		quang_duong_di_duoc_label = GetNodeOrNull<Label>("quang_duong");
		so_buoc_label = GetNodeOrNull<Label>("so_buoc");
		// timer = GetNodeOrNull<Timer>("Timer");
		may_hut.duoc_hanh_dong = true;

		test_label.Text = dataContext.tblDuLieus.Count().ToString();


		if (dataContext.tblDuLieus.Count() % 2 == 0 && dataContext.tblDuLieus.Count() != 0 && dataContext.tblDuLieus.Count() >= 5)
		{
			var lay_cha_me = dataContext.tblDuLieus.Where(x => x.DaQuethet == 1)
			.OrderBy(d => d.TrongSo)
			.Take(5)
			.ToList();


			// for (int index = 1; index < lay_cha_me.Count - 1; index += 2)
			// {
			// 	var first = lay_cha_me[index];
			// 	var second = lay_cha_me[index + 1];
			// 	if (first.DuongDi == second.DuongDi)
			// 	{
			// 		dataContext.Remove(first);
			// 		dataContext.Remove(second);
			// 	}
			// }


			test_label.Text = "dang lai ghep";
			GD.Print("dang lai ghep");
			Random parent1_random = new Random();
			Random parent2_random = new Random();

			//lay cha me
			mang_random.Shuffle();
			var parent1 = lay_cha_me[mang_random[0]];
			var parent2 = lay_cha_me[mang_random[1]];



			Godot.Collections.Array<int> cac_diem_cat_parent1 = new Godot.Collections.Array<int> { };
			Godot.Collections.Array<int> cac_diem_cat_parent2 = new Godot.Collections.Array<int> { };
			int diem_cat_parent1 = 0;
			int diem_cat_parent2 = 0;


			var shuffledMangOTrong = GetNode<GridContainer>("NinePatchRect/GridContainer").mang_o_trong;
			shuffledMangOTrong.Shuffle();
			int diem_trung_nhau = shuffledMangOTrong[0];
			for (int i = 0; i < parent1.ODaDi.Count; i++)
			{
				if (parent1.ODaDi[i] == "o" + diem_trung_nhau.ToString())
				{
					GD.Print("o" + diem_trung_nhau.ToString());
					cac_diem_cat_parent1.Add(i);
				}
			}
			for (int i = 0; i < parent2.ODaDi.Count; i++)
			{
				if (parent2.ODaDi[i] == "o" + diem_trung_nhau.ToString())
				{
					cac_diem_cat_parent2.Add(i);
				}
			}
			GD.Print("cac diem giao nhau: " + string.Join("_", cac_diem_cat_parent1));
			GD.Print("cac diem giao nhau: " + string.Join("_", cac_diem_cat_parent2));

			if (cac_diem_cat_parent1.Count > 0 && cac_diem_cat_parent2.Count > 0)
			{
				cac_diem_cat_parent1.Shuffle();
				cac_diem_cat_parent2.Shuffle();
				diem_cat_parent1 = cac_diem_cat_parent1[0];
				diem_cat_parent2 = cac_diem_cat_parent2[0];
			}

			else
			{
				GD.PrintErr("No valid cutting points found.");
				CallDeferred(nameof(_on_reload_pressed));
			}

			Random xac_xuat_cha_me = new Random();
			if (xac_xuat_cha_me.Next(100) >= 50)
			{
				offspring1.AddRange(parent1.DuongDi.Take(diem_cat_parent1));
				offspring1.AddRange(parent2.DuongDi.Skip(diem_cat_parent2));
			}
			else
			{
				offspring1.AddRange(parent2.DuongDi.Take(diem_cat_parent2));
				offspring1.AddRange(parent1.DuongDi.Skip(diem_cat_parent1));
			}




			// Dot bien (mutation) hung 2
			Random random = new Random();
			int mutationRate = 5;
			for (int i = 0; i < offspring1.Count; i++)
			{
				if (random.Next(100) < mutationRate)
				{
					// Thực hiện đột biến bằng cách thay đổi giá trị tại vị trí i
					offspring1[i] = cac_huong[random.Next(cac_huong.Count)];
				}
			}

			// GD.Print("Da di het cac o");

			GD.Print("con xau: " + offspring1.Count());
			do_dai_offspring = offspring1.Count();
		}
		// GetNode<Godot.Timer>("Timer").Start();
		GetNode<Godot.Timer>("Timer_nghi").Start();

	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		check_phai.ButtonPressed = may_hut.co_the_di["0"];
		check_trai.ButtonPressed = may_hut.co_the_di["1"];
		check_tren.ButtonPressed = may_hut.co_the_di["2"];
		check_duoi.ButtonPressed = may_hut.co_the_di["3"];
		check_trentrai.ButtonPressed = may_hut.co_the_di["4"];
		check_trenphai.ButtonPressed = may_hut.co_the_di["5"];
		check_duoitrai.ButtonPressed = may_hut.co_the_di["6"];
		check_duoiphai.ButtonPressed = may_hut.co_the_di["7"];

		check_phai.Text = "phai: " + may_hut.vi_tri_o_huong["0"].ToString();
		check_trai.Text = "trai: " + may_hut.vi_tri_o_huong["1"].ToString();
		check_tren.Text = "tren: " + may_hut.vi_tri_o_huong["2"].ToString();
		check_duoi.Text = "duoi: " + may_hut.vi_tri_o_huong["3"].ToString();
		check_trentrai.Text = "tren trai: " + may_hut.vi_tri_o_huong["4"].ToString();
		check_trenphai.Text = "tren phai: " + may_hut.vi_tri_o_huong["5"].ToString();
		check_duoitrai.Text = "duoi trai: " + may_hut.vi_tri_o_huong["6"].ToString();
		check_duoiphai.Text = "duoi phai: " + may_hut.vi_tri_o_huong["7"].ToString();

		if (so_buoc > 80)
		{
			// timer.Stop();
			// ket_thuc.Text = "Da ket thuc";
			CallDeferred(nameof(_on_reload_pressed));

			may_hut.duoc_hanh_dong = true;


			// _on_run_pressed();
		}

	}

	public void _on_mo_pressed()
	{
		GD.Print("tat_may");
		GetNode<Godot.Timer>("Timer_nghi").Stop();
	}

	public void _on_kiem_tra_o_pressed()
	{
		ngan_cho.Clear();
		// GD.Print(GetNode<OCham>("NinePatchRect/GridContainer/o1").Name);
		for (int i = 1; i <= 36; i++)
		{
			// GD.Print("kiem tra o: " + i.ToString());
			if (GetNode<OCham>("NinePatchRect/GridContainer/o" + i.ToString()) is OCham ocham)
			{
				if (ocham.la_vat_can == false && ocham.da_duoc_quet == false)
				{
					// GD.Print("o cham: " + ocham.Name);
					ngan_cho.Add(ocham.Name);
				}
			}
		}
		// GD.Print(string.Join("_", ngan_cho));

		if (ngan_cho.Count == 0)
		{
			// timer.Stop();
			ket_thuc.Text = "Da ket thuc";
			ket_thuc_chu_trinh();
		}
		// text_ngan_cho.Text = string.Join("_", ngan_cho);
	}
	public void ket_thuc_chu_trinh()
	{
		//
		mang_o_da_di.RemoveAt(0);
		//csdl
		var data = new tblDuLieu();
		// data.DuongDi = string.Join("", mang_di_duoc);
		data.DuongDi = mang_di_duoc;
		data.TrongSo = quang_duong_di_duoc;
		data.ODaDi = mang_o_da_di;
		data.DaQuethet = 1;  // di het
		data.LaConLai = 0;

		dataContext.tblDuLieus.Add(data);
		dataContext.SaveChanges();

		// SetMeta("so_lan_lap", (int)GetMeta("so_lan_lap") + 1);
		// so_lan_da_reload.Text = (string)GetMeta("so_lan_lap");

		CallDeferred(nameof(_on_reload_pressed));
		// GetTree().ReloadCurrentScene();
		may_hut.duoc_hanh_dong = true;
		// _on_run_pressed();
	}
	public void _on_timer_nghi_timeout()
	{
		GetNode<Godot.Timer>("Timer").Start();
	}

	public void _on_run_pressed()
	{
		GetNode<Godot.Timer>("Timer").Start();
	}
	public System.Collections.Generic.List<string> mang_di_duoc = new System.Collections.Generic.List<string> { };

	public int so_luong_gen = 0;
	public bool da_ket_thuc = false;
	public bool da_bat_quet_random = false;
	public int so_buoc = 0;

	public int so_lan_di_khi_dang_lai = 0;
	public int do_dai_offspring = 0;

	public System.Collections.Generic.List<string> offspring1_real = new System.Collections.Generic.List<string> { };
	public System.Collections.Generic.List<string> mang_o_da_di = new System.Collections.Generic.List<string> { };
	public string o_da_di_truoc_mot_buoc = "";
	public void ChongDiLai(Godot.Collections.Array<string> mang)
	{
		if (may_hut.cho_phep_di_lai && mang.Count != 1 && mang[0] == o_da_di_truoc_mot_buoc)
		{
			mang[0] = mang[1];
		}
	}
	public void _on_timer_timeout()
	{
		if (dataContext.tblDuLieus.Count() % 2 == 0 && dataContext.tblDuLieus.Count() != 0 && dataContext.tblDuLieus.Count() >= 5) //
		{
			
			//sau khi test con lai xong
			if (so_lan_di_khi_dang_lai < do_dai_offspring)
			{


				// GD.Print("dang lai");
				may_hut.cho_phep_di_lai = true;
				so_lan_di_khi_dang_lai += 1;
				so_buoc_label.Text = so_lan_di_khi_dang_lai.ToString();
				// GetNode<GridContainer>("NinePatchRect/GridContainer")._Ready();
				if (may_hut.co_the_di[offspring1[0]] == true)
				{
					may_hut.Position = may_hut.vi_tri_o_huong[offspring1[0]];
					offspring1_real.Add(offspring1[0]);
					offspring1.RemoveAt(0);
					text_ngan_cho.Text = string.Join("", mang_o_da_di);
				}
				else
				{
					offspring1.RemoveAt(0);
				}
				// GD.Print("so_lan_di_khi_dang_lai: " + so_lan_di_khi_dang_lai);
				// GD.Print("so_lan_di_khi_dang_lai: " + do_dai_offspring);
				//neu chua di het
			}
			else
			{
				so_lan_di_khi_dang_lai += 1;
				so_buoc_label.Text = so_lan_di_khi_dang_lai.ToString();
				// GD.Print("so_lan_di_khi_dang_lai: " + so_lan_di_khi_dang_lai + "__" + do_dai_offspring + "that bai");
				ngan_cho.Clear();
				for (int i = 1; i <= 36; i++)
				{
					if (GetNode<OCham>("NinePatchRect/GridContainer/o" + i.ToString()) is OCham ocham)
					{
						if (ocham.la_vat_can == false && ocham.da_duoc_quet == false)
						{
							ngan_cho.Add(ocham.Name);
						}
					}
				}
				//neu con lai di het thanh cong
				if (ngan_cho.Count == 0)
				{
					// Tính trọng số cho offspring1
					foreach (var item in offspring1_real)
					{
						if (item == "0" || item == "1" || item == "2" || item == "3")
						{
							offspring1Weight += 1f;
						}
						if (item == "4" || item == "5" || item == "6" || item == "7")
						{
							offspring1Weight += 1.41f;
						}
					}
					GD.Print("xau con sau kiem tra: " + string.Join("_", offspring1_real));
					test_label.Text = "lai ghep thanh cong";
					GD.Print("lai ghep thanh cong");
					// Thêm offspring1 vào cơ sở dữ liệu
					mang_o_da_di.RemoveAt(0);
					var newEntry = new tblDuLieu
					{
						DuongDi = offspring1_real,
						TrongSo = offspring1Weight,
						ODaDi = mang_o_da_di,
						DaQuethet = 1,
						LaConLai = 1
					};
					dataContext.tblDuLieus.Add(newEntry);


					dataContext.SaveChanges();



					CallDeferred(nameof(_on_reload_pressed));
					may_hut.duoc_hanh_dong = true;
					// _on_run_pressed();
				}
				//neu con lai khong di het
				else
				{
					may_hut.cho_phep_di_lai = false;
					Godot.Collections.Array<string> mang_vi_tri_di_duoc = new Godot.Collections.Array<string> { };
					mang_vi_tri_di_duoc.Clear();
					// Godot.Collections.Array<string> mang_quet_duoc_xung_quanh = new Godot.Collections.Array<string> {};
					foreach (string huong in cac_huong)
					{
						if (may_hut.co_the_di[huong] == true)
						{
							mang_vi_tri_di_duoc.Add(huong.ToString());
						}
					}
					//con o
					if (mang_vi_tri_di_duoc.Count != 0)
					{
						
						///qua 80 buoc
						if (so_lan_di_khi_dang_lai > 80)
						{
							// Tính trọng số cho offspring1
							foreach (var item in offspring1_real)
							{
								if (item == "0" || item == "1" || item == "2" || item == "3")
								{
									offspring1Weight += 1f;
								}
								if (item == "4" || item == "5" || item == "6" || item == "7")
								{
									offspring1Weight += 1.41f;
								}
							}
							// GD.Print("xau con sau kiem tra: " + string.Join("_", offspring1_real));
							test_label.Text = "Lai ghep that bai";
							// GD.Print("lai ghep thanh cong");
							// Thêm offspring1 vào cơ sở dữ liệu
							mang_o_da_di.RemoveAt(0);
							var newEntry = new tblDuLieu
							{
								DuongDi = offspring1_real,
								TrongSo = offspring1Weight,
								ODaDi = mang_o_da_di,
								DaQuethet = 0,
								LaConLai = 1
							};
							dataContext.tblDuLieus.Add(newEntry);


							dataContext.SaveChanges();



							CallDeferred(nameof(_on_reload_pressed));
							may_hut.duoc_hanh_dong = true;
						}

						//// sau khi lai ma chua di het
						/// 
						// mang_vi_tri_di_duoc.Clear();
						may_hut.cho_phep_di_lai = false;

						// GD.Print(mang_vi_tri_di_duoc.Count.ToString());
						// GD.Print("vi_tri_di_duoc (if): " + string.Join("_", mang_vi_tri_di_duoc));

						mang_vi_tri_di_duoc.Shuffle();
						ChongDiLai(mang_vi_tri_di_duoc);
						// GD.Print("mang vi tri di duoc: " + string.Join("_",mang_vi_tri_di_duoc));
						// GD.Print("mang vi tri di duoc da chon: " + mang_vi_tri_di_duoc[0]);
						
						may_hut.Position = may_hut.vi_tri_o_huong[mang_vi_tri_di_duoc[0]];
						offspring1_real.Add(mang_vi_tri_di_duoc[0]);
					}
					//neu khong con o di duoc
					else
					{
						may_hut.cho_phep_di_lai = true;
					}
					
					//.//khong di tiep sau khi lai
					// GD.Print("so_lan_di_khi_dang_lai: " + so_lan_di_khi_dang_lai + "__" + do_dai_offspring + "that bai");
					// foreach (var item in offspring1_real)
					// {
					// 	if (item == "0" || item == "1" || item == "2" || item == "3")
					// 	{
					// 		offspring1Weight += 1f;
					// 	}
					// 	if (item == "4" || item == "5" || item == "6" || item == "7")
					// 	{
					// 		offspring1Weight += 1.41f;
					// 	}
					// }
					// GD.Print("xau con sau kiem tra: " + string.Join("_", offspring1_real));
					// test_label.Text = "lai ghep that bai";
					// GD.Print("lai ghep that bai");
					// mang_o_da_di.RemoveAt(0);
					// var newEntry = new tblDuLieu
					// {
					// 	DuongDi = offspring1_real,
					// 	TrongSo = offspring1Weight,
					// 	ODaDi = mang_o_da_di,
					// 	DaQuethet = 0,
					// 	LaConLai = 1
					// };
					// dataContext.tblDuLieus.Add(newEntry);


					// dataContext.SaveChanges();


					// // new_test.Start();

					// CallDeferred(nameof(_on_reload_pressed));
					// may_hut.duoc_hanh_dong = true;
					// // _on_run_pressed();
				}
			}
		}
		//duong di binh thuong
		else
		{
			may_hut.cho_phep_di_lai = false;
			Godot.Collections.Array<string> mang_vi_tri_di_duoc = new Godot.Collections.Array<string> { };
			mang_vi_tri_di_duoc.Clear();
			// Godot.Collections.Array<string> mang_quet_duoc_xung_quanh = new Godot.Collections.Array<string> {};
			foreach (string huong in cac_huong)
			{
				if (may_hut.co_the_di[huong] == true)
				{
					mang_vi_tri_di_duoc.Add(huong.ToString());
				}
			}
			//con o di duoc
			if (mang_vi_tri_di_duoc.Count != 0)
			{
				// mang_vi_tri_di_duoc.Clear();
				may_hut.cho_phep_di_lai = false;

				// GD.Print(mang_vi_tri_di_duoc.Count.ToString());
				// GD.Print("vi_tri_di_duoc (if): " + string.Join("_", mang_vi_tri_di_duoc));

				mang_vi_tri_di_duoc.Shuffle();
				ChongDiLai(mang_vi_tri_di_duoc);
				// GD.Print("mang vi tri di duoc: " + string.Join("_",mang_vi_tri_di_duoc));
				// GD.Print("mang vi tri di duoc da chon: " + mang_vi_tri_di_duoc[0]);
				may_hut.Position = may_hut.vi_tri_o_huong[mang_vi_tri_di_duoc[0]];
				mang_di_duoc.Add(mang_vi_tri_di_duoc[0]);





				// tinh do dai
				if (mang_vi_tri_di_duoc[0] == "0" || mang_vi_tri_di_duoc[0] == "1" || mang_vi_tri_di_duoc[0] == "2" || mang_vi_tri_di_duoc[0] == "3")
				{
					quang_duong_di_duoc += 1;
					quang_duong_di_duoc_label.Text = quang_duong_di_duoc.ToString();
				}
				if (mang_vi_tri_di_duoc[0] == "4" || mang_vi_tri_di_duoc[0] == "5" || mang_vi_tri_di_duoc[0] == "6" || mang_vi_tri_di_duoc[0] == "7")
				{
					quang_duong_di_duoc += 1.41f;
					quang_duong_di_duoc_label.Text = quang_duong_di_duoc.ToString();
				}

				// GD.Print("cac o da duoc quet: " + mang_vi_tri_di_duoc.ToString());
			}
			//xung quanh khong co o di duoc
			else
			{
				may_hut.cho_phep_di_lai = true;
			}
			so_buoc += 1;
			so_buoc_label.Text = so_buoc.ToString();
			text_ngan_cho.Text = string.Join("", mang_o_da_di);
			_on_kiem_tra_o_pressed();
		}
	}

	public void _on_reload_pressed()
	{
		GetTree().ReloadCurrentScene();
	}
}
