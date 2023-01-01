// Triangle Channel

#pragma once

namespace APUSim
{
	class TriangleChan
	{
		friend APUSimUnitTest::UnitTest;

		APU* apu = nullptr;

		BaseLogic::TriState TCO = BaseLogic::TriState::X;
		BaseLogic::TriState n_FOUT = BaseLogic::TriState::X;
		BaseLogic::TriState LOAD = BaseLogic::TriState::X;
		BaseLogic::TriState STEP = BaseLogic::TriState::X;
		BaseLogic::TriState TSTEP = BaseLogic::TriState::X;

		RegisterBit lc_reg{};
		BaseLogic::FF Reload_FF{};
		BaseLogic::DLatch reload_latch1{};
		BaseLogic::DLatch reload_latch2{};
		BaseLogic::DLatch tco_latch{};

		RegisterBit lin_reg[7]{};
		DownCounterBit lin_cnt[7]{};
		RegisterBit freq_reg[11]{};
		DownCounterBit freq_cnt[11]{};
		BaseLogic::DLatch fout_latch{};
		CounterBit out_cnt[5]{};

		void sim_Control();
		void sim_LinearCounter();
		void sim_FreqCounter();
		void sim_Output();

	public:
		TriangleChan(APU* parent);
		~TriangleChan();

		void sim();
	};
}
