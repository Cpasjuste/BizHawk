using System;
using BizHawk.Emulation.Common;
using System.Collections.Generic;

namespace BizHawk.Emulation.Cores.Components.M6502
{
	public partial class MOS6502X : IDisassemblable
	{
		private static ushort peeker_word(ushort address, Func<ushort, byte> peeker)
		{
			byte l = peeker(address);
			byte h = peeker(++address);
			return (ushort)((h << 8) | l);
		}

		public string Disassemble(ushort pc, out int bytesToAdvance)
		{
			return Disassemble(pc, out bytesToAdvance, PeekMemory);
		}

		/// <summary>
		/// disassemble not from our own memory map, but from the supplied memory domain
		/// </summary>
		public static string Disassemble(ushort pc, out int bytesToAdvance, Func<ushort, byte> peeker)
		{
			byte op = peeker(pc);
			switch (op)
			{
				case 0x00: bytesToAdvance = 1; return "BRK";
				case 0x01: bytesToAdvance = 2; return string.Format("ORA (${0:X2},X)", peeker(++pc));
				case 0x04: bytesToAdvance = 2; return string.Format("NOP ${0:X2}", peeker(++pc));
				case 0x05: bytesToAdvance = 2; return string.Format("ORA ${0:X2}", peeker(++pc));
				case 0x06: bytesToAdvance = 2; return string.Format("ASL ${0:X2}", peeker(++pc));
				case 0x08: bytesToAdvance = 1; return "PHP";
				case 0x09: bytesToAdvance = 2; return string.Format("ORA #${0:X2}", peeker(++pc));
				case 0x0A: bytesToAdvance = 1; return "ASL A";
				case 0x0C: bytesToAdvance = 3; return string.Format("NOP (${0:X4})", peeker_word(++pc, peeker));
				case 0x0D: bytesToAdvance = 3; return string.Format("ORA ${0:X4}", peeker_word(++pc, peeker));
				case 0x0E: bytesToAdvance = 3; return string.Format("ASL ${0:X4}", peeker_word(++pc, peeker));
				case 0x10: bytesToAdvance = 2; return string.Format("BPL ${0:X4}", pc + 2 + (sbyte)peeker(++pc));
				case 0x11: bytesToAdvance = 2; return string.Format("ORA (${0:X2}),Y *", peeker(++pc));
				case 0x14: bytesToAdvance = 2; return string.Format("NOP ${0:X2},X", peeker(++pc));
				case 0x15: bytesToAdvance = 2; return string.Format("ORA ${0:X2},X", peeker(++pc));
				case 0x16: bytesToAdvance = 2; return string.Format("ASL ${0:X2},X", peeker(++pc));
				case 0x18: bytesToAdvance = 1; return "CLC";
				case 0x19: bytesToAdvance = 3; return string.Format("ORA ${0:X4},Y *", peeker_word(++pc, peeker));
				case 0x1A: bytesToAdvance = 1; return "NOP";
				case 0x1C: bytesToAdvance = 2; return string.Format("NOP (${0:X2},X)", peeker(++pc));
				case 0x1D: bytesToAdvance = 3; return string.Format("ORA ${0:X4},X *", peeker_word(++pc, peeker));
				case 0x1E: bytesToAdvance = 3; return string.Format("ASL ${0:X4},X", peeker_word(++pc, peeker));
				case 0x20: bytesToAdvance = 3; return string.Format("JSR ${0:X4}", peeker_word(++pc, peeker));
				case 0x21: bytesToAdvance = 2; return string.Format("AND (${0:X2},X)", peeker(++pc));
				case 0x24: bytesToAdvance = 2; return string.Format("BIT ${0:X2}", peeker(++pc));
				case 0x25: bytesToAdvance = 2; return string.Format("AND ${0:X2}", peeker(++pc));
				case 0x26: bytesToAdvance = 2; return string.Format("ROL ${0:X2}", peeker(++pc));
				case 0x28: bytesToAdvance = 1; return "PLP";
				case 0x29: bytesToAdvance = 2; return string.Format("AND #${0:X2}", peeker(++pc));
				case 0x2A: bytesToAdvance = 1; return "ROL A";
				case 0x2C: bytesToAdvance = 3; return string.Format("BIT ${0:X4}", peeker_word(++pc, peeker));
				case 0x2D: bytesToAdvance = 3; return string.Format("AND ${0:X4}", peeker_word(++pc, peeker));
				case 0x2E: bytesToAdvance = 3; return string.Format("ROL ${0:X4}", peeker_word(++pc, peeker));
				case 0x30: bytesToAdvance = 2; return string.Format("BMI ${0:X4}", pc + 2 + (sbyte)peeker(++pc));
				case 0x31: bytesToAdvance = 2; return string.Format("AND (${0:X2}),Y *", peeker(++pc));
				case 0x34: bytesToAdvance = 2; return string.Format("NOP ${0:X2},X", peeker(++pc));
				case 0x35: bytesToAdvance = 2; return string.Format("AND ${0:X2},X", peeker(++pc));
				case 0x36: bytesToAdvance = 2; return string.Format("ROL ${0:X2},X", peeker(++pc));
				case 0x38: bytesToAdvance = 1; return "SEC";
				case 0x39: bytesToAdvance = 3; return string.Format("AND ${0:X4},Y *", peeker_word(++pc, peeker));
				case 0x3A: bytesToAdvance = 1; return "NOP";
				case 0x3C: bytesToAdvance = 2; return string.Format("NOP (${0:X2},X)", peeker(++pc));
				case 0x3D: bytesToAdvance = 3; return string.Format("AND ${0:X4},X *", peeker_word(++pc, peeker));
				case 0x3E: bytesToAdvance = 3; return string.Format("ROL ${0:X4},X", peeker_word(++pc, peeker));
				case 0x40: bytesToAdvance = 1; return "RTI";
				case 0x41: bytesToAdvance = 2; return string.Format("EOR (${0:X2},X)", peeker(++pc));
				case 0x44: bytesToAdvance = 2; return string.Format("NOP ${0:X2}", peeker(++pc));
				case 0x45: bytesToAdvance = 2; return string.Format("EOR ${0:X2}", peeker(++pc));
				case 0x46: bytesToAdvance = 2; return string.Format("LSR ${0:X2}", peeker(++pc));
				case 0x48: bytesToAdvance = 1; return "PHA";
				case 0x49: bytesToAdvance = 2; return string.Format("EOR #${0:X2}", peeker(++pc));
				case 0x4A: bytesToAdvance = 1; return "LSR A";
				case 0x4C: bytesToAdvance = 3; return string.Format("JMP ${0:X4}", peeker_word(++pc, peeker));
				case 0x4D: bytesToAdvance = 3; return string.Format("EOR ${0:X4}", peeker_word(++pc, peeker));
				case 0x4E: bytesToAdvance = 3; return string.Format("LSR ${0:X4}", peeker_word(++pc, peeker));
				case 0x50: bytesToAdvance = 2; return string.Format("BVC ${0:X4}", pc + 2 + (sbyte)peeker(++pc));
				case 0x51: bytesToAdvance = 2; return string.Format("EOR (${0:X2}),Y *", peeker(++pc));
				case 0x54: bytesToAdvance = 2; return string.Format("NOP ${0:X2},X", peeker(++pc));
				case 0x55: bytesToAdvance = 2; return string.Format("EOR ${0:X2},X", peeker(++pc));
				case 0x56: bytesToAdvance = 2; return string.Format("LSR ${0:X2},X", peeker(++pc));
				case 0x58: bytesToAdvance = 1; return "CLI";
				case 0x59: bytesToAdvance = 3; return string.Format("EOR ${0:X4},Y *", peeker_word(++pc, peeker));
				case 0x5A: bytesToAdvance = 1; return "NOP";
				case 0x5C: bytesToAdvance = 2; return string.Format("NOP (${0:X2},X)", peeker(++pc));
				case 0x5D: bytesToAdvance = 3; return string.Format("EOR ${0:X4},X *", peeker_word(++pc, peeker));
				case 0x5E: bytesToAdvance = 3; return string.Format("LSR ${0:X4},X", peeker_word(++pc, peeker));
				case 0x60: bytesToAdvance = 1; return "RTS";
				case 0x61: bytesToAdvance = 2; return string.Format("ADC (${0:X2},X)", peeker(++pc));
				case 0x64: bytesToAdvance = 2; return string.Format("NOP ${0:X2}", peeker(++pc));
				case 0x65: bytesToAdvance = 2; return string.Format("ADC ${0:X2}", peeker(++pc));
				case 0x66: bytesToAdvance = 2; return string.Format("ROR ${0:X2}", peeker(++pc));
				case 0x68: bytesToAdvance = 1; return "PLA";
				case 0x69: bytesToAdvance = 2; return string.Format("ADC #${0:X2}", peeker(++pc));
				case 0x6A: bytesToAdvance = 1; return "ROR A";
				case 0x6C: bytesToAdvance = 3; return string.Format("JMP (${0:X4})", peeker_word(++pc, peeker));
				case 0x6D: bytesToAdvance = 3; return string.Format("ADC ${0:X4}", peeker_word(++pc, peeker));
				case 0x6E: bytesToAdvance = 3; return string.Format("ROR ${0:X4}", peeker_word(++pc, peeker));
				case 0x70: bytesToAdvance = 2; return string.Format("BVS ${0:X4}", pc + 2 + (sbyte)peeker(++pc));
				case 0x71: bytesToAdvance = 2; return string.Format("ADC (${0:X2}),Y *", peeker(++pc));
				case 0x74: bytesToAdvance = 2; return string.Format("NOP ${0:X2},X", peeker(++pc));
				case 0x75: bytesToAdvance = 2; return string.Format("ADC ${0:X2},X", peeker(++pc));
				case 0x76: bytesToAdvance = 2; return string.Format("ROR ${0:X2},X", peeker(++pc));
				case 0x78: bytesToAdvance = 1; return "SEI";
				case 0x79: bytesToAdvance = 3; return string.Format("ADC ${0:X4},Y *", peeker_word(++pc, peeker));
				case 0x7A: bytesToAdvance = 1; return "NOP";
				case 0x7C: bytesToAdvance = 2; return string.Format("NOP (${0:X2},X)", peeker(++pc));
				case 0x7D: bytesToAdvance = 3; return string.Format("ADC ${0:X4},X *", peeker_word(++pc, peeker));
				case 0x7E: bytesToAdvance = 3; return string.Format("ROR ${0:X4},X", peeker_word(++pc, peeker));
				case 0x80: bytesToAdvance = 2; return string.Format("NOP #${0:X2}", peeker(++pc));
				case 0x81: bytesToAdvance = 2; return string.Format("STA (${0:X2},X)", peeker(++pc));
				case 0x82: bytesToAdvance = 2; return string.Format("NOP #${0:X2}", peeker(++pc));
				case 0x84: bytesToAdvance = 2; return string.Format("STY ${0:X2}", peeker(++pc));
				case 0x85: bytesToAdvance = 2; return string.Format("STA ${0:X2}", peeker(++pc));
				case 0x86: bytesToAdvance = 2; return string.Format("STX ${0:X2}", peeker(++pc));
				case 0x88: bytesToAdvance = 1; return "DEY";
				case 0x89: bytesToAdvance = 2; return string.Format("NOP #${0:X2}", peeker(++pc));
				case 0x8A: bytesToAdvance = 1; return "TXA";
				case 0x8C: bytesToAdvance = 3; return string.Format("STY ${0:X4}", peeker_word(++pc, peeker));
				case 0x8D: bytesToAdvance = 3; return string.Format("STA ${0:X4}", peeker_word(++pc, peeker));
				case 0x8E: bytesToAdvance = 3; return string.Format("STX ${0:X4}", peeker_word(++pc, peeker));
				case 0x90: bytesToAdvance = 2; return string.Format("BCC ${0:X4}", pc + 2 + (sbyte)peeker(++pc));
				case 0x91: bytesToAdvance = 2; return string.Format("STA (${0:X2}),Y", peeker(++pc));
				case 0x94: bytesToAdvance = 2; return string.Format("STY ${0:X2},X", peeker(++pc));
				case 0x95: bytesToAdvance = 2; return string.Format("STA ${0:X2},X", peeker(++pc));
				case 0x96: bytesToAdvance = 2; return string.Format("STX ${0:X2},Y", peeker(++pc));
				case 0x98: bytesToAdvance = 1; return "TYA";
				case 0x99: bytesToAdvance = 3; return string.Format("STA ${0:X4},Y", peeker_word(++pc, peeker));
				case 0x9A: bytesToAdvance = 1; return "TXS";
				case 0x9D: bytesToAdvance = 3; return string.Format("STA ${0:X4},X", peeker_word(++pc, peeker));
				case 0xA0: bytesToAdvance = 2; return string.Format("LDY #${0:X2}", peeker(++pc));
				case 0xA1: bytesToAdvance = 2; return string.Format("LDA (${0:X2},X)", peeker(++pc));
				case 0xA2: bytesToAdvance = 2; return string.Format("LDX #${0:X2}", peeker(++pc));
				case 0xA4: bytesToAdvance = 2; return string.Format("LDY ${0:X2}", peeker(++pc));
				case 0xA5: bytesToAdvance = 2; return string.Format("LDA ${0:X2}", peeker(++pc));
				case 0xA6: bytesToAdvance = 2; return string.Format("LDX ${0:X2}", peeker(++pc));
				case 0xA8: bytesToAdvance = 1; return "TAY";
				case 0xA9: bytesToAdvance = 2; return string.Format("LDA #${0:X2}", peeker(++pc));
				case 0xAA: bytesToAdvance = 1; return "TAX";
				case 0xAC: bytesToAdvance = 3; return string.Format("LDY ${0:X4}", peeker_word(++pc, peeker));
				case 0xAD: bytesToAdvance = 3; return string.Format("LDA ${0:X4}", peeker_word(++pc, peeker));
				case 0xAE: bytesToAdvance = 3; return string.Format("LDX ${0:X4}", peeker_word(++pc, peeker));
				case 0xB0: bytesToAdvance = 2; return string.Format("BCS ${0:X4}", pc + 2 + (sbyte)peeker(++pc));
				case 0xB1: bytesToAdvance = 2; return string.Format("LDA (${0:X2}),Y *", peeker(++pc));
				case 0xB4: bytesToAdvance = 2; return string.Format("LDY ${0:X2},X", peeker(++pc));
				case 0xB5: bytesToAdvance = 2; return string.Format("LDA ${0:X2},X", peeker(++pc));
				case 0xB6: bytesToAdvance = 2; return string.Format("LDX ${0:X2},Y", peeker(++pc));
				case 0xB8: bytesToAdvance = 1; return "CLV";
				case 0xB9: bytesToAdvance = 3; return string.Format("LDA ${0:X4},Y *", peeker_word(++pc, peeker));
				case 0xBA: bytesToAdvance = 1; return "TSX";
				case 0xBC: bytesToAdvance = 3; return string.Format("LDY ${0:X4},X *", peeker_word(++pc, peeker));
				case 0xBD: bytesToAdvance = 3; return string.Format("LDA ${0:X4},X *", peeker_word(++pc, peeker));
				case 0xBE: bytesToAdvance = 3; return string.Format("LDX ${0:X4},Y *", peeker_word(++pc, peeker));
				case 0xC0: bytesToAdvance = 2; return string.Format("CPY #${0:X2}", peeker(++pc));
				case 0xC1: bytesToAdvance = 2; return string.Format("CMP (${0:X2},X)", peeker(++pc));
				case 0xC2: bytesToAdvance = 2; return string.Format("NOP #${0:X2}", peeker(++pc));
				case 0xC4: bytesToAdvance = 2; return string.Format("CPY ${0:X2}", peeker(++pc));
				case 0xC5: bytesToAdvance = 2; return string.Format("CMP ${0:X2}", peeker(++pc));
				case 0xC6: bytesToAdvance = 2; return string.Format("DEC ${0:X2}", peeker(++pc));
				case 0xC8: bytesToAdvance = 1; return "INY";
				case 0xC9: bytesToAdvance = 2; return string.Format("CMP #${0:X2}", peeker(++pc));
				case 0xCA: bytesToAdvance = 1; return "DEX";
				case 0xCC: bytesToAdvance = 3; return string.Format("CPY ${0:X4}", peeker_word(++pc, peeker));
				case 0xCD: bytesToAdvance = 3; return string.Format("CMP ${0:X4}", peeker_word(++pc, peeker));
				case 0xCE: bytesToAdvance = 3; return string.Format("DEC ${0:X4}", peeker_word(++pc, peeker));
				case 0xD0: bytesToAdvance = 2; return string.Format("BNE ${0:X4}", pc + 2 + (sbyte)peeker(++pc));
				case 0xD1: bytesToAdvance = 2; return string.Format("CMP (${0:X2}),Y *", peeker(++pc));
				case 0xD4: bytesToAdvance = 2; return string.Format("NOP ${0:X2},X", peeker(++pc));
				case 0xD5: bytesToAdvance = 2; return string.Format("CMP ${0:X2},X", peeker(++pc));
				case 0xD6: bytesToAdvance = 2; return string.Format("DEC ${0:X2},X", peeker(++pc));
				case 0xD8: bytesToAdvance = 1; return "CLD";
				case 0xD9: bytesToAdvance = 3; return string.Format("CMP ${0:X4},Y *", peeker_word(++pc, peeker));
				case 0xDA: bytesToAdvance = 1; return "NOP";
				case 0xDC: bytesToAdvance = 2; return string.Format("NOP (${0:X2},X)", peeker(++pc));
				case 0xDD: bytesToAdvance = 3; return string.Format("CMP ${0:X4},X *", peeker_word(++pc, peeker));
				case 0xDE: bytesToAdvance = 3; return string.Format("DEC ${0:X4},X", peeker_word(++pc, peeker));
				case 0xE0: bytesToAdvance = 2; return string.Format("CPX #${0:X2}", peeker(++pc));
				case 0xE1: bytesToAdvance = 2; return string.Format("SBC (${0:X2},X)", peeker(++pc));
				case 0xE2: bytesToAdvance = 2; return string.Format("NOP #${0:X2}", peeker(++pc));
				case 0xE4: bytesToAdvance = 2; return string.Format("CPX ${0:X2}", peeker(++pc));
				case 0xE5: bytesToAdvance = 2; return string.Format("SBC ${0:X2}", peeker(++pc));
				case 0xE6: bytesToAdvance = 2; return string.Format("INC ${0:X2}", peeker(++pc));
				case 0xE8: bytesToAdvance = 1; return "INX";
				case 0xE9: bytesToAdvance = 2; return string.Format("SBC #${0:X2}", peeker(++pc));
				case 0xEA: bytesToAdvance = 1; return "NOP";
				case 0xEC: bytesToAdvance = 3; return string.Format("CPX ${0:X4}", peeker_word(++pc, peeker));
				case 0xED: bytesToAdvance = 3; return string.Format("SBC ${0:X4}", peeker_word(++pc, peeker));
				case 0xEE: bytesToAdvance = 3; return string.Format("INC ${0:X4}", peeker_word(++pc, peeker));
				case 0xF0: bytesToAdvance = 2; return string.Format("BEQ ${0:X4}", pc + 2 + (sbyte)peeker(++pc));
				case 0xF1: bytesToAdvance = 2; return string.Format("SBC (${0:X2}),Y *", peeker(++pc));
				case 0xF4: bytesToAdvance = 2; return string.Format("NOP ${0:X2},X", peeker(++pc));
				case 0xF5: bytesToAdvance = 2; return string.Format("SBC ${0:X2},X", peeker(++pc));
				case 0xF6: bytesToAdvance = 2; return string.Format("INC ${0:X2},X", peeker(++pc));
				case 0xF8: bytesToAdvance = 1; return "SED";
				case 0xF9: bytesToAdvance = 3; return string.Format("SBC ${0:X4},Y *", peeker_word(++pc, peeker));
				case 0xFA: bytesToAdvance = 1; return "NOP";
				case 0xFC: bytesToAdvance = 2; return string.Format("NOP (${0:X2},X)", peeker(++pc));
				case 0xFD: bytesToAdvance = 3; return string.Format("SBC ${0:X4},X *", peeker_word(++pc, peeker));
				case 0xFE: bytesToAdvance = 3; return string.Format("INC ${0:X4},X", peeker_word(++pc, peeker));
			}
			bytesToAdvance = 1;
			return "???";
		}

		public string Cpu
		{
			get
			{
				return "6502";
			}
			set
			{
			}
		}

		public string PCRegisterName
		{
			get { return "PC"; }
		}

		public IEnumerable<string> AvailableCpus
		{
			get { yield return "6502"; }
		}

		public string Disassemble(MemoryDomain m, uint addr, out int length)
		{
			return Disassemble((ushort)addr, out length, a => m.PeekByte((int)a));
		}
	}
}
