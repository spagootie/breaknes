// Main module and auxiliary logic.

#include "pch.h"

using namespace BaseLogic;

namespace PPUSim
{
	PPU::PPU(Revision _rev)
	{
		rev = _rev;

		regs = new ControlRegs(this);
		h = new HVCounter(this, 9);
		v = new HVCounter(this, 9);
		hv_dec = new HVDecoder(this);
		hv_fsm = new FSM(this);
		cram = new CRAM(this);
		vid_out = new VideoOut(this);
		mux = new Mux(this);
		eval = new OAMEval(this);
		oam = new OAM(this);
		fifo = new FIFO(this);
		data_reader = new DataReader(this);
		vram_ctrl = new VRAM_Control(this);
	}

	PPU::~PPU()
	{
		delete regs;
		delete h;
		delete v;
		delete hv_dec;
		delete hv_fsm;
		delete cram;
		delete vid_out;
		delete mux;
		delete eval;
		delete oam;
		delete fifo;
		delete data_reader;
		delete vram_ctrl;
	}

	void PPU::sim(TriState inputs[], TriState outputs[], uint8_t* ext, uint8_t* data_bus, uint8_t* ad_bus, uint8_t* addrHi_bus, VideoOutSignal& vout)
	{
		// Input terminals and binding

		wire.RnW = inputs[(size_t)InputPad::RnW];
		wire.RS[0] = inputs[(size_t)InputPad::RS0];
		wire.RS[1] = inputs[(size_t)InputPad::RS1];
		wire.RS[2] = inputs[(size_t)InputPad::RS2];
		wire.n_DBE = inputs[(size_t)InputPad::n_DBE];

		regs->sim_RWDecoder();

		wire.RES = NOT(inputs[(size_t)InputPad::n_RES]);

		wire.CLK = inputs[(size_t)InputPad::CLK];
		wire.n_CLK = NOT(wire.CLK);

		Reset_FF.set(NOR(wire.RES, NOR(Reset_FF.get(), hv_fsm->get_RESCL())));
		wire.RC = NOT(Reset_FF.get());

		sim_BusInput(ext, data_bus, ad_bus);

		sim_PCLK();

		// Regs

		regs->sim();

		// H/V Control logic

		TriState* HPLA;
		hv_dec->sim_HDecoder(hv_fsm->get_VB(), hv_fsm->get_BLNK(wire.BLACK), &HPLA);
		TriState* VPLA;
		hv_dec->sim_VDecoder(&VPLA);

		hv_fsm->sim(HPLA, VPLA);

		h->sim(TriState::One, wire.HC);
		TriState V_IN = HPLA[23];
		v->sim(V_IN, wire.VC);

		// The other parts

		fifo->sim_SpriteH();

		regs->sim_CLP();

		vram_ctrl->sim();
		
#if PPUSIM_OBJECTS
		eval->sim();

		oam->sim();

		fifo->sim();
#endif // PPUSIM_OBJECTS

		data_reader->sim();

		vram_ctrl->sim_TH_MUX();

		vram_ctrl->sim_ReadBuffer();

		mux->sim();

		cram->sim();

		vid_out->sim(vout);

		// Output terminals

		outputs[(size_t)OutputPad::n_INT] = fsm.INT ? TriState::Zero : TriState::Z;
		outputs[(size_t)OutputPad::ALE] = NOT(wire.n_ALE);
		outputs[(size_t)OutputPad::n_RD] = NOT(wire.RD);
		outputs[(size_t)OutputPad::n_WR] = NOT(wire.WR);

		sim_BusOutput(ext, data_bus, ad_bus, addrHi_bus);
	}

	void PPU::sim_PCLK()
	{
		TriState CLK = wire.CLK;
		TriState n_CLK = wire.n_CLK;

		// TBD: Add support for other PPU revisions when a critical mass of code is ready.

		if (rev == Revision::RP2C02G)
		{
			pclk_1.set(NOR(wire.RES, pclk_4.nget()), n_CLK);
			pclk_2.set(pclk_1.nget(), CLK);
			pclk_3.set(pclk_2.nget(), n_CLK);
			pclk_4.set(pclk_3.nget(), CLK);

			TriState pclk = pclk_4.nget();

			// The software PCLK counter is triggered by the falling edge.
			// This is purely a software design for convenience, and has nothing to do with PPU hardware circuitry.

			if (wire.PCLK /*prev*/ == TriState::One && pclk /*new*/ == TriState::Zero)
			{
				pclk_counter++;
			}

			wire.PCLK = pclk;
			wire.n_PCLK = NOT(wire.PCLK);
		}
	}

	void PPU::sim_BusInput(uint8_t* ext, uint8_t* data_bus, uint8_t* ad_bus)
	{
		TriState nSLAVE = regs->get_nSLAVE();

		for (size_t n = 0; n < 4; n++)
		{
			TriState extIn = (((*ext) >> n) & 1) ? TriState::One : TriState::Zero;
			wire.EXT_In[n] = NOR(NOT(extIn), nSLAVE);
		}

		if (wire.n_WR == TriState::Zero)
		{
			DB = *data_bus;
			DB_Dirty = true;
		}

		PD = *ad_bus;
		PD_Dirty = true;
	}

	void PPU::sim_BusOutput(uint8_t* ext, uint8_t* data_bus, uint8_t* ad_bus, uint8_t* addrHi_bus)
	{
		TriState n_PCLK = wire.n_PCLK;

		for (size_t n = 0; n < 4; n++)
		{
			extout_latch[n].set(wire.n_EXT_Out[n], n_PCLK);
		}

		if (wire.n_SLAVE == TriState::One)
		{
			*ext = 0;
			for (size_t n = 0; n < 4; n++)
			{
				TriState extOut = NOR(NOT(wire.n_SLAVE), extout_latch[n].get());
				(*ext) |= ((extOut == TriState::One) ? 1 : 0) << n;
			}
		}

		if (wire.n_RD == TriState::Zero)
		{
			*data_bus = DB;
			DB_Dirty = false;
		}

		if (wire.RD == TriState::Zero)
		{
			uint8_t PABot = 0;
			for (size_t n = 0; n < 8; n++)
			{
				PABot |= (wire.n_PA_Bot[n] == TriState::Zero ? 1 : 0) << n;
			}
			*ad_bus = PABot;
		}

		uint8_t PATop = 0;
		for (size_t n = 0; n < 6; n++)
		{
			PATop |= (wire.n_PA_Top[n] == TriState::Zero ? 1 : 0) << n;
		}
		*addrHi_bus = PATop;
	}

	size_t PPU::GetPCLKCounter()
	{
		return pclk_counter;
	}

	void PPU::ResetPCLKCounter()
	{
		pclk_counter = 0;
	}

	const char* PPU::RevisionToStr(Revision rev)
	{
		switch (rev)
		{
			case Revision::RP2C02G: return "RP2C02G";
			case Revision::RP2C02H: return "RP2C02H";
			case Revision::RP2C03: return "RP2C03";
			case Revision::RP2C03B: return "RP2C03B";
			case Revision::RP2C04_0001: return "RP2C04-0001";
			case Revision::RP2C04_0002: return "RP2C04-0002";
			case Revision::RP2C04_0003: return "RP2C04-0003";
			case Revision::RP2C04_0004: return "RP2C04-0004";
			case Revision::RP2C05: return "RP2C05";
			case Revision::RP2C07_0: return "RP2C07-0";
			default:
				break;
		}
		return "Unknown";
	}

	// A proven way to resolve bus conflicts.
	// We carry 2 variables - the bus value (8 bits) and the dirty flag.
	// If the bus is "dirty", then the operation "Ground wins" is performed (db &= val). Otherwise we set the bus value and the dirty flag.

	TriState PPU::GetDBBit(size_t n)
	{
		TriState DBBit = (DB & (1 << n)) != 0 ? TriState::One : TriState::Zero;
		return DB_Dirty ? DBBit : TriState::Z;
	}

	void PPU::SetDBBit(size_t n, TriState bit_val)
	{
		if (bit_val != TriState::Z)
		{
			uint8_t out = DB & ~(1 << n);
			out |= (bit_val == BaseLogic::One ? 1 : 0) << n;

			if (DB_Dirty)
			{
				DB = DB & out;
			}
			else
			{
				DB = out;
				DB_Dirty = true;
			}
		}
	}

	TriState PPU::GetPDBit(size_t n)
	{
		TriState PDBit = (PD & (1 << n)) != 0 ? TriState::One : TriState::Zero;
		return PD_Dirty ? PDBit : TriState::Z;
	}

	void PPU::SetPDBit(size_t n, TriState bit_val)
	{
		if (bit_val != TriState::Z)
		{
			uint8_t out = PD & ~(1 << n);
			out |= (bit_val == BaseLogic::One ? 1 : 0) << n;

			if (PD_Dirty)
			{
				PD = PD & out;
			}
			else
			{
				PD = out;
				PD_Dirty = true;
			}
		}
	}

	size_t PPU::GetHCounter()
	{
		return h->get();
	}

	size_t PPU::GetVCounter()
	{
		return v->get();
	}

	uint8_t PPU::Dbg_OAMReadByte(size_t addr)
	{
		return oam->Dbg_OAMReadByte(addr);
	}

	uint8_t PPU::Dbg_TempOAMReadByte(size_t addr)
	{
		return oam->Dbg_TempOAMReadByte(addr);
	}

	uint8_t PPU::Dbg_CRAMReadByte(size_t addr)
	{
		return cram->Dbg_CRAMReadByte(addr);
	}

	uint8_t PPU::Dbg_GetCRAMAddress()
	{
		uint8_t addr = 0;
		for (size_t n = 0; n < 5; n++)
		{
			addr |= (wire.PAL[n] == TriState::One ? 1 : 0) << n;
		}
		return addr;
	}

	uint16_t PPU::Dbg_GetPPUAddress()
	{
		uint8_t PABot = 0;
		for (size_t n = 0; n < 8; n++)
		{
			PABot |= (wire.n_PA_Bot[n] == TriState::Zero ? 1 : 0) << n;
		}

		uint8_t PATop = 0;
		for (size_t n = 0; n < 6; n++)
		{
			PATop |= (wire.n_PA_Top[n] == TriState::Zero ? 1 : 0) << n;
		}

		return ((uint16_t)PATop << 8) | PABot;
	}

	void PPU::Dbg_RandomizePicture(bool enable)
	{
		vid_out->Dbg_RandomizePicture(enable);
	}

	void PPU::Dbg_FixedPicture(bool enable)
	{
		vid_out->Dbg_FixedPicture(enable);
	}
}
