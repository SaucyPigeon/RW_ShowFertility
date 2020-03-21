using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using System.Reflection.Emit;
using System.Reflection;

namespace FertilityMapMode
{
	public static class Transpilers
	{
		public static IEnumerable<CodeInstruction> AddCallAfter(this IEnumerable<CodeInstruction> instructions, MethodInfo first, MethodInfo second)
		{
			if (instructions == null)
			{
				throw new ArgumentNullException(nameof(instructions));
			}
			if (first == null)
			{
				throw new ArgumentNullException(nameof(first));
			}
			if (second == null)
			{
				throw new ArgumentNullException(nameof(second));
			}

			foreach (var instruction in instructions)
			{
				yield return instruction;

				if (instruction.Calls(first))
				{
					yield return new CodeInstruction(opcode: OpCodes.Call, operand: second);
				}
			}
		}

		public static IEnumerable<CodeInstruction> ReplaceFieldLoad(this IEnumerable<CodeInstruction> instructions, FieldInfo target, FieldInfo value)
		{
			if (instructions == null)
			{
				throw new ArgumentNullException(nameof(instructions));
			}
			if (target == null)
			{
				throw new ArgumentNullException(nameof(target));
			}
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			foreach (var instruction in instructions)
			{
				if (instruction.LoadsField(target))
				{
					instruction.operand = value;
				}
				yield return instruction;
			}
		}
	}
}
