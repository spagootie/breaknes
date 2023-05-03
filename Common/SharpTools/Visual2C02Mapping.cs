// Mapping PPU signals for Visual2C02

using System.Data;
using System.Reflection.Metadata.Ecma335;

class Visual2C02
{
	static Dictionary<string, string> mappingFrom = new()
    {
		{ "/_ab8", "/PA8" },
		{ "/_ab9", "/PA9" },
		{ "/_ab10", "/PA10" },
		{ "/_ab11", "/PA11" },
		{ "/_ab12", "/PA12" },
		{ "/_ab13", "/PA13" },
		{ "/clk0_int", "/CLK" },
		{ "clk0_int", "CLK" },
		{ "pclk1", "/PCLK" },
		{ "pclk0", "PCLK" },
		{ "_res", "RES" },
		{ "_res2", "RC" },
		{ "_io_rw", "R/W" },
		{ "_io_ab0", "RS[0]" },
		{ "_io_ab1", "RS[1]" },
		{ "_io_ab2", "RS[2]" },
		{ "_io_ce", "/DBE" },
		{ "_io_rw_buf", "/RD" },
		{ "_io_dbe", "/WR" },
		{ "/w2006a", "/W6_1" },
		{ "/w2006b", "/W6_2" },
		{ "/w2005a", "/W5_1" },
		{ "/w2005b", "/W5_2" },
		{ "/r2007", "/R7" },
		{ "/w2007", "/W7" },
		{ "/w2004", "/W4" },
		{ "/w2003", "/W3" },
		{ "/r2002", "/R2" },
		{ "/w2001", "/W1" },
		{ "/w2000", "/W0" },
		{ "/r2004", "/R4" },
		{ "addr_inc_out", "I1/32" },
		{ "/spr_pat_out", "OBSEL" },
		{ "/bkg_pat_out", "BGSEL" },
		{ "spr_size_out", "O8/16" },
		{ "slave_mode", "/SLAVE" },
		{ "enable_nmi", "VBL" },
		{ "pal_mono", "B/W" },
		{ "bkg_clip_out", "/BGCLIP" },
		{ "spr_clip_out", "/OBCLIP" },
		{ "bkg_enable_out", "BGE" },
		{ "rendering_disabled", "BLACK" },
		{ "spr_enable_out", "OBE" },
		{ "/emph0_out", "/TR" },
		{ "/emph1_out", "/TG" },
		{ "/emph2_out", "/TB" },
		{ "++hpos_eq_65_and_rendering", "S/EV" },
		{ "++in_clip_area_and_clipping_spr", "CLIP_O" },
		{ "++in_clip_area_and_clipping_bg", "CLIP_B" },
		{ "++hpos_eq_339_and_rendering", "0/HPOS" },
		{ "++/hpos_eq_63_255_or_339_and_rendering", "/EVAL" },
		{ "++hpos_eq_255_and_rendering", "E/EV" },
		{ "++hpos_lt_64_and_rendering", "I/OAM2" },
		{ "++hpos_eq_256_to_319_and_rendering", "PAR/O" },
		{ "++/in_visible_frame_and_rendering", "/VIS" },
		{ "++/hpos_mod_8_eq_0_or_1_and_rendering", "#F/NT" },
		{ "++hpos_eq_0-255_or_320-335_and_hpos_mod_8_eq_6_or_7_and_rendering", "F/TB" },
		{ "++hpos_eq_0-255_or_320-335_and_hpos_mod_8_eq_4_or_5_and_rendering", "F/TA" },
		{ "+hpos_eq_0-255_or_320-335_and_hpos_mod_8_eq_2_or_3_and_rendering", "F/AT" },
		{ "++hpos_eq_0-255_or_320-335_and_rendering", "/FO" },
		{ "+hpos_eq_270_to_327", "BPORCH" },
		{ "+hpos_eq_279_to_303_and_rendering_enabled", "SC/CNT" },
		{ "+hpos_eq_279_to_303", "/HB" },
		{ "in_range_3", "BURST" },
		{ "in_range_2", "SYNC" },
		{ "++/in_draw_range", "/PICTURE" },
		{ "vbl_clear_flags", "RESCL" },
		{ "+/in_range_1", "VSYNC" },
		{ "+/vpos_eq_241_2", "/VSET" },
		{ "in_vblank", "VB" },
		{ "not_rendering", "BLNK" },
		{ "_int", "INT" },
		{ "+hpos0", "H0'" },
		{ "++hpos0", "H0''" },
		{ "+/hpos1", "/H1'" },
		{ "++hpos1", "H1''" },
		{ "+/hpos2", "/H2'" },
		{ "++hpos2", "H2''" },
		{ "++hpos3", "H3''" },
		{ "++hpos4", "H4''" },
		{ "++hpos5", "H5''" },
		{ "skip_dot", "EvenOddOut" },
		{ "+move_to_next_line", "HC" },
		{ "+clear_vpos_next", "VC" },
		{ "hpos_eq_340", "V_IN" },
		{ "spr_out_/pat0", "/ZCOL0" },
		{ "spr_out_/pat1", "/ZCOL1" },
		{ "spr_out_attr0", "ZCOL2" },
		{ "spr_out_attr1", "ZCOL3" },
		{ "spr_out_prio", "/ZPRIO" },
		{ "/spr_slot_0_opaque", "/SPR0HIT" },
		{ "exp_in0", "EXT_IN0" },
		{ "exp_in1", "EXT_IN1" },
		{ "exp_in2", "EXT_IN2" },
		{ "exp_in3", "EXT_IN3" },
		{ "/exp_out0", "/EXT_OUT0" },
		{ "/exp_out1", "/EXT_OUT1" },
		{ "/exp_out2", "/EXT_OUT2" },
		{ "/exp_out3", "/EXT_OUT3" },
		{ "/read_2007_output_palette", "#CB/DB" },
		{ "(++in_draw_range_or_read_2007_output_palette)_and_/pal_mono", "/BW" },
		{ "/(ab_in_palette_range_and_not_rendering_and_+write_2007_ended)_2", "#DB/CB" },
		{ "pal_ptr0", "PAL0" },
		{ "pal_ptr1", "PAL1" },
		{ "pal_ptr2", "PAL2" },
		{ "pal_ptr3", "PAL3" },
		{ "pal_ptr4", "PAL4" },
		{ "+/pal_d0_out", "#CC0" },
		{ "+/pal_d1_out", "#CC1" },
		{ "+/pal_d2_out", "#CC2" },
		{ "+/pal_d3_out", "#CC3" },
		{ "+++/pal_d4_out", "#LL0" },
		{ "+++/pal_d5_out", "#LL1" },
		{ "spr_addr_load_next_value", "OMSTEP" },
		{ "/spr_load_next_value_or_write_2003_reg", "OMOUT" },
		{ "clear_spr_ptr", "ORES" },
		{ "inc_spr_ptr", "OSTEP" },
		{ "sprite_in_range", "OVZ" },
		{ "/spr_eval_copy_sprite", "OMFG" },
		{ "spr_addr_7_carry_out", "OMV" },
		{ "spr_ptr4_carry_out", "TMV" },
		{ "++hpos0_2_and_pclk_1", "COPY_STEP" },
		{ "copy_sprite_to_sec_oam", "DO_COPY" },
		{ "do_copy_sprite_to_sec_oam", "COPY_OVF" },
		{ "/sprite_0_on_cur_scanline", "/SPR0_EV" },
		{ "delayed_write_2004_value", "OFETCH" },
		{ "end_of_oam_or_sec_oam_overflow", "SPR_OV" },
		{ "spr_ptr_overflow", "OAMCTR2" },
		{ "spr_ptr5", "OAM8" },
		{ "+sprite_in_range_reg", "PD/FIFO" },
		{ "oam_write_disable", "/WE" },
		{ "/spr_loadFlag", "/SH2" },
		{ "/spr_loadX", "/SH3" },
		{ "/spr_loadL", "/SH5" },
		{ "/spr_loadH", "/SH7" },
		{ "+++++/hpos_eq_339_and_rendering", "#0/H" },
		{ "++++/do_sprite_render_ops", "CLPO" },
		{ "+++do_bg_render_ops", "/CLPB" },
		{ "pixel_color0", "BGC0" },
		{ "pixel_color1", "BGC1" },
		{ "pixel_color2", "BGC2" },
		{ "pixel_color3", "BGC3" },
		{ "finex0", "FH0" },
		{ "finex1", "FH1" },
		{ "finex2", "FH2" },
		{ "vramaddr_t12", "FV0" },
		{ "vramaddr_t13", "FV1" },
		{ "vramaddr_t14", "FV2" },
		{ "vramaddr_t11", "NTV" },
		{ "vramaddr_t10", "NTH" },
		{ "vramaddr_t5", "TV0" },
		{ "vramaddr_t6", "TV1" },
		{ "vramaddr_t7", "TV2" },
		{ "vramaddr_t8", "TV3" },
		{ "vramaddr_t9", "TV4" },
		{ "vramaddr_t0", "TH0" },
		{ "vramaddr_t1", "TH1" },
		{ "vramaddr_t2", "TH2" },
		{ "vramaddr_t3", "TH3" },
		{ "vramaddr_t4", "TH4" },
		{ "vramaddr_v0_out", "THO0" },
		{ "vramaddr_v1_out", "THO1" },
		{ "vramaddr_v2_out", "THO2" },
		{ "vramaddr_v3_out", "THO3" },
		{ "vramaddr_v4_out", "THO4" },
		{ "vramaddr_v5_out", "TVO0" },
		{ "vramaddr_v6_out", "TVO1" },
		{ "vramaddr_v7_out", "TVO2" },
		{ "vramaddr_v8_out", "TVO3" },
		{ "vramaddr_v9_out", "TVO4" },
		{ "vramaddr_v13_out", "FVO1" },
		{ "vramaddr_/v12", "/FVO0" },
		{ "vramaddr_/v13", "/FVO1" },
		{ "vramaddr_/v14", "/FVO2" },
		{ "_rd", "RD" },
		{ "_wr", "WR" },
		{ "/_ale", "/ALE" },
		{ "reading_or_writing_2007", "TSTEP" },
		{ "write_2007_ended_2", "DB/PAR" },
		{ "read_2007_ended_2", "PD/RB" },
		{ "ab_in_palette_range_and_not_rendering_2", "TH/MUX" },
		{ "read_2007_output_vrambuf_2", "XRB" },
	};

    static Dictionary<string, string> mappingTo = new()
	{
        { "/PA8", "/_ab8" },
        { "/PA9", "/_ab9" },
        { "/PA10", "/_ab10" },
        { "/PA11", "/_ab11" },
        { "/PA12", "/_ab12" },
        { "/PA13", "/_ab13" },
        { "/CLK", "/clk0_int" },
        { "CLK", "clk0_int" },
        { "/PCLK", "pclk1" },
        { "PCLK", "pclk0" },
        { "RES", "_res" },
        { "RC", "_res2" },
        { "R/W", "_io_rw" },
        { "RS[0]", "_io_ab0" },
        { "RS[1]", "_io_ab1" },
        { "RS[2]", "_io_ab2" },
        { "/DBE", "_io_ce" },
        { "/RD", "_io_rw_buf" },
        { "/WR", "_io_dbe" },
        { "/W6_1", "/w2006a" },
        { "/W6_2", "/w2006b" },
        { "/W5_1", "/w2005a" },
        { "/W5_2", "/w2005b" },
        { "/R7", "/r2007" },
        { "/W7", "/w2007" },
        { "/W4", "/w2004" },
        { "/W3", "/w2003" },
        { "/R2", "/r2002" },
        { "/W1", "/w2001" },
        { "/W0", "/w2000" },
        { "/R4", "/r2004" },
        { "I1/32", "addr_inc_out" },
        { "OBSEL", "/spr_pat_out" },
        { "BGSEL", "/bkg_pat_out" },
        { "O8/16", "spr_size_out" },
        { "/SLAVE", "slave_mode" },
        { "VBL", "enable_nmi" },
        { "B/W", "pal_mono" },
        { "/BGCLIP", "bkg_clip_out" },
        { "/OBCLIP", "spr_clip_out" },
        { "BGE", "bkg_enable_out" },
        { "BLACK", "rendering_disabled" },
        { "OBE", "spr_enable_out" },
        { "/TR", "/emph0_out" },
        { "/TG", "/emph1_out" },
        { "/TB", "/emph2_out" },
        { "S/EV", "++hpos_eq_65_and_rendering" },
        { "CLIP_O", "++in_clip_area_and_clipping_spr" },
        { "CLIP_B", "++in_clip_area_and_clipping_bg" },
        { "0/HPOS", "++hpos_eq_339_and_rendering" },
        { "/EVAL", "++/hpos_eq_63_255_or_339_and_rendering" },
        { "E/EV", "++hpos_eq_255_and_rendering" },
        { "I/OAM2", "++hpos_lt_64_and_rendering" },
        { "PAR/O", "++hpos_eq_256_to_319_and_rendering" },
        { "/VIS", "++/in_visible_frame_and_rendering" },
        { "#F/NT", "++/hpos_mod_8_eq_0_or_1_and_rendering" },
        { "F/TB", "++hpos_eq_0-255_or_320-335_and_hpos_mod_8_eq_6_or_7_and_rendering" },
        { "F/TA", "++hpos_eq_0-255_or_320-335_and_hpos_mod_8_eq_4_or_5_and_rendering" },
        { "F/AT", "+hpos_eq_0-255_or_320-335_and_hpos_mod_8_eq_2_or_3_and_rendering" },
        { "/FO", "++hpos_eq_0-255_or_320-335_and_rendering" },
        { "BPORCH", "+hpos_eq_270_to_327" },
        { "SC/CNT", "+hpos_eq_279_to_303_and_rendering_enabled" },
        { "/HB", "+hpos_eq_279_to_303" },
        { "BURST", "in_range_3" },
        { "SYNC", "in_range_2" },
        { "/PICTURE", "++/in_draw_range" },
        { "RESCL", "vbl_clear_flags" },
        { "VSYNC", "+/in_range_1" },
        { "/VSET", "+/vpos_eq_241_2" },
        { "VB", "in_vblank" },
        { "BLNK", "not_rendering" },
        { "INT", "_int" },
        { "H0'", "+hpos0" },
        { "H0''", "++hpos0" },
        { "/H1'", "+/hpos1" },
        { "H1''", "++hpos1" },
        { "/H2'", "+/hpos2" },
        { "H2''", "++hpos2" },
        { "H3''", "++hpos3" },
        { "H4''", "++hpos4" },
        { "H5''", "++hpos5" },
        { "EvenOddOut", "skip_dot" },
        { "HC", "+move_to_next_line" },
        { "VC", "+clear_vpos_next" },
        { "V_IN", "hpos_eq_340" },
        { "/ZCOL0", "spr_out_/pat0" },
        { "/ZCOL1", "spr_out_/pat1" },
        { "ZCOL2", "spr_out_attr0" },
        { "ZCOL3", "spr_out_attr1" },
        { "/ZPRIO", "spr_out_prio" },
        { "/SPR0HIT", "/spr_slot_0_opaque" },
        { "EXT_IN0", "exp_in0" },
        { "EXT_IN1", "exp_in1" },
        { "EXT_IN2", "exp_in2" },
        { "EXT_IN3", "exp_in3" },
        { "/EXT_OUT0", "/exp_out0" },
        { "/EXT_OUT1", "/exp_out1" },
        { "/EXT_OUT2", "/exp_out2" },
        { "/EXT_OUT3", "/exp_out3" },
        { "#CB/DB", "/read_2007_output_palette" },
        { "/BW", "(++in_draw_range_or_read_2007_output_palette)_and_/pal_mono" },
        { "#DB/CB", "/(ab_in_palette_range_and_not_rendering_and_+write_2007_ended)_2" },
        { "PAL0", "pal_ptr0" },
        { "PAL1", "pal_ptr1" },
        { "PAL2", "pal_ptr2" },
        { "PAL3", "pal_ptr3" },
        { "PAL4", "pal_ptr4" },
        { "#CC0", "+/pal_d0_out" },
        { "#CC1", "+/pal_d1_out" },
        { "#CC2", "+/pal_d2_out" },
        { "#CC3", "+/pal_d3_out" },
        { "#LL0", "+++/pal_d4_out" },
        { "#LL1", "+++/pal_d5_out" },
        { "OMSTEP", "spr_addr_load_next_value" },
        { "OMOUT", "/spr_load_next_value_or_write_2003_reg" },
        { "ORES", "clear_spr_ptr" },
        { "OSTEP", "inc_spr_ptr" },
        { "OVZ", "sprite_in_range" },
        { "OMFG", "/spr_eval_copy_sprite" },
        { "OMV", "spr_addr_7_carry_out" },
        { "TMV", "spr_ptr4_carry_out" },
        { "COPY_STEP", "++hpos0_2_and_pclk_1" },
        { "DO_COPY", "copy_sprite_to_sec_oam" },
        { "COPY_OVF", "do_copy_sprite_to_sec_oam" },
        { "/SPR0_EV", "/sprite_0_on_cur_scanline" },
        { "OFETCH", "delayed_write_2004_value" },
        { "SPR_OV", "end_of_oam_or_sec_oam_overflow" },
        { "OAMCTR2", "spr_ptr_overflow" },
        { "OAM8", "spr_ptr5" },
        { "PD/FIFO", "+sprite_in_range_reg" },
        { "/WE", "oam_write_disable" },
        { "/SH2", "/spr_loadFlag" },
        { "/SH3", "/spr_loadX" },
        { "/SH5", "/spr_loadL" },
        { "/SH7", "/spr_loadH" },
        { "#0/H", "+++++/hpos_eq_339_and_rendering" },
        { "CLPO", "++++/do_sprite_render_ops" },
        { "/CLPB", "+++do_bg_render_ops" },
        { "BGC0", "pixel_color0" },
        { "BGC1", "pixel_color1" },
        { "BGC2", "pixel_color2" },
        { "BGC3", "pixel_color3" },
        { "FH0", "finex0" },
        { "FH1", "finex1" },
        { "FH2", "finex2" },
        { "FV0", "vramaddr_t12" },
        { "FV1", "vramaddr_t13" },
        { "FV2", "vramaddr_t14" },
        { "NTV", "vramaddr_t11" },
        { "NTH", "vramaddr_t10" },
        { "TV0", "vramaddr_t5" },
        { "TV1", "vramaddr_t6" },
        { "TV2", "vramaddr_t7" },
        { "TV3", "vramaddr_t8" },
        { "TV4", "vramaddr_t9" },
        { "TH0", "vramaddr_t0" },
        { "TH1", "vramaddr_t1" },
        { "TH2", "vramaddr_t2" },
        { "TH3", "vramaddr_t3" },
        { "TH4", "vramaddr_t4" },
        { "THO0", "vramaddr_v0_out" },
        { "THO1", "vramaddr_v1_out" },
        { "THO2", "vramaddr_v2_out" },
        { "THO3", "vramaddr_v3_out" },
        { "THO4", "vramaddr_v4_out" },
        { "TVO0", "vramaddr_v5_out" },
        { "TVO1", "vramaddr_v6_out" },
        { "TVO2", "vramaddr_v7_out" },
        { "TVO3", "vramaddr_v8_out" },
        { "TVO4", "vramaddr_v9_out" },
        { "FVO1", "vramaddr_v13_out" },
        { "/FVO0", "vramaddr_/v12" },
        { "/FVO1", "vramaddr_/v13" },
        { "/FVO2", "vramaddr_/v14" },
        { "RD", "_rd" },
        { "WR", "_wr" },
        { "/ALE", "/_ale" },
        { "TSTEP", "reading_or_writing_2007" },
        { "DB/PAR", "write_2007_ended_2" },
        { "PD/RB", "read_2007_ended_2" },
        { "TH/MUX", "ab_in_palette_range_and_not_rendering_2" },
        { "XRB", "read_2007_output_vrambuf_2" }
    };

	static public string FromVisual2C02 (string name)
	{
		if (mappingFrom.ContainsKey(name))
		{
			return mappingFrom[name];
		}	
        else return name;
	}

	static public string ToVisual2C02 (string name)
	{
		if (mappingTo.ContainsKey(name))
		{
			return mappingTo[name];
		}
        else return name;
	}
}
