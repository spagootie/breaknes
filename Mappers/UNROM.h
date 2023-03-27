// Simple Mapper Simulator (2) - UNROM.

#pragma once

namespace Mappers
{
	class UNROM : public AbstractCartridge
	{
		bool valid = false;

		uint8_t* PRG = nullptr;
		size_t PRGSize = 0;

		uint8_t* CHR = nullptr;
		size_t CHRSize = 0;

		// Connect to PPU A10 for vertical mirroring or PPU A11 for horizontal mirroring.
		bool V_Mirroring = false;
		bool IgnoreMirroring = false;

		static uint8_t Dbg_ReadCHRByte(void* opaque, size_t addr);
		static void Dbg_WriteCHRByte(void* opaque, size_t addr, uint8_t data);

		void AddCartMemDescriptors();

		BaseBoard::LS32 quad_or{};
		BaseBoard::LS161 counter{};

	public:
		UNROM(ConnectorType p1, uint8_t* nesImage, size_t nesImageSize);
		virtual ~UNROM();

		bool Valid() override;

		void sim(
			BaseLogic::TriState cart_in[(size_t)CartInput::Max],
			BaseLogic::TriState cart_out[(size_t)CartOutput::Max],
			uint16_t cpu_addr,
			uint8_t* cpu_data, bool& cpu_data_dirty,
			uint16_t ppu_addr,
			uint8_t* ppu_data, bool& ppu_data_dirty,
			// Famicom only
			CartAudioOutSignal* snd_out,
			// NES only
			uint16_t* exp, bool& exp_dirty);
	};
}
