#include "pch.h"

using namespace BaseLogic;

namespace APUSim
{
	APU::APU(M6502Core::M6502* _core, Revision _rev)
	{
		// For ease of integration, the core instance is created by the consumer
		core = _core;
		rev = _rev;

		core_int = new CoreBinding(this);
		clkgen = new CLKGen(this);
		lc[0] = new LengthCounter(this);
		lc[1] = new LengthCounter(this);
		lc[2] = new LengthCounter(this);
		lc[3] = new LengthCounter(this);
		dpcm = new DpcmChan(this);
		noise = new NoiseChan(this);
		square[0] = new SquareChan(this, SquareChanCarryIn::Vdd);
		square[1] = new SquareChan(this, SquareChanCarryIn::Inc);
		tri = new TriangleChan(this);
		regs = new RegsDecoder(this);
		dma = new DMA(this);
		dac = new DAC(this);
	}

	APU::~APU()
	{
		delete core_int;
		delete clkgen;
		delete lc[0];
		delete lc[1];
		delete lc[2];
		delete lc[3];
		delete dpcm;
		delete noise;
		delete square[0];
		delete square[1];
		delete tri;
		delete regs;
		delete dma;
		delete dac;
	}

	void APU::sim(TriState inputs[], TriState outputs[], uint8_t* data, uint16_t* addr, float* AUX_A, float* AUX_B)
	{
		sim_InputPads(inputs, data);

		// TBD: For now it is preliminary, but it will be propagated as the individual modules are debugged and simulated

		// Core & stuff

		clkgen->sim();
		core_int->sim();
		regs->sim();
		dma->sim();

		// Sound channels

		for (size_t n = 0; n < 4; n++)
		{
			lc[n]->sim();
		}
		for (size_t n = 0; n < 2; n++)
		{
			square[n]->sim();
		}
		noise->sim();
		tri->sim();
		dpcm->sim();

		sim_OutputPads(outputs, data, addr);
		dac->sim(AUX_A, AUX_B);
	}

	void APU::sim_InputPads(TriState inputs[], uint8_t* data)
	{

	}

	void APU::sim_OutputPads(TriState outputs[], uint8_t* data, uint16_t* addr)
	{

	}

	TriState APU::GetDBBit(size_t n)
	{
		TriState DBBit = (DB & (1 << n)) != 0 ? TriState::One : TriState::Zero;
		return DBBit;
	}

	void APU::SetDBBit(size_t n, TriState bit_val)
	{
		if (bit_val != TriState::Z)
		{
			uint8_t out = DB & ~(1 << n);
			out |= (bit_val == BaseLogic::One ? 1 : 0) << n;
			DB = out;
		}
	}
}
